namespace HospitalSystem.API.Models.DTO
{
    public class CreateInsuranceRequestDto
    {
        public string Provider { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime CoverageStartDate { get; set; }
        public DateTime CoverageEndDate { get; set; }
        public Guid PatientId { get; set; }
    }
}
