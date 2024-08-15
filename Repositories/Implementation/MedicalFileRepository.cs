using HospitalSystem.API.Data;
using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Repositories.Interface;

namespace HospitalSystem.API.Repositories.Implementation
{
    public class MedicalFileRepository : IMedicalFileRepository
    {
        private readonly ApplicationDbContext dbContext;

        public MedicalFileRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<MedicalFile> CreateAsync(MedicalFile medicalFile)
        {
            await dbContext.MedicalFiles.AddAsync(medicalFile);
            await dbContext.SaveChangesAsync();
            return medicalFile;
        }

        public Task<IEnumerable<MedicalFile>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<MedicalFile?> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<MedicalFile?> UpdateAsync(MedicalFile medicalFile)
        {
            throw new NotImplementedException();
        }

        public Task<MedicalFile?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
