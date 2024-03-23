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
    public class IncomeTaxBracketRepository : Repository<IncomeTaxBracket>, IIncomeTaxBracketRepository
    {
        private readonly OrbitaDbContext _context;

        public IncomeTaxBracketRepository(OrbitaDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> Any(Expression<Func<IncomeTaxBracket, bool>> filter = null)
        {
            IQueryable<IncomeTaxBracket> query = _context.IncomeTaxBrackets;

            if (filter != null)
            {
                return await query.AnyAsync(filter);
            }

            return await query.AnyAsync();
        }

        public void Update(IncomeTaxBracket incomeTax)
        {
            var entity = _context.IncomeTaxBrackets.FirstOrDefault(i => i.Id == incomeTax.Id);

            if (entity != null)
            {
                entity.Name = incomeTax.Name;
                entity.FromAmount = incomeTax.FromAmount;
                entity.ToAmount = incomeTax.ToAmount;
                entity.AboutExcess = incomeTax.AboutExcess;
                entity.Percentage = incomeTax.Percentage;
                entity.FixedAmount = incomeTax.FixedAmount;

                _context.SaveChanges();
            }
        }
    }
}
