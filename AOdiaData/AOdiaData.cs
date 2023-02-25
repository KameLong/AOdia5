using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AOdiaData
{
    /*
    public partial class DbSet<TEntry>: INotifyCollectionChanged, INotifyPropertyChanged where TEntry : class
    {
        public ObservableCollection<int> a;


        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
            {
                    CollectionChanged(this, e);
            }
        }
        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
        }


    }
    */
    public  class DiaFile : DbContext
    {
        public DbSet<Station> stations { get; set; }
        public DbSet<Route> routes { get; set; }

        public DbSet<Path> paths2 { get; set; }

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
