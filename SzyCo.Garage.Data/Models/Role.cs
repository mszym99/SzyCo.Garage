using SzyCo.Garage.Data.Coalesce;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace SzyCo.Garage.Data.Models;

[Create(nameof(Permission.UserAdmin))]
[Edit(nameof(Permission.UserAdmin))]
[Delete(nameof(Permission.UserAdmin))]
[Description("Roles are groups of permissions, analogous to job titles or functions.")]
public class Role
    : IdentityRole
{

    [Required, Search(SearchMethod = SearchMethods.Contains)]
    public override string? Name { get; set; }

    [InternalUse]
    public override string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    [InternalUse]
    public override string? NormalizedName { get; set; }

    public List<Permission>? Permissions { get; set; }

    public class Behaviors(RoleManager<Role> roleManager, CrudContext<AppDbContext> context) : AppBehaviors<Role>(context)
    {
        public override ItemResult BeforeSave(SaveKind kind, Role? oldItem, Role item)
        {

            item.NormalizedName = roleManager.NormalizeKey(item.Name);

            return base.BeforeSave(kind, oldItem, item);
        }
    }
}

[InternalUse]
public class RoleClaim : IdentityRoleClaim<string> 
{
    [ForeignKey(nameof(RoleId))]
    public Role? Role { get; set; }
}
