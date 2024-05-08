using CommunityToolkit.Maui.Views;
using HoldItApp.Models;
using HoldItApp.Services;
using HoldItApp.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;

namespace HoldItApp.Views;

public partial class TimelinePage : ContentPage
{
    public ObservableCollection<PostModel> posts { get; set; }
    public TimelinePage()
	{
		InitializeComponent();
        posts = new ObservableCollection<PostModel>();
    }

    private async void profileBTN_Clicked(object sender, EventArgs e)
    {
        string uName = await SecureStorage.GetAsync("userName");
        if (uName.IsNullOrEmpty())
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));

        }
        else
        {
            await Shell.Current.GoToAsync(nameof(ProfilePage));
        }
    }

    private async void followedBTN_Clicked(object sender, EventArgs e)
    {
        string uName = await SecureStorage.GetAsync("userName");
        if (uName.IsNullOrEmpty())
        {
            await Shell.Current.DisplayAlert("Unauthorized action detected", "To view your follows you must log into our application first!", "Go back");
        }
        else
        {
            await Shell.Current.GoToAsync(nameof(FollowedPage));
        }

    }

    private async void timelineBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(TimelinePage));
    }

    private async void settingsBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }
    private void uploadBTN_Clicked(object sender, EventArgs e)
    {
        Shell.Current.ShowPopup(new PopUpUploadPage());
    }        
}