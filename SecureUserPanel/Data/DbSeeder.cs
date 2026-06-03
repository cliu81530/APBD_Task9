using SecureUserPanel.Data;
using SecureUserPanel.Models;



namespace SecureUserPanel.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (db.AppUsers.Any()) return;

        db.AppUsers.AddRange(
            new AppUser
            {
                Email = "admin@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin1234!"),
                Role = "Admin",
                CreatedAt = DateTime.UtcNow
            },
            new AppUser
            {
                Email = "user@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("User1234!"),
                Role = "User",
                CreatedAt = DateTime.UtcNow
            }
        );

        db.SaveChanges();
    }
}