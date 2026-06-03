using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureUserPanel.Data;
using SecureUserPanel.Models;


namespace SecureUserPanel.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly AppDbContext _db;

    public DashboardController(AppDbContext db) => _db = db;

    private int CurrentUserId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // GET /Dashboard
    public async Task<IActionResult> Index()
    {
        var notes = await _db.UserNotes
            .Where(n => n.AppUserId == CurrentUserId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        return View(notes);
    }

    // GET /Dashboard/AddNote
    [HttpGet]
    public IActionResult AddNote() => View(new NoteViewModel());

    // POST /Dashboard/AddNote
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddNote(NoteViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        _db.UserNotes.Add(new UserNote
        {
            AppUserId = CurrentUserId,
            Title = model.Title,
            Content = model.Content,
            CreatedAt = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // POST /Dashboard/DeleteNote/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteNote(int id)
    {
        var note = await _db.UserNotes
            .FirstOrDefaultAsync(n => n.Id == id && n.AppUserId == CurrentUserId);

        if (note is not null)
        {
            _db.UserNotes.Remove(note);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}