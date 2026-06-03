using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureUserPanel.Data;

namespace SecureUserPanel.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly AppDbContext _db;

    public AdminController(AppDbContext db) => _db = db;

    // GET /Admin
    public async Task<IActionResult> Index()
    {
        var users = await _db.AppUsers
            .OrderBy(u => u.CreatedAt)
            .ToListAsync();

        return View(users);
    }
}