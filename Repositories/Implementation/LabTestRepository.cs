using HospitalSystem.API.Data;
using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.API.Repositories.Implementation
{
    public class LabTestRepository : ILabTestRepository
    {
        private readonly ApplicationDbContext dbContext;

        public LabTestRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<LabTest> CreateAsync(LabTest labTest)
        {
            await dbContext.LabTests.AddAsync(labTest);
            await dbContext.SaveChangesAsync();
            return labTest;
        }

        public async Task<IEnumerable<LabTest>> GetAllAsync()
        {
            return await dbContext.LabTests.ToListAsync();
        }

        public async Task<LabTest?> GetById(Guid id)
        {
            return await dbContext.LabTests.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<LabTest?> UpdateAsync(LabTest labTest)
        {
            var existingLabTest = await dbContext.LabTests.FirstOrDefaultAsync(x => x.Id == labTest.Id);

            if (existingLabTest != null)
            {
                var labAnalysisExists = await dbContext.LabAnalyses.AnyAsync(p => p.Id == labTest.LabAnalysisId);
                if (!labAnalysisExists)
                {
                    throw new InvalidOperationException($"Lab Analysis with Id {labTest.LabAnalysisId} does not exist.");
                }

                existingLabTest.TestName = labTest.TestName;
                existingLabTest.Result = labTest.Result;
                existingLabTest.Units = labTest.Units;
                existingLabTest.ReferenceRange = labTest.ReferenceRange;
                existingLabTest.LabAnalysisId = labTest.LabAnalysisId;

                await dbContext.SaveChangesAsync();
                return existingLabTest;
            }

            return null;
        }
        public async Task<LabTest?> DeleteAsync(Guid id)
        {
            var existingLabTest = await dbContext.LabTests.FirstOrDefaultAsync(x => x.Id == id);

            if (existingLabTest is null)
            {
                return null;
            }

            dbContext.LabTests.Remove(existingLabTest);
            await dbContext.SaveChangesAsync();
            return existingLabTest;
        }
    }
}
