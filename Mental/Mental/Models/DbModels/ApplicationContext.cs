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
        public DbSet<DbMathTaskOptions> FavouriteMathTaskOptions { get; set; }
        
        public DbSet<DbSchulteTableTask> SchulteTableTasks { get; set; }
        public DbSet<DbSchulteTableTaskOptions> FavouriteSchulteTableTaskOptions { get; set; }

        public DbSet<DbStroopTask> StroopTasks { get; set; }
        public DbSet<DbStroopTaskOptions> FavouriteStroopTaskOptions { get; set; }

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
