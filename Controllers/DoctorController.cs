using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Models.DTO;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

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
                SpecializationIds = doctor.DoctorSpecializations.Select(ds => ds.SpecializationId).ToList(),
                QualificationIds = doctor.Qualifications.Select(q => q.Id).ToList(),
                HospitalAffiliationIds = doctor.HospitalAffiliations.Select(ha => ha.Id).ToList(),
                AppointmentIds = doctor.Appointments.Select(a => a.Id).ToList(),
                PatientIds = doctor.Patients.Select(p => p.Id).ToList()

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
                SpecializationIds = existingDoctor.DoctorSpecializations.Select(ds => ds.SpecializationId).ToList(),
                QualificationIds = existingDoctor.Qualifications.Select(ds => ds.Id).ToList(),
                HospitalAffiliationIds = existingDoctor.HospitalAffiliations.Select(ha => ha.Id).ToList(),                
                AppointmentIds = existingDoctor.Appointments.Select(a => a.Id).ToList(),
                PatientIds = existingDoctor.Patients.Select(p => p.Id).ToList()

            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateDoctor([FromRoute] Guid id, UpdateDoctorRequestDto request)
        {
            // Retrieve the existing doctor from the repository
            var existingDoctor = await doctorRepository.GetById(id);

            if (existingDoctor == null)
            {
                return NotFound();
            }

            // Update the simple properties
            existingDoctor.LicenseNumber = request.LicenseNumber;
            existingDoctor.YearsOfExperience = request.YearsOfExperience;
            existingDoctor.AddressId = request.AddressId;
            existingDoctor.ContactId = request.ContactId;

            // Update specializations
            var existingSpecializationIds = existingDoctor.DoctorSpecializations.Select(ds => ds.SpecializationId).ToList();
            var newSpecializationIds = request.SpecializationIds ?? new List<Guid>();

            // Remove specializations that are no longer in the request
            foreach (var specializationId in existingSpecializationIds)
            {
                if (!newSpecializationIds.Contains(specializationId))
                {
                    var specializationToRemove = existingDoctor.DoctorSpecializations
                        .FirstOrDefault(ds => ds.SpecializationId == specializationId);
                    if (specializationToRemove != null)
                    {
                        existingDoctor.DoctorSpecializations.Remove(specializationToRemove);
                    }
                }
            }

            // Add new specializations from the request
            foreach (var specializationId in newSpecializationIds)
            {
                if (!existingSpecializationIds.Contains(specializationId))
                {
                    existingDoctor.DoctorSpecializations.Add(new DoctorSpecialization
                    {
                        DoctorId = id,
                        SpecializationId = specializationId
                    });
                }
            }

            //// Update Qualifications
            //var existingQualificationIds = existingDoctor.Qualifications.Select(q => q.Id).ToList();
            //var newQualificationIds = request.QualificationIds ?? new List<Guid>();

            //// Remove qualifications that are no longer in the request
            //foreach (var existingQualificationId in existingQualificationIds)
            //{
            //    if (!newQualificationIds.Contains(existingQualificationId))
            //    {
            //        var qualificationToRemove = existingDoctor.Qualifications
            //            .FirstOrDefault(q => q.Id == existingQualificationId);
            //        if (qualificationToRemove != null)
            //        {
            //            existingDoctor.Qualifications.Remove(qualificationToRemove);
            //        }
            //    }
            //}

            //// Add new qualifications
            //foreach (var qualificationId in newQualificationIds)
            //{
            //    if (!existingQualificationIds.Contains(qualificationId))
            //    {
            //        // Assuming GetQualificationById is a method in the repository
            //        var qualification = await doctorRepository.GetQualificationByIdAsync(qualificationId);

            //        if (qualification != null)
            //        {
            //            existingDoctor.Qualifications.Add(qualification);
            //        }
            //    }
            //}

            // Update the doctor in the repository
            await doctorRepository.UpdateAsync(existingDoctor);

            // Create the response DTO
            var response = new DoctorDto
            {
                Id = existingDoctor.Id,
                LicenseNumber = existingDoctor.LicenseNumber,
                YearsOfExperience = existingDoctor.YearsOfExperience,
                AddressId = existingDoctor.AddressId,
                ContactId = existingDoctor.ContactId,
                SpecializationIds = existingDoctor.DoctorSpecializations.Select(ds => ds.SpecializationId).ToList()
            };

            return Ok(response);
        }
    }
}