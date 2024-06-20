namespace HoldItApp.Views;

public partial class IncognitoSupportPage : ContentPage
{
	public IncognitoSupportPage()
	{
		InitializeComponent();
	}

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}