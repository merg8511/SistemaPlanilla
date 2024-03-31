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
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private readonly OrbitaDbContext _context;

        public ProjectRepository(OrbitaDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Project model)
        {
            var entity = _context.Projects.FirstOrDefault(p => p.Id == model.Id);

            if (entity != null)
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.ManagerId = model.ManagerId;
                entity.EndDate = model.EndDate;
                entity.IsActive = model.IsActive;
                entity.UpdateAt = DateTime.Now;
                _context.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> GetAllDropDownList(string obj)
        {
            if (obj == "Employee")
            {
                return _context.Employees
                    .Where(e => e.PositionId == "01HSJ8EMPB38J8DFR99SGN1P4P" || e.PositionId == "01HSJ8FCZFRQF5CA6QC0BDZBCW")
                    .Select(g => new SelectListItem
                    {
                        Text = $"{g.FirstName} {g.LastName}",
                        Value = g.Id
                    });
            }

            return null;
        }

    }
}
