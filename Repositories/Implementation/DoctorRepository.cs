using HospitalSystem.API.Data;
using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.API.Repositories.Implementation
{
    public class DoctorRepository : IDoctorRepository
    {

        private readonly ApplicationDbContext dbContext;
        private readonly IAddressRepository addressRepository;
        private readonly IContactRepository contactRepository;

        public DoctorRepository(ApplicationDbContext dbContext, IAddressRepository addressRepository, IContactRepository contactRepository)
        {
            this.dbContext = dbContext;
            this.addressRepository = addressRepository;
            this.contactRepository = contactRepository;
        }

        public async Task<Doctor> CreateAsync(Doctor doctor, Address address, Contact contact)
        {
            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    
                    if (address != null)
                    {
                        address.Id = Guid.NewGuid();
                        await addressRepository.CreateAsync(address);

                        doctor.AddressId = address.Id;
                    }

                    if (contact != null)
                    {
                        contact.Id = Guid.NewGuid();
                        await contactRepository.CreateAsync(contact);

                        doctor.ContactId = contact.Id;
                    }

                    dbContext.Doctors.Add(doctor);
                    await dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return doctor;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    Console.WriteLine($"Error creating doctor: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                        Console.WriteLine($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                    }

                    throw; // Re-throw the exception to be handled at a higher level
                }
            }
        }




        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await dbContext.Doctors
                .Include(d => d.Address) 
                .Include(d => d.Contact)
                .Include(d => d.DoctorSpecializations)
                .Include(d => d.Qualifications)
                .Include(d => d.DoctorHospitals)
                .Include(d => d.Appointments)
                .ToListAsync();

        }

        public async Task<Doctor?> GetById(Guid id)
        {
            return await dbContext.Doctors
                .Include(d => d.Address)
                .Include(d => d.Contact)
                .Include(d => d.DoctorSpecializations)
                .Include(d => d.Qualifications)
                .Include(d => d.DoctorHospitals)
                .Include(d => d.Appointments)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        
        public async Task<Doctor?> UpdateAsync(Doctor doctor)
        {
            var existingDoctor = await dbContext.Doctors.FirstOrDefaultAsync(x => x.Id == doctor.Id);

            if (existingDoctor != null)
            {
                dbContext.Entry(existingDoctor).CurrentValues.SetValues(doctor);
                await dbContext.SaveChangesAsync();
                return doctor;
            }

            return null;
        }

        public async Task<Doctor?> DeleteAsync(Guid id)
        {
            // Fetch the doctor with related entities
            var existingDoctor = await dbContext.Doctors
                .Include(d => d.Address)
                .Include(d => d.Contact)
                .Include(d => d.DoctorSpecializations)
                .Include(d => d.Qualifications)
                .Include(d => d.DoctorHospitals)
                .Include(d => d.Appointments)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingDoctor is null)
            {
                return null;
            }

            // Remove related entities
            if (existingDoctor.Address != null)
            {
                // Address should be removed from Doctors first if it's required
                existingDoctor.Address.Doctors.Remove(existingDoctor);
                if (!existingDoctor.Address.Doctors.Any()) // Remove Address if no other Doctors reference it
                {
                    dbContext.Addresses.Remove(existingDoctor.Address);
                }
            }

            if (existingDoctor.Contact != null)
            {
                // Contact should be removed from Doctors first if it's required
                existingDoctor.Contact.Doctors.Remove(existingDoctor);
                if (!existingDoctor.Contact.Doctors.Any()) // Remove Contact if no other Doctors reference it
                {
                    dbContext.Contacts.Remove(existingDoctor.Contact);
                }
            }

            // Remove related entities
            dbContext.DoctorSpecializations.RemoveRange(existingDoctor.DoctorSpecializations);
            dbContext.Qualifications.RemoveRange(existingDoctor.Qualifications);
            dbContext.DoctorHospitals.RemoveRange(existingDoctor.DoctorHospitals);
            dbContext.Appointments.RemoveRange(existingDoctor.Appointments);

            // Remove the doctor
            dbContext.Doctors.Remove(existingDoctor);

            // Save changes
            await dbContext.SaveChangesAsync();

            return existingDoctor;
        }






    }
}
