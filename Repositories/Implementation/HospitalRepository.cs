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
            var existinHospital = await dbContext.Hospitals.FirstOrDefaultAsync(x => x.Id == id);

            if (existinHospital is null)
            {
                return null;
            }

            dbContext.Hospitals.Remove(existinHospital);
            await dbContext.SaveChangesAsync();
            return existinHospital;
        }
    
    }
}
