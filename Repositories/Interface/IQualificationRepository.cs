using HospitalSystem.API.Models.Domain;

namespace HospitalSystem.API.Repositories.Interface
{
    public interface IQualificationRepository
    {
        Task<IEnumerable<Qualification>> GetAllAsync();
        Task<Qualification?> GetById(Guid id);
    }
}
