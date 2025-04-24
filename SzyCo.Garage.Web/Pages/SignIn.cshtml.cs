using SzyCo.Garage.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace SzyCo.Garage.Web.Pages;

[AllowAnonymous]
public class SignInModel(SignInManager<User> signInManager) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    [Required]
    [BindProperty]
    public required string Username { get; set; }

    [Required]
    [BindProperty]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var result = await signInManager.PasswordSignInAsync(Username, Password, true, true);
        if (result.Succeeded)
        {
            return LocalRedirect(ReturnUrl ?? "/");
        }

        ModelState.AddModelError(string.Empty, result.IsLockedOut ? "Account locked out" : "Invalid login attempt.");

        return Page();
    }
}
