namespace HospitalSystem.API.Models.DTO
{
    public class CreateHospitalRequestDto
    {
        public string Name { get; set; }
        public CreateAddressRequestDto Address { get; set; }
        public CreateContactRequestDto Contact { get; set; }
    }
}
