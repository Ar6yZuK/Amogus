using CommunityToolkit.Maui.Views;

namespace Amogus.Views;

public partial class AmogusPage : ContentPage
{
	public static int Count { get; private set; }
	public static int MaxCount { get; set; } = 5;
	
	public AmogusPage()
	{
		InitializeComponent();

		if (IsAmogusPage())
			return;

		media.StateChanged += Media_StateChanged;
	}

	private bool IsAmogusPage()
	{
		return App.Current.MainPage == null || Shell.Current.CurrentPage == this;
	}

	private void Media_StateChanged(object? sender, CommunityToolkit.Maui.Core.Primitives.MediaStateChangedEventArgs e)
	{
		if (e.NewState is CommunityToolkit.Maui.Core.Primitives.MediaElementState.Opening or not CommunityToolkit.Maui.Core.Primitives.MediaElementState.Stopped)
			return;

		if (Count++ >= MaxCount)
			return;

		Application.Current.Dispatcher.Dispatch(() =>
		{
			Application.Current.OpenWindow(new Window(new AmogusPage()));
		});
		media.StateChanged -= Media_StateChanged;
	}

	bool loaded = false;
	object lockObj = new();
	protected override async void OnAppearing()
	{
		lock (lockObj)
			if (!loaded)
			{
				Window.Destroying += Window_Destroying;
				loaded = true;
			}

		if (IsAmogusPage())
			return;

		await Task.Delay(200);
		media.Play();
	}

	private void Window_Destroying(object? sender, EventArgs e)
	{
		media.Stop();
		media.Dispose();
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		media.Play();
	}
}