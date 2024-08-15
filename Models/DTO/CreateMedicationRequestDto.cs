namespace HospitalSystem.API.Models.DTO
{
    public class CreateMedicationRequestDto
    {
        public Guid PatientId { get; set; }
        public string Name { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        
    }
}
