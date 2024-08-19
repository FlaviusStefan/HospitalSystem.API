using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Models.DTO;
using HospitalSystem.API.Repositories.Interface;
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

        [HttpGet]
        public async Task<IActionResult> GetAllLabAnalyses()
        {
            var labAnalyses = await labAnalysisRepository.GetAllAsync();
            var response = labAnalyses.Select(labAnalysis => new LabAnalysisDto
            {
                Id = labAnalysis.Id,
                AnalysisDate = labAnalysis.AnalysisDate,
                AnalysisType = labAnalysis.AnalysisType,
                PatientId = labAnalysis.PatientId,
                LabTestIds = labAnalysis.LabTests.Select(lt => lt.Id).ToList()
            }).ToList();

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetLabAnalysisById([FromRoute]Guid id)
        {
            var existingLabAnalysis = await labAnalysisRepository.GetById(id);

            if(existingLabAnalysis is null)
            {
                return NotFound();
            }

            var response = new LabAnalysisDto
            {
                Id = existingLabAnalysis.Id,
                AnalysisDate = existingLabAnalysis.AnalysisDate,
                AnalysisType = existingLabAnalysis.AnalysisType,
                PatientId = existingLabAnalysis.PatientId,
                LabTestIds = existingLabAnalysis.LabTests.Select(lt => lt.Id).ToList()
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateLabAnalysis([FromRoute] Guid id, [FromBody] UpdateLabAnalysisRequestDto request)
        {
            var existingLabAnalysis = await labAnalysisRepository.GetById(id);
            if (existingLabAnalysis == null)
            {
                return NotFound();
            }

            // Update basic properties
            existingLabAnalysis.AnalysisDate = request.AnalysisDate;
            existingLabAnalysis.AnalysisType = request.AnalysisType;

            var existingLabTestIds = existingLabAnalysis.LabTests.Select(lt => lt.Id).ToList();
            var newLabTestIds = request.LabTestIds ?? new List<Guid>();

            // Remove specializations that are no longer in the request
            foreach (var labTestId in existingLabTestIds)
            {
                if (!newLabTestIds.Contains(labTestId))
                {
                    var labTestToRemove = existingLabAnalysis.LabTests
                        .FirstOrDefault(ds => ds.Id == labTestId);
                    if (labTestToRemove != null)
                    {
                        existingLabAnalysis.LabTests.Remove(labTestToRemove);
                    }
                }
            }

            // Add new specializations from the request
            foreach (var labTestId in newLabTestIds)
            {
                if (!existingLabTestIds.Contains(labTestId))
                {
                    existingLabAnalysis.LabTests.Add(new LabTest
                    {
                        Id = labTestId
                    });
                }
            }


            await labAnalysisRepository.UpdateAsync(existingLabAnalysis);

            var response = new LabAnalysisDto
            {
                Id = existingLabAnalysis.Id,
                AnalysisDate = existingLabAnalysis.AnalysisDate,
                AnalysisType = existingLabAnalysis.AnalysisType,
                PatientId = existingLabAnalysis.PatientId,
                LabTestIds = existingLabAnalysis.LabTests.Select(lt => lt.Id).ToList()
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> DeleteLabAnalysis([FromRoute] Guid id)
        {
            var labAnalysis = await labAnalysisRepository.DeleteAsync(id);

            if(labAnalysis is null)
            {
                return NotFound();
            }

            var response = new LabAnalysisDto
            {
                Id = labAnalysis.Id,
                AnalysisDate = labAnalysis.AnalysisDate,
                AnalysisType = labAnalysis.AnalysisType,
                PatientId = labAnalysis.PatientId,
                LabTestIds = labAnalysis.LabTests.Select(lt => lt.Id).ToList()
            };

            return Ok(response);
        }


    }
}
