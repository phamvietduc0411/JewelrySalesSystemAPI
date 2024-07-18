using AutoMapper;
using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.Categories;
using JewelrySalesSystem.BAL.Models.Customers;
using JewelrySalesSystem.DAL.Common;
using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.BAL.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(CreateCustomerRequest createCustomerRequest)
        {
            var customer = await _unitOfWork.Customers.GetCustomerByPhoneAsync(createCustomerRequest.PhoneNumber);

            if (customer == null)
            {
                var result = _unitOfWork.Customers.AddEntity(new DAL.Entities.Customer
                {
                    FullName = createCustomerRequest.CustomerName,
                    PhoneNumber = createCustomerRequest.PhoneNumber,
                    Point = createCustomerRequest.Point
                });

                await _unitOfWork.CompleteAsync();
            }
            else
            {
                throw new Exception($"Customer with {createCustomerRequest.PhoneNumber} has already existed");
            }
        }

        public async Task<GetCustomerResponse?> GetCustomerByNameAsync(string customerName)
        => _mapper.Map<GetCustomerResponse>(await _unitOfWork.Customers.GetCustomerByNameAsync(customerName));

        public async Task<GetCustomerResponse?> GetCustomerByPhoneAsync(string phoneNumber)
        => _mapper.Map<GetCustomerResponse>(await _unitOfWork.Customers.GetCustomerByPhoneAsync(phoneNumber));

        public async Task<PaginatedList<GetCustomerResponse>> PaginationAsync(string? searchTerm, string? sortColumn, string? sortOrder, int page, int pageSize)
        {
            var result = _mapper.Map<PaginatedList<GetCustomerResponse>>(await _unitOfWork.Customers.PaginationAsync(searchTerm, sortColumn, sortOrder, page, pageSize));

            GetDiscount(result.Items);

            return result;
        }

        public async Task UpdateAsync(UpdateCustomerRequest updateCustomerRequest)
        {
            
            var checkDuplicateCustomer = await _unitOfWork.Customers.GetCustomerByPhoneAsync(updateCustomerRequest.PhoneNumber);
            if(checkDuplicateCustomer != null && (checkDuplicateCustomer.CustomerId != updateCustomerRequest.CustomerId))
            {
                throw new Exception($"A customer with the phone number {updateCustomerRequest.PhoneNumber} already exists.");
            }
            else
            {
                _unitOfWork.Customers.UpdateEntity(_mapper.Map<Customer>(updateCustomerRequest));
            }
            await _unitOfWork.CompleteAsync();

        }

        private void GetDiscount(List<GetCustomerResponse> list)
        {
            foreach (var item in list)
            {
                if (item.Point > 49 && item.Point < 99)
                {
                    item.Discount = 5;
                }
                else if (item.Point > 99 && item.Point < 199)
                {
                    item.Discount = 10;
                }
                else if (item.Point > 199)
                {
                    item.Discount = 15;
                }
            }
        }
    }
}
