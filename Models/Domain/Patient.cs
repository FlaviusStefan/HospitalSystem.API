namespace HospitalSystem.API.Models.Domain
{
    public class Patient
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public Guid AddressId { get; set; }
        public Address Address { get; set; }
        public Guid ContactId { get; set; }
        public Contact Contact { get; set; }
        public Guid PrimaryCarePhysicianId { get; set; }
        public Doctor PrimaryCarePhysician { get; set; }
        public ICollection<MedicalFile> MedicalFiles { get; set; }
        public ICollection<Medication> CurrentMedications { get; set; }
        public ICollection<Insurance> Insurances{ get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<LabAnalysis> LabAnalyses { get; set; }

    }
}
