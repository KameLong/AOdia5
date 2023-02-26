using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOdiaData
{
    public static class AOdiaData
    {
        public static DiaFile staticDia
        {
            get
            {
                if (_dia == null)
                {
                    DateTime now= DateTime.Now;
                    var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    Debug.WriteLine(path);
                    var DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}aodia.db";
                    System.Net.WebClient wc = new System.Net.WebClient();
                    wc.DownloadFile("https://kamelong.com/aodia_test.db", DbPath);
                    Debug.WriteLine($"{(DateTime.Now-now).TotalMilliseconds}  DL終了");
                    wc.Dispose();

                    _dia = new DiaFile();
                    Debug.WriteLine($"{(DateTime.Now - now).TotalMilliseconds}  ロード完了");
                }
                return _dia;
            }
        }
        private static DiaFile? _dia = null;

        public static DbSet<Station> stations { get { return staticDia.stations; } }
        public static DbSet<Route> routes { get { return staticDia.routes; } }
    }
}
