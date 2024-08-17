using HospitalSystem.API.Data;
using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.API.Repositories.Implementation
{
    public class MedicationRepository : IMedicationRepository
    {
        private readonly ApplicationDbContext dbContext;

        public MedicationRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Medication> CreateAsync(Medication medication)
        {
            await dbContext.Medications.AddAsync(medication);
            await dbContext.SaveChangesAsync();

            return medication;
        }
        public async Task<IEnumerable<Medication>> GetAllAsync()
        {
            return await dbContext.Medications.ToListAsync();
        }
        

        public async Task<Medication?> GetById(Guid id)
        {
            return await dbContext.Medications.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Medication?> UpdateAsync(Medication medication)
        {
            var existingMedication = await dbContext.Medications.FirstOrDefaultAsync(x => x.Id ==  medication.Id);

            if(existingMedication != null)
            {
                var patientExists = await dbContext.Patients.AnyAsync(p => p.Id == medication.PatientId);
                if(!patientExists)
                {
                    throw new InvalidOperationException($"Patient with Id {medication.PatientId} does not exist.");
                }

                existingMedication.Name = medication.Name;
                existingMedication.Frequency = medication.Frequency;
                existingMedication.Dosage = medication.Dosage;

                await dbContext.SaveChangesAsync();
                return existingMedication;
            }

            return null;
        }

        public async Task<Medication?> DeleteAsync(Guid id)
        {
            var existingMedication = await dbContext.Medications.FirstOrDefaultAsync(x => x.Id == id);

            if (existingMedication is null)
            {
                return null;
            }

            dbContext.Medications.Remove(existingMedication);
            await dbContext.SaveChangesAsync();
            return existingMedication;
        }

    }
}
