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

        public async Task<Qualification?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
