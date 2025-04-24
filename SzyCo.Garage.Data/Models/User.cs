using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace SzyCo.Garage.Data.Models;

[Edit(AllowAuthenticated)]
[Create(DenyAll)]
[Delete(DenyAll)]
[Description("A user profile within the application.")]
public class User : IdentityUser
{
    [Search(SearchMethod = SearchMethods.Contains)]
    [ListText]
    public string? FullName { get; set; }

    [Search]
    public override string? UserName { get; set; }

    [Read] // Email readonly - sourced from external identity providers
    public override string? Email { get; set; }

    [Read]
    public override bool EmailConfirmed { get; set; }




    [InternalUse]
    public override string? NormalizedUserName { get; set; }

    [InternalUse]
    public override string? PasswordHash { get; set; }

    [InternalUse]
    public override string? SecurityStamp { get; set; }

    [InternalUse]
    public override string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    [InternalUse]
    public override string? PhoneNumber { get; set; }
    [InternalUse]
    public override bool PhoneNumberConfirmed { get; set; }

    [InternalUse]
    public override string? NormalizedEmail { get; set; }

    [InternalUse]
    public override bool TwoFactorEnabled { get; set; }

    [InternalUse]
    public override int AccessFailedCount { get; set; }

    [Description("If set, the user will be blocked from signing in until this date.")]
    [Read(nameof(Permission.UserAdmin)), Edit(nameof(Permission.UserAdmin))]
    public override DateTimeOffset? LockoutEnd { get; set; }

    [Description("If enabled, the user can be locked out.")]
    [Read(nameof(Permission.UserAdmin)), Edit(nameof(Permission.UserAdmin))]
    public override bool LockoutEnabled { get; set; }

    [Read(nameof(Permission.UserAdmin), NoAutoInclude = true)]
    [InverseProperty(nameof(UserRole.User))]
    [ManyToMany("Roles")]
    [Hidden]
    public ICollection<UserRole>? UserRoles { get; set; }

    [Display(Name = "Roles")]
    [Read(nameof(Permission.UserAdmin))]
    public IEnumerable<string>? RoleNames => UserRoles?.Where(ur => ur.Role != null).Select(r => r.Role!.Name!);




    [Coalesce]
    public async Task<ItemResult> SetEmail(
        [Inject] UserManagementService userService,
        ClaimsPrincipal currentUser,
        [DataType(DataType.EmailAddress)] string newEmail
    )
    {
        if (currentUser.GetUserId() != this.Id && !currentUser.Can(Permission.UserAdmin)) return "Unauthorized.";
        return await userService.SendEmailChangeRequest(this, newEmail);
    }

    [Coalesce]
    public async Task<ItemResult> SendEmailConfirmation(
        [Inject] UserManagementService userService,
        ClaimsPrincipal currentUser
    )
    {
        if (currentUser.GetUserId() != this.Id && !currentUser.Can(Permission.UserAdmin)) return "Unauthorized.";
        return await userService.SendEmailConfirmationRequest(this);
    }

    [Coalesce]
    public async Task<ItemResult> SetPassword(
        [Inject] UserManager<User> userManager,
        [Inject] SignInManager<User> signInManager,
        ClaimsPrincipal currentUser,
        [DataType(DataType.Password)] string? currentPassword,
        [DataType(DataType.Password)] string newPassword,
        [DataType(DataType.Password)] string confirmNewPassword
    )
    {
        if (currentUser.GetUserId() != this.Id) return "Unauthorized.";

        if (newPassword != confirmNewPassword) return "New passwords must match";

        var result = this.PasswordHash is null
            ? await userManager.AddPasswordAsync(this, newPassword)
            : await userManager.ChangePasswordAsync(this, currentPassword ?? "", newPassword);

        if (!result.Succeeded)
        {
            return string.Join("; ", result.Errors.Select(e => e.Description));
        }

        if (currentUser.GetUserId() == this.Id)
        {
            await signInManager.RefreshSignInAsync(this);
        }
        return new ItemResult(true, $"Password was successfully changed.");
    }

    [DefaultDataSource]
    public class DefaultSource(CrudContext<AppDbContext> context) : AppDataSource<User>(context)
    {
        public override IQueryable<User> GetQuery(IDataSourceParameters parameters)
        {
            var query = base.GetQuery(parameters);
            if (User.Can(Permission.UserAdmin))
            {
                query = query.Include(u => u.UserRoles!).ThenInclude(ur => ur.Role);
            }

            return query;
        }
    }

    public class UserBehaviors(
        CrudContext<AppDbContext> context,
        UserManager<User> userManager,
        SignInManager<User> signInManager
    ) : AppBehaviors<User>(context)
    {
        public override ItemResult BeforeSave(SaveKind kind, User? oldItem, User item)
        {
            // Users who aren't user admins can only edit their own profile.
            if (item.Id != User.GetUserId() && !User.Can(Permission.UserAdmin)) return "Forbidden.";

            if (item.UserName != oldItem?.UserName)
            {
                if (Db.Users.Any(u => u.UserName == item.UserName && u.Id != item.Id))
                {
                    return "Username is already taken.";
                }

                item.NormalizedUserName = userManager.NormalizeName(item.UserName);
            }

            if (oldItem != null)
            {
                if (item.LockoutEnd != oldItem.LockoutEnd)
                {
                    // Auto-enable lockout when setting a lockout date.
                    if (item.LockoutEnd != null) item.LockoutEnabled = true;

                    // Invalidate existing sessions when manually locking a user's account.
                    item.SecurityStamp = Guid.NewGuid().ToString();
                }

                if (!item.LockoutEnabled)
                {
                    // Make it clear to the administrator that lockout is only respected when LockoutEnabled.
                    item.LockoutEnd = null;
                }
            }

            return base.BeforeSave(kind, oldItem, item);
        }

        public override async Task<ItemResult<User>> AfterSaveAsync(SaveKind kind, User? oldItem, User item)
        {
            if (User.GetUserId() == item.Id)
            {
                // If the user was editing their own profile,
                // refresh their current sign-in so they aren't kicked out if
                // the change required a refresh to the user's security stamp.
                await signInManager.RefreshSignInAsync(item);
            }

            return await base.AfterSaveAsync(kind, oldItem, item);
        }
    }
}
