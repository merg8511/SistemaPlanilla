using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaOrbita.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.Model.ViewModels
{
    public class PositionVM
    {
        public Position Position { get; set; }
        public IEnumerable<SelectListItem>? Organizations { get; set; }
    }
}
