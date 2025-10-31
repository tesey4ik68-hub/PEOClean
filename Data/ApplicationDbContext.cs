using Microsoft.EntityFrameworkCore;
using PEOcleanWPFApp.Models;

namespace PEOcleanWPFApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for entities
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ServiceAddress> ServiceAddresses { get; set; }
        public DbSet<WorkType> WorkTypes { get; set; }
        public DbSet<EmployeeServiceAddress> EmployeeServiceAddresses { get; set; }
        public DbSet<HouseWorkSchedule> HouseWorkSchedules { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<AttendanceRecordServiceAddress> AttendanceRecordServiceAddresses { get; set; }
        public DbSet<WorkReport> WorkReports { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<EmployeeAssignment> EmployeeAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<EmployeeServiceAddress>()
                .HasOne(esa => esa.Employee)
                .WithMany(e => e.EmployeeServiceAddresses)
                .HasForeignKey(esa => esa.EmployeeId);

            modelBuilder.Entity<EmployeeServiceAddress>()
                .HasOne(esa => esa.ServiceAddress)
                .WithMany(sa => sa.EmployeeServiceAddresses)
                .HasForeignKey(esa => esa.ServiceAddressId);

            modelBuilder.Entity<HouseWorkSchedule>()
                .HasOne(hws => hws.ServiceAddress)
                .WithMany(sa => sa.HouseWorkSchedules)
                .HasForeignKey(hws => hws.ServiceAddressId);

            modelBuilder.Entity<HouseWorkSchedule>()
                .HasOne(hws => hws.WorkType)
                .WithMany(wt => wt.HouseWorkSchedules)
                .HasForeignKey(hws => hws.WorkTypeId);

            modelBuilder.Entity<AttendanceRecord>()
                .HasOne(ar => ar.Employee)
                .WithMany(e => e.AttendanceRecords)
                .HasForeignKey(ar => ar.EmployeeId);

            modelBuilder.Entity<AttendanceRecord>()
                .HasOne(ar => ar.ReplacementEmployee)
                .WithMany()
                .HasForeignKey(ar => ar.ReplacementEmployeeId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<AttendanceRecordServiceAddress>()
                .HasOne(arsa => arsa.AttendanceRecord)
                .WithMany(ar => ar.AttendanceRecordServiceAddresses)
                .HasForeignKey(arsa => arsa.AttendanceRecordId);

            modelBuilder.Entity<AttendanceRecordServiceAddress>()
                .HasOne(arsa => arsa.ServiceAddress)
                .WithMany(sa => sa.AttendanceRecordServiceAddresses)
                .HasForeignKey(arsa => arsa.ServiceAddressId);

            modelBuilder.Entity<WorkReport>()
                .HasOne(wr => wr.Employee)
                .WithMany(e => e.WorkReports)
                .HasForeignKey(wr => wr.EmployeeId);

            modelBuilder.Entity<WorkReport>()
                .HasOne(wr => wr.ServiceAddress)
                .WithMany(sa => sa.WorkReports)
                .HasForeignKey(wr => wr.ServiceAddressId);

            modelBuilder.Entity<WorkReport>()
                .HasOne(wr => wr.WorkType)
                .WithMany(wt => wt.WorkReports)
                .HasForeignKey(wr => wr.WorkTypeId);

            modelBuilder.Entity<WorkReport>()
                .HasOne(wr => wr.AttendanceRecord)
                .WithMany()
                .HasForeignKey(wr => wr.AttendanceRecordId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Employee)
                .WithMany(e => e.Payments)
                .HasForeignKey(p => p.EmployeeId);

            // Configure EmployeeAssignment relationships
            modelBuilder.Entity<EmployeeAssignment>()
                .HasOne(ea => ea.Employee)
                .WithMany()
                .HasForeignKey(ea => ea.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeAssignment>()
                .HasOne(ea => ea.ServiceAddress)
                .WithMany()
                .HasForeignKey(ea => ea.ServiceAddressId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}