using HospitalSystem.API.Models.Domain;

namespace HospitalSystem.API.Repositories.Interface
{
    public interface IPatientRepository
    {
        //Task<Patient> CreateAsync(Patient patient);
        Task<Patient> CreateAsync(Patient patient, Address address, Contact contact);
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient?> GetById(Guid id);
        Task<Patient?> UpdateAsync(Patient patient);
        Task<Patient?> DeleteAsync(Guid id);
    }
}
