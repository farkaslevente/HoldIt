using HoldItApp.ViewModels;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Maui.ApplicationModel.Communication;

namespace HoldItApp.Views;

public partial class HomePage : ContentPage
{
    public ProfilePageViewModel PPVM { get; set; }

    public HomePage()
	{
        PPVM = new ProfilePageViewModel();
		InitializeComponent();
        this.BindingContext = PPVM;
        //searchBTN.IsEnabled = true;
        finalSearchBTN.IsEnabled = true;
        finalSearchBTN.Command = PPVM.SearchCommand;

    }
    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        cameraView.Camera = cameraView.Cameras.First();
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await cameraView.StopCameraAsync();
            var result = await cameraView.StartCameraAsync();
        });
    }

    private async void TakePicture_Clicked(object sender, EventArgs e)
    {        
        testIMG.Source = cameraView.GetSnapShot(Camera.MAUI.ImageFormat.PNG);
        await BRDShowcase.TranslateTo(0, -800, 300);

    }

    private void torchBTN_Clicked(object sender, EventArgs e)
    {
        cameraView.TorchEnabled = !cameraView.TorchEnabled;
        if (cameraView.TorchEnabled == true)
        {
            torchBTN.Source = "torchactivated.svg";
        } else {
            torchBTN.Source = "torchdeactivated.svg";
        }
        
    }

    private async void saveImageBTN_Clicked(object sender, EventArgs e)
    {
        //tobe done tomorrow
        await cameraView.SaveSnapShot(Camera.MAUI.ImageFormat.PNG,"DCIM/Camera");
        BRDShowcase.ZIndex = -1;
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
            await Shell.Current.DisplayAlert("Unauthorized action detected","To view your follows you must log into our application first!","Go back");
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

    private async void PageTesterBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PageTester));
    }

    private async void searchBTN_Clicked(object sender, EventArgs e)
    {
        await SearchGrid.TranslateTo(0, 100, 300);
       
        //searchBTN.IsEnabled = false;
        //searchBTN.IsVisible = false;
        
        finalSearchBTN.IsEnabled = true;
        finalSearchBTN.IsVisible = true;
    }

    private void finalSearchBTN_Clicked(object sender, EventArgs e)
    {
        //searchBTN.IsEnabled = true;
        //searchBTN.IsVisible = true;
        
        finalSearchBTN.IsEnabled = false;
        finalSearchBTN.IsVisible = false;
    }
}