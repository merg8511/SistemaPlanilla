using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SistemaOrbita.Model.Models
{
    public partial class EventLog
    {
        [Required(ErrorMessage = "Field {0} is required")]
        [DataType(DataType.Currency)]
        public decimal? Amount { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        public string EmployeeId { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        public string EventId { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        public string AuthorizedBy { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        public DateTime? CreatedAt { get; set; }
        public sbyte? Recurring { get; set; }
        
    }
}
