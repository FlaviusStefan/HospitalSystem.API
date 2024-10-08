﻿namespace HospitalSystem.API.Models.DTO
{
    public class LabAnalysisDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public PatientDto Patient { get; set; }
        public DateTime AnalysisDate { get; set; }
        public string AnalysisType { get; set; }
        public ICollection<Guid> LabTestIds { get; set; }
    }
}
