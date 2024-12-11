using System.Text;
using IdentityDataModels;
using IdentityServer.EF.DataAccess;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace IdentityServerAspNetIdentity.Pages.Admin.Accounts;

public class Edit : PageModel
{
    private readonly IUserRepository _userRepository;
    private readonly IRolesRepository _rolesRepository;
    private readonly IClaimsRepository _claimsRepository;
    private readonly UserManager<ApplicationUser> _userManager; 
    private readonly IEmailSender _emailSender;
    
  public Edit(IUserRepository userRepository, IRolesRepository rolesRepository, IClaimsRepository claimsRepository, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
    {
        _userRepository = userRepository;
        _rolesRepository = rolesRepository;
        _claimsRepository = claimsRepository;
        _userManager = userManager;
        _emailSender = emailSender;
    }


    [BindProperty(SupportsGet = true)] public string Id { get; set; }

    [BindProperty] public ApplicationUserDetails User { get; set; }

    [BindProperty] public List<string> AllRoles { get; set; }

    [BindProperty] public List<UserClaim> AllClaims { get; set; }

    [BindProperty] public List<string> SelectedRoles { get; set; }

    [BindProperty]
    public List<string> AvailableRoles
        => AllRoles.Except(User.Roles).ToList();

    [BindProperty]
    public List<UserClaim> AvailableClaims
        => AllClaims.Where(c => User.UserClaims.All(uc => uc.ClaimType != c.ClaimType)).ToList();


    [BindProperty] public List<string> ClaimsDeleted { get; set; }

    // [BindProperty]
    // public List<string> AllClaims { get => User.UserClaims.Select(uc => uc.ClaimType).Union(AvailableClaims.Select(c => c.ClaimType)).ToList(); }
    //

    public async Task<IActionResult> OnGetAsync()
    {
        User = await _userRepository.GetUserById(Id);


        AllRoles = (await _rolesRepository.GetRoles()).Select(r => r.Name).ToList();
        AllClaims = await _claimsRepository.GetClaims();

        //        System.Security.Claims.ClaimTypes
        if (User == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        foreach (var deletedClaim in ClaimsDeleted)
        {
            if (User.UserClaims.Any(uc => uc.ClaimType == deletedClaim))
            {
                User.UserClaims.Remove(User.UserClaims.First(uc => uc.ClaimType == deletedClaim));
            }
        }


        var result = await _userRepository.UpdateUser(User);

        if (!result)
        {
            // Handle update failure
            return Page();
        }

        return RedirectToPage("/Admin/Accounts/Details", new {Id});
    }

    public async Task<IActionResult> OnPostLockAsync(DateTime until)
    {
        var result = await _userRepository.LockUser(Id, until);
        if (!result)
        {
            return new JsonResult(new {success = false, message = "Failed to lock user"});
        }

        return new JsonResult(new {success = true, message = "User Locked"});
    }

    public async Task<IActionResult> OnPostUnLockAsync()
    {
        var result = await _userRepository.UnlockUser(Id);
        if (!result)
        {
            return new JsonResult(new {success = false, message = "Failed to unlock user"});
        }

        return new JsonResult(new {success = true, message = "User UnLocked"});
    }


    public async Task<IActionResult> OnPostResendConfirmationEmailAsync()
    {
        var user = await _userManager.FindByIdAsync(Id);
        if (user == null)
        {
            return NotFound();
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var confirmationLink = Url.Page(
            "/Account/ConfirmEmail/Index",
            pageHandler: null,
            values: new {userId = user.Id, code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token))},
            protocol: Request.Scheme);

        await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
            $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

        return new JsonResult(new {success = true, message = "Confirmation email sent"});
    }

    public async Task<IActionResult> OnPostSendResetPasswordLinkAsync()
    {
        var user = await _userManager.FindByIdAsync(Id);
        if (user == null)
        {
            return NotFound();
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var resetPasswordLink = Url.Page(
            "/Account/ResetPassword/Index",
            pageHandler: null,
            values: new {userId = user.Id, token = code},
            protocol: Request.Scheme);

        await _emailSender.SendEmailAsync(user.Email, "Reset your password",
            $"Please reset your password by <a href='{resetPasswordLink}'>clicking here</a>.");

        return new JsonResult(new {success = true, message = "Reset password link sent"});
    }
}