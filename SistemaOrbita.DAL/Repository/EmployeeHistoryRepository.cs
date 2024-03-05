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
    public class EmployeeHistoryRepository : Repository<EmployeeHistory>, IEmployeeHistoryRepository
    {
        private readonly OrbitaDbContext _context;

        public EmployeeHistoryRepository(OrbitaDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(EmployeeHistory model)
        {
            var entity = _context.EmployeeHistories.FirstOrDefault(eh => eh.Id == model.Id);

            if(entity != null)
            {
                entity.ChangeReason = model.ChangeReason;
            }
        }
    }
}
