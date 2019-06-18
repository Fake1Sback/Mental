using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mental.Models.DbModels
{
    public class ApplicationContext : DbContext
    {
        private string databasePath;
     
        public DbSet<DbMathTask> mathTasks { get; set; }

        public ApplicationContext(string _databasePath)
        {
            databasePath = DependencyService.Get<IPath>().GetDatabasePath(_databasePath);   
        }
    
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }
    }
}
