using HoldItApp.ViewModels;

namespace HoldItApp.Views;

public partial class FollowedPage : ContentPage
{
	public FollowedPage()
	{
		InitializeComponent();		
	}

    private async void homeBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(HomePage));
    }

    private async void timelineBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(TimelinePage));
    }

    private async void profileBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ProfilePage));
    }
}