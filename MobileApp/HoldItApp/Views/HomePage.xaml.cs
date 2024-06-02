using Camera.MAUI;
using HoldItApp.Services;
using HoldItApp.ViewModels;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Maui.ApplicationModel.Communication;

namespace HoldItApp.Views;

public partial class HomePage : ContentPage
{
    public ProfilePageViewModel PPVM { get; set; }
    public CameraView cv { get; set; }

    public HomePage()
	{
        PPVM = new ProfilePageViewModel();
        cv = new CameraView();
		InitializeComponent();
        this.BindingContext = PPVM;        
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
        cv = cameraView;
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
        await BRDShowcase.TranslateTo(0, 800, 300);
        string uId = await SecureStorage.GetAsync("userId");
        if (uId.IsNullOrEmpty())
        {
            await DisplayAlert("Please log in", "To post your moments please log into our system", "Back");
        }
        else
        {
            ActivityIndicator activityIndicator = new ActivityIndicator { IsRunning = true };
            await DataService.CaptureAndUploadImageAsync(Convert.ToInt32(uId), 0, cv);
            activityIndicator = new ActivityIndicator { IsRunning = false };
        }
        
             
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


}