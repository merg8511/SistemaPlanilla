using SistemaOrbita.DAL.Data;
using SistemaOrbita.DAL.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrbitaDbContext _context;
        public IPositionRepository Position { get; private set; }
        public IEventTypeRepository EventType { get; private set; }
        public IEmployeeRepository Employee { get; private set; }
        public IOrganizationDepartmentRepository OrganizationDepartment { get; private set; }
        public IProjectRepository Project { get; private set; }
        public IEmployeeHistoryRepository EmployeeHistory { get; private set; }
        public IEPAssignmentRepository EmployeeAssignment { get; private set; }
        public IAuditLogRepository AuditLog { get; private set; }
        public IEmployerRepository Employer { get; private set; }
        public IQuotationTypeRepository QuotationType { get; private set; }
        public IIncomeTaxBracketRepository IncomeTaxBracket { get; private set; }
        public IEventLogRepository EventLog { get; private set; }

        public UnitOfWork(OrbitaDbContext context)
        {
            _context = context;
            Position = new PositionRepository(_context);
            EventType = new EventTypeRepository(_context);
            Employee = new EmployeeRepository(_context);
            OrganizationDepartment = new OrganizationDepartmentRepository(_context);
            Project = new ProjectRepository(_context);
            EmployeeHistory = new EmployeeHistoryRepository(_context);
            EmployeeAssignment = new EPAssignmentRepository(_context);
            AuditLog = new AuditLogRepository(_context);
            Employer = new EmployerRepository(_context);
            QuotationType = new QuotationTypeRepository(_context);
            IncomeTaxBracket = new IncomeTaxBracketRepository(_context);
            EventLog = new EventLogRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
