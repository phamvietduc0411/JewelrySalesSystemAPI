using AutoMapper;
using FluentValidation;
using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.Gems;
using JewelrySalesSystem.DAL.Common;
using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.BAL.Services
{
    public class GemService : IGemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateGemRequest> _createValidator;
        private readonly IValidator<UpdateGemRequest> _updateValidator;

        public GemService(
            IUnitOfWork unitOfWork
            , IMapper mapper, IValidator<CreateGemRequest> createValidator, IValidator<UpdateGemRequest> updateValidator)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<PaginatedList<GetGemResponse>> PaginationAsync(
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            bool isActive,
            int page,
            int pageSize)
        {
            var result = _mapper.Map<PaginatedList<GetGemResponse>>(await _unitOfWork.Gems.PaginationAsync(searchTerm, sortColumn, sortOrder, isActive, page, pageSize));

            foreach (var item in result.Items)
            {
                float price = 0;

                var gem = await _unitOfWork.Gems.GetEntityByIdAsync(item.GemId);

                if (gem != null)
                {
                    price = await _unitOfWork.Gems.GetGemPriceAsync(gem);

                    float shapePriceRate = await _unitOfWork.Gems.GetShapePriceRateAsync(gem.ShapeId);

                    price = price * (1 + shapePriceRate / 100);
                }

                item.Price = price;
            }

            return result;
        }


        public async Task<CreateGemRequest> AddAsync(CreateGemRequest createGemRequest)
        {

            var validator = await _createValidator.ValidateAsync(createGemRequest);
            if (!validator.IsValid)
            {
                throw new ValidationException(validator.Errors);
            }

            var gem = new Gem
            {
                GemName = createGemRequest.GemName,
                FeaturedImage = createGemRequest.FeaturedImage,
                OriginId = createGemRequest.OriginId,
                CaratId = createGemRequest.CaratId,
                ColorId = createGemRequest.ColorId,
                ClarityId = createGemRequest.ClarityId,
                CutId = createGemRequest.CutId,
                ShapeId = createGemRequest.ShapeId
            };

            if (createGemRequest.Price == 0)
            {
                float price = await _unitOfWork.Gems.GetGemPriceAsync(gem);

                float shapePriceRate = await _unitOfWork.Gems.GetShapePriceRateAsync(gem.ShapeId);

                var product = new Product
                {
                    ProductName = createGemRequest.GemName,
                    FeaturedImage = createGemRequest.FeaturedImage,
                    ProductPrice = price * (1 + shapePriceRate / 100),
                    ProductTypeId = 4,
                    UnitId = 3,
                    Quantity = 50,
                    CounterId = 1
                };

                var gemResult = _unitOfWork.Gems.AddEntity(gem);

                var produtResult = _unitOfWork.Products.AddEntity(product);              
            }
            else
            {
                var gemPrice = new GemPriceList
                {
                    OriginId = createGemRequest.OriginId,
                    CaratId = createGemRequest.CaratId,
                    ColorId = createGemRequest.ColorId,
                    ClarityId = createGemRequest.ClarityId,
                    CutId = createGemRequest.CutId,
                    Price = createGemRequest.Price
                };

                float shapePriceRate = await _unitOfWork.Gems.GetShapePriceRateAsync(gem.ShapeId);

                var product = new Product
                {
                    ProductName = createGemRequest.GemName,
                    FeaturedImage = createGemRequest.FeaturedImage,
                    ProductPrice = createGemRequest.Price * (1 + shapePriceRate / 100),
                    ProductTypeId = 4,
                    UnitId = 3,
                    Quantity = 50,
                    CounterId = 1
                };

                var gemResult = _unitOfWork.Gems.AddEntity(gem);

                _unitOfWork.Gems.AddGemPrice(gemPrice);

                var produtResult = _unitOfWork.Products.AddEntity(product);             
            }

            await _unitOfWork.CompleteAsync();

            return createGemRequest;
        }

        public async Task UpdateAsync(UpdateGemRequest updateGemRequest)
        {
            var validator = await _updateValidator.ValidateAsync(updateGemRequest);
            if (!validator.IsValid)
            {
                throw new ValidationException(validator.Errors);
            }

            var gem = await _unitOfWork.Gems.GetEntityByIdAsync(updateGemRequest.GemId);


            if (gem != null)
            {
                gem.GemName = updateGemRequest.GemName;
                gem.FeaturedImage = updateGemRequest.FeaturedImage;
                gem.OriginId = updateGemRequest.OriginId;
                gem.CaratId = updateGemRequest.CaratId;
                gem.ColorId = updateGemRequest.ColorId;
                gem.ClarityId = updateGemRequest.ClarityId;
                gem.CutId = updateGemRequest.CutId;
                gem.ShapeId = updateGemRequest.ShapeId;

                float shapePriceRate = await _unitOfWork.Gems.GetShapePriceRateAsync(gem.ShapeId);

                float price = await _unitOfWork.Gems.GetGemPriceAsync(gem);

                var product = await _unitOfWork.Products.GetByNameAsync(gem.GemName);

                if (price == 1)
                {
                    var gemPriceList = new GemPriceList
                    {
                        OriginId = gem.OriginId,
                        CaratId = gem.CaratId,
                        ColorId = gem.ColorId,
                        CutId = gem.CutId,
                        ClarityId = gem.CutId,
                        Price = 10000000
                    };

                    _unitOfWork.Gems.AddGemPrice(gemPriceList);

                    if (product != null)
                    {
                        product.FeaturedImage = updateGemRequest.FeaturedImage;
                        product.ProductName = updateGemRequest.GemName;
                        product.ProductPrice = gemPriceList.Price * (1 + shapePriceRate / 100);
                        _unitOfWork.Products.UpdateEntity(product);
                    }
                }
                else
                {
                    if (product != null)
                    {
                        product.FeaturedImage = updateGemRequest.FeaturedImage;
                        product.ProductName = updateGemRequest.GemName;
                        product.ProductPrice = price * (1 + shapePriceRate / 100);
                        _unitOfWork.Products.UpdateEntity(product);
                    }
                }

                await _unitOfWork.Gems.UpdateGem(gem);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<GetGemResponse?> GetByIdWithIncludeAsync(int id)
        {
            var result = _mapper.Map<GetGemResponse>(await _unitOfWork.Gems.GetByIdWithIncludeAsync(id));

            float price = 0;

            var gem = await _unitOfWork.Gems.GetEntityByIdAsync(id);

            if (gem != null)
            {
                price = await _unitOfWork.Gems.GetGemPriceAsync(gem);

                float shapePriceRate = await _unitOfWork.Gems.GetShapePriceRateAsync(gem.ShapeId);

                price = price * (1 + shapePriceRate / 100);
            }

            result.Price = price;

            return result;
        }

        public async Task<GetGemResponse?> GetGemById(int id) => _mapper.Map<GetGemResponse>(await _unitOfWork.Gems.GetEntityByIdAsync(id));

        public async Task<float> GetGemPriceAsync(GemPriceRequest gemPriceRequest)
        {
            var gemPrice = new GemPriceList
            {
                CaratId = gemPriceRequest.CaratId,
                ClarityId = gemPriceRequest.ClarityId,
                ColorId = gemPriceRequest.ColorId,
                CutId = gemPriceRequest.CutId,
                OriginId = gemPriceRequest.OriginId
            };

            return await _unitOfWork.Gems.GetGemPriceAsync(gemPrice);
        }

        public async Task<List<GetGemPriceResponse>> GetGemPricesAsync() => _mapper.Map<List<GetGemPriceResponse>>(await _unitOfWork.Gems.GetGemPricesAsync());

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Gems.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
