using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaOrbita.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.DAL.Repository.IRepository
{
    public interface IPositionRepository : IRepository<Position>
    {
        void Update(Position position);
        IEnumerable<SelectListItem> GetAllDropDownList(string obj, string id = null);
    }
}
