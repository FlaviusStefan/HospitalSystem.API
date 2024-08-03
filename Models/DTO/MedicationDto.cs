namespace HospitalSystem.API.Models.DTO
{
    public class MedicationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public Guid PatientId { get; set; }
        public PatientDto Patient { get; set; }
    }
}
