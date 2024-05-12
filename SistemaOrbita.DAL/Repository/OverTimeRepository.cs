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
    public class OverTimeRepository : Repository<OverTime>, IOverTimeRepository
    {
        private readonly OrbitaDbContext _context;

        public OverTimeRepository(OrbitaDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(OverTime overTime)
        {
            var entity = _context.OverTimes.FirstOrDefault(s => s.Id == overTime.Id);

            if (entity != null)
            {
                entity.DayOvertime = overTime.DayOvertime;
                entity.NightOvertime = overTime.NightOvertime;

                _context.SaveChanges();
            }
        }
    }
}
