namespace HoldItApp.Views;

public partial class SupportPage : ContentPage
{
	public SupportPage()
	{
		InitializeComponent();
	}

    private async void BTNBack_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("..");
    }
}