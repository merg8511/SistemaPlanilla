using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.Model.ViewModels
{
    public class PageTitleViewModel
    {
        public string? Title { get; set; }
        public List<BreadcrumbItem>? Breadcrumbs { get; set; }
    }

    public class BreadcrumbItem
    {
        public string? Text { get; set; }
        public string? Area { get; set; }
        public string? Controller { get; set; }
        public string? Action { get; set; }
    }
}
