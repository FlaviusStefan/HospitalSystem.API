﻿using HospitalSystem.API.Data;
using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.API.Repositories.Implementation
{
    public class DoctorRepository : IDoctorRepository
    {



        //private readonly ApplicationDbContext dbContext;

        //public DoctorRepository(ApplicationDbContext dbContext)
        //{
        //    this.dbContext = dbContext;
        //}

        //public async Task<Doctor> CreateAsync(Doctor doctor)
        //{
        //    await dbContext.Doctors.AddAsync(doctor);
        //    await dbContext.SaveChangesAsync();

        //    return doctor;
        //}    

        private readonly ApplicationDbContext dbContext;
        private readonly IAddressRepository addressRepository;
        private readonly IContactRepository contactRepository;

        public DoctorRepository(ApplicationDbContext dbContext, IAddressRepository addressRepository, IContactRepository contactRepository)
        {
            this.dbContext = dbContext;
            this.addressRepository = addressRepository;
            this.contactRepository = contactRepository;
        }

        public async Task<Doctor> CreateAsync(Doctor doctor, Address address, Contact contact)
        {
            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // Save address first if provided
                    if (address != null)
                    {
                        address.Id = Guid.NewGuid();
                        await addressRepository.CreateAsync(address);

                        // Now associate the address with the doctor
                        doctor.AddressId = address.Id;
                    }

                    // Save contact next if provided
                    if (contact != null)
                    {
                        contact.Id = Guid.NewGuid();
                        await contactRepository.CreateAsync(contact);

                        // Now associate the contact with the doctor
                        doctor.ContactId = contact.Id;
                    }

                    // Save the doctor last
                    dbContext.Doctors.Add(doctor);
                    await dbContext.SaveChangesAsync();

                    // Commit transaction
                    await transaction.CommitAsync();

                    return doctor;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    // Log the exception in detail
                    Console.WriteLine($"Error creating doctor: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                        Console.WriteLine($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                    }

                    throw; // Re-throw the exception to be handled at a higher level
                }
            }
        }




        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await dbContext.Doctors
                .Include(d => d.Address) 
                .Include(d => d.Contact)
                .Include(d => d.DoctorSpecializations)
                .Include(d => d.Qualifications)
                .Include(d => d.DoctorHospitals)
                .Include(d => d.Appointments)
                .Include(d => d.Patients)
                .ToListAsync();

        }

        public async Task<Doctor?> GetById(Guid id)
        {
            return await dbContext.Doctors
                .Include(d => d.Address)
                .Include(d => d.Contact)
                .Include(d => d.DoctorSpecializations)
                .Include(d => d.Qualifications)
                .Include(d => d.DoctorHospitals)
                .Include(d => d.Appointments)
                .Include(d => d.Patients)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        
        public async Task<Doctor?> UpdateAsync(Doctor doctor)
        {
            var existingDoctor = await dbContext.Doctors.FirstOrDefaultAsync(x => x.Id == doctor.Id);

            if (existingDoctor != null)
            {
                dbContext.Entry(existingDoctor).CurrentValues.SetValues(doctor);
                await dbContext.SaveChangesAsync();
                return doctor;
            }

            return null;
        }

        public async Task<Doctor?> DeleteAsync(Guid id)
        {
            var existingDoctor = await dbContext.Doctors.FirstOrDefaultAsync(x => x.Id == id);

            if (existingDoctor is null)
            {
                return null;
            }

            dbContext.Doctors.Remove(existingDoctor);
            await dbContext.SaveChangesAsync();
            return existingDoctor;
        }



    }
}
