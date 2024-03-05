using SistemaOrbita.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.Model.ViewModels
{
    public class EmployeeAssignmentVM
    {
        public IEnumerable<Project>? Projects { get; set; }
        public IEnumerable<EmployeeProjectAssignment>? EmployeeProjectAssignments { get; set; }

        public Project Project { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
        public List<string> SelectedEmployeeIds { get; set; }
    }
}
