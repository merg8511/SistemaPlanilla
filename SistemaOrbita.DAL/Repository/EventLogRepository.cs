using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class EventLogRepository : Repository<EventLog>, IEventLogRepository
    {
        private readonly OrbitaDbContext _context;

        public EventLogRepository(OrbitaDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(EventLog eventLog)
        {
            var entity = _context.EventLogs.FirstOrDefault(x => x.Id == eventLog.Id);

            if (entity != null)
            {
                entity.Notes = eventLog.Notes;
                entity.Amount = eventLog.Amount;
                entity.EmployeeId = eventLog.EmployeeId;
                entity.EventId = eventLog.EventId;
                entity.AuthorizedBy = eventLog.AuthorizedBy;
                entity.CreatedAt = eventLog.CreatedAt;
                entity.Recurring = eventLog.Recurring;
                entity.Frequency = eventLog.Frequency;
                entity.StartDate = eventLog.StartDate;
                entity.EndDate = eventLog.EndDate;
                entity.IsActive = eventLog.IsActive;

                _context.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> GetAllDropDownList(string obj)
        {
            if (obj == "Employee")
            {
                return _context.Employees.Include(e => e.Position).Where(e => e.IsActive == 1).Select(e => new SelectListItem
                {
                    Text = $"{e.FirstName} {e.LastName} - {e.Position.Title}",
                    Value = e.Id
                });
            }

            if (obj == "Event")
            {
                return _context.EventTypes.Where(e => e.IsActive == 1).Select(e => new SelectListItem
                {
                    Text = e.Name,
                    Value = e.Id
                });
            }

            if (obj == "Authorize")
            {
                return _context.Employees
                    .Include(e => e.Position)
                    .ThenInclude(o => o.OrganizationDepartment)
                    .Where(e => e.IsActive == 1 && e.Position.OrganizationDepartment.Id == "01HSJ89N0X6FFACYKPTXXJ4T7Q")
                    .Select(e => new SelectListItem
                    {
                        Text = $"{e.FirstName} {e.LastName} - {e.Position.Title}",
                        Value = e.Id
                    });
            }
            return null;
        }

        
    }
}
