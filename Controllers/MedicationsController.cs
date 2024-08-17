using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Models.DTO;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicationsController : ControllerBase
    {
        private readonly IMedicationRepository medicationRepository;

        public MedicationsController(IMedicationRepository medicationRepository)
        {
            this.medicationRepository = medicationRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMedication(CreateMedicationRequestDto request)
        {
            var medication = new Medication
            {
                Id = Guid.NewGuid(),
                PatientId = request.PatientId,
                Name = request.Name,
                Dosage = request.Dosage,
                Frequency = request.Frequency
            };

            await medicationRepository.CreateAsync(medication);

            var response = new MedicationDto
            {
                Id = medication.Id,
                PatientId = medication.PatientId,
                Name = medication.Name,
                Dosage = medication.Dosage,
                Frequency = medication.Frequency
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMedications()
        {
            var medications = await medicationRepository.GetAllAsync();

            var response = medications.Select(medication => new MedicationDto
            {
                Id = medication.Id,
                PatientId = medication.PatientId,
                Name = medication.Name,
                Dosage = medication.Dosage,
                Frequency = medication.Frequency
            }).ToList();

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetMedicationById([FromRoute]Guid id)
        {
            var existingMedication = await medicationRepository.GetById(id);

            if(existingMedication is null)
            {
                return NotFound();
            }

            var response = new MedicationDto
            {
                Id = existingMedication.Id,
                PatientId = existingMedication.PatientId,
                Name = existingMedication.Name,
                Dosage = existingMedication.Dosage,
                Frequency = existingMedication.Frequency
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateMedication([FromRoute] Guid id, UpdateMedicationRequestDto request)
        {
            var existingMedication = await medicationRepository.GetById(id);

            if(existingMedication is null) 
            { 
                return NotFound();
            }

            existingMedication.Name = request.Name;
            existingMedication.Frequency = request.Frequency;
            existingMedication.Dosage = request.Dosage;

            var updatedMedication = await medicationRepository.UpdateAsync(existingMedication);

            if(updatedMedication is null)
            {
                return NotFound();
            }

            var response = new MedicationDto
            {
                Id = updatedMedication.Id,
                PatientId = updatedMedication.PatientId,
                Name = updatedMedication.Name,
                Dosage = updatedMedication.Dosage,
                Frequency = updatedMedication.Frequency
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteMedication([FromRoute]Guid id)
        {
            var medication = await medicationRepository.DeleteAsync(id);

            if(medication is null)
            {
                return NotFound();
            }

            var response = new MedicationDto
            {
                Id = medication.Id,
                PatientId = medication.PatientId,
                Name = medication.Name,
                Dosage = medication.Dosage,
                Frequency = medication.Frequency
            };

            return Ok(response);
        }
    }
}
