using d9.dgl;
namespace d9.dgl.viewer;
public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();		
	}
    private bool _started = false;
    private async void StartGame(object sender, EventArgs e)
    {
        if (_started)
            return;
        _started = true;
        (sender as Button).Text = "running...";
        Board currentBoard = new(100, 100, 0.1f);
        for (int i = 0; i < 1000; i++)
        {
            Image.Source = ImageSource.FromStream(() => new MemoryStream(currentBoard.BmpBytes));
            _ = await Task.Run(() => currentBoard = new(currentBoard));
        }
        (sender as Button).Text = "Start";
    }
}