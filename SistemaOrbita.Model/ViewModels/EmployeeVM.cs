using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaOrbita.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.Model.ViewModels
{
    public class EmployeeVM
    {
        public Employee Employee { get; set; }
        public IEnumerable<SelectListItem>? Municipalities { get; set; }
        public IEnumerable<SelectListItem>? Genders { get; set; }
        public IEnumerable<Position>? PositionsList{ get; set; }
    }
}
