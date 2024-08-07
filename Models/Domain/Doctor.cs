﻿namespace HospitalSystem.API.Models.Domain
{
    public class Doctor
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string LicenseNumber { get; set; }
        public int YearsOfExperience { get; set; }
        public Guid AddressId { get; set; }
        public Address Address { get; set; }
        public Guid ContactId { get; set; }
        public Contact Contact { get; set; }
        public ICollection<DoctorSpecialization> DoctorSpecializations { get; set; } = new List<DoctorSpecialization>();
        public ICollection<HospitalAffiliation> HospitalAffiliations { get; set; } = new List<HospitalAffiliation>();
        public ICollection<Qualification> Qualifications { get; set; } = new List<Qualification>();
        public ICollection<Patient> Patients { get; set; } = new List<Patient>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
