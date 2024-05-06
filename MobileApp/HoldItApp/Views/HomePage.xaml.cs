using HoldItApp.ViewModels;
using Microsoft.IdentityModel.Tokens;

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

    private async void TakePicture_Clicked(object sender, EventArgs e)
    {
        // SCPVM.source = cameraView.GetSnapShot(Camera.MAUI.ImageFormat.PNG);
        // testIMG.Source = cameraView.GetSnapShot(Camera.MAUI.ImageFormat.PNG);
        //BRDShowcase.ZIndex = 1;
        //if (MediaPicker.Default.IsCaptureSupported)
        //{
        //    FileResult myPhoto = await MediaPicker.Default.CapturePhotoAsync();
        //    if (myPhoto != null)
        //    {
        //        string localFilePath = Path.Combine(FileSystem.CacheDirectory, myPhoto.FileName);
        //        using Stream sourceStream = await myPhoto.OpenReadAsync();
        //        using FileStream localFileStream = File.OpenWrite(localFilePath);
        //        await sourceStream.CopyToAsync(localFileStream);
        //    }
        //}
        //else
        //{
        //    await Shell.Current.DisplayAlert("Error", "It seems like your device isn't supported", "Go back");
        //}

        //try
        //{
        //    var snapshot = await cameraView.GetSnapshotAsync();

        //    if (snapshot != null)
        //    {
        //        // Save the snapshot to the device's storage
        //        var photoPath = Path.Combine(FileSystem.CacheDirectory, "snapshot.png");
        //        using (var stream = new FileStream(photoPath, FileMode.Create))
        //        {
        //            await stream.WriteAsync(snapshot);
        //        }

        //        // Save the photo to the device's gallery
        //        await MediaGallery.SaveToGallery(photoPath);

        //        // Display success message
        //        await Shell.Current.DisplayAlert("Success", "Snapshot saved to gallery successfully", "OK");
        //    }
        //    else
        //    {
        //        await Shell.Current.DisplayAlert("Error", "Failed to capture snapshot", "OK");
        //    }
        //}
        //catch (Exception ex)
        //{
        //    // Handle exception
        //    await Shell.Current.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        //}
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
}