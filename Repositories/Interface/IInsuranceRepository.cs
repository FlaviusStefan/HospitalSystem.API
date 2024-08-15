using HospitalSystem.API.Models.Domain;

namespace HospitalSystem.API.Repositories.Interface
{
    public interface IInsuranceRepository
    {
        Task<Insurance> CreateAsync(Insurance insurance);
        Task<IEnumerable<Insurance>> GetAllAsync();
        Task<Insurance?> GetById(Guid id);
        Task<Insurance?> UpdateAsync(Insurance insurance);
        Task<Insurance?> DeleteAsync(Guid id);
    }
}
