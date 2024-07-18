using AutoMapper;
using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.Colors;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.BAL.Services
{
    public class ColorService : IColorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ColorService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetColorResponse>> GetAllAsync() => _mapper.Map<List<GetColorResponse>>(await _unitOfWork.Colors.GetAllAsync());

        public async Task<GetColorResponse?> GetByIdAsync(int id) => _mapper.Map<GetColorResponse>(await _unitOfWork.Colors.GetEntityByIdAsync(id));
    }
}
