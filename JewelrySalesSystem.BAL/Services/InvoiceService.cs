using AutoMapper;
using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.Invoices;
using JewelrySalesSystem.DAL.Common;
using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;
using ZXing;
using ZXing.Common;
using ZXing.Windows.Compatibility;
using System.Text;
using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using MigraDocCore.DocumentObjectModel.Tables;
using MigraDocCore.DocumentObjectModel;
using PdfSharpCore.Utils;
using SixLabors.ImageSharp.PixelFormats;
using OfficeOpenXml;

namespace JewelrySalesSystem.BAL.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public InvoiceService(
            IUnitOfWork unitOfWork
            , IMapper mapper
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;  
        }

        public async Task<PaginatedList<GetInvoiceResponse>> PaginationAsync(
            string? invoiceStatus,
            string? invoiceType,
            string? inOrOut,
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            bool isActive,
            int page,
            int pageSize)
        => _mapper.Map<PaginatedList<GetInvoiceResponse>>(await _unitOfWork.Invoices.PaginationAsync(invoiceStatus, invoiceType, inOrOut, searchTerm, sortColumn, sortOrder, isActive, page, pageSize));

        public async Task<CreateInvoiceRequest> AddAsync(CreateInvoiceRequest createInvoiceRequest)
        {
            var invoiceDetails = new List<InvoiceDetail>();

            if (createInvoiceRequest.InvoiceDetails.Count > 0)
            {
                foreach (var item in createInvoiceRequest.InvoiceDetails)
                {
                    var existedProduct = await _unitOfWork.Products.GetEntityByIdAsync(item.ProductId);

                    if (existedProduct != null)
                    {
                        switch (existedProduct.ProductTypeId)
                        {
                            case 2:
                                var material = await _unitOfWork.Materials.GetByNameWithIncludeAsync(existedProduct.ProductName);

                                if (material != null)
                                {
                                    invoiceDetails.Add(new InvoiceDetail
                                    {
                                        ProductId = item.ProductId,
                                        ProductPrice = existedProduct.ProductPrice * item.Quantity,
                                        Quantity = item.Quantity
                                    });
                                }

                                break;
                            case 3:
                                invoiceDetails.Add(new InvoiceDetail
                                {
                                    ProductId = item.ProductId,
                                    ProductPrice = existedProduct.ProductPrice * item.Quantity,
                                    Quantity = item.Quantity
                                });

                                break;
                            default:
                                var gem = await _unitOfWork.Gems.GetByNameWithIncludeAsync(existedProduct.ProductName);

                                if (gem != null)
                                {
                                    invoiceDetails.Add(new InvoiceDetail
                                    {
                                        ProductId = item.ProductId,
                                        ProductPrice = existedProduct.ProductPrice * item.Quantity,
                                        Quantity = item.Quantity
                                    });
                                }

                                break;
                        }
                        existedProduct.Quantity -= item.Quantity;
                        _unitOfWork.Products.UpdateEntity(existedProduct);
                    }
                }
            }

            var invoice = new Invoice
            {
                OrderDate = DateTime.Now,
                InvoiceStatus = createInvoiceRequest.InvoiceStatus,
                UserId = createInvoiceRequest.UserId,
                Total = createInvoiceRequest.Total,
                WarrantyId = 1,
                PerDiscount = createInvoiceRequest.PerDiscount,
                TotalWithDiscount = createInvoiceRequest.Total - (createInvoiceRequest.Total * createInvoiceRequest.PerDiscount) / 100,
                InvoiceDetails = invoiceDetails
            };

            var customer = await _unitOfWork.Customers.GetCustomerByNameAsync(createInvoiceRequest.CustomerName);

            if (customer != null)
            {
                invoice.CustomerId = customer.CustomerId;
                //tru diem dua tren perDiscount
                await ProcessPointByPerDiscount((int)createInvoiceRequest.PerDiscount, customer.CustomerId, createInvoiceRequest.InvoiceStatus);
            }
            else
            {
                var newestCustomer = new Customer { FullName = createInvoiceRequest.CustomerName, PhoneNumber = createInvoiceRequest.PhoneNumber };

                invoice.PerDiscount = 0;

                invoice.Customer = newestCustomer;
            }

            var result = _unitOfWork.Invoices.AddEntity(invoice);

            await _unitOfWork.CompleteAsync();

            return createInvoiceRequest;
        }

        //tru diem dua tren perDiscount
        private async Task ProcessPointByPerDiscount(int perDiscount, int customerId, string invoiceStatus)
        {
            if (invoiceStatus.ToLower() == "pending")
            {
                var customer = await _unitOfWork.Customers.GetEntityByIdAsync(customerId);
                if (customer != null)
                {
                    if (perDiscount == 5)
                    {
                        customer.Point -= 50;
                    }
                    else if (perDiscount == 10)
                    {
                        customer.Point -= 100;
                    }
                    else if (perDiscount == 15)
                    {
                        customer.Point -= 200;
                    }
                    if (customer.Point < 0) { customer.Point = 0; }
                    _unitOfWork.Customers.UpdateEntity(customer);
                    await _unitOfWork.CompleteAsync();
                }
            }
            else if (invoiceStatus.ToLower() == "draft" || invoiceStatus.ToLower() == "cancel")
            {
                var customer = await _unitOfWork.Customers.GetEntityByIdAsync(customerId);
                if (customer != null)
                {
                    if (perDiscount == 5)
                    {
                        customer.Point += 50;
                    }
                    else if (perDiscount == 10)
                    {
                        customer.Point += 100;
                    }
                    else if (perDiscount == 15)
                    {
                        customer.Point += 200;
                    }
                    if (customer.Point < 0) { customer.Point = 0; }
                    _unitOfWork.Customers.UpdateEntity(customer);
                    await _unitOfWork.CompleteAsync();
                }
            }
        }

        public async Task<UpdateInvoiceRequest> UpdateAsync(UpdateInvoiceRequest updateInvoiceRequest)
        {
            var invoiceDetails = new List<InvoiceDetail>();

            if (updateInvoiceRequest.InvoiceDetails.Count > 0)
            {
                foreach (var item in updateInvoiceRequest.InvoiceDetails)
                {
                    var existedProduct = await _unitOfWork.Products.GetEntityByIdAsync(item.ProductId);

                    if (existedProduct != null)
                    {
                        switch (existedProduct.ProductTypeId)
                        {
                            case 2:
                                var material = await _unitOfWork.Materials.GetByNameWithIncludeAsync(existedProduct.ProductName);

                                if (material != null)
                                {
                                    invoiceDetails.Add(new InvoiceDetail
                                    {
                                        ProductId = item.ProductId,
                                        ProductPrice = existedProduct.ProductPrice * item.Quantity,
                                        Quantity = item.Quantity
                                    });
                                }

                                break;
                            case 3:
                                invoiceDetails.Add(new InvoiceDetail
                                {
                                    ProductId = item.ProductId,
                                    ProductPrice = existedProduct.ProductPrice * item.Quantity,
                                    Quantity = item.Quantity
                                });

                                break;
                            default:
                                var gem = await _unitOfWork.Gems.GetByNameWithIncludeAsync(existedProduct.ProductName);

                                if (gem != null)
                                {
                                    invoiceDetails.Add(new InvoiceDetail
                                    {
                                        ProductId = item.ProductId,
                                        ProductPrice = existedProduct.ProductPrice * item.Quantity,
                                        Quantity = item.Quantity
                                    });
                                }

                                break;
                        }
                    }
                }
            }

            var invoice = new Invoice
            {
                InvoiceId = updateInvoiceRequest.InvoiceId,
                InvoiceStatus = updateInvoiceRequest.InvoiceStatus,
                OrderDate = DateTime.Now,
                //UserId = updateInvoiceRequest.UserId,
                WarrantyId = 1,
                InvoiceDetails = invoiceDetails,
                Total = updateInvoiceRequest.Total,
                //PerDiscount = updateInvoiceRequest.PerDiscount,
                //TotalWithDiscount = updateInvoiceRequest.Total - (updateInvoiceRequest.Total * updateInvoiceRequest.PerDiscount) / 100
            };

            //var customer = await _unitOfWork.Customers.GetCustomerByNameAsync(updateInvoiceRequest.CustomerName);

            //if (customer != null)
            //{
            //    invoice.CustomerId = customer.CustomerId;

            //    //changes here
            //    if (updateInvoiceRequest.PerDiscount == 0)
            //    {
            //        //1M vnd = 1 point
            //        int points = (int)(updateInvoiceRequest.Total / 1000000);
            //        await ProcessPoint(points, customer.CustomerId, updateInvoiceRequest.InvoiceStatus);
            //    }
            //    else
            //    {
            //        await ProcessPointByPerDiscount((int)updateInvoiceRequest.PerDiscount, customer.CustomerId, updateInvoiceRequest.InvoiceStatus);                    
            //    }
            //}

            await _unitOfWork.Invoices.UpdateInvoice(invoice);

            await _unitOfWork.CompleteAsync();

            return updateInvoiceRequest;
        }

        //changes here
        private async Task ProcessPoint(int point, int customerId, string invoiceStatus)
        {
            if (invoiceStatus.ToLower() == "delivered")
            {
                var customer = await _unitOfWork.Customers.GetEntityByIdAsync(customerId);

                if (customer != null)
                {
                    customer.Point += point;
                    _unitOfWork.Customers.UpdateEntity(customer);
                    await _unitOfWork.CompleteAsync();
                }
            }
        }

        public async Task<GetInvoiceResponse?> GetByIdWithIncludeAsync(int id) => _mapper.Map<GetInvoiceResponse>(await _unitOfWork.Invoices.GetByIdWithIncludeAsync(id));

        //private async Task<float> CalculateProductPrice(int productId)
        //{
        //    var product = await _unitOfWork.Products.GetByIdWithIncludeAsync(productId);

        //    float productPrice = 0;

        //    if (product != null)
        //    {
        //        productPrice += product.ProductionCost;
        //        if (product.ProductGems.Count > 0)
        //        {
        //            foreach (var gem in product.ProductGems)
        //            {
        //                var temp = await _unitOfWork.Gems.GetByIdWithIncludeAsync(gem.GemId);

        //                if (temp != null)
        //                {
        //                    var gemPrice = temp.GemPrice;

        //                    if (gemPrice != null)
        //                    {
        //                        productPrice += gemPrice.CutPrice + gemPrice.CaratWeightPrice + gemPrice.ClarityPrice + gemPrice.ColourPrice;
        //                    }
        //                }
        //            }
        //        }

        //        if (product.ProductMaterials.Count > 0)
        //        {
        //            foreach (var material in product.ProductMaterials)
        //            {
        //                var temp = await _unitOfWork.Materials.GetByIdWithIncludeAsync(material.MaterialId);

        //                if (temp != null)
        //                {
        //                    var materialPrice = temp.MaterialPrices.SingleOrDefault();

        //                    if (materialPrice != null) productPrice += (product.Weight * materialPrice.SellPrice);
        //                }
        //            }
        //        }
        //        productPrice += (productPrice * (product.PercentPriceRate) / 100);
        //    }
        //    return productPrice;
        //}

        public async Task DeleteInvoice(int id)
        {
            await _unitOfWork.Invoices.DeleteById(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<GetInvoiceResponse?> GetByIdAsync(int id) => _mapper.Map<GetInvoiceResponse>(await _unitOfWork.Invoices.GetEntityByIdAsync(id));

        public async Task ChangeInvoiceStatus(int id)
        {
            await _unitOfWork.Invoices.ChangeInvoiceStatus(id);

            await _unitOfWork.CompleteAsync();
        }

        public async Task CancelInvoice(int id)
        {
            await _unitOfWork.Invoices.CancelInvoice(id);

            await _unitOfWork.CompleteAsync();
        }

        public async Task<CreatePurchaseInvoiceRequest> AddPurchaseInvoiceAsync(CreatePurchaseInvoiceRequest createPurchaseInvoiceRequest)
        {
            var invoiceDetails = new List<InvoiceDetail>();

            float total = 0;
            //int countProductHasMaterial = 0;

            if (createPurchaseInvoiceRequest.InvoiceDetails.Count > 0)
            {

                foreach (var item in createPurchaseInvoiceRequest.InvoiceDetails)
                {
                    var existedProduct = await _unitOfWork.Products.GetEntityByIdAsync(item.ProductId);

                    if (existedProduct != null)
                    {
                        switch (existedProduct.ProductTypeId)
                        {
                            case 2:
                                var material = await _unitOfWork.Materials.GetByNameWithIncludeAsync(existedProduct.ProductName);

                                if (material != null)
                                {
                                    var buyMaterialPrice = await _unitOfWork.MaterialPrices.GetNewestMaterialPriceByMaterialIdAsync(material.MaterialId);

                                    //if(createPurchaseInvoiceRequest.InvoiceType.Equals("in"))

                                    invoiceDetails.Add(new InvoiceDetail
                                    {
                                        ProductId = item.ProductId,
                                        Quantity = item.Quantity,
                                        //ProductPrice = existedProduct.ProductPrice
                                        //ProductPrice = buyMaterialPrice.BuyPrice * item.Quantity * 375 / 100,
                                        ProductPrice = buyMaterialPrice.BuyPrice * item.Quantity,
                                    });;
                                    //countProductHasMaterial++;
                                }

                                break;
                            case 3:
                                var materialInProduct = await _unitOfWork.ProductMaterials.GetProductMaterialByProductIdAsync(existedProduct.ProductId);
                                var materialPrice = await _unitOfWork.MaterialPrices.GetNewestMaterialPriceByMaterialIdAsync(materialInProduct.MaterialId);
                                invoiceDetails.Add(new InvoiceDetail
                                {

                                    ProductId = item.ProductId,
                                    Quantity = item.Quantity,
                                    //ProductPrice = existedProduct.ProductPrice,
                                    ProductPrice = (float)(materialPrice.BuyPrice * materialInProduct.Weight * item.Quantity / 375),
                                    

                                });
                                break;
                            default:
                                var gem = await _unitOfWork.Gems.GetByNameWithIncludeAsync(existedProduct.ProductName);

                                if (gem != null)
                                {
                                    invoiceDetails.Add(new InvoiceDetail
                                    {
                                        ProductId = item.ProductId,
                                        Quantity = item.Quantity,
                                        ProductPrice = existedProduct.ProductPrice * item.Quantity * 70 / 100,

                                    });
                                }

                                break;
                        }
                    }
                }
                //if (createPurchaseInvoiceRequest.InvoiceType.Equals("in"))
                //{
                //    foreach(var item in createPurchaseInvoiceRequest.InvoiceDetails)
                //    {
                //        var existedProduct = await _unitOfWork.Products.GetEntityByIdAsync(item);
                //        if (existedProduct != null)
                //        {
                //            var existingInvoiceDetails = await _unitOfWork.
                //        }
                //    }
                //}
            }

            foreach(var item in invoiceDetails)
            {
                total += item.ProductPrice;
            }

            var customer = await _unitOfWork.Customers.GetCustomerByNameAsync(createPurchaseInvoiceRequest.CustomerName);

            var invoice = new Invoice
            {
                //OrderDate = DateTime.Now,
                //CustomerId = await _unitOfWork.Customers.GetCustomerByNameAsync(createPurchaseInvoiceRequest.CustomerName),
                InvoiceType = "Purchase",
                InvoiceStatus = "Pending",
                InOrOut = createPurchaseInvoiceRequest.InOrOut,
                UserId = createPurchaseInvoiceRequest.UserId,
                WarrantyId = 1,
                Total = total,
                PerDiscount = 0,
                TotalWithDiscount = total,
                InvoiceDetails = invoiceDetails
            };

            if (customer != null)
            {
                invoice.CustomerId = customer.CustomerId;
            }
            else
            {
                var newestCustomer = new Customer { FullName = createPurchaseInvoiceRequest.CustomerName, PhoneNumber = createPurchaseInvoiceRequest.PhoneNumber };

                invoice.Customer = newestCustomer;
            }

            var allAreGoldProduct = true;

            //var allAreExistingProduct = true;

            //if (invoice.InvoiceType.Equals("in"))
            //{
            //    foreach (var item in invoice.InvoiceDetails)
            //    {
            //        var existingProduct = await _unitOfWork.Products.GetEntityByIdAsync(item.ProductId);
            //        if (existingProduct == null)
            //        {
            //            allAreExistingProduct = false;
            //            break;
            //        }

            //    }
            //}
            //else
            //{
            //    //var allAreGoldProduct = true;
            //    foreach (var item in invoice.InvoiceDetails)
            //    {
            //        var existingProduct = await _unitOfWork.Products.GetEntityByIdAsync(item.ProductId);
            //        if (existingProduct != null)
            //        {
            //            if (existingProduct.ProductTypeId != 2)
            //            {
            //                allAreGoldProduct = false;
            //                break;
            //            }
            //        }
            //    }

            //}
            if (invoice.InOrOut.ToLower() == "out")
            {
                foreach (var item in invoice.InvoiceDetails)
                {
                    var existingProduct = await _unitOfWork.Products.GetEntityByIdAsync(item.ProductId);
                    if (existingProduct != null)
                    {
                        if (existingProduct.ProductTypeId != 2)
                        {
                            allAreGoldProduct = false;
                            break;
                        }
                    }
                }
            }

            //if (allAreGoldProduct == true  && countProductHasMaterial==createPurchaseInvoiceRequest.Weights.Count)  //&& allAreExistingProduct == true
            //{
            //    var result = _unitOfWork.Invoices.AddEntity(invoice);
            //}
            try
            {
                if (allAreGoldProduct == true)
                //if (allAreGoldProduct == true && countProductHasMaterial == createPurchaseInvoiceRequest.Weights.Count)
                {
                    var result = _unitOfWork.Invoices.AddEntity(invoice);
                    await _unitOfWork.CompleteAsync();
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Can not add new purchase invoice.", ex);
            }

            return createPurchaseInvoiceRequest;
        }

        //public async Task<byte[]> GenerateInvoicePdf(int invoiceId)
        //{
        //    var invoice = await _unitOfWork.Invoices.GetByIdWithIncludeAsync(invoiceId);
        //    if (invoice == null)
        //    {
        //        throw new Exception($"Invoice with id {invoiceId} not found.");
        //    }

        //    //tạo file pdf
        //    PdfDocument pdf = new PdfDocument();

        //    // thêm trang
        //    PdfPage page = pdf.AddPage();

        //    XGraphics gfx = XGraphics.FromPdfPage(page);


        //    // Khai báo font chữ cho các phần riêng biệt
        //    XFont boldFont = new XFont("Verdana", 15, XFontStyle.Bold);
        //    XFont regularFont = new XFont("Verdana", 15, XFontStyle.Regular);
        //    XFont largerFont = new XFont("Verdana", 20, XFontStyle.Bold);

        //    void DrawLabelValue(XGraphics gfx, XFont boldFont, XFont regularFont, string label, string value, double x, double y)
        //    {
        //        // In ra label in đậm
        //        gfx.DrawString($"{label}: ", boldFont, XBrushes.Black,
        //            new XRect(x, y, gfx.PageSize.Width, gfx.PageSize.Height), XStringFormats.TopLeft);

        //        // Lấy chiều rộng của chuỗi label in đậm để tính toán vị trí tiếp theo
        //        double boldTextWidth = gfx.MeasureString($"{label}: ", boldFont).Width;

        //        // In ra giá trị value in nhạt, tính toán vị trí dựa trên chiều rộng của label in đậm
        //        gfx.DrawString($"{value}", regularFont, XBrushes.Gray,
        //            new XRect(x + boldTextWidth, y, gfx.PageSize.Width, gfx.PageSize.Height), XStringFormats.TopLeft);

        //    }

        //    //title
        //    gfx.DrawString("Jewelry Sales System", largerFont, XBrushes.Black,
        //        new XRect(200, 20, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
        //    // In ra InvoiceId
        //    DrawLabelValue(gfx, boldFont, regularFont, "InvoiceId", invoice.InvoiceId.ToString(), 30, 70);

        //    // In ra CustomerName
        //    DrawLabelValue(gfx, boldFont, regularFont, "CustomerName", invoice.Customer.FullName, 30, 100);

        //    // In ra PhoneNumber 

        //    DrawLabelValue(gfx, boldFont, regularFont, "PhoneNumber", invoice.Customer.PhoneNumber, 30, 130);

        //    // In ra OrderDate
        //    DrawLabelValue(gfx, boldFont, regularFont, "OrderDate", invoice.OrderDate.ToString("yyyy-MM-dd "), 30, 160);



        //    float tableTop = 190; // Điểm bắt đầu của bảng
        //    float column1Left = 30; // Cột đầu tiên
        //    float column2Left = 150; // Cột thứ hai
        //    float column3Left = 300; // Cột thứ ba

        //    // Tiêu đề của các cột
        //    gfx.DrawString("Product ID", boldFont, XBrushes.Black,
        //        new XRect(column1Left, tableTop, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
        //    gfx.DrawString("Product Name", boldFont, XBrushes.Black,
        //        new XRect(column2Left, tableTop, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
        //    gfx.DrawString("Price", boldFont, XBrushes.Black,
        //        new XRect(column3Left, tableTop, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);

        //    float dataTop = tableTop + 20;

        //    // Lặp qua từng sản phẩm trong danh sách và in ra thông tin của từng sản phẩm
        //    foreach (var item in invoice.InvoiceDetails)
        //    {
        //        // Dữ liệu của từng sản phẩm
        //        string productId = $"{item.ProductId}";
        //        string productName = $"{item.Product.ProductName}";
        //        string productPrice = $" {item.ProductPrice}";

        //        // In ra các dòng dữ liệu của từng sản phẩm
        //        gfx.DrawString(productId, regularFont, XBrushes.Black,
        //            new XRect(column1Left, dataTop, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
        //        gfx.DrawString(productName, regularFont, XBrushes.Black,
        //            new XRect(column2Left, dataTop, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
        //        gfx.DrawString(productPrice, regularFont, XBrushes.Black,
        //            new XRect(column3Left, dataTop, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);

        //        // Di chuyển vị trí dòng dữ liệu xuống cho sản phẩm tiếp theo
        //        dataTop += 30;
        //    }


        //    float additionalInfoTop = dataTop + 30; // Vị trí bắt đầu in các thông tin khác

        //    // In ra các thông tin khác của hóa đơn trên từng dòng riêng biệt
        //    gfx.DrawString($"Discount: {invoice.PerDiscount}", boldFont, XBrushes.Black,
        //        new XRect(column3Left - 105, dataTop, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
        //    //dataTop += 30;

        //    //gfx.DrawString($"TotalWithDiscount: {invoice.TotalWithDiscount}", boldFont, XBrushes.Black,
        //    //    new XRect(column3Left-160, dataTop, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
        //    dataTop += 30;

        //    gfx.DrawString($"Total: {invoice.Total}", boldFont, XBrushes.Black,
        //        new XRect(column3Left - 45, dataTop, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);


        //    byte[] barcodeBytes = await GenerateBarCode(invoiceId);
        //    //XImage barcodeImage = XImage.FromStream(new MemoryStream(barcodeBytes));
        //    XImage barcodeImage = XImage.FromStream(() => new MemoryStream(barcodeBytes));
        //    // Tính toán vị trí để đặt barcode trên trang PDF
        //    float barcodeX = 50; // Khoảng cách từ lề trái
        //    float barcodeY = (float)(page.Height - 150); // Khoảng cách từ lề trên
        //    //gfx = XGraphics.FromPdfPage(page);
        //    gfx.DrawImage(barcodeImage, barcodeX, barcodeY);
        //    // Save the document to a byte array

        //    using (MemoryStream stream = new MemoryStream())
        //    {

        //        pdf.Save(stream, false);
        //        return stream.ToArray();
        //    }



        //}

        public async Task<byte[]> GenerateInvoicePdf(int invoiceId)
        {
            var invoice = await _unitOfWork.Invoices.GetByIdWithIncludeAsync(invoiceId);
            if (invoice == null)
            {
                throw new Exception($"Invoice with id {invoiceId} not found.");
            }

            MigraDocCore.DocumentObjectModel.Document doc = new();
            Section sec = doc.AddSection();

            var titleStyle = doc.AddStyle("TitleStyle", "Normal");
            titleStyle.Font.Name = "Times New Roman";
            titleStyle.Font.Size = 13;
            titleStyle.Font.Bold = true;

            var arialStyle = doc.AddStyle("ArialStyle", "Normal");
            arialStyle.Font.Name = "Arial";
            arialStyle.Font.Size = 15;



            Paragraph titleParagraph = sec.AddParagraph("Jewelry Sales System", "TitleStyle");
            titleParagraph.Format.Alignment = ParagraphAlignment.Center;
            titleParagraph.Format.Font.Size = 24;
            sec.AddParagraph();

            Paragraph titleType;
            if (invoice.InvoiceType.Equals("Sale"))
            {
                titleType = sec.AddParagraph("Sale Invoice", "TitleStyle");

            }
            else
            {
                titleType = sec.AddParagraph("Purchase Invoice", "TitleStyle");
            }
            titleType.Format.Alignment = ParagraphAlignment.Center;
            titleType.Format.Font.Size = 16;
            titleParagraph.Format.Alignment = ParagraphAlignment.Center;
            sec.AddParagraph();

            sec.AddParagraph($"Invoice ID: {invoice.InvoiceId}", "TitleStyle");

            sec.AddParagraph($"Customer Name: {invoice.Customer.FullName}", "TitleStyle");
            sec.AddParagraph($"Phone: {invoice.Customer.PhoneNumber}", "TitleStyle");
            sec.AddParagraph($"Order Date: {invoice.OrderDate.ToString("yyyy-MM-dd HH:mm:ss")}", "TitleStyle");

            sec.AddParagraph();
            Table table = new();
            table.Borders.Width = 0.5;
            Column column = table.AddColumn(MigraDocCore.DocumentObjectModel.Unit.FromCentimeter(1));
            column = table.AddColumn(MigraDocCore.DocumentObjectModel.Unit.FromCentimeter(8));
            column = table.AddColumn(MigraDocCore.DocumentObjectModel.Unit.FromCentimeter(2));
            column = table.AddColumn(MigraDocCore.DocumentObjectModel.Unit.FromCentimeter(2));
            column = table.AddColumn(MigraDocCore.DocumentObjectModel.Unit.FromCentimeter(3));

            Row row = table.AddRow();
            Cell cell = row.Cells[0];
            cell = row.Cells[0];
            Paragraph itemParagraph = cell.AddParagraph("Item");
            itemParagraph.Format.Font.Bold = true;
            itemParagraph.Format.Alignment = ParagraphAlignment.Center;
            cell = row.Cells[1];
            Paragraph productNameParagraph = cell.AddParagraph("Product Name");
            productNameParagraph.Format.Font.Bold = true;
            productNameParagraph.Format.Alignment = ParagraphAlignment.Center;
            cell = row.Cells[2];
            Paragraph quantityParagraph = cell.AddParagraph("Quantity");
            quantityParagraph.Format.Font.Bold = true;
            quantityParagraph.Format.Alignment = ParagraphAlignment.Center;
            cell = row.Cells[3];
            Paragraph unit = cell.AddParagraph("Unit");
            unit.Format.Font.Bold = true;
            unit.Format.Alignment = ParagraphAlignment.Center;
            cell = row.Cells[4];
            Paragraph priceParagraph = cell.AddParagraph("Price");
            priceParagraph.Format.Font.Bold = true;
            priceParagraph.Format.Alignment = ParagraphAlignment.Center;
            var count = 1; /*var totalQuantity = 0;*/
            //float total = 0;
            foreach (var item in invoice.InvoiceDetails)
            {
                row = table.AddRow();
                row.Cells[0].AddParagraph($"{count}");
                row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[1].AddParagraph(item.Product.ProductName);
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[2].AddParagraph(item.Quantity.ToString());
                row.Cells[2].Format.Alignment = ParagraphAlignment.Center;
                if (item.Product.Unit != null)
                {
                    row.Cells[3].AddParagraph(item.Product.Unit.Name);
                }
                else
                {
                    row.Cells[3].AddParagraph("N/A");
                }
                row.Cells[3].Format.Alignment = ParagraphAlignment.Center;
                string formattedPrice = item.ProductPrice.ToString("N0", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                row.Cells[4].AddParagraph($"{formattedPrice} đ");
                row.Cells[4].Format.Alignment = ParagraphAlignment.Right;
                count++;
                //totalQuantity += item.Quantity;
                //total += item.ProductPrice * item.Quantity;
            }
            row = table.AddRow();
            row.Cells[0].AddParagraph("Total");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[0].Format.Font.Bold = true;
            //row.Cells[2].AddParagraph($"{totalQuantity}");
            //row.Cells[2].Format.Alignment = ParagraphAlignment.Center;
            string formattedTotal = invoice.Total.ToString("N0", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            row.Cells[4].AddParagraph($"{formattedTotal} đ");
            row.Cells[4].Format.Alignment = ParagraphAlignment.Right;
            //row = table.AddRow();
            doc.LastSection.Add(table);
            sec.AddParagraph();
            var discount = sec.AddParagraph($"Discount: {invoice.PerDiscount}%");
            discount.Format.Alignment = ParagraphAlignment.Right;
            discount.Format.Font.Bold = true;
            string formattedTotalDiscount = invoice.TotalWithDiscount.ToString("N0", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            var totalWithDiscount = sec.AddParagraph($"Total with Discount: {formattedTotalDiscount} đ");
            totalWithDiscount.Format.Font.Bold = true;
            totalWithDiscount.Format.Alignment = ParagraphAlignment.Right;
            for (int i = 0; i < 4; i++)
            {
                sec.AddParagraph();
            }
            //var barcodeBytes = await GenerateBarCode(invoiceId);
            //if (barcodeBytes != null)
            //{
            //    string tempFilePath = Path.Combine(Path.GetTempPath(), "barcode.jpeg");
            //    File.WriteAllBytes(tempFilePath, barcodeBytes);
            //    if (File.Exists(tempFilePath))
            //    {
            //        if (ImageSource.ImageSourceImpl == null)
            //            ImageSource.ImageSourceImpl = new ImageSharpImageSource<Rgba32>();

            //        //var image = MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes.ImageSource.FromFile(tempFilePath);
            //        sec.AddImage(ImageSource.FromFile(tempFilePath));
            //    }
            //    //string tempFilePath = Path.Combine(Path.GetTempPath(), "barcode.png");





            //    //image.Width = "4cm";
            //    //image.Height = "2cm";

            //    //image.Left = "0.5cm";

            //    //Xóa tệp tạm thời sau khi sử dụng xong
            //    File.Delete(tempFilePath);
            //}
            //var saleTitle = sec.AddParagraph("Salesman", "TitleStyle");
            //saleTitle.Format.LeftIndent = 350;
            //for (int i = 0; i < 3; i++)
            //{
            //    sec.AddParagraph();
            //}
            //var saleName = sec.AddParagraph($"{invoice.User.FullName}", "TitleStyle");
            //saleName.Format.LeftIndent = 340;

            Table table2 = new();
            table2.Borders.Width = 0;
            Column column2 = table2.AddColumn(MigraDocCore.DocumentObjectModel.Unit.FromCentimeter(8));
            column2 = table2.AddColumn(MigraDocCore.DocumentObjectModel.Unit.FromCentimeter(8));
            Row row2 = table2.AddRow();
            var barcodeBytes = await GenerateBarCode(invoiceId);
            if (barcodeBytes != null)
            {
                string tempFilePath = Path.Combine(Path.GetTempPath(), "barcode.jpeg");
                File.WriteAllBytes(tempFilePath, barcodeBytes);
                if (File.Exists(tempFilePath))
                {
                    if (ImageSource.ImageSourceImpl == null)
                        ImageSource.ImageSourceImpl = new ImageSharpImageSource<Rgba32>();

                    //var image = MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes.ImageSource.FromFile(tempFilePath);
                    row2.Cells[0].AddImage(ImageSource.FromFile(tempFilePath));
                }

                File.Delete(tempFilePath);
            }
            row2.Cells[1].AddParagraph("Salesman");
            //row2.Cells[1].Format.Font.Name = "Times New Roman";
            //row2.Cells[1].Format.Font.Size = 13;
            //row2.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            for (int i = 0; i < 3; i++)
            {
                row2.Cells[1].AddParagraph();
            }
            var saleName = row2.Cells[1].AddParagraph($"{invoice.User.FullName}");
            row2.Cells[1].Format.Font.Name = "Times New Roman";
            row2.Cells[1].Format.Font.Size = 13;
            row2.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            doc.LastSection.Add(table2);


#pragma warning disable
            MigraDocCore.Rendering.PdfDocumentRenderer docRend = new MigraDocCore.Rendering.PdfDocumentRenderer(true);
            docRend.Document = doc;


            docRend.RenderDocument();


            using (MemoryStream ms = new MemoryStream())
            {

                //pdf.Save(ms);
                docRend.PdfDocument.Save(ms);
                return ms.ToArray();
            }



        }

        public async Task ChangePendingToDraft(int id)
        {
            await _unitOfWork.Invoices.ChangePendingToDraft(id);

            await _unitOfWork.CompleteAsync();
        }

        public async Task<byte[]> GenerateBarCode(int id)
        {
            var invoice = await _unitOfWork.Invoices.GetByIdWithIncludeAsync(id);

            if (invoice != null)
            {
#pragma warning disable
                var barcodeWriter = new BarcodeWriter
                {
                    Format = BarcodeFormat.CODE_128,
                    Options = new EncodingOptions
                    {
                        Height = 80,
                        Width = 325,

                    }
                };
                var count = 0;
                var barcodeContent = $"{invoice.InvoiceId}|{invoice.OrderDate.ToString("yyyy-MM-dd")}|";
                foreach (var detail in invoice.InvoiceDetails)
                {
                    barcodeContent += $"{detail.ProductId}";
                    if (invoice.InvoiceDetails.Count > 1 && count < (invoice.InvoiceDetails.Count - 1))
                    {
                        barcodeContent += $",";
                    }
                    count++;
                }
                var barcodeBitmap = barcodeWriter.Write(barcodeContent);

                byte[] barcodeBytes;
                using (var memoryStream = new MemoryStream())
                {
                    barcodeBitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    barcodeBytes = memoryStream.ToArray();
                }
                return barcodeBytes;
            }
            return null;
        }

        public async Task<byte[]> GenerateInvoiceExcel(int month, int year)
        {
            var isValidYear = await _unitOfWork.Invoices.CheckValidYear(year);
            if (!IsValidMonth(month) || !isValidYear)
            {
                return null;
            }
            var invoices = await _unitOfWork.Invoices.GetInvoicesForMonthAsync(month, year);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Invoices");
                worksheet.Cells[1, 1].Value = "Invoice ID";
                worksheet.Cells[1, 2].Value = "Order Date";
                worksheet.Cells[1, 3].Value = "Customer ID";
                worksheet.Cells[1, 4].Value = "User ID";
                worksheet.Cells[1, 5].Value = "Invoice Type";
                worksheet.Cells[1, 6].Value = "Per Discount";
                worksheet.Cells[1, 7].Value = "Total";

                int row = 2;
                foreach (var invoice in invoices)
                {
                    worksheet.Cells[row, 1].Value = invoice.InvoiceId;
                    worksheet.Cells[row, 2].Value = invoice.OrderDate.ToString("d");
                    worksheet.Cells[row, 3].Value = invoice.CustomerId;
                    worksheet.Cells[row, 4].Value = invoice.UserId;
                    worksheet.Cells[row, 5].Value = invoice.InvoiceType;
                    worksheet.Cells[row, 6].Value = invoice.PerDiscount;
                    worksheet.Cells[row, 7].Value = invoice.Total;
                    row++;
                }

                worksheet.Cells[row, 1].Value = "Total Invoices:";
                worksheet.Cells[row, 2].Value = invoices.Count;
                worksheet.Cells[row, 6].Value = "Total:";
                worksheet.Cells[row, 7].Value = invoices.Sum(i => i.Total);

                worksheet.Cells[1, 1, row, 7].AutoFitColumns();

                return package.GetAsByteArray();
            }

            bool IsValidMonth(int month)
            {
                return month >= 1 && month <= 12;
            }
        }

        public async Task<float> GetMonthlyRevenueAsync( int month , int year)
        {
            var invoices = await _unitOfWork.Invoices.GetMonthlyRevenue(month, year);
           var total = invoices.Sum(invoice => invoice.Total);
            return total;
        }

        public async Task<int> GetTransactionCountAsync( int month, int year)
        {
            var invoices = await _unitOfWork.Invoices.GetMonthlyRevenue(month, year);
            return invoices.Count();
        }

        public async Task<float> GetDailyRevenueAsync(int day, int month, int year)
        {
            var invoices = await _unitOfWork.Invoices.GetDailyRevenue(day,month, year);
            var total = invoices.Sum(invoice => invoice.Total);
            return total;
        }

        public async Task<float> GetMonthlyProfitChangeAsync()
        {
            var currentDate = DateTime.Now;
            var month = currentDate.Month;
            var year = currentDate.Year;

            var currentMonthProfit = await GetMonthlyRevenueAsync(month, year);
            
            var previousMonthProfit = await GetMonthlyRevenueAsync(month-1, year);
;

            if (previousMonthProfit == 0)
            {
                return currentMonthProfit > 0 ? 100 : -100;
            }

            float profitChange = (float)((currentMonthProfit - previousMonthProfit) /(previousMonthProfit * 0.01));
            return profitChange;
        }

        public async Task<List<float>> GetRevenueForEachMonthAsync(DateTime date)
        {
            var monthlyRevenue = new List<float>();

            for (int month = 1; month <= 12; month++)
            {
                var invoices = await _unitOfWork.Invoices.GetInvoicesForMonthAsync(month, date.Year);
                var monthRevenue = invoices.Sum(invoice => invoice.Total);
                monthlyRevenue.Add(monthRevenue);
            }
            return monthlyRevenue;
        }

        public async Task<List<int>> GetQuantiyProductForEachMonthAsync(DateTime date)
        {
            var monthlyProductsList = new List<int>();

            for (int month = 1; month <= 12; month++)
            {
                var quantityProducts = await _unitOfWork.Invoices.GetMonthlyProductSalesAsync(month, date.Year);
                var monthQuantity = quantityProducts.Sum();
                monthlyProductsList.Add(monthQuantity);
            }
            return monthlyProductsList;
        }

        public async Task<byte[]> GenerateWarrantyInvoicePdf(int invoiceId, int warrantyId)
        {
            var invoice = await _unitOfWork.Invoices.GetByIdWithIncludeAsync(invoiceId);
            if (invoice == null)
            {
                throw new Exception($"Invoice with id {invoiceId} not found.");
            }
            var warranty = await _unitOfWork.Warranties.GetEntityByIdAsync(warrantyId);
            if (warranty == null)
            {
                throw new Exception($"Invoice with id {warrantyId} not found.");
            }

            
            MigraDocCore.DocumentObjectModel.Document doc = new();
            Section sec = doc.AddSection();

            var titleStyle = doc.AddStyle("TitleStyle", "Normal");
            titleStyle.Font.Name = "Times New Roman";
            titleStyle.Font.Size = 13;
            titleStyle.Font.Bold = true;

            var arialStyle = doc.AddStyle("ArialStyle", "Normal");
            arialStyle.Font.Name = "Arial";
            arialStyle.Font.Size = 15;

            var propertyStyle = doc.AddStyle("PropertyStyle", "Normal");
            propertyStyle.Font.Name = "Times New Roman";
            propertyStyle.Font.Size = 14;
            propertyStyle.Font.Bold = true;



            Paragraph titleParagraph = sec.AddParagraph("Jewelry Sales System", "TitleStyle");
            titleParagraph.Format.Alignment = ParagraphAlignment.Center;
            titleParagraph.Format.Font.Size = 24;
            sec.AddParagraph();
            var titleType = sec.AddParagraph("Warranty Card", "TitleStyle");
            titleParagraph.Format.Alignment = ParagraphAlignment.Center;
            titleType.Format.Alignment = ParagraphAlignment.Center;
            titleType.Format.Font.Size = 16;

            for (int i = 0; i < 2; i++)
            {
                sec.AddParagraph();
            }

            Table table = new();
            table.Borders.Width = 0;
            Column column = table.AddColumn(MigraDocCore.DocumentObjectModel.Unit.FromCentimeter(5));
            column = table.AddColumn(MigraDocCore.DocumentObjectModel.Unit.FromCentimeter(8));
            Row row = table.AddRow();
            row.Cells[0].AddParagraph($"Customer Name:").Style = "PropertyStyle";
            row.Cells[1].AddParagraph($"{invoice.Customer.FullName}");
            row.Cells[1].Format.Font.Name = "Times New Roman";
            row.Cells[1].Format.Font.Size = 13;

            row = table.AddRow();
            row.Cells[0].AddParagraph($"Phone:").Style = "PropertyStyle";
            row.Cells[1].AddParagraph($"{invoice.Customer.PhoneNumber}");
            row.Cells[1].Format.Font.Name = "Times New Roman";
            row.Cells[1].Format.Font.Size = 13;

            row = table.AddRow();
            row.Cells[0].AddParagraph($"Application Date:").Style = "PropertyStyle";
            row.Cells[1].AddParagraph($"{warranty.StartDate.ToString("yyyy-MM-dd")}");
            row.Cells[1].Format.Font.Name = "Times New Roman";
            row.Cells[1].Format.Font.Size = 13;
            doc.LastSection.Add(table);
            //sec.AddParagraph();
            //sec.AddParagraph($"Products are warranted from now: ");

            for (int i = 0; i < 1; i++)
            {
                sec.AddParagraph();
            }

            Table table2 = new();
            table2.Borders.Width = 0.5;
            Column column2 = table2.AddColumn(MigraDocCore.DocumentObjectModel.Unit.FromCentimeter(3));
            column = table2.AddColumn(MigraDocCore.DocumentObjectModel.Unit.FromCentimeter(8));
            column = table2.AddColumn(MigraDocCore.DocumentObjectModel.Unit.FromCentimeter(5));
            var count = 1;

            Row row2 = table2.AddRow();
            Cell cell = row2.Cells[0];
            cell = row2.Cells[0];
            Paragraph itemParagraph = cell.AddParagraph("Item");
            itemParagraph.Format.Font.Bold = true;
            itemParagraph.Format.Alignment = ParagraphAlignment.Center;
            cell = row2.Cells[1];
            Paragraph productNameParagraph = cell.AddParagraph("Product Name");
            productNameParagraph.Format.Font.Bold = true;
            productNameParagraph.Format.Alignment = ParagraphAlignment.Center;
            cell = row2.Cells[2];
            Paragraph quantityParagraph = cell.AddParagraph("Warranty End Date");
            quantityParagraph.Format.Font.Bold = true;
            quantityParagraph.Format.Alignment = ParagraphAlignment.Center;
           
            foreach (var item in invoice.InvoiceDetails)
            {
                if(item.Product.ProductTypeId == 3)
                {
                    row2 = table2.AddRow();
                    row2.Cells[0].AddParagraph($"{count}");
                    row2.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                    row2.Cells[1].AddParagraph(item.Product.ProductName);
                    row2.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                    row2.Cells[2].AddParagraph(warranty.StartDate.AddMonths(6).ToString("yyyy-MM-dd"));
                    row2.Cells[2].Format.Alignment = ParagraphAlignment.Center;
                    count++;
                }
               
            }

            doc.LastSection.Add(table2);

            for (int i = 0; i < 2; i++)
            {
                sec.AddParagraph();
            }
            Table table3 = new();
            table3.Borders.Width = 0;
            Column column3 = table3.AddColumn(MigraDocCore.DocumentObjectModel.Unit.FromCentimeter(8));
            column3 = table3.AddColumn(MigraDocCore.DocumentObjectModel.Unit.FromCentimeter(8));
            Row row3 = table3.AddRow();
            
            row3.Cells[1].AddParagraph("Salesman");
            //row2.Cells[1].Format.Font.Name = "Times New Roman";
            //row2.Cells[1].Format.Font.Size = 13;
            //row2.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            for (int i = 0; i < 3; i++)
            {
                row3.Cells[1].AddParagraph();
            }
            var saleName = row3.Cells[1].AddParagraph($"{invoice.User.FullName}");
            row3.Cells[1].Format.Font.Name = "Times New Roman";
            row3.Cells[1].Format.Font.Size = 13;
            row3.Cells[1].Format.Alignment = ParagraphAlignment.Center;

            doc.LastSection.Add(table3);
#pragma warning disable
            MigraDocCore.Rendering.PdfDocumentRenderer docRend = new MigraDocCore.Rendering.PdfDocumentRenderer(true);
            docRend.Document = doc;


            docRend.RenderDocument();


            using (MemoryStream ms = new MemoryStream())
            {

                //pdf.Save(ms);
                docRend.PdfDocument.Save(ms);
                return ms.ToArray();
            }

        }

    }
}


