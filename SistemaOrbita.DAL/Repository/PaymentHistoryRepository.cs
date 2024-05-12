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
    public class PaymentHistoryRepository : Repository<PaymentHistory>, IPaymentHistoryRepository
    {
        private readonly OrbitaDbContext _context;
        public PaymentHistoryRepository(OrbitaDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetAllDropDownList(string obj)
        {
            if (obj == "Payroll")
            {
                return _context.Payrolls.Select(g => new SelectListItem
                {
                    Text = g.CreatedAt != null ? g.CreatedAt.Value.ToString("dd/MM/yyyy") : "Date not available",
                    Value = g.Id
                });
            }

            if (obj == "Employee")
            {
                return _context.Employees.Select(m => new SelectListItem
                {
                    Text = $"{m.FirstName} {m.LastName}",
                    Value = m.Id
                });
            }

            if (obj == "Organization")
            {
                return _context.OrganizationDepartments.Select(m => new SelectListItem
                {
                    Text = m.Name,
                    Value = m.Id
                });
            }
            return null;
        }
    }
}
