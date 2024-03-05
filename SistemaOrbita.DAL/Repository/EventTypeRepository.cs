using Microsoft.EntityFrameworkCore;
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
    public class EventTypeRepository : Repository<EventType>, IEventTypeRepository
    {
        private readonly OrbitaDbContext _context;
        public EventTypeRepository(OrbitaDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(EventType model)
        {
            var entity = _context.EventTypes.FirstOrDefault(e => e.Id == model.Id);

            if (entity != null)
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.IsDeduction = model.IsDeduction;
                entity.IsActive = model.IsActive;
                _context.SaveChanges();
            }
        }
    }
}
