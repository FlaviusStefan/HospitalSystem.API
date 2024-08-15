using HospitalSystem.API.Data;
using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.API.Repositories.Implementation
{
    public class InsuranceRepository : IInsuranceRepository

    {
        private readonly ApplicationDbContext dbContext;

        public InsuranceRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Insurance> CreateAsync(Insurance insurance)
        {
            await dbContext.Insurances.AddAsync(insurance);
            await dbContext.SaveChangesAsync();

            return insurance;
        }



        public async Task<IEnumerable<Insurance>> GetAllAsync()
        {
            return await dbContext.Insurances.ToListAsync();
        }

        public async Task<Insurance?> GetById(Guid id)
        {
            return await dbContext.Insurances.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Insurance?> UpdateAsync(Insurance insurance)
        {
            var existingInsurance = await dbContext.Insurances.FirstOrDefaultAsync(x => x.Id == insurance.Id);

            if (existingInsurance != null)
            {
                var patientExists = await dbContext.Patients.AnyAsync(d => d.Id == insurance.PatientId);
                if (!patientExists)
                {
                    throw new InvalidOperationException($"Patient with Id {insurance.PatientId} does not exist.");
                }

                existingInsurance.PolicyNumber = insurance.PolicyNumber;
                existingInsurance.Provider=insurance.Provider;
                existingInsurance.CoverageStartDate = insurance.CoverageStartDate;
                existingInsurance.CoverageEndDate = insurance.CoverageEndDate;
                existingInsurance.PatientId = insurance.PatientId;
               
                await dbContext.SaveChangesAsync();
                return existingInsurance;
            }

            return null;
        }

        public async Task<Insurance?> DeleteAsync(Guid id)
        {
            var existingInsurance = await dbContext.Insurances.FirstOrDefaultAsync(x => x.Id == id);

            if(existingInsurance is null)
            {
                return null;
            }

            dbContext.Insurances.Remove(existingInsurance);
            await dbContext.SaveChangesAsync();
            return existingInsurance;
        }
    }
}
