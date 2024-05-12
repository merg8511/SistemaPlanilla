using SistemaOrbita.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SistemaOrbita.Model.Models
{
    public partial class Employer
    {
        [JsonIgnore]
        public virtual ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();
    }
}
