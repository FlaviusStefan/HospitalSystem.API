using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Models.DTO;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HospitalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentRepository appointmentRepository;
        private readonly IDoctorRepository doctorRepository;
        private readonly IPatientRepository patientRepository;

        public AppointmentsController(IAppointmentRepository appointmentRepository, IDoctorRepository doctorRepository, IPatientRepository patientRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.doctorRepository = doctorRepository;
            this.patientRepository = patientRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment(CreateAppointmentRequestDto request)
        {
            var appointment = new Appointment
            {
                DoctorId = request.DoctorId,
                PatientId = request.PatientId,
                DateTime = request.AppointmentDateTime,
                Status = request.Status,
                Reason = request.Reason,
                Details = request.Details
            };

            await appointmentRepository.CreateAsync(appointment);

            var response = await MapAppointmentToDto(appointment);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await appointmentRepository.GetAllAsync();
            var response = new List<AppointmentDto>();

            foreach (var appointment in appointments)
            {
                var appointmentDto = await MapAppointmentToDto(appointment);
                response.Add(appointmentDto);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetAppointmentById([FromRoute] Guid id)
        {
            var appointment = await appointmentRepository.GetById(id);

            if (appointment == null)
            {
                return NotFound();
            }

            var appointmentDto = await MapAppointmentToDto(appointment);
            return Ok(appointmentDto);
        }

        //[HttpPut]
        //[Route("{id:Guid}")]
        //public async Task<IActionResult> UpdateAppointment([FromRoute] Guid id, UpdateAppointmentRequestDto request)
        //{
        //    // Validate the DoctorId and PatientId using repositories
        //    var doctor = await doctorRepository.GetById(request.DoctorId);
        //    if (doctor == null)
        //    {
        //        return BadRequest("Invalid DoctorId");
        //    }

        //    var patient = await patientRepository.GetById(request.PatientId);
        //    if (patient == null)
        //    {
        //        return BadRequest("Invalid PatientId");
        //    }

        //    var appointment = await appointmentRepository.GetById(id);
        //    if (appointment == null)
        //    {
        //        return NotFound();
        //    }

        //    // Update the appointment properties
        //    appointment.DoctorId = request.DoctorId;
        //    appointment.PatientId = request.PatientId;
        //    appointment.DateTime = request.AppointmentDateTime;
        //    appointment.Status = request.Status;
        //    appointment.Reason = request.Reason;
        //    appointment.Details = request.Details;

        //    await appointmentRepository.UpdateAsync(appointment);

        //    var response = await MapAppointmentToDto(appointment);
        //    return Ok(response);
        //}

        //[HttpDelete]
        //[Route("{id:Guid}")]
        //public async Task<IActionResult> DeleteAppointment([FromRoute] Guid id)
        //{
        //    var appointment = await appointmentRepository.DeleteAsync(id);

        //    if (appointment == null)
        //    {
        //        return NotFound();
        //    }

        //    var response = await MapAppointmentToDto(appointment);
        //    return Ok(response);
        //}

        private async Task<AppointmentDto> MapAppointmentToDto(Appointment appointment)
        {
            var doctor = await doctorRepository.GetById(appointment.DoctorId);
            var patient = await patientRepository.GetById(appointment.PatientId);

            var appointmentDto = new AppointmentDto
            {
                Id = appointment.Id,
                DoctorId = appointment.DoctorId,
                DoctorFirstName = doctor?.FirstName,
                DoctorLastName = doctor?.LastName,
                PatientId = appointment.PatientId,
                PatientFirstName = patient?.FirstName,
                PatientLastName = patient?.LastName,
                AppointmentDateTime = appointment.DateTime,
                Status = appointment.Status,
                Reason = appointment.Reason,
                Details = appointment.Details
            };

            return appointmentDto;
        }
    }
}