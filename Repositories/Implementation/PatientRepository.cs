using HospitalSystem.API.Data;
using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.API.Repositories.Implementation
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IAddressRepository addressRepository;
        private readonly IContactRepository contactRepository;

        public PatientRepository(ApplicationDbContext dbContext, IAddressRepository addressRepository, IContactRepository contactRepository)
        {
            this.dbContext = dbContext;
            this.addressRepository = addressRepository;
            this.contactRepository = contactRepository;
        }
        public async Task<Patient> CreateAsync(Patient patient, Address address, Contact contact)
        {
            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // Save address first if provided
                    if (address != null)
                    {
                        address.Id = Guid.NewGuid();
                        await addressRepository.CreateAsync(address);

                        // Now associate the address with the doctor
                        patient.AddressId = address.Id;
                    }

                    // Save contact next if provided
                    if (contact != null)
                    {
                        contact.Id = Guid.NewGuid();
                        await contactRepository.CreateAsync(contact);

                        // Now associate the contact with the doctor
                        patient.ContactId = contact.Id;
                    }

                    // Save the doctor last
                    dbContext.Patients.Add(patient);
                    await dbContext.SaveChangesAsync();

                    // Commit transaction
                    await transaction.CommitAsync();

                    return patient;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    // Log the exception in detail
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

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await dbContext.Patients
                .Include(p => p.Address)
                .Include(p => p.Contact)
                .Include(p => p.PrimaryCarePhysician)
                .Include(p => p.MedicalFiles)
                .Include(p => p.CurrentMedications)
                .Include(p => p.Insurances)
                .Include(p => p.Appointments)
                .Include(p => p.LabAnalyses)
                .ToListAsync();
        }

        public async Task<Patient?> GetById(Guid id)
        {
            return await dbContext.Patients
                .Include(p => p.Address)
                .Include(p => p.Contact)
                .Include(p => p.PrimaryCarePhysician)
                .Include(p => p.MedicalFiles)
                .Include(p => p.CurrentMedications)
                .Include(p => p.Insurances)
                .Include(p => p.Appointments)
                .Include(p => p.LabAnalyses)
                .FirstOrDefaultAsync();
        }

        public async Task<Patient?> UpdateAsync(Patient patient)
        {
            var existingPatient = await dbContext.Patients.FirstOrDefaultAsync(x => x.Id == patient.Id);

            if(existingPatient != null) 
            {
                dbContext.Entry(existingPatient).CurrentValues.SetValues(patient);
                await dbContext.SaveChangesAsync();
                return patient;
            }

            return null;
        }

        public async Task<Patient?> DeleteAsync(Guid id)
        {
            var existingPatient = await dbContext.Patients
                .Include(p => p.Address)
                .Include(p => p.Contact)
                .Include(p => p.PrimaryCarePhysicianId)
                .Include(p => p.MedicalFiles)
                .Include(p => p.CurrentMedications)
                .Include(p => p.Insurances)
                .Include(p => p.Appointments)
                .Include(p => p.LabAnalyses)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingPatient is null)
            {
                return null;
            }

            // Remove related entities
            if (existingPatient.Address != null)
            {
                // Address should be removed from Doctors first if it's required
                existingPatient.Address.Patients.Remove(existingPatient);
                if (!existingPatient.Address.Patients.Any()) // Remove Address if no other Doctors reference it
                {
                    dbContext.Addresses.Remove(existingPatient.Address);
                }
            }

            if (existingPatient.Contact != null)
            {
                // Contact should be removed from Doctors first if it's required
                existingPatient.Contact.Patients.Remove(existingPatient);
                if (!existingPatient.Contact.Patients.Any()) // Remove Contact if no other Doctors reference it
                {
                    dbContext.Contacts.Remove(existingPatient.Contact);
                }
            }

            dbContext.MedicalFiles.RemoveRange(existingPatient.MedicalFiles);
            dbContext.Medications.RemoveRange(existingPatient.CurrentMedications);
            dbContext.Insurances.RemoveRange(existingPatient.Insurances);
            dbContext.Appointments.RemoveRange(existingPatient.Appointments);
            dbContext.LabAnalyses.RemoveRange(existingPatient.LabAnalyses);


            dbContext.Patients.Remove(existingPatient);
            await dbContext.SaveChangesAsync();
            return existingPatient;
        }
    }
}
