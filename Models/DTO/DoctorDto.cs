using HospitalSystem.API.Models.Domain;

namespace HospitalSystem.API.Models.DTO
{
    public class DoctorDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string LicenseNumber { get; set; }
        public int YearsOfExperience { get; set; }
        public Guid AddressId { get; set; }
        public AddressDto Address { get; set; }
        public Guid ContactId { get; set; }
        public ContactDto Contact { get; set; }
        public ICollection<Guid> SpecializationIds { get; set; } 

        public ICollection<Guid> HospitalAffiliationIds { get; set; }
        public ICollection<Guid> QualificationIds { get; set; }
        public ICollection<Guid> PatientIds { get; set; }
        public ICollection<Guid> AppointmentIds { get; set; }
    }
}
