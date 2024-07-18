using AutoMapper;
using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.CounterTypes;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.BAL.Services
{
    public class CounterTypeService : ICounterTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CounterTypeService
            (IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<GetCounterTypeResponse>> GetAllAsync() => _mapper.Map<List<GetCounterTypeResponse>>(await _unitOfWork.CounterTypes.GetAllEntitiesAsync());
    }
}
