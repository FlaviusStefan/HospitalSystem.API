using HospitalSystem.API.Models.Domain;

namespace HospitalSystem.API.Repositories.Interface
{
    public interface ILabTestRepository
    {
        Task<LabTest> CreateAsync(LabTest labTest);
        Task<IEnumerable<LabTest>> GetAllAsync();
        Task<LabTest?> GetById(Guid id);
        Task<LabTest?> UpdateAsync(LabTest labTest);
        Task<LabTest?> DeleteAsync(Guid id);
    }
}
