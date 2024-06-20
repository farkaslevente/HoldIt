using CommunityToolkit.Maui.Views;
using HoldItApp.ViewModels;

namespace HoldItApp.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePageViewModel vm { get; set; }
    public bool VisibilityState { get; set; }
    public bool InversVisibilityState { get; set; }

    public ProfilePage()
    {
        fromPP();
        InitializeComponent();
        this.BindingContext = new ProfilePageViewModel();        
    }

    private async Task fromPP()
    {
        await SecureStorage.SetAsync("fromPP", false.ToString());
    }

    private void logoutBTN_Clicked(object sender, EventArgs e)
    {
        ShellViewModel ShellInstance = new ShellViewModel();
        DisplayAlert("Logout successful", "Your profile was logged out", "Okay");
        SecureStorage.Remove("userName");
        SecureStorage.Remove("userEmail");
        SecureStorage.Remove("userImage");
        SecureStorage.Remove("userFollowed");
        SecureStorage.Remove("userId");
        SecureStorage.Remove("userRole");
        SecureStorage.Remove("userToken");
        ShellInstance.VisibilityLP();
        ShellInstance.LoggedInAdmin = false;
        Shell.Current.GoToAsync(nameof(HomePage));
    }

    private async void followedBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FollowedPage));
    }    

    private async void timelineBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(TimelinePage));
    }

    private async void homeBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(HomePage));
    }

    private async void suppport_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SupportPage));
    }

    private async void BTNChange_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PPCatalogPage));
    }
}