using Microsoft.Data.Sqlite;
using System.Diagnostics;

namespace AOdia5;

public class TestPage : ContentPage
{
	public TestPage()
	{
		Content = new VerticalStackLayout
		{
			Children = {
				new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "Welcome to .NET MAUI!"
				}
			}
		};
		var now=DateTime.Now;
        var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        Debug.WriteLine(path);
        var DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}aodia.db";
        var rand=new Random();
        using (var connection = new SqliteConnection($"Data Source={DbPath}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"SELECT * FROM route";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var name = reader.GetString(1);
                    if (rand.Next(0, 100) == 0)
                    {
                        Debug.WriteLine($"{name}");

                    }
                }
            }
        }
        Debug.WriteLine($"End:{(DateTime.Now-now).TotalMilliseconds}");

	}
}