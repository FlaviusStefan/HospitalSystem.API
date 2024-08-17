using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Models.DTO;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabAnalysesController : ControllerBase
    {
        private readonly ILabAnalysisRepository labAnalysisRepository;

        public LabAnalysesController(ILabAnalysisRepository labAnalysisRepository)
        {
            this.labAnalysisRepository = labAnalysisRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLabAnalysis(CreateLabAnalysisRequestDto request)
        {
            var labAnalysis = new LabAnalysis
            {
                Id = Guid.NewGuid(),
                AnalysisDate = request.AnalysisDate,
                AnalysisType = request.AnalysisType,
                PatientId = request.PatientId
            };

            await labAnalysisRepository.CreateAsync(labAnalysis);

            var response = new LabAnalysisDto
            {
                Id = labAnalysis.Id,
                AnalysisDate = labAnalysis.AnalysisDate,
                AnalysisType = labAnalysis.AnalysisType,
                PatientId = labAnalysis.PatientId
            };

            return Ok(response);
        }
    }
}
