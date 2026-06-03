using System.ComponentModel.DataAnnotations;
namespace SecureUserPanel.Models;

public class NoteViewModel
{
    [Required]
    [MaxLength(200)]
    public string Title{get;set;} = string.Empty;

    [Required]
    public string Content {get;set;} = string.Empty;
    

}