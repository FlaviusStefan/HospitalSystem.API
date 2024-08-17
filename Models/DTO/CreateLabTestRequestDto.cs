namespace HospitalSystem.API.Models.DTO
{
    public class CreateLabTestRequestDto
    {
        public Guid LabAnalysisId { get; set; }
        public string TestName { get; set; }
        public string Result { get; set; }
        public string Units { get; set; }
        public string ReferenceRange { get; set; }
    }
}
