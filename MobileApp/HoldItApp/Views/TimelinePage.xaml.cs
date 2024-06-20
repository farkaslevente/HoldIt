using CommunityToolkit.Maui.Views;
using HoldItApp.Models;
using HoldItApp.Services;
using HoldItApp.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;

namespace HoldItApp.Views;

public partial class TimelinePage : ContentPage
{
    public ObservableCollection<PostModel> tempposts { get; set; }
    public TimelinePageViewModel vm { get; set; }
    public TimelinePage()
	{
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

    private async void settingsBTN_Clicked(object sender, EventArgs e)
    {
        string uName = await SecureStorage.GetAsync("userName");
        if (uName.IsNullOrEmpty())
        {            
            await Shell.Current.GoToAsync(nameof(IncognitoSupportPage));
        }
        else
        {
            await Shell.Current.GoToAsync(nameof(SupportPage));
        }
    }
    private void uploadBTN_Clicked(object sender, EventArgs e)
    {
        Shell.Current.ShowPopup(new PopUpUploadPage());
    }
    private void CVPosts_SizeChanged(object sender, EventArgs e)
    {
        if (CVPosts.ItemsSource != null && vm.posts != null && vm.posts.Any())
        {
            PostModel pm = vm.posts.LastOrDefault();            
            CVPosts.ScrollTo(pm, position: ScrollToPosition.End);
        }
    }

    private async void homeBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(HomePage));
    }
}
