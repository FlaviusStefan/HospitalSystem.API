using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Models.DTO;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository doctorRepository;

        public DoctorController(IDoctorRepository doctorRepository)
        {
            this.doctorRepository = doctorRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDoctor(CreateDoctorRequestDto request)
        {
            // Create the Doctor entity
            var doctor = new Doctor
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                LicenseNumber = request.LicenseNumber,
                YearsOfExperience = request.YearsOfExperience,
                AddressId = request.AddressId,
                ContactId = request.ContactId
            };

            // Create the DoctorSpecializations relationships
            var doctorSpecializations = request.SpecializationIds.Select(sid => new DoctorSpecialization
            {
                DoctorId = doctor.Id,
                SpecializationId = sid
            }).ToList();

            // Set the DoctorSpecializations on the Doctor entity
            doctor.DoctorSpecializations = doctorSpecializations;

            // Save the doctor to the database
            await doctorRepository.CreateAsync(doctor);

            // Create the response DTO
            var response = new DoctorDto
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                DateOfBirth = doctor.DateOfBirth,
                Gender = doctor.Gender,
                SpecializationIds = request.SpecializationIds,
                LicenseNumber = doctor.LicenseNumber,
                YearsOfExperience = doctor.YearsOfExperience,
                AddressId = doctor.AddressId,
                ContactId = doctor.ContactId
            };

            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await doctorRepository.GetAllAsync();

            var response = doctors.Select(doctor => new DoctorDto
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                DateOfBirth = doctor.DateOfBirth,
                Gender = doctor.Gender,
                LicenseNumber = doctor.LicenseNumber,
                YearsOfExperience = doctor.YearsOfExperience,
                AddressId = doctor.AddressId,
                ContactId = doctor.ContactId,
                Address = new AddressDto
                {
                    Id = doctor.Address.Id,
                    Street = doctor.Address.Street,
                    StreetNr = doctor.Address.StreetNr,
                    City = doctor.Address.City,
                    State = doctor.Address.State,
                    Country = doctor.Address.Country,
                    PostalCode = doctor.Address.PostalCode
                },
                Contact = new ContactDto
                {
                    Id = doctor.ContactId,
                    Phone = doctor.Contact.Phone,
                    Email = doctor.Contact.Email,
                },
                SpecializationIds = doctor.DoctorSpecializations.Select(ds => ds.SpecializationId).ToList()
            }).ToList();

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetDoctorById([FromRoute] Guid id)
        {
            var existingDoctor = await doctorRepository.GetById(id);

            if (existingDoctor is null)
            {
                return NotFound();
            }

            var response = new DoctorDto
            {
                Id = existingDoctor.Id,
                FirstName = existingDoctor.FirstName,
                LastName = existingDoctor.LastName,
                DateOfBirth = existingDoctor.DateOfBirth,
                Gender = existingDoctor.Gender,
                LicenseNumber = existingDoctor.LicenseNumber,
                YearsOfExperience = existingDoctor.YearsOfExperience,
                AddressId = existingDoctor.AddressId,
                ContactId = existingDoctor.ContactId,
                Address = new AddressDto
                {
                    Id = existingDoctor.Address.Id,
                    Street = existingDoctor.Address.Street,
                    StreetNr = existingDoctor.Address.StreetNr,
                    City = existingDoctor.Address.City,
                    State = existingDoctor.Address.State,
                    Country = existingDoctor.Address.Country,
                    PostalCode = existingDoctor.Address.PostalCode
                },
                Contact = new ContactDto
                {
                    Id = existingDoctor.Contact.Id,
                    Phone = existingDoctor.Contact.Phone,
                    Email = existingDoctor.Contact.Email,
                },
                SpecializationIds = existingDoctor.DoctorSpecializations.Select(ds => ds.SpecializationId).ToList(),
                HospitalAffiliationIds = existingDoctor.HospitalAffiliations.Select(ha => ha.Id).ToList(),
                QualificationIds = existingDoctor.Qualifications.Select(q => q.Id).ToList(),
                PatientIds= existingDoctor.Patients.Select(p => p.Id).ToList(),
                AppointmentIds = existingDoctor.Appointments.Select(a => a.Id).ToList()
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateDoctor([FromRoute] Guid id, UpdateDoctorRequestDto request)
        {
            var doctor = new Doctor
            {
                Id = id,
                LicenseNumber = request.LicenseNumber,
                YearsOfExperience = request.YearsOfExperience,
                AddressId = request.AddressId,
                ContactId = request.ContactId,
                Qualifications = request.QualificationIds?.Select(qualificationId => new Qualification
                {
                    Id = qualificationId,
                    DoctorId = id,
                }).ToList(),

                Appointments = request.AppointmentIds?.Select(appointmentId => new Appointment
                {
                    Id = appointmentId,
                    DoctorId = id,
                }).ToList(),

                Patients = request.PatientIds?.Select(patientId => new Patient
                {
                    Id = patientId,
                    PrimaryCarePhysicianId = id,
                }).ToList(),

                HospitalAffiliations = request.HospitalAffiliationIds?.Select(hospitalAffiliationId => new HospitalAffiliation
                {
                    Id = hospitalAffiliationId,
                    DoctorId = id,
                    HospitalId = hospitalAffiliationId
                }).ToList(),

            };

            doctor = await doctorRepository.UpdateAsync(doctor);

            if (doctor == null)
            {
                return NotFound();
            }

            var response = new DoctorDto
            {
                Id = doctor.Id,
                LicenseNumber = doctor.LicenseNumber,
                YearsOfExperience = doctor.YearsOfExperience,
                AddressId = doctor.AddressId,
                ContactId = doctor.ContactId,
                SpecializationIds = doctor.DoctorSpecializations?.Select(ds => ds.SpecializationId).ToList() ?? new List<Guid>(),
                HospitalAffiliationIds = doctor.HospitalAffiliations?.Select(ha => ha.HospitalId).ToList() ?? new List<Guid>(),
                QualificationIds = doctor.Qualifications?.Select(q => q.Id).ToList() ?? new List<Guid>(),
                PatientIds = doctor.Patients?.Select(p => p.Id).ToList() ?? new List<Guid>(),
                AppointmentIds = doctor.Appointments?.Select(a => a.Id).ToList() ?? new List<Guid>()
            };

            return Ok(response);
        }
    }
}
