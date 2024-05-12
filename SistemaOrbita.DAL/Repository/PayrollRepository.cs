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
    public class PayrollRepository : Repository<Payroll>, IPayrollRepository
    {
        private readonly OrbitaDbContext _context;

        public PayrollRepository(OrbitaDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Payroll payroll)
        {
            var entity = _context.Payrolls.FirstOrDefault(p => p.Id == payroll.Id);

            if (entity != null)
            {
                entity.TotalAmount = payroll.TotalAmount;
                entity.IsActive = payroll.IsActive;
                entity.StartDate = payroll.StartDate;
                entity.EndDate = payroll.EndDate;
                entity.Status = payroll.Status;

                _context.SaveChanges();
            }
        }

    }
}
