namespace HoldItApp.Views;

public partial class PageTester : ContentPage
{
	public PageTester()
	{
		InitializeComponent();
	}

    private async void LoginBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(LoginPage));
    }

    private async void RegistrationBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RegistrationPage));
    }

    private async void ForgottenPwdPageBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ForgottenPwdPage));
    }

    private async void ProfilePageBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ProfilePage));
    }

    private async void HomePageBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(HomePage));
    }

    private async void PPCatalogPageBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PPCatalogPage));
    }

    private async void TimeLinePageBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(TimelinePage));
    }

    private async void SearchResultPageBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SearchResultPage));
    }

    private async void FollowedPageBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FollowedPage));
    }

    private async void SupportPageBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SupportPage));
    }

    private async void settingsBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }
}