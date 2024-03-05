using SistemaOrbita.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.DAL.Repository.IRepository
{
    public interface IEmployeeHistoryRepository : IRepository<EmployeeHistory>
    {
        void Update(EmployeeHistory model);
    }
}
