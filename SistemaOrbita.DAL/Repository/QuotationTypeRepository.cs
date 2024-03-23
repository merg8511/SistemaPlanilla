using Microsoft.EntityFrameworkCore;
using SistemaOrbita.DAL.Data;
using SistemaOrbita.DAL.Repository.IRepository;
using SistemaOrbita.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.DAL.Repository
{
    public class QuotationTypeRepository : Repository<QuotationType>, IQuotationTypeRepository
    {
        private readonly OrbitaDbContext _context;

        public QuotationTypeRepository(OrbitaDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> Any(Expression<Func<QuotationType, bool>> filter = null)
        {
            IQueryable<QuotationType> query = _context.QuotationTypes;

            if(filter!= null)
            {
                return await query.AnyAsync(filter);
            }

            return await query.AnyAsync();
        }

        public void Update(QuotationType quotation)
        {
            var model = _context.QuotationTypes.FirstOrDefault(q => q.Id == quotation.Id);

            if (model != null)
            {
                model.Name = quotation.Name;
                model.EmployerPercentage = quotation.EmployerPercentage;
                model.EmployeePercentage = quotation.EmployeePercentage;

                _context.SaveChanges();
            }
        }
    }
}
