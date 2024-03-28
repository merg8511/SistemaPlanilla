using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaOrbita.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.Model.ViewModels
{
    public class EventLogVM
    {
        public EventLog EventLog { get; set; }
        public IEnumerable<SelectListItem>? Employees { get; set; }
        public IEnumerable<SelectListItem>? Events { get; set; }
        public IEnumerable<SelectListItem>? AuthorizedBy { get; set; }
        public IEnumerable<EventType>? EventTypes { get; set; }
        public bool IsRecurring { get; set; }
    }
}
