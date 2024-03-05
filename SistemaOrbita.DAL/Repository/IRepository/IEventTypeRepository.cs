using Microsoft.EntityFrameworkCore.Update.Internal;
using SistemaOrbita.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.DAL.Repository.IRepository
{
    public interface IEventTypeRepository : IRepository<EventType>
    {
        void Update(EventType model);
    }
}
