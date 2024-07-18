using AutoMapper;
using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.MaterialPriceList;
using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.BAL.Services
{
    public class MaterialPriceListService : IMaterialPriceListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MaterialPriceListService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<CreateMaterialPriceList>> AddAsync(List<CreateMaterialPriceList> prices)
        {
            var materialPrices = _mapper.Map<List<MaterialPriceList>>(prices);

            await _unitOfWork.MaterialPrices.AddEntities(materialPrices);            

            await _unitOfWork.CompleteAsync();

            return prices;
        }
    }
}
