namespace HospitalSystem.API.Models.DTO
{
    public class PatientDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public Guid AddressId { get; set; }
        public AddressDto Address { get; set; }
        public Guid ContactId { get; set; }
        public ContactDto Contact { get; set; }
        public Guid PrimaryCarePhysicianId { get; set; }
        public ICollection<Guid> MedicalFileIds { get; set; }
        public ICollection<Guid> CurrentMedicationIds { get; set; }
        public ICollection<Guid> InsuranceIds { get; set; }
        public ICollection<Guid> AppointmentIds { get; set; }
        public ICollection<Guid> LabAnalysisIds { get; set; }
    }
}
