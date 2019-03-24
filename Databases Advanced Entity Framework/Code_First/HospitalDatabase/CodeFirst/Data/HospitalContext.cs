using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }

        public DbSet<Visitation> Visitations { get; set; }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<PatientMedicament> PatientMedicaments { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config.connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigfurePatientEntity(modelBuilder);
            ConfigfureVisitationEntity(modelBuilder);
            ConfigfureDiagnoseEntity(modelBuilder);
            ConfigfureMedicamentEntity(modelBuilder);
            ConfigfurePatientMedicamentEntity(modelBuilder);
            ConfigfureDoctorEntity(modelBuilder);
        }

        private void ConfigfureDoctorEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Doctor>()
                .HasKey(p => p.DoctorId);

            modelBuilder
                .Entity<Doctor>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsUnicode();

            modelBuilder
                .Entity<Doctor>()
                .Property(p => p.Specialty)
                .HasMaxLength(100)
                .IsUnicode();

            modelBuilder
                .Entity<Doctor>()
                .HasMany(p => p.Visitations)
                .WithOne(d => d.Doctor);

        }

        private void ConfigfurePatientMedicamentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<PatientMedicament>()
                .HasKey(p => new {p.PatientId, p.MedicamentId});

            modelBuilder
                .Entity<PatientMedicament>()
                .HasOne(bc => bc.Patient)
                .WithMany(c => c.Prescriptions)
                .HasForeignKey(bc => bc.PatientId);

            modelBuilder
                .Entity<PatientMedicament>()
                .HasOne(bc => bc.Medicament)
                .WithMany(c => c.Prescriptions)
                .HasForeignKey(bc => bc.MedicamentId);
        }

        private void ConfigfureMedicamentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Medicament>()
                .HasKey(p => p.MedicamentId);

            modelBuilder
                .Entity<Medicament>()
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsUnicode();
        }

        private void ConfigfureDiagnoseEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Diagnose>()
                .HasKey(p => p.DiagnoseId);

            modelBuilder
                .Entity<Diagnose>()
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsUnicode();

            modelBuilder
                .Entity<Diagnose>()
                .Property(p => p.Comments)
                .HasMaxLength(250)
                .IsUnicode();
        }

        private void ConfigfureVisitationEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Visitation>()
                .HasKey(p => p.VisitationId);

            modelBuilder
                .Entity<Visitation>()
                .Property(p => p.Comments)
                .HasMaxLength(250)
                .IsUnicode();

            modelBuilder
                .Entity<Visitation>()
                .HasOne(p => p.Patient);

        }

        private void ConfigfurePatientEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Patient>()
                .HasKey(p => p.PatientId);

            modelBuilder
                .Entity<Patient>()
                .Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsUnicode();

            modelBuilder
                .Entity<Patient>()
                .Property(p => p.LastName)
                .HasMaxLength(50)
                .IsUnicode();

            modelBuilder
                .Entity<Patient>()
                .Property(p => p.Address)
                .HasMaxLength(250)
                .IsUnicode();

            modelBuilder
                .Entity<Patient>()
                .Property(p => p.Email)
                .HasMaxLength(80);

            modelBuilder
                .Entity<Patient>()
                .HasMany(p => p.Visitations)
                .WithOne(p => p.Patient);

            modelBuilder
                .Entity<Patient>()
                .HasMany(p => p.Diagnoses)
                .WithOne(p => p.Patient);

        }
    }
}
