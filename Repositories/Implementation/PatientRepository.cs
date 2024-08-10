using HospitalSystem.API.Data;
using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.API.Repositories.Implementation
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext dbContext;

        public PatientRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Patient> CreateAsync(Patient patient)
        {
            await dbContext.Patients.AddAsync(patient);
            await dbContext.SaveChangesAsync();

            return patient;
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
            var existingPatient = await dbContext.Patients.FirstOrDefaultAsync(x => x.Id == id);

            if (existingPatient is null)
            {
                return null;
            }

            dbContext.Patients.Remove(existingPatient);
            await dbContext.SaveChangesAsync();
            return existingPatient;
        }
    }
}
