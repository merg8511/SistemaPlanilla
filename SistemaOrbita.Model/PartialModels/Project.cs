using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.Model.Models
{
    public partial class Project
    {
        [Required(ErrorMessage = "Field {0} is required")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        [MaxLength(255)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        public string ManagerId { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        public DateTime? EndDate { get; set; }
    }
}
