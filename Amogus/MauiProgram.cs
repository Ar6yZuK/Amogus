using Amogus.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

namespace Amogus;
public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkitMediaElement()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		string fileNameMaxCount = Path.Combine(Path.GetDirectoryName(typeof(MauiProgram).Assembly.Location)!, "count");
		if (!File.Exists(fileNameMaxCount))
			WriteCount();
		else
			AmogusPage.MaxCount = int.TryParse(File.ReadAllText(fileNameMaxCount), out int count) ? count : WriteCount();

		builder.ConfigureLifecycleEvents(x => x.AddWindows(windows =>
		{
			windows.OnWindowCreated(new WindowsLifecycle.OnWindowCreated(window =>
			{
				if (window is MauiWinUIWindow { Title: App.Amogus })
					Application.Current.OpenWindow(new Window(new AmogusPage()));
			}));

			windows.OnClosed(new WindowsLifecycle.OnClosed((window, args) =>
			{
				if (window is MauiWinUIWindow { Title: App.Amogus })
				{
					Application.Current.Quit();
					Environment.Exit(0);
				}
			}));
		}));

		return builder.Build();

		int WriteCount()
		{
			int maxCount = AmogusPage.MaxCount;
			File.WriteAllText(fileNameMaxCount, maxCount.ToString());
			return maxCount;
		}
	}
}
