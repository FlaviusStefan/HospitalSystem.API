﻿using HospitalSystem.API.Models.Domain;

namespace HospitalSystem.API.Repositories.Interface
{
    public interface ILabAnalysisRepository
    {
        Task<LabAnalysis> CreateAsync(LabAnalysis labAnalysis);
        Task<IEnumerable<LabAnalysis>> GetAllAsync();
        Task<LabAnalysis?> GetById(Guid id);
        Task<LabAnalysis?> UpdateAsync(LabAnalysis labAnalysis);
        Task<LabAnalysis?> DeleteAsync(Guid id);
    }
}
