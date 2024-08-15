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
        public async Task<IActionResult> CreateMedicalFile(CreateMedicaltFileRequestDto request)
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
    }
}
