using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class PositionRepository : Repository<Position>, IPositionRepository
    {
        private readonly OrbitaDbContext _context;

        public PositionRepository(OrbitaDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetAllDropDownList(string obj, string id = null)
        {
            if (obj == "OrganizationDepartment")
            {
                return _context.OrganizationDepartments.Where(p => p.IsActive == 1).Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id
                });
            }

            return null;
        }

        public void Update(Position position)
        {
            var entity = _context.Positions.FirstOrDefault(p => p.Id == position.Id);

            if (entity != null)
            {
                entity.Title = position.Title;
                entity.Description = position.Description;
                entity.MinSalary = position.MinSalary;
                entity.MaxSalary = position.MaxSalary;
                entity.IsActive = position.IsActive;
                entity.OrganizationDepartmentId = position.OrganizationDepartmentId;

                _context.SaveChanges();
            }
        }

    }
}
