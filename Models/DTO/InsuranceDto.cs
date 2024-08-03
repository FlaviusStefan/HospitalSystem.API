namespace HospitalSystem.API.Models.DTO
{
    public class InsuranceDto
    {
        public Guid Id { get; set; }
        public string Provider { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime CoverageStartDate { get; set; }
        public DateTime CoverageEndDate { get; set; }
        public Guid PatientId { get; set; }
        public PatientDto Patient { get; set; }
    }
}
