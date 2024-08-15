namespace HospitalSystem.API.Models.DTO
{
    public class UpdateDoctorRequestDto
    {
        public string LicenseNumber { get; set; }
        public int YearsOfExperience { get; set; }
        public Guid AddressId { get; set; }
        public Guid ContactId { get; set; }
        public ICollection<Guid> SpecializationIds { get; set; }
        public ICollection<Guid> QualificationIds { get; set; }
        public ICollection<Guid> HospitalIds { get; set; }
        public ICollection<Guid> AppointmentIds { get; set; }

    }
}
