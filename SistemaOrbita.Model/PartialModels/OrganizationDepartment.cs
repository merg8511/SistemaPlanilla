using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SistemaOrbita.Model.Models
{
    public partial class OrganizationDepartment
    {
        [Required(ErrorMessage = "Field {0} is required")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        [MaxLength(255)]
        public string Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<Position> Positions { get; set; } = new List<Position>();

    }
}
