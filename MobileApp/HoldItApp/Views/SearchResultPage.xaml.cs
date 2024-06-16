using HoldItApp.ViewModels;
namespace HoldItApp.Views;

public partial class SearchResultPage : ContentPage
{
	public SearchResultPage()
	{
		InitializeComponent();
		this.BindingContext = new SearchResultsViewModel();
	}

    private void ImageButton_Clicked(object sender, EventArgs e)
    {

    }
}