using SistemaOrbita.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.DAL.Repository.IRepository
{
    public interface IPayrollDetailRepository : IRepository<PayrollDetail>
    {
        IEnumerable<PayrollDetail> GetPayrollDetails(DateTime? startDate, DateTime? endDate);
    }
}
