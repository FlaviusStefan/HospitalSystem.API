namespace HospitalSystem.API.Models.DTO
{
    public class UpdateHospitalRequestDto
    {
        public string Name { get; set; }
        public Guid AddressId { get; set; }
        public Guid ContactId { get; set; }
    }
}
