using SistemaOrbita.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.DAL.Repository.IRepository
{
    public interface IQuotationTypeRepository : IRepository<QuotationType>
    {
        void Update(QuotationType quotation);
        Task<bool> Any(Expression<Func<QuotationType, bool>> filter = null);
    }
}
