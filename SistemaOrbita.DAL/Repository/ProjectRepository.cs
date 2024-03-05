﻿using Microsoft.AspNetCore.Mvc.Rendering;
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
                return _context.Employees.Select(g => new SelectListItem
                {
                    Text = $"{g.FirstName} {g.LastName}",
                    Value = g.Id
                });
            }

            return null;
        }

        public async Task<Project> GetProjectData(string id)
        {
            return await _context.Projects
                .Include(p => p.EmployeeProjectAssignments)
                .ThenInclude(epa => epa.Employee)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
