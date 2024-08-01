namespace HospitalSystem.API.Models.Domain
{
    public class HospitalAffiliation
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public Guid HospitalId { get; set; }
        public Hospital Hospital { get; set; }
    }
}
