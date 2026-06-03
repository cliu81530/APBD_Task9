using System.ComponentModel.DataAnnotations;

namespace SecureUserPanel.Models;
public class RegisterViewModel
{
    [Required][EmailAddress]
    public string Email{get;set;} = string.Empty;
    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    [DataType(DataType.Password)]
    public string Password{get;set;} = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
    
}