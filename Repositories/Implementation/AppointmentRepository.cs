using HospitalSystem.API.Data;
using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.API.Repositories.Implementation
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext dbContext;

        public AppointmentRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Appointment> CreateAsync(Appointment appointment)
        {
            await dbContext.Appointments.AddAsync(appointment);
            await dbContext.SaveChangesAsync();
            return appointment;
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await dbContext.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .ToListAsync();
        }

        public async Task<Appointment?> GetById(Guid id)
        {
            return await dbContext.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Appointment?> UpdateAsync(Appointment appointment)
        {
            var existingAppointment = await dbContext.Appointments.FirstOrDefaultAsync(x => x.Id == appointment.Id);

            if (existingAppointment != null)
            {
                // Update only the allowed fields
                existingAppointment.DateTime = appointment.DateTime;
                existingAppointment.Status = appointment.Status;
                existingAppointment.Reason = appointment.Reason;
                existingAppointment.Details = appointment.Details;

                // Save changes
                await dbContext.SaveChangesAsync();
                return existingAppointment;
            }

            return null;
        }

        public async Task<Appointment?> DeleteAsync(Guid id)
        {
            var existingAppointment = await dbContext.Appointments.FirstOrDefaultAsync(x => x.Id == id);
            
            if(existingAppointment is null) 
            {
                return null;
            }

            dbContext.Appointments.Remove(existingAppointment);
            await dbContext.SaveChangesAsync();
            return existingAppointment;
        }

    }
}
