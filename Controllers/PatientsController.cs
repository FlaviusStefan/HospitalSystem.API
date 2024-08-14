using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Models.DTO;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientRepository patientRepository;
        private readonly IAddressRepository addressRepository;
        private readonly IContactRepository contactRepository;

        public PatientsController(IPatientRepository patientRepository, IAddressRepository addressRepository, IContactRepository contactRepository)
        {
            this.patientRepository = patientRepository;
            this.addressRepository = addressRepository;
            this.contactRepository = contactRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreatePatient(CreatePatientRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patient = new Patient
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                Height = request.Height,
                Weight = request.Weight,
                PrimaryCarePhysicianId = request.PrimaryCarePhysicianId
            };

            // Create the Address entity if provided
            Address address = null;
            if (request.Address != null)
            {
                address = new Address
                {
                    Street = request.Address.Street,
                    StreetNr = request.Address.StreetNr,
                    City = request.Address.City,
                    State = request.Address.State,
                    Country = request.Address.Country,
                    PostalCode = request.Address.PostalCode
                };
            }

            // Create the Contact entity if provided
            Contact contact = null;
            if (request.Contact != null)
            {
                contact = new Contact
                {
                    Phone = request.Contact.Phone,
                    Email = request.Contact.Email
                };
            }

            try
            {
                var createdPatient = await patientRepository.CreateAsync(patient, address, contact);

                var response = new PatientDto
                {
                    Id = createdPatient.Id,
                    FirstName = createdPatient.FirstName,
                    LastName = createdPatient.LastName,
                    DateOfBirth = createdPatient.DateOfBirth,
                    Gender = createdPatient.Gender,
                    Height = createdPatient.Height,
                    Weight = createdPatient.Weight,
                    AddressId = createdPatient.AddressId,
                    ContactId = createdPatient.ContactId
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the detailed exception
                Console.WriteLine($"Error creating patient: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                }

                return StatusCode(500, "An error occurred while creating the patient.");
            }
        }

    }
}
