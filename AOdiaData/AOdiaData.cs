using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AOdiaData
{
    public  class DiaFile : DbContext
    {
        public DbSet<Station>? stations { get; set; }
        public DbSet<Route>? routes { get; set; }

        public DbSet<Path>? paths { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Path>()
                .HasOne(p => p.route)
                .WithMany(b => b.paths);
        }

        public string DbPath { get; }
        public DiaFile()
        {
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Debug.WriteLine(path);
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}aodia.db";
                SQLitePCL.Batteries_V2.Init();
                this.Database.EnsureCreated();


        }
        // デスクトップ上にSQLiteのDBファイルが作成される
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
