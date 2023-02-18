namespace AOdia5;
using AOdiaData;
using System.Diagnostics;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();

        DiaFile aodia = new DiaFile();
		try
		{
			Console.WriteLine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));
			File.WriteAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/test", "aa");
			Debug.WriteLine(File.ReadAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/test")[0]);
			using (var db = new DiaFile())
			{
				// 新規登録
				//db.Add(new Station {  Name = "test" });
    //            db.Add(new Station { Name = "test" });
    //            db.Add(new Station { Name = "test" });
    //            db.Add(new Station { Name = "test" });
    //            db.Add(new Station { Name = "test" });
    //            db.Add(new Station { Name = "test" });
    //            db.SaveChanges();

				// 読み込み
				foreach(var station in db.Stations.OrderBy(b => b.StationId))
				{
                    Debug.WriteLine($"駅を新規登録しました！　" + $"駅ID：{station.StationId}, 駅名：{station.Name}");

                }
            }
		}
        catch(Exception ex)
		{
			Debug.WriteLine("エラー");
			Debug.WriteLine(ex.Message);
            Debug.WriteLine("エラー終わり");
        }


    }

    private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}

