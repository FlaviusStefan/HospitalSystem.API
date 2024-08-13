using HospitalSystem.API.Models.Domain;

namespace HospitalSystem.API.Repositories.Interface
{
    public interface IDoctorRepository
    {
        //Task<Doctor> CreateAsync(Doctor doctor);
        Task<Doctor> CreateAsync(Doctor doctor,Address address, Contact contact);

        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<Doctor?> GetById(Guid id);
        Task<Doctor?> UpdateAsync(Doctor doctor);
        Task<Doctor?> DeleteAsync(Guid id);
    }
}
