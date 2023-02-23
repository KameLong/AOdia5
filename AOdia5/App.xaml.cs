using AOdiaData;


namespace AOdia5;

public partial class App : Application
{
    //https://learn.microsoft.com/ja-jp/dotnet/maui/user-interface/pages/flyoutpage?view=net-maui-7.0

    public App()
	{
		InitializeComponent();
        MainPage = new MainPage();
		
	}
    protected override Window CreateWindow(IActivationState activationState)//オーバーライドしているので、そのメソッドを呼び出す。
    {
        Window window = base.CreateWindow(activationState);//Windowクラスのインスタンスを作成する
        window.Stopped += (s, e) =>   //←window.ライフサイクルイベント名(今回はStopped)でイベントハンドラーを作成
        {
            StaticData.staticDia.SaveChanges();
        };
        window.Deactivated += (s, e) =>
        {
            StaticData.staticDia.SaveChanges();
        };
        window.Destroying += (s, e) =>
        {
            StaticData.staticDia.SaveChanges();

        };
        return window;//windowを返すようにする
    }

}
