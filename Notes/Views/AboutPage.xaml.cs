using Notes.Models;

namespace Notes.Views;

public partial class AboutPage : ContentPage
{
	public AboutPage()
	{
		InitializeComponent();
	}

    private async void LearnMoreButton_Clicked(object sender, EventArgs e)
    {
		//await Launcher.Default.OpenAsync("https://aka.ms/maui");

		if (BindingContext is About about)
		{
			await Launcher.OpenAsync(about.MoreInfoUrl);
		}
    }
}