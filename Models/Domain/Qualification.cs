namespace HospitalSystem.API.Models.Domain
{
    public class Qualification
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public string Degree { get; set; }
        public string Institution { get; set; }
        public int StudiedYears { get; set; }
        public DateTime YearOfCompletion { get; set; }
    }
}
