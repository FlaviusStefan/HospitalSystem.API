using HospitalSystem.API.Models.Domain;

namespace HospitalSystem.API.Models.DTO
{
    public class UpdatePatientRequestDto
    {
        public double Height { get; set; }
        public double Weight { get; set; }
        public Guid AddressId { get; set; }
        public Guid ContactId { get; set; }
        public ICollection<Guid> MedicalFileIds { get; set; }
        public ICollection<Guid> CurrentMedicationIds { get; set; }
        public ICollection<Guid> InsuranceIds { get; set; }
        public ICollection<Guid> AppointmentIds { get; set; }
        public ICollection<Guid> LabAnalysisIds { get; set; }
    }
    
}
