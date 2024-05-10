using HoldItApp.ViewModels;
namespace HoldItApp.Views;

public partial class SearchResultPage : ContentPage
{
	public SearchResultPage()
	{
		InitializeComponent();
		this.BindingContext = new SearchResultsViewModel();
	}
}