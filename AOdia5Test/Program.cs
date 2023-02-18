using AOdiaData;
using System.Reflection.Metadata;

DiaFile aodia = new DiaFile();

using (var db = new DiaFile())
{
    // 新規登録
    db.Add(new Station { Name="test" });
    db.SaveChanges();

    // 読み込み
    var station = db.Stations.OrderBy(b => b.StationId).First();
    Console.WriteLine($"駅を新規登録しました！　" + $"駅ID：{station.StationId}, 駅名：{station.Name}");

}