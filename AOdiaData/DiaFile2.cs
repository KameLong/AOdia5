using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOdiaData
{

    /*
    public class DiaFile
    {
        private static DiaFile? _dia=null;
        public static DiaFile staticDia
        {
            get
            {
                if (_dia == null)
                {
                    _dia = new DiaFile();
                }
                return _dia;
            }
        }
        public Dictionary<long, Station> stations=new Dictionary<long, Station>();
        public Dictionary<long, Route> routes=new Dictionary<long, Route>();
        public Dictionary<long, Path> paths=new Dictionary<long, Path>();
        public DiaFile()
        {
            var now=DateTime.Now;
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}aodia.db";
            
            using (var connection = new SqliteConnection($"Data Source={DbPath}"))
            {
                connection.Open();
                Debug.WriteLine($"mysql読み込み開始 {(DateTime.Now-now).TotalMilliseconds}");
                var command = connection.CreateCommand();
                command.CommandText = @"SELECT * FROM routes";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        routes.Add(reader.GetInt64(0),new Route() {
                            dbName = reader.GetString(1),
                        dbColor=reader.GetString(2)});
                    }
                }
                Debug.WriteLine($"station読み込み開始 {(DateTime.Now - now).TotalMilliseconds}");

                command.CommandText = @"SELECT * FROM stations";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var s = new Station();
                        s.DbName = reader.GetString(1);
                        s.DbLat = reader.GetFloat(2);
                        s.DbLon = reader.GetFloat(3);


                        stations.Add(reader.GetInt64(0), s);
                    }
                }
                Debug.WriteLine($"path読み込み開始 {(DateTime.Now - now).TotalMilliseconds}");

                command.CommandText = @"SELECT * FROM path order by seq";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var p = new Path()
                        {
                            routeId = reader.GetInt64(1),
                            seq=reader.GetInt32(2),
                            startStationID = reader.GetInt64(3),
                            endStationID = reader.GetInt64(4)
                        };
                        paths.Add(reader.GetInt64(0), p);
                        routes[p.routeId].Paths.Add(p);

                    }
                    }
                Debug.WriteLine($"読み込み終わり {(DateTime.Now - now).TotalMilliseconds}");

                Debug.WriteLine("読み込み終わり");


            }

        }
        public void SaveChanges()
        {

        }
    }
    */
}
