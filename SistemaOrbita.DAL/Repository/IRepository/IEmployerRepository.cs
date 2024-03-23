using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaOrbita.Model.Models;
using SistemaOrbita.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.DAL.Repository.IRepository
{
    public interface IEmployerRepository : IRepository<Employer>
    {
        void Update(Employer employer);
    }
}
