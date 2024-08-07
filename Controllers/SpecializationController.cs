using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Models.DTO;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private readonly ISpecializationRepository specializationRepository;

        public SpecializationController(ISpecializationRepository specializationRepository)
        {
            this.specializationRepository = specializationRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSpecialization(CreateSpecializationRequestDto request)
        {
            var specialization = new Specialization
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description
            };

            await specializationRepository.CreateAsync(specialization);

            var response = new SpecializationDto
            {
                Id = specialization.Id,
                Name = specialization.Name,
                Description = specialization.Description
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSpecializations()
        {
            var specializations = await specializationRepository.GetAllAsync();

            var response = specializations.Select(specialization => new SpecializationDto
            {
                Id = specialization.Id,
                Name = specialization.Name,
                Description = specialization.Description
            }).ToList();

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetSpecializationById([FromRoute]Guid id)
        {
            var existingSpecialization = await specializationRepository.GetById(id);
            if (existingSpecialization is null) 
            {
                return NotFound();
            }

            var response = new SpecializationDto
            {
                Id = existingSpecialization.Id,
                Name = existingSpecialization.Name,
                Description = existingSpecialization.Description
            };

            return Ok(response);

        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateSpecialization([FromRoute]Guid id, UpdateSpecializationRequestDto request)
        {
            var existingSpecialization = await specializationRepository.GetById(id);
            if(existingSpecialization == null)
            {
                return NotFound();
            }
            existingSpecialization.Description = request.Description;

            var updatedSpecialization = await specializationRepository.UpdateAsync(existingSpecialization);
            if(updatedSpecialization == null) 
            { 
                return NotFound();
            }

            var response = new SpecializationDto
            {
                Id = updatedSpecialization.Id,
                Name = updatedSpecialization.Name,
                Description = updatedSpecialization.Description
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteSpecialization([FromRoute] Guid id)
        {
            var specialization = await specializationRepository.DeleteAsync(id);

            if(specialization is null)
            {
                return NotFound();
            }

            var response = new SpecializationDto
            {
                Id = specialization.Id,
                Name = specialization.Name,
                Description = specialization.Description
            };

            return Ok(response);
        }
    }
}
