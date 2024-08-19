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
                Units = request.Units,
                Result = request.Result,
                ReferenceRange = request.ReferenceRange
            };

            await labTestRepository.CreateAsync(labTest);

            var response = new LabTestDto
            {
                Id = labTest.Id,
                LabAnalysisId = labTest.LabAnalysisId,
                TestName = labTest.TestName,
                Units = labTest.Units,
                Result = labTest.Result,
                ReferenceRange = labTest.ReferenceRange
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLabTests()
        {
            var labTests = await labTestRepository.GetAllAsync();
            var response = labTests.Select(labTest => new LabTestDto
            {
                Id = labTest.Id,
                LabAnalysisId = labTest.LabAnalysisId,
                TestName = labTest.TestName,
                Units = labTest.Units,
                Result = labTest.Result,
                ReferenceRange = labTest.ReferenceRange
            }).ToList();

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetLabTestById([FromRoute]Guid id)
        {
            var existingLabTest = await labTestRepository.GetById(id);

            if (existingLabTest is null)
            {
                return NotFound();
            }

            var response = new LabTestDto
            {
                Id = existingLabTest.Id,
                LabAnalysisId = existingLabTest.LabAnalysisId,
                TestName = existingLabTest.TestName,
                Units = existingLabTest.Units,
                Result = existingLabTest.Result,
                ReferenceRange = existingLabTest.ReferenceRange
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateLabTest([FromRoute]Guid id, UpdateLabTestRequestDto request)
        {
            var existingLabTest = await labTestRepository.GetById(id);
            if (existingLabTest == null)
            {
                return NotFound();
            }
            existingLabTest.TestName = request.TestName;
            existingLabTest.Units = request.Units;
            existingLabTest.Result = request.Result;
            existingLabTest.ReferenceRange = request.ReferenceRange;

            var updatedLabTest = await labTestRepository.UpdateAsync(existingLabTest);
            if (updatedLabTest == null)
            {
                return NotFound();
            }

            var response = new LabTestDto
            {
                Id = updatedLabTest.Id,
                TestName = updatedLabTest.TestName,
                Units = updatedLabTest.Units,
                Result = updatedLabTest.Result,
                ReferenceRange = updatedLabTest.ReferenceRange,
                LabAnalysisId = updatedLabTest.LabAnalysisId
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteLabTest([FromRoute]Guid id)
        {
            var labTest = await labTestRepository.DeleteAsync(id);
            if(labTest is null)
            {
                return NotFound();
            }

            var response = new LabTestDto
            {
                Id = labTest.Id,
                TestName = labTest.TestName,
                Units = labTest.Units,
                Result = labTest.Result,
                ReferenceRange = labTest.ReferenceRange
            };

            return Ok(response);
        }

    }
}
