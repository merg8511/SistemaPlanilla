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
    public class OrganizationDepartmentRepository : Repository<OrganizationDepartment>, IOrganizationDepartmentRepository
    {
        private readonly OrbitaDbContext _context;
        public OrganizationDepartmentRepository(OrbitaDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(OrganizationDepartment model)
        {
            var entity = _context.OrganizationDepartments.FirstOrDefault(e => e.Id == model.Id);

            if (entity != null)
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.UpdatedAt = DateTime.Now;
                entity.IsActive = model.IsActive;
                _context.SaveChanges();
            }
        }
    }
}
