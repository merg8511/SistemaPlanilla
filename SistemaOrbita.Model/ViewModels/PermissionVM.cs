using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.Model.ViewModels
{
    public class PermissionVM
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public int RoleClaimsCount { get; set; }
        public IList<RoleClaimsVM> RoleClaims { get; set; }
    }
}
