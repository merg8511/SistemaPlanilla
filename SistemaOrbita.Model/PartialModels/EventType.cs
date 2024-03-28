using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SistemaOrbita.Model.Models
{
    public partial class EventType
    {
        [Required(ErrorMessage = "Field {0} is required")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        [MaxLength(255)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        public sbyte? IsDeduction { get; set; }

        [JsonIgnore]
        public virtual ICollection<EventLog> EventLogs { get; set; } = new List<EventLog>();

    }
}
