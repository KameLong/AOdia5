using AOdiaData;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Drawing;
using Path = AOdiaData.Path;

void LoadStationCSV(DiaFile db)
{
    string routeCsv = @"C:\Users\kamelong\Downloads\line20230105free.csv";
    var colors = new Color[] {Color.Red, Color.Green, Color.Blue,Color.Orange,Color.DarkViolet,Color.DarkCyan };

    Dictionary<int, Route> routes = new Dictionary<int, Route>();
    foreach (var line in File.ReadAllLines(routeCsv))
    {
        var lines = line.Split(',');
        Route route = new Route();
        route.Name.Value = lines[2];
        route.color.Value = colors[route.RouteId % colors.Count()];
        routes.Add(int.Parse(lines[0]), route);
        db.routes.Add(route);
    }
   db.SaveChanges();


    string stationCsv = @"C:\Users\kamelong\Downloads\station20230105free.csv";
    Station? prevStation = null;
    int prevRouteIndex = -1;
    foreach(var line in File.ReadAllLines(stationCsv))
    {
        var lines=line.Split(',');
        Station station= new Station();
        station.Name.Value = lines[2];
        station.Lat.Value = float.Parse(lines[10]);
        station.Lon.Value = float.Parse(lines[9]);
        Console.WriteLine($"{lines[2]}\t{lines[10]}\t{lines[9]}");
        db.stations.Add(station);

        int routeIndex = int.Parse(lines[0]) / 100;
        if (prevStation != null&&prevRouteIndex==routeIndex)
        {
            var path = new Path();
            path.route =routes[routeIndex];
            path.startStationID = prevStation.StationId;
            path.endStationID = station.StationId;
            path.seq = path.route.Paths.Count();
            path.route.Paths.Add(path);
            db.paths.Add(path);
        }
        
        prevRouteIndex= routeIndex;
        prevStation= station;
    }
    db.SaveChanges();

}

DateTime now = DateTime.Now;

using (var db = new DiaFile())
{

    var a = db.routes.Include(x => x.Paths).First();
    int b=0;
    //    Random rand = new Random();
    //    var paths = db.routes.Include(r => r.Paths);

    //    db.stations.RemoveRange(db.stations);
    //    db.routes.RemoveRange(db.routes);
    //    db.SaveChanges();
    //    LoadStationCSV(db);
    //    db.SaveChanges();
    //    return;

//    LoadStationCSV(db);
}

//    Console.WriteLine((DateTime.Now - now).TotalMilliseconds);

//    // 駅の初期化
//    Station s = new Station();
//    //// 新規登録
//    for (int i = 0; i < 1000; i++)
//    {
//        s = new Station();
//        s.Name.Value = i.ToString();
//        db.stations.Add(s);
//    }
//    db.SaveChanges();
//    Console.WriteLine((DateTime.Now - now).TotalMilliseconds);
//    db.SaveChanges();
//    Console.WriteLine((DateTime.Now - now).TotalMilliseconds);
//    var stations = db.stations.ToList();
//    for (int i = 0; i < 10; i++)
//    {
//        Route route = new Route();
//        route.Name.Value = i.ToString();
//        for (int j = 0; j < 10; j++)
//        {
//            Path p = new Path();
//                        p.route = route;
////            p.routeId = route.RouteId;
//            p.startStationID = stations[(rand.Next()) % 100].StationId;
//            p.endStationID = stations[(rand.Next()) % 100].StationId;
//            p.seq = route.Paths.Count();
//            route.Paths.Add(p);
//        }
//        db.routes.Add(route);
//    }
//    Console.WriteLine((DateTime.Now - now).TotalMilliseconds);
//    db.SaveChanges();

//    // 読み込み
//    Console.WriteLine((DateTime.Now - now).TotalMilliseconds);

//}