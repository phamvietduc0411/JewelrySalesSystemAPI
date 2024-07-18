using AutoMapper;
using JewelrySalesSystem.BAL.Models.Carats;
using JewelrySalesSystem.BAL.Models.Categories;
using JewelrySalesSystem.BAL.Models.Clarities;
using JewelrySalesSystem.BAL.Models.Colors;
using JewelrySalesSystem.BAL.Models.Counters;
using JewelrySalesSystem.BAL.Models.CounterTypes;
using JewelrySalesSystem.BAL.Models.Customers;
using JewelrySalesSystem.BAL.Models.Cuts;
using JewelrySalesSystem.BAL.Models.Gems;
using JewelrySalesSystem.BAL.Models.Genders;
using JewelrySalesSystem.BAL.Models.Invoices;
using JewelrySalesSystem.BAL.Models.MaterialPriceList;
using JewelrySalesSystem.BAL.Models.Materials;
using JewelrySalesSystem.BAL.Models.Origins;
using JewelrySalesSystem.BAL.Models.Products;
using JewelrySalesSystem.BAL.Models.ProductTypes;
using JewelrySalesSystem.BAL.Models.Roles;
using JewelrySalesSystem.BAL.Models.Shapes;
using JewelrySalesSystem.BAL.Models.Users;
using JewelrySalesSystem.BAL.Models.Warranties;
using JewelrySalesSystem.DAL.Common;
using JewelrySalesSystem.DAL.Entities;

namespace JewelrySalesSystem.BAL.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region Role
            CreateMap<CreateRoleRequest, Role>();
            CreateMap<Role, GetRoleResponse>() ;
            #endregion

            #region Gender
            CreateMap<Gender, GetGenderResponse>();

            CreateMap<CreateGenderRequest, Gender>();
            #endregion

            #region User
            CreateMap<User, GetUserResponse>()
                .ForMember(u => u.Role, u => u.MapFrom(u => u.Role.RoleName))
                .ForMember(u => u.Counter, u => u.MapFrom(u => u.Counter.CounterName));

            CreateMap<PaginatedList<User>, PaginatedList<GetUserResponse>>();

            CreateMap<CreateUserRequest, User>();

            CreateMap<UpdateUserRequest, User>();
            #endregion

            #region Product
            CreateMap<Product, GetProductResponse>()
                .ForMember(p => p.Materials, p => p.MapFrom(p => p.ProductMaterials
                .Select(y => y.Material).ToList()))

                .ForMember(p => p.Gems, p => p.MapFrom(p => p.ProductGems
                .Select(y => y.Gem).ToList()))

                .ForMember(p => p.Category, p => p.MapFrom(p => p.Category.CategoryName))

                .ForMember(p => p.ProductType, p => p.MapFrom(p => p.ProductType.Name))

                .ForMember(p => p.Gender, p => p.MapFrom(p => p.Gender.GenderName))

                .ForMember(p => p.Counter, p => p.MapFrom(p => p.Counter.CounterName))

                .ForMember(p => p.Unit, p => p.MapFrom(p => p.Unit.Name));

            CreateMap<Product, GetGemProductResponse>()
                .ForMember(p => p.ProductType, p => p.MapFrom(p => p.ProductType.Name))
                .ForMember(p => p.Counter, p => p.MapFrom(p => p.Counter.CounterName))
                .ForMember(p => p.Unit, p => p.MapFrom(p => p.Unit.Name));

            CreateMap<Product, GetMaterialProductResponse>()
                .ForMember(p => p.ProductType, p => p.MapFrom(p => p.ProductType.Name))
                .ForMember(p => p.Counter, p => p.MapFrom(p => p.Counter.CounterName))
                .ForMember(p => p.Unit, p => p.MapFrom(p => p.Unit.Name));

            CreateMap<Product, ProductCategoryResponse>()
                .ForMember(p => p.ProductType, p => p.MapFrom(p => p.ProductType.Name))

                .ForMember(p => p.Gender, p => p.MapFrom(p => p.Gender.GenderName));


            CreateMap<PaginatedList<Product>, PaginatedList<GetProductResponse>>();

            CreateMap<PaginatedList<Product>, PaginatedList<GetGemProductResponse>>();

            CreateMap<PaginatedList<Product>, PaginatedList<GetMaterialProductResponse>>();

            CreateMap<Product, ProductAndProductTypeResponse>()
                .ForMember(p => p.Category, p => p.MapFrom(p => p.Category.CategoryName))
                .ForMember(p => p.Gender, p => p.MapFrom(p => p.Gender.GenderName));
            #endregion

            #region Gem               
            CreateMap<Gem, GetGemResponse>()
                .ForMember(g => g.Cut, g => g.MapFrom(g => g.Cut.Level))

                .ForMember(g => g.Origin, g => g.MapFrom(g => g.Origin.Name))

                .ForMember(g => g.Clarity, g => g.MapFrom(g => g.Clarity.Level))

                .ForMember(g => g.Color, g => g.MapFrom(g => g.Color.Name))

                .ForMember(g => g.Shape, g => g.MapFrom(g => g.Shape.Name))

                .ForMember(g => g.Carat, g => g.MapFrom(g => g.Carat.Weight / 100));

            CreateMap<PaginatedList<Gem>, PaginatedList<GetGemResponse>>();

            CreateMap<GemPriceList, GetGemPriceResponse>();

            CreateMap<UpdateGemRequest, Gem>().ReverseMap();
            #endregion

            #region Material
            CreateMap<Material, MaterialItem>()
                .ForMember(m => m.MaterialPrice, m => m.MapFrom(m => m.MaterialPrices.SingleOrDefault()));

            CreateMap<Material, GetMaterialResponse>()
                .ForMember(m => m.MaterialPrice, m => m.MapFrom(m => m.MaterialPrices.SingleOrDefault()));

            CreateMap<PaginatedList<Material>, PaginatedList<GetMaterialResponse>>();

            CreateMap<MaterialPriceList, MaterialPrice>();

            CreateMap<CreateMaterialPriceList, MaterialPriceList>();
            #endregion

            #region Invoice
            CreateMap<Invoice, GetInvoiceResponse>()
                .ForMember(i => i.Items, i => i.MapFrom(i => i.InvoiceDetails))

                //.ForMember(i => i.Total, i => i.MapFrom(i => i.InvoiceDetails
                //.Sum(i => i.ProductPrice)))

                .ForMember(i => i.CustomerName, i => i.MapFrom(i => i.Customer.FullName))

                .ForMember(i => i.UserName, i => i.MapFrom(i => i.User.UserName))

                .ForMember(i => i.PhoneNumber, i => i.MapFrom(i => i.User.PhoneNumber))

                .ForMember(i => i.Warranty, i => i.MapFrom(i => i.Warranty.Description));

            CreateMap<PaginatedList<Invoice>, PaginatedList<GetInvoiceResponse>>();

            CreateMap<InvoiceDetail, InvoiceItem>()
                .ForMember(i => i.ProductName, i => i.MapFrom(i => i.Product.ProductName))
                .ForMember(i => i.Unit, i => i.MapFrom(i => i.Product.Unit.Name));
            #endregion

            #region Warranty
            CreateMap<UpdateWarrantyRequest, Warranty>().ReverseMap();

            CreateMap<GetWarrantyResponse, Warranty>().ReverseMap();

            CreateMap<PaginatedList<Warranty>, PaginatedList<GetWarrantyResponse>>();

            CreateMap<CreateWarrantyRequest, Warranty>().ReverseMap();
            #endregion

            #region Category
            CreateMap<CreateCategoryRequest, Category>();

            CreateMap<UpdateCategoryRequest, Category>();

            CreateMap<Category, GetCategoryResponse>()
                .ForMember(c => c.Products, c => c.MapFrom(c => c.Products));

            CreateMap<Category, GetRawCategoryResponse>();
            #endregion

            #region ProductType
            CreateMap<ProductType, GetProductTypeResponse>();

            CreateMap<ProductType, ProductTypeIdCollectionResponse>()
                .ForMember(pt => pt.Products, pt => pt.MapFrom(pt => pt.Products));

            CreateMap<ProductType, CreateProductTypeRequest>().ReverseMap();

            CreateMap<ProductType, UpdateTypeRequest>().ReverseMap();
            #endregion

            #region Color
            CreateMap<Color, GetColorResponse>();
            #endregion

            #region Customer
            CreateMap<PaginatedList<Customer>, PaginatedList<GetCustomerResponse>>();

            CreateMap<Customer, GetCustomerResponse>();
            CreateMap<UpdateCustomerRequest, Customer>().ReverseMap();

            #endregion

            #region Carat
            CreateMap<Carat, GetCaratWeightResponse>()
                .ForMember(c => c.Weight, c => c.MapFrom(c => c.Weight / 100));
            #endregion

            #region Clarity
            CreateMap<Clarity, GetClarityResponse>();
            #endregion

            #region Cut
            CreateMap<Cut, GetCutResponse>();
            #endregion

            #region Origin
            CreateMap<Origin, GetOriginResponse>();
            #endregion

            #region Shape
            CreateMap<Shape, GetShapeResponse>()
                .ForMember(g => g.PriceRate, g => g.MapFrom(g => g.PriceRate / 100));
            #endregion

            #region Counter
            CreateMap<Counter, GetCounterResponse>()
                .ForMember(c => c.UserName, c => c.MapFrom(c => c.User.FullName))
                .ForMember(c => c.CounterType, c => c.MapFrom(c => c.CounterType.CounterTypeName));

            CreateMap<PaginatedList<Counter>, PaginatedList<GetCounterResponse>>();
            #endregion

            #region Counter Type
            CreateMap<CounterType, GetCounterTypeResponse>();
            #endregion
        }
    }
}
