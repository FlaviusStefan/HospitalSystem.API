using HospitalSystem.API.Models.Domain;

namespace HospitalSystem.API.Models.DTO
{
    public class CreateMedicalFileRequestDto
    {
        public Guid PatientId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public DateTime DateUploaded { get; set; }
    }
}
