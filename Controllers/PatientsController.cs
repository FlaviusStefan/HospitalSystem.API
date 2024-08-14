using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Models.DTO;
using HospitalSystem.API.Repositories.Implementation;
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

        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await patientRepository.GetAllAsync();

            var response = patients.Select(patient => new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                Height = patient.Height,
                Weight = patient.Weight,
                AddressId = patient.AddressId,
                ContactId = patient.ContactId,
                MedicalFileIds = patient.MedicalFiles.Select(mf => mf.Id).ToList(),
                CurrentMedicationIds = patient.CurrentMedications.Select(cm => cm.Id).ToList(),
                InsuranceIds = patient.Insurances.Select(i => i.Id).ToList(),
                AppointmentIds = patient.Appointments.Select(a => a.Id).ToList(),
                LabAnalysisIds = patient.LabAnalyses.Select(la => la.Id).ToList()

            }).ToList();

            return Ok(response);
        }



        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetPatientById([FromRoute] Guid id)
        {
            var existingPatient = await patientRepository.GetById(id);

            if (existingPatient is null)
            {
                return NotFound();
            }

            var response = new PatientDto
            {
                Id = existingPatient.Id,
                FirstName = existingPatient.FirstName,
                LastName = existingPatient.LastName,
                DateOfBirth = existingPatient.DateOfBirth,
                Gender = existingPatient.Gender,
                Height = existingPatient.Height,
                Weight = existingPatient.Weight,
                AddressId = existingPatient.AddressId,
                ContactId = existingPatient.ContactId,
                MedicalFileIds = existingPatient.MedicalFiles.Select(mf => mf.Id).ToList(),
                CurrentMedicationIds = existingPatient.CurrentMedications.Select(cm => cm.Id).ToList(),
                InsuranceIds = existingPatient.Insurances.Select(i => i.Id).ToList(),
                AppointmentIds = existingPatient.Appointments.Select(a => a.Id).ToList(),
                LabAnalysisIds = existingPatient.LabAnalyses.Select(la => la.Id).ToList()

            };

            return Ok(response);
        }

    }
}
