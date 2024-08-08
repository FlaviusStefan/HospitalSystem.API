using HospitalSystem.API.Models.Domain;

namespace HospitalSystem.API.Repositories.Interface
{
    public interface IAddressRepository
    {
        Task<Address> CreateAsync(Address address);
        Task<IEnumerable<Address>> GetAllAsync();
        Task<Address?> GetById(Guid id);
        Task<Address?> UpdateAsync(Address address);
        Task<Address?> DeleteAsync(Guid id);
    }
}
