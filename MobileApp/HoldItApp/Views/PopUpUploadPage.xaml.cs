using CommunityToolkit.Maui.Views;
using HoldItApp.ViewModels;

namespace HoldItApp.Views;

public partial class PopUpUploadPage : Popup
{
	public PopUpUploadPage()
    {
        var viewModel = new PopUpViewModel();
        if (viewModel != null)
        {
            viewModel.comment = ""; // Initialize the comment property
            this.BindingContext = viewModel;
        }
        InitializeComponent();
        PopUpViewModel.ImageDataUpdated += OnImageDataUpdated;
    }

    private void OnImageDataUpdated(object sender, byte[] imageData)
    {
        showCaseIMG.Source = ImageSource.FromStream(() => new MemoryStream(imageData));
    }
}