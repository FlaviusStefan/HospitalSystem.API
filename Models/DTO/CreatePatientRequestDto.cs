namespace HospitalSystem.API.Models.DTO
{
    public class CreatePatientRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public CreateAddressRequestDto Address { get; set; }
        public CreateContactRequestDto Contact { get; set; }
    }
}
