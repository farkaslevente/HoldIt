using Camera.MAUI;
using HoldItApp.Services;
using HoldItApp.ViewModels;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Maui.ApplicationModel.Communication;

namespace HoldItApp.Views;

public partial class HomePage : ContentPage
{
    public ProfilePageViewModel PPVM { get; set; }
    public Stream cv;

    public HomePage()
	{
        PPVM = new ProfilePageViewModel();        
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
        cv = await cameraView.TakePhotoAsync();

        previewIMG.Source = cameraView.GetSnapShot(Camera.MAUI.ImageFormat.PNG);
        await BRDShowcase.TranslateTo(0, -800, 300);

    }
    private async void saveImageBTN_Clicked(object sender, EventArgs e)
    {        
        await BRDShowcase.TranslateTo(0, 800, 300);
        string uId = await SecureStorage.GetAsync("userId");
        if (uId.IsNullOrEmpty())
        {
            await DisplayAlert("Please log in", "To post your moments please log into our system", "Back");
        }
        else
        {            
            string fileName = await DataService.CaptureAndUploadImageAsync(Convert.ToInt32(uId), 0, cv);
            string result = await DataService.newPostUpload($"http://192.168.0.165:9000/uploads/{fileName}", PPVM.comment, Convert.ToInt32(fileName.Split("_")[0]));
            if (result == "error") {
                await DisplayAlert("Something went wrong...", "An error occured while we uploaded your post, please try again later.", "Back");
            }
            PPVM.comment = "";
        }
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
        string uName = await SecureStorage.GetAsync("userName");
        if (uName.IsNullOrEmpty())
        {
            await Shell.Current.GoToAsync(nameof(SettingsPage));
        }
        else
        {
            await Shell.Current.GoToAsync(nameof(SettingsPage));
        }
        
    }

    private async void PageTesterBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PageTester));
    }

    private async void dismissBTN_Clicked(object sender, EventArgs e)
    {
        await BRDShowcase.TranslateTo(0, 800, 300);
    }
}