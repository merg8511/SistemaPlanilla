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
    public class TaskExecutionLogRepository : Repository<TaskExecutionLog>, ITaskExecutionLogRepository
    {
        private readonly OrbitaDbContext _context;

        public TaskExecutionLogRepository(OrbitaDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> CheckTaskExecutedLogToday(string name, DateTime date)
        {
            return await _context.TaskExecutionLogs.AnyAsync(t => t.TaskId == name && t.ExecutionDate == date);
        }
    }
}
