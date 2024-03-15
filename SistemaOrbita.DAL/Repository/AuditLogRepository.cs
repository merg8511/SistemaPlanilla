using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class AuditLogRepository : Repository<AuditLog> , IAuditLogRepository
    {
        private readonly OrbitaDbContext _context;

        public AuditLogRepository(OrbitaDbContext context) : base(context)
        {
            _context = context;
        }
            
    }
}
