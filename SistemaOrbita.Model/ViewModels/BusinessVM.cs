using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaOrbita.Model.Models;
using SistemaOrbita.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.Model.ViewModels
{
    public class BusinessVM
    {
        public Employer? Employer { get; set; }
        public IEnumerable<Models.QuotationType>? QuotationTypes { get; set; }
        public IEnumerable<Models.IncomeTaxBracket>? IncomeTaxBrackets { get; set; }
        public IEnumerable<SelectListItem>? QuotationType { get; set; }
        public IEnumerable<SelectListItem>? StretchNumber { get; set; }
    }
}
