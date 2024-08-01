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
        public ICollection<Guid> SpecializationIds { get; set; } // Use ICollection<Guid> to return the IDs
        public ICollection<HospitalAffiliation> HospitalAffiliations { get; set; }
        public ICollection<Qualification> Qualifications { get; set; }
        public ICollection<Patient> Patients { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
