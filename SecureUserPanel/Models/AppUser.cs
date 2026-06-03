using System.ComponentModel.DataAnnotations;

namespace SecureUserPanel.Models;
public class AppUser
{
    public int Id{get;set;}
    public string Email {get; set;} = null!;
    public string PasswordHash{get;set;} = null!;
    public string Role{get;set;} = null!;
    public DateTime CreatedAt{get;set;} = DateTime.UtcNow;

    public ICollection<UserNote> Notes{get;set;} = new List<UserNote>();
}