namespace HospitalSystem.API.Models.DTO
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public DateTime DateTime { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string Details { get; set; }
    }
}
