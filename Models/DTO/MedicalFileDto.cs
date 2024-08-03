namespace HospitalSystem.API.Models.DTO
{
    public class MedicalFileDto
    {
        public Guid Id { get; set; }
        public string FileType { get; set; }
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }
    }
}
