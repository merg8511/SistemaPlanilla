using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SistemaOrbita.ActionFilters
{
    public class CheckUserPermissions : Attribute, IAsyncActionFilter
    {

        private readonly string _propertyName;
        private readonly object _expectedValue;
        private UserManager<IdentityUser> _userManager;

        public CheckUserPermissions(string propertyName, object expectedValuer)
        {
            _propertyName = propertyName;
            _expectedValue = expectedValuer;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _userManager = (UserManager<IdentityUser>)context.HttpContext.RequestServices.GetService(typeof(UserManager<IdentityUser>));

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(context.HttpContext.User);

                var property = typeof(IdentityUser).GetProperty(_propertyName);

                if (property == null)
                {
                    throw new ArgumentException($"The property '{_propertyName}' does not exist on app user");
                }

                var propertyValue = property.GetValue(user);

                context.HttpContext.Items["UserPropertyCheckResult"] = propertyValue.Equals(_expectedValue);

                if (!propertyValue.Equals(_expectedValue))
                {
                    context.Result = new RedirectToActionResult("Error", "Home", new { area = "BusinessManagement", code = 403 });
                    return;
                }
            }
            else
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            await next();
        }
    }
}
