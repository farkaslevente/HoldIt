using HoldItApp.ViewModels;

namespace HoldItApp.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePageViewModel vm { get; set; }
    public bool VisibilityState { get; set; }
    public bool InversVisibilityState { get; set; }

    public ProfilePage()
    {                        
        InitializeComponent();
        this.BindingContext = new ProfilePageViewModel();
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

    private async void uploadBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PopUpUploadPage));
    }

    private async void timelineBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(TimelinePage));
    }

    private async void settingsBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }
}