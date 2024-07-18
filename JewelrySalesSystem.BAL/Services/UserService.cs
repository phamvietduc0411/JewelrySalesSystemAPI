using AutoMapper;
using FluentValidation;
using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.Users;
using JewelrySalesSystem.DAL.Common;
using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;
using OfficeOpenXml;

namespace JewelrySalesSystem.BAL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUserRequest> _createUserValidator;
        private readonly IValidator<UpdateUserRequest> _updateUserValidator;

        public UserService(
            IUnitOfWork unitOfWork
            , IMapper mapper, IValidator<CreateUserRequest> validator, IValidator<UpdateUserRequest> updateUserValidator)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _createUserValidator = validator;
            _updateUserValidator = updateUserValidator;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public async Task<User?> LoginAsync(string userName, string passWord)
            => await _unitOfWork.Users.LoginAsync(userName, passWord);

        public async Task<PaginatedList<GetUserResponse>> PaginationAsync(
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            bool isActive,
            int page,
            int pageSize)
        {
            var result = _mapper.Map<PaginatedList<GetUserResponse>>(await _unitOfWork.Users.PaginationAsync(searchTerm, sortColumn, sortOrder, isActive, page, pageSize));

            foreach (var item in result.Items)
            {
                if (item.Counter == null)
                {
                    item.Counter = "Unassigned";
                }
            }

            return result;
        }


        public async Task<CreateUserRequest> AddAsync(CreateUserRequest createUserRequest)
        {

            var validationResult = await _createUserValidator.ValidateAsync(createUserRequest);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var createUser = _mapper.Map<User>(createUserRequest);
            _unitOfWork.Users.AddEntity(createUser);
            await _unitOfWork.CompleteAsync();

            return createUserRequest;
        }

        public async Task UpdateAsync(UpdateUserRequest updateUserRequest)
        {
            var validationResult = await _updateUserValidator.ValidateAsync(updateUserRequest);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            _unitOfWork.Users.UpdateEntity(_mapper.Map<User>(updateUserRequest));
            await _unitOfWork.CompleteAsync();
        }

        public async Task<GetUserResponse?> GetByIdWithIncludeAsync(int id) => _mapper.Map<GetUserResponse>(await _unitOfWork.Users.GetByIdWithIncludeAsync(id));

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Users.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<GetUserResponse?> GetByIdAsync(int id) => _mapper.Map<GetUserResponse>(await _unitOfWork.Users.GetEntityByIdAsync(id));

        public async Task AssignUserToCounter(int userId, int counterId)
        {
            await _unitOfWork.Users.AssignUserToCounter(userId, counterId);
            await _unitOfWork.CompleteAsync();
        }
        public async Task<byte[]> GetEmployeeRevenue(int month, int year)
        {
            var allUsers = await _unitOfWork.Users.GetAllEntitiesAsync();
            var invoices = await _unitOfWork.Invoices.GetInvoicesForMonthAsync(month, year);

            var groupedInvoices = invoices.GroupBy(i => i.UserId)
                                          .Select(g => new
                                          {
                                              UserId = g.Key,
                                              Revenue = g.Sum(i => i.Total)
                                          });

            var userRevenue = allUsers.Select(user => new
            {
                UserName = user.UserName,
                UserId = user.UserId,
                Revenue = groupedInvoices.FirstOrDefault(g => g.UserId == user.UserId)?.Revenue ?? 0
            });

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Employee Revenue");

            // Set the headers
            worksheet.Cells[1, 1].Value = "EmployeeId";
            worksheet.Cells[1, 2].Value = "FullName";
            worksheet.Cells[1, 3].Value = "Revenue";

            // Add data rows
            var row = 2;
            foreach (var ur in userRevenue)
            {
                worksheet.Cells[row, 1].Value = ur.UserId;
                worksheet.Cells[row, 2].Value = ur.UserName;
                worksheet.Cells[row, 3].Value = ur.Revenue;
                row++;
            }

            // Convert the package to a byte array
            var stream = new MemoryStream();
            package.SaveAs(stream);
            return stream.ToArray();
        }

        public async Task<List<GetUserResponse>> GetUsersWithRoleSeller(int roleId) 
        { 
            var result = _mapper.Map<List<GetUserResponse>>(await _unitOfWork.Users.GetUsersWithRoleSeller(roleId));

            foreach (var item in result)
            {
                if (item.Counter == null)
                {
                    item.Counter = "Unassigned";
                }
            }

            return result;
        }

        public async Task<List<GetUserResponse>> GetUsersWithRoleCashier(int roleId)
        {
            var result = _mapper.Map<List<GetUserResponse>>(await _unitOfWork.Users.GetUsersWithRoleCashier(roleId));

            foreach (var item in result)
            {
                if (item.Counter == null)
                {
                    item.Counter = "Unassigned";
                }
            }

            return result;
        }
    }
}
