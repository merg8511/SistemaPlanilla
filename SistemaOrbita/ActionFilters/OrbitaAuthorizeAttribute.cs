using Microsoft.AspNetCore.Mvc;

namespace SistemaOrbita.ActionFilters
{
    public class OrbitaAuthorizeAttribute : TypeFilterAttribute
    {
        public OrbitaAuthorizeAttribute(string permission) : base(typeof(OrbitaAuthorizeActionFilter))
        {
            Arguments = new object[] { permission };
        }
    }
}
