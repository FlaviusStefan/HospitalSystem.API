using HospitalSystem.API.Models.Domain;

namespace HospitalSystem.API.Repositories.Interface
{
    public interface IMedicationRepository
    {
        Task<Medication> CreateAsync(Medication medication);
        Task<IEnumerable<Medication>> GetAllAsync();
        Task<Medication?> GetById(Guid id);
        Task<Medication?> UpdateAsync(Medication medication);
        Task<Medication?> DeleteAsync(Guid id);
    }
}
