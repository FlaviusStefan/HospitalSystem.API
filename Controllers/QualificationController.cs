using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Models.DTO;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QualificationController : ControllerBase
    {
        private readonly IQualificationRepository qualificationRepository;

        public QualificationController(IQualificationRepository qualificationRepository)
        {
            this.qualificationRepository = qualificationRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateQualification(CreateQualificationRequestDto request)
        {
            var qualification = new Qualification
            {
                Id = Guid.NewGuid(),
                DoctorId = request.DoctorId,
                Degree = request.Degree,
                Institution = request.Institution,
                StudiedYears = request.StudiedYears,
                YearOfCompletion = request.YearOfCompletion
            };

            await qualificationRepository.CreateAsync(qualification);

            var response = new QualificationDto
            {
                Id = qualification.Id,
                DoctorId = qualification.DoctorId,
                Degree = qualification.Degree,
                Institution = qualification.Institution,
                StudiedYears = qualification.StudiedYears,
                YearOfCompletion = qualification.YearOfCompletion
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllQualifications()
        {
            var qualifications = await qualificationRepository.GetAllAsync();

            var response = qualifications.Select(qualification => new QualificationDto
            {
                Id = qualification.Id,
                DoctorId = qualification.DoctorId,
                Degree = qualification.Degree,
                Institution = qualification.Institution,
                StudiedYears = qualification.StudiedYears,
                YearOfCompletion = qualification.YearOfCompletion
            }).ToList();
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetQualificationById([FromRoute] Guid id)
        {
            var existingQualification = await qualificationRepository.GetById(id);
            if (existingQualification is null)
            {
                return NotFound();
            }

            var response = new QualificationDto
            {
                Id = existingQualification.Id,
                DoctorId = existingQualification.DoctorId,
                Degree = existingQualification.Degree,
                Institution = existingQualification.Institution,
                StudiedYears = existingQualification.StudiedYears,
                YearOfCompletion = existingQualification.YearOfCompletion
            };

            return Ok(response);
        }        

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateQualification([FromRoute] Guid id, UpdateQualificationRequestDto request)
        {
            var existingQualification = await qualificationRepository.GetById(id);
            if (existingQualification == null)
            {
                return NotFound();
            }
            existingQualification.Degree = request.Degree;
            existingQualification.Institution = request.Institution;

            var updatedQualification = await qualificationRepository.UpdateAsync(existingQualification);
            if (updatedQualification == null)
            {
                return NotFound();
            }

            var response = new QualificationDto
            {
                Id = updatedQualification.Id,
                DoctorId = updatedQualification.DoctorId,
                Degree = updatedQualification.Degree,
                Institution = updatedQualification.Institution,
                StudiedYears = updatedQualification.StudiedYears,
                YearOfCompletion = updatedQualification.YearOfCompletion
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteQualification([FromRoute] Guid id)
        {
            var qualification = await qualificationRepository.DeleteAsync(id);

            if(qualification is null)
            {
                return NotFound();
            }

            var response = new QualificationDto
            {
                Id = qualification.Id,
                DoctorId = qualification.DoctorId,
                Degree = qualification.Degree,
                Institution = qualification.Institution,
                StudiedYears= qualification.StudiedYears,
                YearOfCompletion= qualification.YearOfCompletion
            };

            return Ok(response);
        }

    }
}
