using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Models.DTO;
using HospitalSystem.API.Repositories.Implementation;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdatePatient([FromRoute] Guid id, UpdatePatientRequestDto request)
        {
            // Retrieve the existing doctor from the repository
            var existingPatient = await patientRepository.GetById(id);

            if (existingPatient == null)
            {
                return NotFound();
            }


            // Update the simple properties
            existingPatient.Height = request.Height;
            existingPatient.Weight = request.Weight;
            existingPatient.AddressId = request.AddressId;
            existingPatient.ContactId = request.ContactId;

           
            // Update medical files
            var existingMedicalFileIds = existingPatient.MedicalFiles.Select(mf => mf.Id).ToList();
            var newMedicalFileIds = request.MedicalFileIds ?? new List<Guid>();

            // Remove medical files that are no longer in the request
            foreach (var existingMedicalFileId in existingMedicalFileIds)
            {
                if (!newMedicalFileIds.Contains(existingMedicalFileId))
                {
                    var medicalFileToRemove = existingPatient.MedicalFiles
                        .FirstOrDefault(ds => ds.Id == existingMedicalFileId);
                    if (medicalFileToRemove != null)
                    {
                        existingPatient.MedicalFiles.Remove(medicalFileToRemove);
                    }
                }
            }

            // Add new medical files from the request
            foreach (var medicalFileId in newMedicalFileIds)
            {
                if (!existingMedicalFileIds.Contains(medicalFileId))
                {
                    existingPatient.MedicalFiles.Add(new MedicalFile
                    {
                        Id = medicalFileId,
                    });
                }
            }

            // Update current medications
            var existingMedicationIds = existingPatient.CurrentMedications.Select(cm => cm.Id).ToList();
            var newMedicationIds = request.CurrentMedicationIds ?? new List<Guid>();

            // Remove current medications that are no longer in the request
            foreach (var existingMedicationId in existingMedicationIds)
            {
                if (!newMedicationIds.Contains(existingMedicationId))
                {
                    var medicationToRemove = existingPatient.CurrentMedications
                        .FirstOrDefault(cm => cm.Id == existingMedicationId);
                    if (medicationToRemove != null)
                    {
                        existingPatient.CurrentMedications.Remove(medicationToRemove);
                    }
                }
            }

            // Add new current medications from the request
            foreach (var medicationId in newMedicationIds)
            {
                if (!existingMedicationIds.Contains(medicationId))
                {
                    existingPatient.CurrentMedications.Add(new Medication
                    {
                        Id = medicationId
                        
                    });
                }
            }

            // Update current insurances
            var existingInsuranceIds = existingPatient.Insurances.Select(i => i.Id).ToList();
            var newInsuranceIds = request.InsuranceIds ?? new List<Guid>();

            // Remove current insurances that are no longer in the request
            foreach (var existingInsuranceId in existingInsuranceIds)
            {
                if (!newInsuranceIds.Contains(existingInsuranceId))
                {
                    var insuranceToRemove = existingPatient.Insurances
                        .FirstOrDefault(i=> i.Id == existingInsuranceId);
                    if (insuranceToRemove != null)
                    {
                        existingPatient.Insurances.Remove(insuranceToRemove);
                    }
                }
            }

            // Add new current insurances from the request
            foreach (var insuranceId in newInsuranceIds)
            {
                if (!existingInsuranceIds.Contains(insuranceId))
                {
                    existingPatient.Insurances.Add(new Insurance
                    {
                        Id = insuranceId

                    });
                }
            }

            // Update lab analysis insurances
            var existingLabAnalysisIds = existingPatient.LabAnalyses.Select(la => la.Id).ToList();
            var newLabAnalysisIds = request.LabAnalysisIds ?? new List<Guid>();

            // Remove current lab analysis that are no longer in the request
            foreach (var labAnalysisId in existingLabAnalysisIds)
            {
                if (!newLabAnalysisIds.Contains(labAnalysisId))
                {
                    var labAnalysisToRemove = existingPatient.LabAnalyses
                        .FirstOrDefault(la => la.Id == labAnalysisId);
                    if (labAnalysisToRemove != null)
                    {
                        existingPatient.LabAnalyses.Remove(labAnalysisToRemove);
                    }
                }
            }

            // Add new current lab analysis from the request
            foreach (var labAnalysisId in newLabAnalysisIds)
            {
                if (!existingLabAnalysisIds.Contains(labAnalysisId))
                {
                    existingPatient.LabAnalyses.Add(new LabAnalysis
                    {
                        Id = labAnalysisId

                    });
                }
            }



            // Update appointments
            var existingAppointmentIds = existingPatient.Appointments.Select(a => a.Id).ToList();
            var newAppointmentIds = request.AppointmentIds ?? new List<Guid>();

            // Remove appointments that are no longer in the request
            foreach (var existingAppointmentId in existingAppointmentIds)
            {
                if (!newAppointmentIds.Contains(existingAppointmentId))
                {
                    var appointmentToRemove = existingPatient.Appointments
                        .FirstOrDefault(a => a.Id == existingAppointmentId);
                    if (appointmentToRemove != null)
                    {
                        existingPatient.Appointments.Remove(appointmentToRemove);
                    }
                }
            }

            // Add new appointments
            foreach (var appointmentId in newAppointmentIds)
            {
                if (!existingAppointmentIds.Contains(appointmentId))
                {
                    existingPatient.Appointments.Add(new Appointment
                    {
                        Id = appointmentId
                    });
                }
            }

            // Update the doctor in the repository
            await patientRepository.UpdateAsync(existingPatient);

            // Create the response DTO
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

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeletePatient([FromRoute] Guid id)
        {
            var patient = await patientRepository.DeleteAsync(id);

            if (patient is null)
            {
                return NotFound();
            }

            var response = new PatientDto
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
            };

            return Ok(response);
        }

    }
}
