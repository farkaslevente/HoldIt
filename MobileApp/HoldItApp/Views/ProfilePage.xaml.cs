using HoldItApp.ViewModels;

namespace HoldItApp.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePageViewModel vm { get; set; }
    public bool VisibilityState { get; set; }
    public bool InversVisibilityState { get; set; }

    public ProfilePage()
    {
        this.BindingContext = new ShellViewModel();
        this.BindingContext = new ProfilePageViewModel();
        vm = new ProfilePageViewModel();
        vm.userChangeVisibility = false;
        VisibilityState = false;
        InversVisibilityState = true;
        InitializeComponent();

    }

    private async void ProfilePicChangeBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PPCatalogPage));
    }

    private void BTNLogout_Clicked(object sender, EventArgs e)
    {
        ShellViewModel ShellInstance = new ShellViewModel();
        DisplayAlert("Logout successful", "You logged out", "Rendben");
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

    private async void BTNProfile_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ProfilePage));
    }

    private async void BTNLogin_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(LoginPage));
    }

    //private async void BTNMyAds_Clicked(object sender, EventArgs e)
    //{
    //    await Shell.Current.GoToAsync(nameof(AdsPage));
    //}

    private async void BTNSupport_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SupportPage));
    }
    private async void BTNMainPage_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(HomePage));
    }

    //private async void BTNFav_Clicked(object sender, EventArgs e)
    //{
    //    await Shell.Current.GoToAsync(nameof(FavPage));
    //}   
}