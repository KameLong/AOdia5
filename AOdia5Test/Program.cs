using AOdiaData;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Xml.Linq;
using Path = AOdiaData.Path;
DiaFile.dbName = "test.db";

var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
var DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}{DiaFile.dbName}";
if (File.Exists(DbPath))
{
    File.Delete(DbPath);
}
using (var client = new WebClient())
{
    client.DownloadFile("https://kamelong.com/aodia.db",DbPath);
}



RoutePathTest();




void RoutePathTest()
{
    DiaFile diaFile = DiaFile.staticDia;
    Route route = diaFile.routes.Include(r=>r.Paths).First(r => r.dbName == "阪急京都本線");
    int initpathCOunt=route.Paths.Count();

    Path deletePath = route.Paths.OrderBy(p => p.seq).First();
    route.DeleteStation(deletePath);
    diaFile.SaveChanges();
    route.InsertStation(deletePath);
    diaFile.SaveChanges();

    int lastpathCOunt = route.Paths.Count();
    if(lastpathCOunt != initpathCOunt)
    {
        Console.WriteLine("え？");
    }

    deletePath = route.Paths.First(p => p.seq == 3);
    route.DeleteStation(deletePath);
    diaFile.SaveChanges();
    route.InsertStation(deletePath);
    diaFile.SaveChanges();
    lastpathCOunt = route.Paths.Count();
    if (lastpathCOunt != initpathCOunt)
    {
        Console.WriteLine("え？");
    }
    while (route.Paths.Count() > 0)
    {
        route.DeleteEndStation();
        diaFile.SaveChanges();
    }



    diaFile.SaveChanges();
}

void LoadStationCSV(DiaFile db)
{
    return;


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

bool AddStationTest()
{
    var route = DiaFile.staticDia.routes.Include(r=>r.Paths).Where(r => r.dbName == "阪急京都本線").First();
    var newStation = Station.CreateNewStation();
    newStation.Name.Value = "test";
    newStation.Lat.Value = 35;
    newStation.Lon.Value = 135;
    Path path = route.Paths.OrderBy(path => path.seq).First();
    var newPath = route.AddStation(path, newStation);

    DiaFile.staticDia.SaveChanges();
    var stationList=route.Paths.OrderBy(path => path.seq).Select(p => p.startStation.DbName);

        return true;
}
//AddStationTest();