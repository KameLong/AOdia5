using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AOdiaData
{
    public  class DiaFile : DbContext
    {
        public DbSet<Station> stations { get; set; }
        public DbSet<Route> routes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Route>()
                .HasMany(r => r.Paths)
                .WithOne(p => p.route)
               .HasForeignKey(p =>p.routeId);

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
