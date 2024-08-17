namespace HospitalSystem.API.Models.DTO
{
    public class CreateLabAnalysisRequestDto
    {
        public DateTime AnalysisDate { get; set; }
        public string AnalysisType { get; set; }
        public Guid PatientId { get; set; }

    }
}
