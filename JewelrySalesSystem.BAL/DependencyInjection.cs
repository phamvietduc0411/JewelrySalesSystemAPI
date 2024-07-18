using FluentValidation;
using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Mappings;
using JewelrySalesSystem.BAL.Models.Categories;
using JewelrySalesSystem.BAL.Models.Gems;
using JewelrySalesSystem.BAL.Models.MaterialPriceList;
using JewelrySalesSystem.BAL.Models.Materials;
using JewelrySalesSystem.BAL.Models.Products;
using JewelrySalesSystem.BAL.Models.ProductTypes;
using JewelrySalesSystem.BAL.Models.Roles;
using JewelrySalesSystem.BAL.Models.Users;
using JewelrySalesSystem.BAL.Services;
using JewelrySalesSystem.BAL.Validators.Category;
using JewelrySalesSystem.BAL.Validators.Gems;
using JewelrySalesSystem.BAL.Validators.Materials;
using JewelrySalesSystem.BAL.Validators.Product;
using JewelrySalesSystem.BAL.Validators.ProductType;
using JewelrySalesSystem.BAL.Validators.Role;
using JewelrySalesSystem.BAL.Validators.User;
using JewelrySalesSystem.DAL.Interfaces;
using JewelrySalesSystem.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace JewelrySalesSystem.BAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureBALServices(
            this IServiceCollection services)
        {
            services.AddScoped<IWarrantyService, WarrantyService>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IMaterialService, MaterialService>();

            services.AddScoped<IInvoiceService, InvoiceService>();

            services.AddScoped<IGemService, GemService>();

            services.AddScoped<ICategoryService, CategoryService>();

            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IGenderService, GenderService>();

            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IMaterialPriceListService, MaterialPriceListService>();

            services.AddScoped<IProductTypeService, ProductTypeService>();

            services.AddScoped<IColorService, ColorService>();

            services.AddScoped<ICustomerService, CustomerService>();

            services.AddScoped<ICaratService, CaratService>();

            services.AddScoped<IClarityService, ClarityService>();

            services.AddScoped<ICutService, CutService>();

            services.AddScoped<IOriginService, OriginService>();

            services.AddScoped<IShapeService, ShapeService>();

            services.AddScoped<ICounterService, CounterService>();

            services.AddScoped<ICounterTypeService, CounterTypeService>();

            services.AddScoped<IVnPayService, VnPayService>();

            //Validate

            services.AddScoped<IValidator<CreateUserRequest>, CreateUserValidator>();

            services.AddScoped<IValidator<UpdateUserRequest>, UpdateUserValidator>();

            services.AddScoped<IValidator<CreateProductTypeRequest>, CreateProductTypeValidator>();

            services.AddScoped<IValidator<CreateRoleRequest>, AddRoleValidator>();

            services.AddScoped<IValidator<UpdateTypeRequest>, UpdateProductTypeValidator>();

            services.AddScoped<IValidator<CreateCategoryRequest>, CreateCategoryValidator>();

            services.AddScoped<IValidator<UpdateCategoryRequest>, UpdateCategoryValidator>();

            //changes here
            services.AddScoped<IValidator<CreateMaterialRequest>, AddMaterialsValidation>();

            services.AddScoped<IValidator<MaterialPrice>, MaterialPriceValidation>();

            services.AddScoped<IValidator<CreateProductRequest>, AddProductValidation>();

            services.AddScoped<IValidator<UpdateProductRequest>, UpdateProductValidation>();

            services.AddScoped<IValidator<CreateGemRequest>, CreateGemRequestValidator>();

            services.AddScoped<IValidator<UpdateGemRequest>, UpdateGemRequestValidator>();

            return services;
        }
    }
}
