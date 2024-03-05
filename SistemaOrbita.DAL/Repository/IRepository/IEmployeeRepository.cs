using SistemaOrbita.Model.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace SistemaOrbita.DAL.Repository.IRepository
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        void Update(Employee employe);
        Task<Employee> GetEmployeeDetails(string id);
        IEnumerable<SelectListItem> GetAllDropDownList(string obj);
    }
}
