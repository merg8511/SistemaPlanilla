using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaOrbita.DAL.Data;
using SistemaOrbita.DAL.Repository.IRepository;
using SistemaOrbita.Model.Models;
using SistemaOrbita.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.DAL.Repository
{
    public class EmployerRepository : Repository<Employer>, IEmployerRepository
    {
        private readonly OrbitaDbContext _context;

        public EmployerRepository(OrbitaDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Employer employer)
        {
            var entity = _context.Employers.FirstOrDefault(e => e.Id == employer.Id);

            if (entity != null)
            {
                entity.Name = employer.Name;
                entity.Address = employer.Address;
                entity.EconomicActivityCode = employer.EconomicActivityCode;
                entity.PatronalIsssNumber = employer.PatronalIsssNumber;
                entity.PatronalAfpNumber = employer.PatronalAfpNumber;
                entity.PatronalInpepNumber = employer.PatronalInpepNumber;
                entity.LiquidationType = employer.LiquidationType;
                entity.PayrollType = employer.PayrollType;

                _context.SaveChanges();
            }
        }
    }
}
