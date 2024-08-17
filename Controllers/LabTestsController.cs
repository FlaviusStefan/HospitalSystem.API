using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Models.DTO;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabTestsController : ControllerBase
    {
        private readonly ILabTestRepository labTestRepository;

        public LabTestsController(ILabTestRepository labTestRepository)
        {
            this.labTestRepository = labTestRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLabTest(CreateLabTestRequestDto request)
        {
            var labTest = new LabTest
            {
                Id = Guid.NewGuid(),
                LabAnalysisId = request.LabAnalysisId,
                TestName = request.TestName,
                Result = request.Result,
                Units = request.Units,
                ReferenceRange = request.ReferenceRange
            };

            await labTestRepository.CreateAsync(labTest);

            var response = new LabTestDto
            {
                Id = labTest.Id,
                LabAnalysisId = labTest.LabAnalysisId,
                TestName = labTest.TestName,
                Result = labTest.Result,
                Units = labTest.Units,
                ReferenceRange = labTest.ReferenceRange
            };

            return Ok(response);
        }
    }
}
