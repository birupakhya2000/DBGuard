using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGuard.Demo
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Data Source=10.66.13.24;Initial Catalog=journalitrack_uat;Persist Security Info=True;user id=skeltadbusr;password=skelta@bjd;Encrypt=False");
        }
    }
}
