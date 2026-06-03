namespace SecureUserPanel.Data;

using Microsoft.EntityFrameworkCore;
using SecureUserPanel.Models;
public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options){ }

    public DbSet<AppUser> AppUsers=> Set<AppUser>();
    public DbSet<UserNote> UserNotes => Set<UserNote>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>()
        .HasIndex(u => u.Email)
        .IsUnique();

        modelBuilder.Entity<UserNote>()
        .HasOne(n => n.User)
        .WithMany(u => u.Notes)
        .HasForeignKey(n => n.AppUserId)
        .OnDelete(DeleteBehavior.Cascade);
    }

}