using SistemaOrbita.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.DAL.Repository.IRepository
{
    public interface IIncomeTaxBracketRepository : IRepository<IncomeTaxBracket>
    {
        void Update(IncomeTaxBracket incomeTax);
        Task<bool> Any(Expression<Func<IncomeTaxBracket, bool>> filter = null);
    }
}
