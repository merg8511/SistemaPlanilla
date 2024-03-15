using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using SistemaOrbita.DAL.Data;
using SistemaOrbita.Model.Models;
using System.Security.Claims;

namespace SistemaOrbita.ActionFilters
{
    public class AuditLogFilter : IAsyncActionFilter
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly OrbitaDbContext _context;

        public AuditLogFilter(UserManager<IdentityUser> userManager, OrbitaDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(context.HttpContext.User);

                var auditAttribute = context.ActionDescriptor.EndpointMetadata
                                .OfType<AuditAttribute>()
                                .FirstOrDefault();

                if (user is not null && auditAttribute != null)
                {
                    _context.AuditLogs.Add(new AuditLog
                    {
                        Id = NUlid.Ulid.NewUlid().ToString(),
                        ActionType = context.ActionDescriptor.DisplayName,
                        EntityType = context.Controller.ToString(),
                        Changes = auditAttribute.Description,
                        ChangeDate = DateTime.Now,
                        UserId = user.UserName
                    });

                    _context.SaveChanges();
                }
            }
            await next();
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class AuditAttribute : Attribute
    {
        public string Description { get; }

        public AuditAttribute(string description = "")
        {
            Description = description;
        }
    }
}
