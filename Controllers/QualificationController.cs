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
    }
}
