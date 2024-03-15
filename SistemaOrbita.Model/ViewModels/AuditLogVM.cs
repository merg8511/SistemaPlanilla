using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaOrbita.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.Model.ViewModels
{
    public class AuditLogVM
    {
        public string? UserId { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public IEnumerable<SelectListItem>? Users { get; set; }

    }
}
