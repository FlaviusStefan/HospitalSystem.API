﻿namespace HospitalSystem.API.Models.DTO
{
    public class CreateAppointmentRequestDto
    {
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string Details { get; set; }
    }
}
