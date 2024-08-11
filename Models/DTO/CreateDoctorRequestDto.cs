namespace HospitalSystem.API.Models.DTO
{
    public class CreateDoctorRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string LicenseNumber { get; set; }
        public int YearsOfExperience { get; set; }
        public CreateAddressRequestDto Address { get; set; }
        public CreateContactRequestDto Contact { get; set; }
        public ICollection<Guid> SpecializationIds { get; set; }
    }
}
