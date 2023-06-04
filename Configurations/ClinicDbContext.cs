using Microsoft.EntityFrameworkCore;
using zadanie.Models;

namespace zadanie.Configurations
{
    public interface IClinicDbContext
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    public class ClinicDbContext : DbContext, IClinicDbContext
    {
        private IConfiguration _configuration;
        public ClinicDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ClinicDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Default"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Doctor>().ToTable("Doctor");
            modelBuilder.Entity<Doctor>().HasKey(e => e.IdDoctor);
            modelBuilder.Entity<Doctor>().Property(e => e.IdDoctor).ValueGeneratedOnAdd();
            modelBuilder.Entity<Doctor>().Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Doctor>().Property(e => e.LastName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Doctor>().Property(e => e.Email).IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Medicament>().ToTable("Medicament");
            modelBuilder.Entity<Medicament>().HasKey(e => e.IdMedicament);
            modelBuilder.Entity<Medicament>().Property(e => e.IdMedicament).ValueGeneratedOnAdd();
            modelBuilder.Entity<Medicament>().Property(e => e.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Medicament>().Property(e => e.Description).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Medicament>().Property(e => e.Type).IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Patient>().ToTable("Patient");
            modelBuilder.Entity<Patient>().HasKey(e => e.IdPatient);
            modelBuilder.Entity<Patient>().Property(e => e.IdPatient).ValueGeneratedOnAdd();
            modelBuilder.Entity<Patient>().Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Patient>().Property(e => e.LastName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Patient>().Property(e => e.BirthDate).IsRequired().HasColumnType("datetime");

            modelBuilder.Entity<Prescription>().ToTable("Prescription");
            modelBuilder.Entity<Prescription>().HasKey(e => e.IdPrescription);
            modelBuilder.Entity<Prescription>().Property(e => e.IdPrescription).ValueGeneratedOnAdd();
            modelBuilder.Entity<Prescription>().Property(e => e.Date).IsRequired().HasColumnType("datetime");
            modelBuilder.Entity<Prescription>().Property(e => e.DueDate).IsRequired().HasColumnType("datetime");
            modelBuilder.Entity<Prescription>().HasOne(e => e.Patient)
                .WithMany(m => m.Prescriptions)
                .HasForeignKey(m => m.IdPatient)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Prescription>().HasOne(e => e.Doctor)
                .WithMany(m => m.Prescriptions)
                .HasForeignKey(m => m.IdDoctor)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PrescriptionMedicament>().ToTable("Prescription_Medicament");
            modelBuilder.Entity<PrescriptionMedicament>().HasKey(e => new { e.IdPrescription, e.IdMedicament });
            modelBuilder.Entity<PrescriptionMedicament>().Property(e => e.Details).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<PrescriptionMedicament>().HasOne(e => e.Prescription)
                .WithMany(m => m.PrescrptionMedicaments)
                .HasForeignKey(m => m.IdPrescription)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PrescriptionMedicament>().HasOne(e => e.Medicament)
                .WithMany(m => m.PrescrptionMedicaments)
                .HasForeignKey(m => m.IdMedicament)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Seed();
        }
    }
}