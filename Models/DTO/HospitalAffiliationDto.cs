namespace HospitalSystem.API.Models.DTO
{
    public class HospitalAffiliationDto
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public Guid HospitalId { get; set; }
    }
}
