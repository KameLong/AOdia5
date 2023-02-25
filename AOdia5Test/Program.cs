using AOdiaData;
using Microsoft.EntityFrameworkCore;
using Path = AOdiaData.Path;

DateTime now = DateTime.Now;

using (var db = new DiaFile())
{
    Random rand = new Random();
    var paths = db.routes.Include(r => r.Paths);

    db.stations.RemoveRange(db.stations);
    db.routes.RemoveRange(db.routes);
    db.paths2.RemoveRange(db.paths2);
    db.SaveChanges();

    Console.WriteLine((DateTime.Now - now).TotalMilliseconds);

    // 駅の初期化
    Station s = new Station();
    //// 新規登録
    for (int i = 0; i < 1000; i++)
    {
        s = new Station();
        s.Name.Value = i.ToString();
        db.stations.Add(s);
    }
    db.SaveChanges();
    Console.WriteLine((DateTime.Now - now).TotalMilliseconds);
    db.SaveChanges();
    Console.WriteLine((DateTime.Now - now).TotalMilliseconds);
    var stations = db.stations.ToList();
    for (int i = 0; i < 10; i++)
    {
        Route route = new Route();
        route.Name.Value = i.ToString();
        for (int j = 0; j < 10; j++)
        {
            Path p = new Path();
                        p.route = route;
//            p.routeId = route.RouteId;
            p.startStationID = stations[(rand.Next()) % 100].StationId;
            p.endStationID = stations[(rand.Next()) % 100].StationId;
            p.seq = route.Paths.Count();
            route.Paths.Add(p);
        }
        db.routes.Add(route);
    }
    Console.WriteLine((DateTime.Now - now).TotalMilliseconds);
    db.SaveChanges();

    // 読み込み
    Console.WriteLine((DateTime.Now - now).TotalMilliseconds);

}