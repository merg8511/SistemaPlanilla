using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SistemaOrbita.Model.Models
{
    public partial class Employee
    {
        [Required(ErrorMessage = "Field {0} is required")]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        public DateTime? Birthday { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        [MaxLength(230)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        [DataType(DataType.Currency)]
        public decimal? Salary { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        public string PositionId { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        public string MunicipalityId { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        public string GenderId { get; set; }

        [JsonIgnore]
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    }
}
