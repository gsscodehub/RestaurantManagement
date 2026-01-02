using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using RestaurantManagement.Domain.Interfaces.Security;
using System;
using System.Linq;

namespace RestaurantManagement.Infrastructure.Security
{
    public class RightResolver : IRightResolver
    {
        public string? ResolveRequiredPermission(HttpContext httpContext)
        {
            var endpoint = httpContext.GetEndpoint();
            var cad = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();
            if (cad == null) return null;

            var path = httpContext.Request.Path.Value?.ToLower();
            if (string.IsNullOrEmpty(path)) return null;

            // ✅ Split into segments: /api/user/create → ["api","user","create"]
            var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

            // ✅ Take the base segment after "api"
            // Example: ["api","user","create"] → "user"
            var baseSegment = segments.Length > 1 ? segments[1] : segments[0];

            // ✅ Normalize to plural to match PageUrl in DB (/users, /roles, /pages)
            if (!baseSegment.EndsWith("s"))
                baseSegment += "s";

            var operation = MapActionToOperation(cad.ActionName.ToLower());
            if (operation == null) return null;

            // ✅ Build permission string: /users:create
            return $"/{baseSegment}:{operation}";
        }

        private static string? MapActionToOperation(string action)
        {
            if (action.Contains("create")) return "create";
            if (action.Contains("edit") || action.Contains("update")) return "edit";
            if (action.Contains("delete") || action.Contains("remove")) return "delete";
            if (action.Contains("get") || action.Contains("list") || action.Contains("all") || action.Contains("view")) return "view";
            return null;
        }
    }
}
