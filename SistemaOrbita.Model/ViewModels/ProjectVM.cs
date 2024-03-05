using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaOrbita.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.Model.ViewModels
{
    public class ProjectVM
    {
        public Project? Project { get; set; }
        public IEnumerable<SelectListItem>? Managers { get; set; }
    }
}
