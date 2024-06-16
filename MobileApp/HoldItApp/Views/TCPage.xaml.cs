namespace HoldItApp.Views;

public partial class TCPage : ContentPage
{
	public TCPage()
	{
		InitializeComponent();
	}

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}