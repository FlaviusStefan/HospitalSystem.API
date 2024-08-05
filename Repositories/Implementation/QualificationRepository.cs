using HospitalSystem.API.Data;
using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace HospitalSystem.API.Repositories.Implementation
{
    public class QualificationRepository : IQualificationRepository
    {
        private readonly ApplicationDbContext dbContext;

        public QualificationRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Qualification> CreateAsync(Qualification qualification)
        {
            await dbContext.Qualifications.AddAsync(qualification);
            await dbContext.SaveChangesAsync();

            return qualification;
        }    

        public async Task<IEnumerable<Qualification>> GetAllAsync()
        {
            return await dbContext.Qualifications
                .Include(q => q.Doctor)
                .ToListAsync();
        }

        public async Task<Qualification?> GetById(Guid id)
        {
            return await dbContext.Qualifications
                .Include(q => q.Doctor)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Qualification?> UpdateAsync(Qualification qualification)
        {
            var existingQualification = await dbContext.Qualifications.FirstOrDefaultAsync(x => x.Id == qualification.Id);

            if (existingQualification != null)
            {
                // Verify the DoctorId exists in the Doctors table
                var doctorExists = await dbContext.Doctors.AnyAsync(d => d.Id == qualification.DoctorId);
                if (!doctorExists)
                {
                    throw new InvalidOperationException($"Doctor with Id {qualification.DoctorId} does not exist.");
                }

                // Update properties
                existingQualification.Degree = qualification.Degree;
                existingQualification.Institution = qualification.Institution;
                existingQualification.DoctorId = qualification.DoctorId; // Ensure this is valid

                await dbContext.SaveChangesAsync();
                return existingQualification;
            }

            return null;
        }

        public async Task<Qualification?> DeleteAsync(Guid id)
        {
            var existingQualification = await dbContext.Qualifications.FirstOrDefaultAsync(x => x.Id == id);

            if (existingQualification is null)
            {
                return null;
            }

            dbContext.Qualifications.Remove(existingQualification);
            await dbContext.SaveChangesAsync();
            return existingQualification;
        }



    }
}
