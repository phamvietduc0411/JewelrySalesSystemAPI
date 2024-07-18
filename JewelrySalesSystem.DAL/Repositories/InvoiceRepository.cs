using JewelrySalesSystem.DAL.Common;
using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;
using JewelrySalesSystem.DAL.Interfaces;
using JewelrySalesSystem.DAL.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace JewelrySalesSystem.DAL.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(
            JewelryDbContext context,
            ILogger logger) : base(context, logger)
        {
        }

        public async Task<PaginatedList<Invoice>> PaginationAsync(
            string? invoiceStatus
            , string? invoiceType
            , string? inOrOut
            , string? searchTerm
            , string? sortColumn
            , string? sortOrder
            , bool isActive
            , int page
            , int pageSize)
        {
            IQueryable<Invoice> invoicesQuery = _dbSet.OrderByDescending(i => i.OrderDate)
                                                      .Include(i => i.InvoiceDetails)
                                                            .ThenInclude(i => i.Product)
                                                                  .ThenInclude(i => i.Unit)
                                                      .Include(i => i.Customer)
                                                      .Include(i => i.User)
                                                      .Include(i => i.Warranty);

            if (isActive) invoicesQuery = invoicesQuery.Where(i => i.IsActive);

            if (invoiceStatus != null)
            {
                if (invoiceStatus.Equals("Pending"))
                {
                    invoicesQuery = invoicesQuery.Where(i => i.InvoiceStatus.Equals(invoiceStatus));
                }
                else if (invoiceStatus.Equals("Processing"))
                {
                    invoicesQuery = invoicesQuery.Where(i => i.InvoiceStatus.Equals(invoiceStatus));
                }
                else if (invoiceStatus.Equals("Delivered"))
                {
                    invoicesQuery = invoicesQuery.Where(i => i.InvoiceStatus.Equals(invoiceStatus));
                }
                else if (invoiceStatus.Equals("Cancelled"))
                {
                    invoicesQuery = invoicesQuery.Where(i => i.InvoiceStatus.Equals(invoiceStatus));
                }
                else if (invoiceStatus.Equals("Draft"))
                {
                    invoicesQuery = invoicesQuery.Where(i => i.InvoiceStatus.Equals(invoiceStatus));
                }
                else
                {
                    invoicesQuery = invoicesQuery.Where(i => i.InvoiceStatus.Equals(invoiceStatus));
                }
            }

            if (invoiceType != null)
            {
                if (invoiceType.Equals("Sale"))
                {
                    invoicesQuery = invoicesQuery.Where(i => i.InvoiceType.Equals(invoiceType));
                }
                else if (invoiceType.Equals("Purchase"))
                {
                    invoicesQuery = invoicesQuery.Where(i => i.InvoiceType.Equals(invoiceType));
                }
                else
                {
                    invoicesQuery = invoicesQuery.Where(i => i.InvoiceType.Equals(invoiceType));
                }
            }

            if (inOrOut != null)
            {
                if (inOrOut.Equals("In"))
                {
                    invoicesQuery = invoicesQuery.Where(i => i.InOrOut.Equals(inOrOut));
                }
                else if (inOrOut.Equals("Out"))
                {
                    invoicesQuery = invoicesQuery.Where(i => i.InOrOut.Equals(inOrOut));
                }
                else
                {
                    invoicesQuery = invoicesQuery.Where(i => i.InOrOut.Equals(inOrOut));
                }
            }

            //if (!string.IsNullOrWhiteSpace(searchTerm))
            //{
            //    invoicesQuery = invoicesQuery.Where(c =>
            //        c.FullName.Contains(searchTerm) ||
            //        c.PhoneNumber.Contains(searchTerm) ||
            //        c.Email.Contains(searchTerm));
            //}

            //if (sortOrder?.ToLower() == "desc")
            //{
            //    invoicesQuery = invoicesQuery.OrderByDescending(GetSortProperty(sortColumn));
            //}
            //else
            //{
            //    invoicesQuery = invoicesQuery.OrderBy(GetSortProperty(sortColumn));
            //}

            var invoices = await PaginatedList<Invoice>.CreateAsync(invoicesQuery, page, pageSize);

            return invoices;
        }

        private static Expression<Func<Invoice, object>> GetSortProperty(string? sortColumn)
        => sortColumn?.ToLower() switch
        {
            "date" => invoice => invoice.OrderDate,
            //"dob" => invoice => invoice.DoB,
            _ => invoice => invoice.InvoiceId
        };

        public async Task<Invoice?> GetByIdWithIncludeAsync(int id)
        {
            var result = await _dbSet
                                .Include(i => i.InvoiceDetails)
                                    .ThenInclude(i => i.Product)
                                           .ThenInclude(i => i.Unit)
                                .Include(i => i.Customer)
                                .Include(i => i.User)
                                .Include(i => i.Warranty)
                                .FirstOrDefaultAsync(i => i.InvoiceId == id);

            if (result == null) return null;

            return result;
        }

        public async Task DeleteById(int id)
        {
            var found = await _dbSet.FindAsync(id) ?? throw new Exception($"Invoice with {id} is not found!");
            if (found.IsActive)
            {
                found.IsActive = false;
            }
            else
            {
                found.IsActive = true;
            }
            _dbSet.Update(found);
        }

        public async Task UpdateInvoice(Invoice invoice)
        {
            var existingInvoice = await _dbSet
                .Include(i => i.InvoiceDetails)
                .FirstOrDefaultAsync(i => i.InvoiceId == invoice.InvoiceId) ?? throw new Exception($"Invoice with id {invoice.InvoiceId} not found.");
            existingInvoice.OrderDate = invoice.OrderDate;
            //existingInvoice.CustomerId = invoice.CustomerId;
            //existingInvoice.UserId = invoice.UserId;
            existingInvoice.WarrantyId = invoice.WarrantyId;
            existingInvoice.InvoiceStatus = invoice.InvoiceStatus;
            existingInvoice.Total = invoice.Total;
            existingInvoice.TotalWithDiscount = invoice.Total * (100 - existingInvoice.PerDiscount);
            //existingInvoice.PerDiscount = invoice.PerDiscount;
            //existingInvoice.IsActive = invoice.IsActive;
            //existingInvoice.InvoiceType = invoice.InvoiceType;


            var detailsToRemove = existingInvoice.InvoiceDetails
                .Where(d => !invoice.InvoiceDetails.Any(nd => nd.ProductId == d.ProductId))
                .ToList();

            foreach (var detail in detailsToRemove)
            {
                _context.InvoiceDetails.Remove(detail);
            }

            foreach (var newDetail in invoice.InvoiceDetails)
            {
                var existingDetail = existingInvoice.InvoiceDetails
                    .FirstOrDefault(d => d.ProductId == newDetail.ProductId);

                if (existingDetail != null)
                {
                    existingDetail.ProductPrice = newDetail.ProductPrice;
                    existingDetail.Quantity = newDetail.Quantity;
                }
                else
                {
                    existingInvoice.InvoiceDetails.Add(new InvoiceDetail
                    {
                        ProductId = newDetail.ProductId,
                        ProductPrice = newDetail.ProductPrice,
                        InvoiceId = existingInvoice.InvoiceId,
                        Quantity = newDetail.Quantity,
                    });
                }
            }

            _dbSet.Update(existingInvoice);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeInvoiceStatus(int id)
        {
            var result = await _dbSet.FindAsync(id);

            if (result != null)
            {
                if (result.InvoiceStatus.Equals("Pending"))
                {
                    result.InvoiceStatus = "Processing";
                }
                else if (result.InvoiceStatus.Equals("Processing"))
                {
                    result.InvoiceStatus = "Delivered";
                }
                _dbSet.Update(result);
            }
        }

        public async Task CancelInvoice(int id)
        {
            var result = await _dbSet.FindAsync(id);

            if (result != null)
            {
                result.InvoiceStatus = "Cancelled";
                _dbSet.Update(result);
            }
        }

        public async Task ChangePendingToDraft(int id)
        {
            var result = await _dbSet.FindAsync(id);

            if (result != null)
            {
                if (result.InvoiceStatus.Equals("Draft"))
                {
                    result.InvoiceStatus = "Pending";
                }
                result.InvoiceStatus = "Draft";
                _dbSet.Update(result);
            }
        }

        public async Task<List<Invoice>> GetInvoicesForMonthAsync(int month, int year)
        {
            return await _context.Invoices
                         .Where(i => i.OrderDate.Month == month && i.OrderDate.Year == year)
                         .ToListAsync();
        }

        public async Task<bool> CheckValidYear(int year)
        {
            return await _context.Invoices.AnyAsync(i => i.OrderDate.Year == year);
        }

        public async Task<List<Invoice>> GetMonthlyRevenue(int month, int year)
        {
            try
            {
                return await _context.Invoices
                    .Where(invoice => invoice.OrderDate.Month == month &&
                                      invoice.OrderDate.Year == year)
                    .ToListAsync();
            }
            catch (Exception ex)
            {

                throw new Exception($"Error retrieving invoices: {ex.Message}");
            }
        }

        public async Task<List<Invoice>> GetDailyRevenue(int day, int month, int year)
        {
            try
            {
                return await _context.Invoices
                    .Where(invoice => invoice.OrderDate.Day == day &&
                    invoice.OrderDate.Month == month &&
                                      invoice.OrderDate.Year == year)
                    .ToListAsync();
            }
            catch (Exception ex)
            {

                throw new Exception($"Error retrieving invoices: {ex.Message}");
            }
        }

        //public async Task<List<Invoice>> GetInvoicesByMonthAsync(int month, int year)
        //{
        //    return await _context.Invoices
        //    .Where(invoice => invoice.OrderDate.Month == month &&
        //                      invoice.OrderDate.Year == year)
        //    .ToListAsync();
        //}

        public async Task<List<int>> GetMonthlyProductSalesAsync(int month, int year)
        {
            var invoices = await _context.Invoices
                .Where(i => i.OrderDate.Year == year && i.OrderDate.Month == month && i.InvoiceType == "Sale")
                .ToListAsync();

            var productSales = new List<int>();

            foreach (var invoice in invoices)
            {
                var invoiceDetails = await _context.InvoiceDetails
                    .Where(id => id.InvoiceId == invoice.InvoiceId)
                    .ToListAsync();

                foreach (var detail in invoiceDetails)
                {
                    productSales.Add(detail.Quantity);
                }
            }

            return productSales;
        }


        //public async Task<Invoice> AddPurchaseInvoice(Invoice invoice)
        //{
        //    //if (createPurchaseInvoiceRequest.InvoiceType.Equals("in"))
        //    //{
        //    //    foreach(var item in createPurchaseInvoiceRequest.InvoiceDetails)
        //    //    {
        //    //        var existedProduct = await _unitOfWork.Products.GetEntityByIdAsync(item);
        //    //        if (existedProduct != null)
        //    //        {
        //    //            var existingInvoiceDetails = await _unitOfWork.
        //    //        }
        //    //    }
        //    //}
        //    if (invoice.InvoiceType.Equals("in"))
        //    {
        //        bool hasExistingSaleInvoice = false;
        //        foreach (var item in invoice.InvoiceDetails)
        //        {
        //            //var existingInvoiceDetails = await _dbSet
        //            //.Include(i => i.InvoiceDetails)
        //            //.FirstOrDefaultAsync(i => i.CustomerId == invoice.CustomerId && i.InvoiceType == "Sale" && i.InvoiceStatus == "Delivered" && i.InvoiceDetails.Any(id => id.ProductId == item.ProductId));
        //            //&& i.InvoiceStatus == "Delivered"
        //            var existingInvoices = _dbSet
        //                //.Where(i => i.CustomerId == invoice.CustomerId && i.InvoiceType == "Sale" && i.InvoiceStatus == "Delivered")
        //                //.ToListAsync();
        //                .Include(i => i.InvoiceDetails)
        //                .FirstOrDefault();
        //            var existingInvoiceDetails = await existingInvoices
        //                .Where(i => i.InvoiceDetails.Any(id => id.ProductId == item.ProductId))
        //                .FirstOrDefaultAsync();
        //            if (existingInvoiceDetails != null)
        //            {
        //                hasExistingSaleInvoice = true;
        //            }
        //        } 
        //        if (hasExistingSaleInvoice)
        //        {
        //            _context.Invoices.Add(invoice);
        //            //_dbSet.Add(invoice);
        //            //await _context.SaveChangesAsync();
        //        }
        //    }
        //    else
        //    {
        //        bool isGoldProduct = true;
        //        foreach (var item in invoice.InvoiceDetails)
        //        {
        //            var existingProduct = await _context.Products.FindAsync(item.ProductId);
        //            if(existingProduct!=null)
        //            {
        //                if(existingProduct.ProductTypeId != 2)
        //                {
        //                    isGoldProduct = false;
        //                    break;
        //                }

        //            }
        //        }
        //        if(isGoldProduct)
        //        {
        //            _context.Invoices.Add(invoice);
        //            //await _context.SaveChangesAsync();
        //        }
        //    }
        //    await _context.SaveChangesAsync();

        //    return invoice;
        //}
    }
}
