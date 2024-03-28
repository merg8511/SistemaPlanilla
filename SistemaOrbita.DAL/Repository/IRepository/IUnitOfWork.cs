using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.DAL.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IPositionRepository Position { get; }
        IEventTypeRepository EventType { get; }
        IEmployeeRepository Employee { get; }
        IOrganizationDepartmentRepository OrganizationDepartment { get; }
        IProjectRepository Project { get; }
        IEmployeeHistoryRepository EmployeeHistory { get; }
        IEPAssignmentRepository EmployeeAssignment { get; }
        IAuditLogRepository AuditLog { get; }
        IEmployerRepository Employer { get; }
        IQuotationTypeRepository QuotationType { get; }
        IIncomeTaxBracketRepository IncomeTaxBracket { get; }
        IEventLogRepository EventLog { get; }
        Task Save();
    }
}
