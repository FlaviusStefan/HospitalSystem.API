using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Models.DTO;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalFilesController : ControllerBase
    {
        private readonly IMedicalFileRepository medicalFileRepository;

        public MedicalFilesController(IMedicalFileRepository medicalFileRepository)
        {
            this.medicalFileRepository = medicalFileRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMedicalFile(CreateMedicalFileRequestDto request)
        {
            var medicalFile = new MedicalFile
            {
                Id = Guid.NewGuid(),
                PatientId = request.PatientId,
                FileType = request.FileType,
                FileName = request.FileName,
                FilePath = request.FilePath,
                DateUploaded = request.DateUploaded
            };

            await medicalFileRepository.CreateAsync(medicalFile);

            var response = new MedicalFileDto
            {
                Id = medicalFile.Id,
                PatientId = medicalFile.PatientId,
                FileType = medicalFile.FileType,
                FileName = medicalFile.FileName,
                FilePath = medicalFile.FilePath,
                DateUploaded = medicalFile.DateUploaded
            };
            
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMedicalFiles()
        {
            var medicalFiles = await medicalFileRepository.GetAllAsync();

            var response = medicalFiles.Select(medicalFile => new MedicalFileDto
            {
                Id = medicalFile.Id,
                PatientId = medicalFile.PatientId,
                FileType = medicalFile.FileType,
                FileName = medicalFile.FileName,
                FilePath = medicalFile.FilePath,
                DateUploaded = medicalFile.DateUploaded
            }).ToList();

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetMedicalFileById([FromRoute] Guid id)
        {
            var existingMedicalFile = await medicalFileRepository.GetById(id);

            if (existingMedicalFile is null)
            {
                return NotFound();
            }

            var response = new MedicalFileDto
            {
                Id = existingMedicalFile.Id,
                PatientId = existingMedicalFile.PatientId,
                FileType = existingMedicalFile.FileType,
                FileName = existingMedicalFile.FileName,
                FilePath = existingMedicalFile.FilePath,
                DateUploaded = existingMedicalFile.DateUploaded
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateMedicalFile([FromRoute]Guid id,UpdateMedicalFileRequestDto request)
        {
            var existingMedicalFile = await medicalFileRepository.GetById(id);

            if (existingMedicalFile is null) 
            { 
                return NotFound();
            }

            existingMedicalFile.FileName = request.FileName;

            var updatedMedicalFile = await medicalFileRepository.UpdateAsync(existingMedicalFile);
            if(updatedMedicalFile is null)
            {
                return NotFound();
            }

            var response = new MedicalFileDto
            {
                Id = updatedMedicalFile.Id,
                PatientId = updatedMedicalFile.PatientId,
                FileType = updatedMedicalFile.FileType,
                FileName = updatedMedicalFile.FileName,
                FilePath = updatedMedicalFile.FilePath,
                DateUploaded = updatedMedicalFile.DateUploaded
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteMedicalFile([FromRoute]Guid id)
        {
            var medicalFile = await medicalFileRepository.DeleteAsync(id);
            if (medicalFile is null)
            {
                return NotFound();
            }

            var response = new MedicalFileDto
            {
                Id = medicalFile.Id,
                PatientId = medicalFile.PatientId,
                FileType = medicalFile.FileType,
                FileName = medicalFile.FileName,
                FilePath = medicalFile.FilePath,
                DateUploaded = medicalFile.DateUploaded
            };

            return Ok(response);
        }
    }
}
