using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SistemaOrbita.Model.Models
{
    public partial class Position
    {
        [Required(ErrorMessage = "Field {0} is required")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        [MaxLength(255)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        public decimal? MinSalary { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        public decimal? MaxSalary { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        public string OrganizationDepartmentId { get; set; }

        [JsonIgnore]
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
