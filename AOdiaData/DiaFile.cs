using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;

namespace AOdiaData
{


    public  class DiaFile : DbContext
    {
        public static DiaFile staticDia
        {
            get
            {
                if (_dia == null)
                {
                    DateTime now = DateTime.Now;

                    _dia = new DiaFile();

                    Debug.WriteLine($"{(DateTime.Now - now).TotalMilliseconds}  ロード完了");
                }
                return _dia;
            }
        }
        private static DiaFile? _dia = null;





        public DbSet<Station> stations { get; set; }
        public DbSet<Route> routes { get; set; }
        public DbSet<Path> paths { get; set; }


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
            using (var client = new WebClient())
            {
//                client.DownloadFile("https://kamelong.com/aodia.db", DbPath);
            }
            SQLitePCL.Batteries_V2.Init();
                this.Database.EnsureCreated();


        }
        // デスクトップ上にSQLiteのDBファイルが作成される
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
            // 出力ウィンドウに EF Core ログを表示
            options.LogTo(msg =>Debug.WriteLine(msg));
        }
    }
}
