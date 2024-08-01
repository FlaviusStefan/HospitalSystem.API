namespace HospitalSystem.API.Models.DTO
{
    public class CreateDoctorRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public ICollection<Guid> SpecializationIds { get; set; } 
        public string LicenseNumber { get; set; }
        public int YearsOfExperience { get; set; }
        public Guid AddressId { get; set; }
        public Guid ContactId { get; set; }
    }
}
