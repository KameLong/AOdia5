using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;

namespace AOdiaData
{


    public  class DiaFile : DbContext,INotifyPropertyChanged
    {
        public static string dbName = "aodia.db";
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

        public event PropertyChangedEventHandler? PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
          => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));




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

        public Action OnSavedAction { get; set; }
        public DiaFile()
        {

            var path = Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}{dbName}";
            SQLitePCL.Batteries_V2.Init();
                this.Database.EnsureCreated();


        }
        public override int SaveChanges()
        {
            if (OnSavedAction != null)
            {
                OnSavedAction();
            }
            return base.SaveChanges();
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
