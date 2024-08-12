using HospitalSystem.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<MedicalFile> MedicalFiles { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<LabAnalysis> LabAnalyses { get; set; }
        public DbSet<LabTest> LabTests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            // Configuration for Address entity
            modelBuilder.Entity<Address>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Address>()
                .HasMany(a => a.Doctors)
                .WithOne(d => d.Address)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Address>()
                .HasMany(a => a.Patients)
                .WithOne(p => p.Address)
                .HasForeignKey(p => p.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Address>()
                .HasMany(a => a.Hospitals)
                .WithOne(h => h.Address)
                .HasForeignKey(h => h.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration for Contact entity
            modelBuilder.Entity<Contact>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Contact>()
                .HasMany(c => c.Doctors)
                .WithOne(d => d.Contact)
                .HasForeignKey(d => d.ContactId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Contact>()
                .HasMany(c => c.Patients)
                .WithOne(p => p.Contact)
                .HasForeignKey(p => p.ContactId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Contact>()
                .HasMany(c => c.Hospitals)
                .WithOne(h => h.Contact)
                .HasForeignKey(h => h.ContactId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration for Doctor entity
            modelBuilder.Entity<Doctor>()
                .HasKey(d => d.Id);

            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Address)
                .WithMany(a => a.Doctors)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Contact)
                .WithMany(c => c.Doctors)
                .HasForeignKey(d => d.ContactId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.DoctorSpecializations)
                .WithOne(ds => ds.Doctor)
                .HasForeignKey(ds => ds.DoctorId);

            modelBuilder.Entity<Specialization>()
                .HasMany(s => s.DoctorSpecializations)
                .WithOne(ds => ds.Specialization)
                .HasForeignKey(ds => ds.SpecializationId);

            modelBuilder.Entity<DoctorSpecialization>()
                .HasKey(ds => new { ds.DoctorId, ds.SpecializationId });

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.DoctorHospitals)
                .WithOne(ha => ha.Doctor)
                .HasForeignKey(ha => ha.DoctorId);

            modelBuilder.Entity<Hospital>()
                .HasMany(h => h.DoctorHospitals)
                .WithOne(ds => ds.Hospital)
                .HasForeignKey(ds => ds.HospitalId);

            modelBuilder.Entity<DoctorHospital>()
                .HasKey(ds => new { ds.DoctorId, ds.HospitalId });

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Qualifications)
                .WithOne(q => q.Doctor)
                .HasForeignKey(q => q.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Patients)
                .WithOne(p => p.PrimaryCarePhysician)
                .HasForeignKey(p => p.PrimaryCarePhysicianId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Appointments)
                .WithOne(a => a.Doctor)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration for Hospital entity
            modelBuilder.Entity<Hospital>()
                .HasKey(h => h.Id);

            modelBuilder.Entity<Hospital>()
                .HasOne(h => h.Address)
                .WithMany(a => a.Hospitals)
                .HasForeignKey(h => h.AddressId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Hospital>()
                .HasOne(h => h.Contact)
                .WithMany(c => c.Hospitals)
                .HasForeignKey(h => h.ContactId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration for Patient entity
            modelBuilder.Entity<Patient>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Patient>()
                .HasOne(p => p.Address)
                .WithMany(a => a.Patients)
                .HasForeignKey(p => p.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Patient>()
                .HasOne(p => p.Contact)
                .WithMany(c => c.Patients)
                .HasForeignKey(p => p.ContactId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Patient>()
                .HasOne(p => p.PrimaryCarePhysician)
                .WithMany(d => d.Patients)
                .HasForeignKey(p => p.PrimaryCarePhysicianId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.MedicalFiles)
                .WithOne(mf => mf.Patient)
                .HasForeignKey(mf => mf.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.CurrentMedications)
                .WithOne(m => m.Patient)
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Insurances)
                .WithOne(i => i.Patient)
                .HasForeignKey(i => i.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Appointments)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.LabAnalyses)
                .WithOne(la => la.Patient)
                .HasForeignKey(la => la.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration for Specialization entity
            modelBuilder.Entity<Specialization>()
                .HasKey(s => s.Id);

            // Configuration for MedicalFile entity
            modelBuilder.Entity<MedicalFile>()
                .HasKey(mf => mf.Id);

            modelBuilder.Entity<MedicalFile>()
                .HasOne(mf => mf.Patient)
                .WithMany(p => p.MedicalFiles)
                .HasForeignKey(mf => mf.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration for Medication entity
            modelBuilder.Entity<Medication>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Medication>()
                .HasOne(m => m.Patient)
                .WithMany(p => p.CurrentMedications)
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration for Insurance entity
            modelBuilder.Entity<Insurance>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<Insurance>()
                .HasOne(i => i.Patient)
                .WithMany(p => p.Insurances)
                .HasForeignKey(i => i.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration for Appointment entity
            modelBuilder.Entity<Appointment>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration for Qualification entity
            modelBuilder.Entity<Qualification>()
                .HasKey(q => q.Id);

            modelBuilder.Entity<Qualification>()
                .HasOne(q => q.Doctor)
                .WithMany(d => d.Qualifications)
                .HasForeignKey(q => q.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration for LabAnalysis entity
            modelBuilder.Entity<LabAnalysis>()
                .HasKey(la => la.Id);

            modelBuilder.Entity<LabAnalysis>()
                .HasOne(la => la.Patient)
                .WithMany(p => p.LabAnalyses)
                .HasForeignKey(la => la.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LabAnalysis>()
                .HasMany(la => la.LabTests)
                .WithOne(lt => lt.LabAnalysis)
                .HasForeignKey(lt => lt.LabAnalysisId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration for LabTest entity
            modelBuilder.Entity<LabTest>()
                .HasKey(lt => lt.Id);

            modelBuilder.Entity<LabTest>()
                .HasOne(lt => lt.LabAnalysis)
                .WithMany(la => la.LabTests)
                .HasForeignKey(lt => lt.LabAnalysisId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}