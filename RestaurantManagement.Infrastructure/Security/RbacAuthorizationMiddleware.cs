using Microsoft.AspNetCore.Http;
using RestaurantManagement.Domain.Interfaces.Security;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement.Infrastructure.Security
{
    public class RbacAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IRightResolver _resolver;

        public RbacAuthorizationMiddleware(RequestDelegate next, IRightResolver resolver)
        {
            _next = next;
            _resolver = resolver;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var allowAnonymous = endpoint?.Metadata
                .Any(m => m.GetType().Name == "AllowAnonymousAttribute") == true;

            if (allowAnonymous)
            {
                await _next(context);
                return;
            }

            // 🔑 First check: is there a valid authenticated user?
            if (context.User?.Identity == null || !context.User.Identity.IsAuthenticated)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: user not authenticated");
                return;
            }

            // Resolve required permission for this request
            var requiredPermission = _resolver.ResolveRequiredPermission(context);

            if (requiredPermission == null)
            {
                await _next(context);
                return;
            }

            // 🔑 Second check: does the user have the required permission?
            var hasPermission = context.User.Claims
                .Any(c => c.Type == "Permission" && c.Value == requiredPermission);

            if (!hasPermission)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Forbidden: missing permission " + requiredPermission);
                return;
            }

            await _next(context);
        }
    }
}
