using HoldItApp.ViewModels;

namespace HoldItApp.Views;

public partial class HomePage : ContentPage
{
    public ShowCasePageViewModel SCPVM { get; set; }

    public HomePage()
	{
        SCPVM = new ShowCasePageViewModel();
		InitializeComponent();
        this.BindingContext = SCPVM;

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

    private void TakePicture_Clicked(object sender, EventArgs e)
    {
        SCPVM.source = cameraView.GetSnapShot(Camera.MAUI.ImageFormat.PNG);
        testIMG.Source = cameraView.GetSnapShot(Camera.MAUI.ImageFormat.PNG);
       BRDShowcase.ZIndex = 1;
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
        await cameraView.SaveSnapShot(Camera.MAUI.ImageFormat.PNG,"");
    }

    private async void profileBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(LoginPage));
    }

    private async void followedBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FollowedPage));
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