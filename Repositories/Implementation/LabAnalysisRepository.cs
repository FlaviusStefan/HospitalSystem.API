using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Repositories.Interface;

namespace HospitalSystem.API.Repositories.Implementation
{
    public class LabAnalysisRepository : ILabAnalysisRepository
    {
        public Task<LabAnalysis> CreateAsync(LabAnalysis labAnalysis)
        {
            throw new NotImplementedException();
        }

        public Task<LabAnalysis?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LabAnalysis>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<LabAnalysis?> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<LabAnalysis?> UpdateAsync(LabAnalysis labAnalysis)
        {
            throw new NotImplementedException();
        }
    }
}
