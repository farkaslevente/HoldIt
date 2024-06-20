using CommunityToolkit.Maui.Views;
using FFImageLoading.Work;
using HoldItApp.Models;
using HoldItApp.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace HoldItApp.Views;

public partial class FollowedTimelinePage : ContentPage
{
    private PrivateTimelinePageViewModel vm = new PrivateTimelinePageViewModel();
	public FollowedTimelinePage()
	{
        this.BindingContext = vm;
        InitializeComponent();
		
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
        string uName = await SecureStorage.GetAsync("userName");
        if (uName.IsNullOrEmpty())
        {
            //Incognito support page as soon as the user manual is completed
            await Shell.Current.GoToAsync(nameof(SupportPage));
        }
        else
        {
            await Shell.Current.GoToAsync(nameof(SupportPage));
        }
    }
    private void uploadBTN_Clicked(object sender, EventArgs e)
    {
        Shell.Current.ShowPopup(new PrivateUploadPopUpPage());
    }

    private async void BTNBack_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void BTNInfo_Clicked(object sender, EventArgs e)
    {
        await SecureStorage.SetAsync("Includeprivate", "yes");
        Shell.Current.ShowPopup(new PopUpUserPage(vm.target));
    }
}