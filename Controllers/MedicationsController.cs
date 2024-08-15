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
    }
}
