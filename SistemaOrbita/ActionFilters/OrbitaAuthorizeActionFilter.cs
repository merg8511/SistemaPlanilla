using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace SistemaOrbita.ActionFilters
{
    public class OrbitaAuthorizeActionFilter : IAuthorizationFilter
    {
        private readonly string _permission;
        private readonly IAuthorizationService _authorizationService;

        public OrbitaAuthorizeActionFilter(string permission, IAuthorizationService authorizationService)
        {
            _permission = permission;
            _authorizationService = authorizationService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isAuthorized = CheckUserPermission(context.HttpContext.User, _permission);

            if (!isAuthorized)
            {
                context.Result = new RedirectToActionResult("Error", "Home", new { area = "BusinessManagement", code = 403 });
            }
        }

        private bool CheckUserPermission(ClaimsPrincipal user, string permission)
        {
            return _authorizationService.AuthorizeAsync(user, permission).Result.Succeeded;
        }
    }
}
