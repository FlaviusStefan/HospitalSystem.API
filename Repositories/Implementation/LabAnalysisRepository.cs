using HospitalSystem.API.Data;
using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.API.Repositories.Implementation
{
    public class LabAnalysisRepository : ILabAnalysisRepository
    {
        private readonly ApplicationDbContext dbContext;

        public LabAnalysisRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<LabAnalysis> CreateAsync(LabAnalysis labAnalysis)
        {
            await dbContext.LabAnalyses.AddAsync(labAnalysis);
            await dbContext.SaveChangesAsync();
            return labAnalysis;
        }
        public async Task<IEnumerable<LabAnalysis>> GetAllAsync()
        {
            return await dbContext.LabAnalyses
                .Include(la => la.LabTests)
                .ToListAsync();
        }
        public async Task<LabAnalysis?> GetById(Guid id)
        {
            return await dbContext.LabAnalyses
                .Include(la => la.LabTests)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<LabAnalysis?> UpdateAsync(LabAnalysis labAnalysis)
        {
            var existingLabAnalysis = await dbContext.LabAnalyses.FirstOrDefaultAsync(x => x.Id == labAnalysis.Id);

            if (existingLabAnalysis != null)
            {
                var patientExists = await dbContext.Patients.AnyAsync(p => p.Id == labAnalysis.PatientId);
                if (!patientExists)
                {
                    throw new InvalidOperationException($"Patient with Id {labAnalysis.PatientId} does not exist.");
                }

                existingLabAnalysis.AnalysisDate = labAnalysis.AnalysisDate;
                existingLabAnalysis.AnalysisType = labAnalysis.AnalysisType;

                await dbContext.SaveChangesAsync();
                return existingLabAnalysis;
            }

            return null;

        }

        public async Task<LabAnalysis?> DeleteAsync(Guid id)
        {
            var existingLabAnalysis = await dbContext.LabAnalyses.Include(la => la.LabTests).FirstOrDefaultAsync(x => x.Id == id);

            if (existingLabAnalysis is null)
            {
                return null;
            }

            dbContext.LabAnalyses.Remove(existingLabAnalysis);
            await dbContext.SaveChangesAsync();
            return existingLabAnalysis;
        }
    }
}
