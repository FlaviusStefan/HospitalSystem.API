namespace HospitalSystem.API.Models.DTO
{
    public class CreateQualificationRequestDto
    {
        public Guid DoctorId { get; set; }
        public string Degree { get; set; }
        public string Institution { get; set; }
        public int StudiedYears { get; set; }
        public int YearOfCompletion { get; set; }
    }
}

