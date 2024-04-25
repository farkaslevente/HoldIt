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

    private void Button_Clicked(object sender, EventArgs e)
    {
        SCPVM.source = cameraView.GetSnapShot(Camera.MAUI.ImageFormat.PNG);
        //testIMG.Source = cameraView.GetSnapShot(Camera.MAUI.ImageFormat.PNG);
       BRDShowcase.ZIndex = 1;
    }
}