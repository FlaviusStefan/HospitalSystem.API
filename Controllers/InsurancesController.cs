using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Models.DTO;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsurancesController : ControllerBase
    {
        private readonly IInsuranceRepository insuranceRepository;

        public InsurancesController(IInsuranceRepository insuranceRepository) 
        {
            this.insuranceRepository = insuranceRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInsurance(CreateInsuranceRequestDto request)
        {
            var insurance = new Insurance
            {
                Id = Guid.NewGuid(),
                Provider = request.Provider,
                PolicyNumber = request.PolicyNumber,
                CoverageStartDate = request.CoverageStartDate,
                CoverageEndDate = request.CoverageEndDate,
                PatientId = request.PatientId
            };

            await insuranceRepository.CreateAsync(insurance);

            var response = new InsuranceDto
            {
                Id = insurance.Id,
                Provider = insurance.Provider,
                PolicyNumber = insurance.PolicyNumber,
                CoverageStartDate = insurance.CoverageStartDate,
                CoverageEndDate = insurance.CoverageEndDate,
                PatientId = insurance.PatientId
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInsurances()
        {
            var insurances = await insuranceRepository.GetAllAsync();

            var response = insurances.Select(insurance => new InsuranceDto
            {
                Id = insurance.Id,
                Provider = insurance.Provider,
                PolicyNumber = insurance.PolicyNumber,
                CoverageStartDate = insurance.CoverageStartDate,
                CoverageEndDate = insurance.CoverageEndDate,
                PatientId = insurance.PatientId

            }).ToList();

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetInsuranceById([FromRoute] Guid id)
        {
            var existingInsurance = await insuranceRepository.GetById(id);

            if(existingInsurance is null)
            {
                return NotFound();
            }

            var response = new InsuranceDto
            {
                Id = existingInsurance.Id,
                Provider = existingInsurance.Provider,
                PolicyNumber = existingInsurance.PolicyNumber,
                CoverageStartDate = existingInsurance.CoverageStartDate,
                CoverageEndDate = existingInsurance.CoverageEndDate,
                PatientId = existingInsurance.PatientId
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateInsurance([FromRoute] Guid id,UpdateInsuranceRequestDto request)
        {
            var existingInsurance = await insuranceRepository.GetById(id);
            if (existingInsurance == null)
            {
                return NotFound();
            }
            existingInsurance.Provider = request.Provider;
            existingInsurance.PolicyNumber = request.PolicyNumber;

            var updatedInsurance = await insuranceRepository.UpdateAsync(existingInsurance);
            if (updatedInsurance == null)
            {
                return NotFound();
            }

            var response = new InsuranceDto
            {
                Id = updatedInsurance.Id,
                Provider = updatedInsurance.Provider,
                PolicyNumber = updatedInsurance.PolicyNumber,
                CoverageStartDate = updatedInsurance.CoverageStartDate,
                CoverageEndDate = updatedInsurance.CoverageEndDate,
                PatientId = updatedInsurance.PatientId
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteInsurance([FromRoute] Guid id)
        {
            var insurance = await insuranceRepository.DeleteAsync(id);

            if(insurance is null)
            {
                return NotFound();
            }

            var response = new InsuranceDto
            {
                Id = insurance.Id,
                Provider = insurance.Provider,
                PolicyNumber = insurance.PolicyNumber,
                CoverageStartDate = insurance.CoverageStartDate,
                CoverageEndDate = insurance.CoverageEndDate
            };

            return Ok(response);
        }

    }
}
