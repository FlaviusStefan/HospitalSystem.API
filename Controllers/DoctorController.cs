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
                SpecializationIds = existingDoctor.DoctorSpecializations.Select(ds => ds.SpecializationId).ToList()
            };

            return Ok(response);
        }
    }
}
