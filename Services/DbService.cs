using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zadanie.Configurations;
using zadanie.Models;
using zadanie.Models.DTO;

namespace zadanie.Services
{
    public interface IDbService
    {
        public Task<IActionResult> GetDoctors();
        public Task<IActionResult> AddDoctor(AddDoctorCommand request);
        public Task<IActionResult> DeleteDoctor(int idDoctor);
        public Task<IActionResult> ModifyDoctor(UpdateDoctorCommand request);
        public Task<IActionResult> GetPrescription(GetPrescriptionCommand request);


    }

    public class DbService
    {
        private ClinicDbContext _context;
        public DbService(ClinicDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetDoctors()
        {  
           return new OkObjectResult(await _context.Doctors.ToListAsync());
        }
        public async Task<IActionResult> AddDoctor(AddDoctorCommand request)
        {
            var doctor = new Doctor()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            };

            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
            return new OkObjectResult($"Doctor {request.FirstName} {request.LastName} was added to database");
        }
        public async Task<IActionResult> DeleteDoctor(int idDoctor)
        {
            if (!await CheckDoctor(idDoctor)) return new BadRequestObjectResult($"Doctor {idDoctor} does not exist");
            _context.Doctors.Remove(await _context.Doctors.SingleOrDefaultAsync(d => d.IdDoctor == idDoctor));
            await _context.SaveChangesAsync();

            return new OkObjectResult($"Doctor {idDoctor} was removed from database");
        }

        public async Task<IActionResult> ModifyDoctor(UpdateDoctorCommand request)
        {
            if (!await CheckDoctor(request.IdDoctor)) return new BadRequestObjectResult($"Doctor {request.IdDoctor} does not exist");
            var doctor = await _context.Doctors.SingleOrDefaultAsync(d => d.IdDoctor == request.IdDoctor);
            doctor.FirstName = request.FirstName;
            doctor.LastName = request.LastName;
            doctor.Email = request.Email;
            await _context.SaveChangesAsync();
            return new OkObjectResult($"Doctor {request.IdDoctor} was modified from database");
        }
        public async Task<IActionResult> GetPrescription(GetPrescriptionCommand request)
        {
            if (!await CheckDoctor(request.IdDoctor)) return new BadRequestObjectResult($"Doctor {request.IdDoctor} does not exist");
            if (!await CheckPatient(request.IdPatient)) return new BadRequestObjectResult($"Patient {request.IdPatient} does not exist");
            if (!await CheckMedicament(request.Medicament)) return new BadRequestObjectResult($"Medicament {request.Medicament} does not exist");


            var prescription = await _context.Prescriptions
                .Where(p => p.IdDoctor == request.IdDoctor && p.IdPatient == request.IdPatient)
                .SingleOrDefaultAsync();

            var medicament = await _context.Medicaments
                .Where(m => m.Name == request.Medicament)
                .SingleOrDefaultAsync();



            var response = await _context.Prescriptions
                .Where(p => p.IdDoctor == request.IdDoctor && p.IdPatient == request.IdPatient)
                .Select(p => new GetPrescriptionDto
                {
                    Medicament = medicament.Name,
                    Date = prescription.Date,
                    DueDate = prescription.DueDate
                }).ToListAsync();


            return new OkObjectResult(response);
        }
        private async Task<bool> CheckDoctor(int id)
        {
            return await _context.Doctors
                .AnyAsync(p => p.IdDoctor == id);
        }
        private async Task<bool> CheckPatient(int id)
        {
            return await _context.Patients
                .AnyAsync(p => p.IdPatient == id);
        }
        private async Task<bool> CheckMedicament(string name)
        {
            return await _context.Medicaments
                .AnyAsync(p => p.Name == name);
        }
    } 
}
