using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaOrbita.Model.ViewModels
{
    public class UserVM
    {
        public IdentityUser User { get; set; }
        public int? Type { get; set; }
        public string RoleId { get; set; }
        public IEnumerable<SelectListItem>? Roles { get; set; }
        public string? Password { get; set; }
    }
}
