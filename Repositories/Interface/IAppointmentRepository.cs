﻿using HospitalSystem.API.Models.Domain;

namespace HospitalSystem.API.Repositories.Interface
{
    public interface IAppointmentRepository
    {
        Task<Appointment> CreateAsync(Appointment appointment);
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<Appointment?> GetById(Guid id);
        Task<Appointment?> UpdateAsync(Appointment appointment);
        Task<Appointment?> DeleteAsync(Guid id);
    }
}
