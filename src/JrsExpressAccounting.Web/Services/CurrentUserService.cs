namespace JrsExpressAccounting.Web.Services;

public interface ICurrentUserService
{
    string GetCurrentUsername();
}

public class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
{
    public string GetCurrentUsername()
    {
        return accessor.HttpContext?.User?.Identity?.Name ?? "system";
    }
}
