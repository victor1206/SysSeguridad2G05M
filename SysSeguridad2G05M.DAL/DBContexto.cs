using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using SysSeguridad2G05M.EN;

namespace SysSeguridad2G05M.DAL
{
    public class DBContexto : DbContext
    {
        public DbSet<Rol> Rol { get; set; }

        public DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=VictorDuran;Initial Catalog=DbSysSeguridad2G05;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
        }
    }
}
