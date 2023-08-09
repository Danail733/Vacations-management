using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VacationsManagement.Data.Models;

namespace VacationsManagement.Data
{
    public class VacationManagementDbContext : IdentityDbContext
    {
        public VacationManagementDbContext(DbContextOptions<VacationManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<VacationStatus> VacationStatuses { get; set; }

        public DbSet<VacationRequest> VacationRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Employee>()
                .HasOne(e => e.Manager)
                .WithMany(m => m.Employees)
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<VacationRequest>()
                .HasOne(vr => vr.Status)
                .WithMany(vs => vs.VacationRequests)
                .HasForeignKey(vr => vr.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<VacationRequest>()
                .HasOne(vr => vr.Requestor)
                .WithMany(r => r.RequestVacations)
                .HasForeignKey(vr => vr.RequestorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<VacationRequest>()
                .HasOne(vr => vr.Reviewer)
                .WithMany(r => r.RequestsToReview)
                .HasForeignKey(vr => vr.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}