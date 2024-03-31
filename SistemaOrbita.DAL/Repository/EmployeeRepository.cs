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
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private readonly OrbitaDbContext _context;
        public EmployeeRepository(OrbitaDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Employee employee)
        {
            var entity = _context.Employees.FirstOrDefault(e => e.Id == employee.Id);

            if (entity != null)
            {
                entity.FirstName = employee.FirstName;
                entity.LastName = employee.LastName;
                entity.Dui = employee.Dui;
                entity.Birthday = employee.Birthday;
                entity.EndDate = employee.EndDate;
                entity.Address = employee.Address;
                entity.Email = employee.Email;
                entity.PhoneNumber = employee.PhoneNumber;
                entity.Salary = employee.Salary;
                entity.IsActive = employee.IsActive;
                entity.PositionId = employee.PositionId;
                entity.MunicipalityId = employee.MunicipalityId;

                _context.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> GetAllDropDownList(string obj)
        {
            if (obj == "Gender")
            {
                return _context.Genders.Select(g => new SelectListItem
                {
                    Text = g.Name,
                    Value = g.Id
                });
            }

            if (obj == "Municipality")
            {
                return _context.Municipalities.Select(m => new SelectListItem
                {
                    Text = m.Name,
                    Value = m.Id
                });
            }
            return null;
        }
    }
}
