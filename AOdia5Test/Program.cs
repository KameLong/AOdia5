using AOdiaData;
using Reactive.Bindings;
using System.Reflection.Metadata;
using Path = AOdiaData.Path;

DiaFile aodia = new DiaFile();

using (var db = new DiaFile())
{
    Random rand = new Random();
    DateTime now = DateTime.Now;

    db.stations.RemoveRange(db.stations);
    db.routes.RemoveRange(db.routes);
    db.paths.RemoveRange(db.paths);
    db.SaveChanges();

    Console.WriteLine((DateTime.Now - now).TotalMilliseconds);

    // 駅の初期化
    Station s =new Station();
    // 新規登録
    for (int i = 0; i < 1000; i++)
    {
        s = new Station();
        s.Name.Value= i.ToString();
        db.stations.Add(s);
    }
    Console.WriteLine((DateTime.Now - now).TotalMilliseconds);
    db.SaveChanges();
    Console.WriteLine((DateTime.Now - now).TotalMilliseconds);
    var stations = db.stations.ToList();
    for (int i = 0; i < 100; i++)
    {
        Route route = new Route();
        route.Name.Value= i.ToString();
        for(int j = 0; j < 100; j++)
        {
            Path p = new Path();
            p.route = route;
            p.startStationID = stations[(rand.Next()) % 100].StationId;
            p.endStationID = stations[(rand.Next()) % 100].StationId;
            p.seq = route.paths.Count();
            route.paths.Add(p);
        }
        db.routes.Add(route);
    }
    Console.WriteLine((DateTime.Now - now).TotalMilliseconds);
    db.SaveChanges();
    Console.WriteLine((DateTime.Now - now).TotalMilliseconds);

    // 読み込み
    var station = db.stations.OrderBy(b => b.StationId).First();
    Console.WriteLine($"駅を新規登録しました！　" + $"駅ID：{station.StationId}, 駅名：{station.Name}");

}