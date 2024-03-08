
namespace Amogus;

public partial class App : Application
{
	public const string Amogus = "AmogusCore";
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
