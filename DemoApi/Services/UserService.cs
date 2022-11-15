using System.Security.Claims;

namespace Stio.WorkflowManager.DemoApi.Services;

public class UserService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId => new("AA9AFDAF-2C5D-4CA6-81A1-64D98CC56878");
}