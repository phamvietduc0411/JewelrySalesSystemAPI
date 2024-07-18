using JewelrySalesSystem.BAL.Models.Genders;

namespace JewelrySalesSystem.BAL.Interfaces
{
    public interface IGenderService
    {
        Task<CreateGenderRequest> AddGender(CreateGenderRequest gender);

        Task<GetGenderResponse?> GetByIdAsync(int id);

        Task<List<GetGenderResponse>> GetAllAsync();
    }
}
