using HospitalSystem.API.Data;
using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.API.Repositories.Implementation
{
    public class ContactRepository : IContactRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ContactRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Contact> CreateAsync(Contact contact)
        {
            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();

            return contact;
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            return await dbContext.Contacts.ToListAsync();
        }

        public async Task<Contact?> GetById(Guid id)
        {
            return await dbContext.Contacts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Contact?> UpdateAsync(Contact contact)
        {
            var existingContact = await dbContext.Contacts.FirstOrDefaultAsync(x => x.Id == contact.Id);

            if(existingContact != null) 
            {
                existingContact.Phone = contact.Phone;
                existingContact.Email = contact.Email;

                await dbContext.SaveChangesAsync();
                return existingContact;
            }

            return null;
        }

        public async Task<Contact?> DeleteAsync(Guid id)
        {
            var existingContact = await dbContext.Contacts.FirstOrDefaultAsync(x => x.Id == id);
            if(existingContact is null) 
            {
                return null;
            }

            dbContext.Contacts.Remove(existingContact);
            await dbContext.SaveChangesAsync();
            return existingContact;
        }
    }
}
