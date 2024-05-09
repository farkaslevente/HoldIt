using HoldItApp.ViewModels;

namespace HoldItApp.Views;

public partial class RegistrationPage : ContentPage
{
	public RegistrationPage()
	{
		InitializeComponent();
		this.BindingContext = new RegisterViewModel();
	}
}