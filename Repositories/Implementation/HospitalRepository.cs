﻿using HospitalSystem.API.Data;
using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HospitalSystem.API.Repositories.Implementation
{
    public class HospitalRepository : IHospitalRepository
    {
        private readonly ApplicationDbContext dbContext;

        public HospitalRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Hospital> CreateAsync(Hospital hospital)
        {
            await dbContext.Hospitals.AddAsync(hospital);
            await dbContext.SaveChangesAsync();

            return hospital;
        }

        public async Task<IEnumerable<Hospital>> GetAllAsync()
        {
            return await dbContext.Hospitals.ToListAsync();
        }

        public async Task<Hospital?> GetById(Guid id)
        {
            return await dbContext.Hospitals.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Hospital?> UpdateAsync(Hospital hospital)
        {
            var existinHospital = await dbContext.Hospitals.FirstOrDefaultAsync(x => x.Id == hospital.Id);

            if (existinHospital != null)
            {
                dbContext.Entry(existinHospital).CurrentValues.SetValues(hospital);
                await dbContext.SaveChangesAsync();
                return hospital;
            }

            return null;
        }
        public async Task<Hospital?> DeleteAsync(Guid id)
        {
            var existingHospital = await dbContext.Hospitals
                .Include(h => h.Address)
                .Include(h => h.Contact)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingHospital is null)
            {
                return null;
            }

            dbContext.Hospitals.Remove(existingHospital);
            await dbContext.SaveChangesAsync();
            return existingHospital;
        }

        public async Task<Hospital?> DeleteWithAddressAndContactAsync(Guid id)
        {
            var existingHospital = await dbContext.Hospitals
                .Include(h => h.Address)
                .Include(h => h.Contact)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingHospital == null)
            {
                return null;
            }

            // Delete related address and contact if they are not associated with other hospitals
            if (existingHospital.Address != null && !dbContext.Hospitals.Any(h => h.AddressId == existingHospital.Address.Id && h.Id != id))
            {
                dbContext.Addresses.Remove(existingHospital.Address);
            }

            if (existingHospital.Contact != null && !dbContext.Hospitals.Any(h => h.ContactId == existingHospital.Contact.Id && h.Id != id))
            {
                dbContext.Contacts.Remove(existingHospital.Contact);
            }

            // Delete the hospital
            dbContext.Hospitals.Remove(existingHospital);

            await dbContext.SaveChangesAsync();

            return existingHospital;
        }


    }
}
