using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaOrbita.DAL.Data;
using SistemaOrbita.DAL.Repository.IRepository;
using SistemaOrbita.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.DAL.Repository
{
    public class EPAssignmentRepository : Repository<EmployeeProjectAssignment>, IEPAssignmentRepository
    {
        private readonly OrbitaDbContext _context;

        public EPAssignmentRepository(OrbitaDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(EmployeeProjectAssignment model)
        {
            var entity = _context.EmployeeProjectAssignments.FirstOrDefault(e => e.Id == model.Id);

            if (entity != null)
            {
                entity.UnassignedDate = DateTime.Now;
            }
            _context.SaveChanges();
        }

        public IEnumerable<SelectListItem> GetAllDropDownList(string obj)
        {
            if (obj == "Project")
            {
                return _context.Projects.Select(g => new SelectListItem
                {
                    Text = g.Name,
                    Value = g.Id
                });
            }

            if (obj == "Employee")
            {
                return _context.Employees.Select(m => new SelectListItem
                {
                    Text = $"{m.FirstName} + {m.LastName}",
                    Value = m.Id
                });
            }
            return null;
        }

        public async Task<IEnumerable<Project>> GetEmployeeProjectAssignment()
        {
            var projects = await _context.Projects
                .Include(p => p.Manager)
                .Include(p => p.EmployeeProjectAssignments)
                .ThenInclude(a => a.Employee)
                .Where(p => p.IsActive == 1).ToListAsync();

            return projects;
        }
    }
}
