using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaOrbita.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.DAL.Repository.IRepository
{
    public interface IProjectRepository : IRepository<Project>
    {
        void Update(Project model);
        IEnumerable<SelectListItem> GetAllDropDownList(string obj);
    }
}
