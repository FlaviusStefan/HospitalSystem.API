using HospitalSystem.API.Data;
using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.API.Repositories.Implementation
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext dbContext;

        public AddressRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Address> CreateAsync(Address address)
        {
            await dbContext.Addresses.AddAsync(address);
            await dbContext.SaveChangesAsync();

            return address;
        }       

        public async Task<IEnumerable<Address>> GetAllAsync()
        {
            return await dbContext.Addresses.ToListAsync();
        }

        public async Task<Address?> GetById(Guid id)
        {
            return await dbContext.Addresses.FirstOrDefaultAsync(x=> x.Id == id);
        }

        public async Task<Address?> UpdateAsync(Address address)
        {

            var existingAddress = await dbContext.Addresses.FirstOrDefaultAsync(x => x.Id == address.Id);

            if (existingAddress != null)
            {
                existingAddress.Street = address.Street;
                existingAddress.StreetNr = address.StreetNr;
                existingAddress.City = address.City;
                existingAddress.State = address.State;
                existingAddress.Country = address.Country;
                existingAddress.PostalCode = address.PostalCode;

                await dbContext.SaveChangesAsync();
                return existingAddress;
            }


            return null;
        }


        public async Task<Address?> DeleteAsync(Guid id)
        {
            var existingAddress = await dbContext.Addresses.FirstOrDefaultAsync(x => x.Id == id);

            if (existingAddress is null)
            {
                return null;
            }

            dbContext.Addresses.Remove(existingAddress);
            await dbContext.SaveChangesAsync();
            return existingAddress;
        }
    }
}
