using System.Security.Claims;
using JrsExpressAccounting.Web.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace JrsExpressAccounting.Web.Services;

public class AuthService(AccountingDbContext db, IHttpContextAccessor accessor)
{
    public async Task<bool> LoginAsync(string username, string password)
    {
        var user = await db.UserAccounts
            .Include(x => x.Roles).ThenInclude(x => x.Role)
            .FirstOrDefaultAsync(x => x.Username == username && x.IsActive);
        if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash) || accessor.HttpContext is null)
        {
            return false;
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new("full_name", user.FullName)
        };

        claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Role.Name)));

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        return true;
    }
}
