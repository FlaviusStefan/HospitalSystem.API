using HospitalSystem.API.Models.Domain;

namespace HospitalSystem.API.Repositories.Interface
{
    public interface IHospitalRepository
    {
        Task<Hospital> CreateAsync(Hospital hospital);
        Task<IEnumerable<Hospital>> GetAllAsync();
        Task<Hospital?> GetById(Guid id);
        Task<Hospital?> UpdateAsync(Hospital hospital);
        Task<Hospital?> DeleteAsync(Guid id);
        Task<Hospital?> DeleteWithAddressAndContactAsync(Guid id);
    }
}
