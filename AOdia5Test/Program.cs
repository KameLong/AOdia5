using AOdiaData;
using Reactive.Bindings;
using System.Reflection.Metadata;

DiaFile aodia = new DiaFile();

using (var db = new DiaFile())
{
    DateTime now = DateTime.Now;
    // 新規登録
    Station s=new Station();

    // 新規登録
    for (int i = 0; i < 5; i++)
    {
        s = new Station();
        s.Name.Value= i.ToString();
        db.Stations.Add(s);

    }
    db.Stations.Remove(s);
    Console.WriteLine((DateTime.Now - now).TotalMilliseconds);
    Console.WriteLine((DateTime.Now - now).TotalMilliseconds);

    // 読み込み
    var station = db.Stations.OrderBy(b => b.StationId).First();
    Console.WriteLine($"駅を新規登録しました！　" + $"駅ID：{station.StationId}, 駅名：{station.Name}");

}