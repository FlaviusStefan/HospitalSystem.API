namespace HospitalSystem.API.Models.DTO
{
    public class UpdateLabAnalysisRequestDto
    {
        public DateTime AnalysisDate { get; set; }
        public string AnalysisType { get; set; }
        public ICollection<Guid> LabTestIds { get; set; }
    }
}
