using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.DAL.Data
{
    public class OrbitaAuthDbContext : IdentityDbContext
    {
        public OrbitaAuthDbContext(DbContextOptions<OrbitaAuthDbContext> options)
            : base(options)
        {
        }
    }
}
