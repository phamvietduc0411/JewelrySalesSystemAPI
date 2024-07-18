using AutoMapper;
using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.Counters;
using JewelrySalesSystem.DAL.Common;
using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.BAL.Services
{
    public class CounterService : ICounterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CounterService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(CreateCounterRequest request)
        {
            var counter = new Counter
            {
                CounterName = request.CounterName,
                CounterTypeId = request.CounterTypeId,
            };

            var result = _unitOfWork.Counters.AddEntity(counter);

            await _unitOfWork.CompleteAsync();
        }

        public async Task AssignStaffToCounterAsync(int counterId, int userId)
        {
            var user = await _unitOfWork.Users.GetEntityByIdAsync(userId) ?? throw new Exception($"User with {userId} does not exist");

            var counter = await _unitOfWork.Counters.GetEntityByIdAsync(counterId) ?? throw new Exception($"Counter with {counterId} does not exist");

            counter.User = user;

            _unitOfWork.Counters.UpdateEntity(counter);

            await _unitOfWork.CompleteAsync();
        }

        public async Task UnassignCounterAsync(int id)
        {
            var counter = await _unitOfWork.Counters.GetByIdWithIncludeAsync(id);

            if (counter != null)
            {
                if (counter.User != null)
                {
                    var user = await _unitOfWork.Users.GetEntityByIdAsync(counter.User.UserId);

                    if (user != null)
                    {
                        user.CounterId = null;

                        _unitOfWork.Users.UpdateEntity(user);

                        await _unitOfWork.CompleteAsync();
                    }
                }
            }
            else
            {
                throw new Exception($"Counter with {id} does not exist");
            }
        }

        public async Task ChangeStatusAsync(int id)
        {
            var result = await _unitOfWork.Counters.GetEntityByIdAsync(id);

            if (result == null)
            {
                throw new Exception($"Counter with {id} does not exist");
            }
            else
            {
                if (result.IsActive) result.IsActive = false;

                result.IsActive = true;

                _unitOfWork.Counters.UpdateEntity(result);

                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<PaginatedList<GetCounterResponse>> PaginationAsync(string? searchTerm, string? sortColumn, string? sortOrder, bool isActive, int page, int pageSize)
        {
            var result = _mapper.Map<PaginatedList<GetCounterResponse>>(await _unitOfWork.Counters.PaginationAsync(searchTerm, sortColumn, sortOrder, isActive, page, pageSize));

            foreach (var item in result.Items)
            {
                if (item.UserName == null)
                    item.UserName = "Unassigned";
            }

            return result;
        }

        public async Task UpdateAsync(UpdateCounterRequest request)
        {
            var result = await _unitOfWork.Counters.GetEntityByIdAsync(request.CounterId);

            if (result == null)
            {
                throw new Exception($"Counter with {request.CounterId} does not exist");
            }
            else
            {
                result.CounterName = request.CounterName;
                result.CounterTypeId = request.CounterTypeId;

                _unitOfWork.Counters.UpdateEntity(result);

                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<List<GetAllCounterName>> GetAllCounterIdAndNamesAsync()
        {
            var counters = await _unitOfWork.Counters.GetAllEntitiesAsync();
            return counters.Select(c => new GetAllCounterName
            {
                CounterId = c.CounterId,
                CounterName = c.CounterName
            }).ToList();
        }
    }
}
