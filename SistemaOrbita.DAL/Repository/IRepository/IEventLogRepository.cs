using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaOrbita.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.DAL.Repository.IRepository
{
    public interface IEventLogRepository : IRepository<EventLog>
    {
        void Update(EventLog eventLog);
        IEnumerable<SelectListItem> GetAllDropDownList(string obj);

        Task<EventLog> GetEventLogData(string id);
    }
}
