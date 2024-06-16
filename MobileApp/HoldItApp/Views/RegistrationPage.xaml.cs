using HoldItApp.ViewModels;

namespace HoldItApp.Views;

public partial class RegistrationPage : ContentPage
{
	public RegistrationPage()
	{
		InitializeComponent();
		this.BindingContext = new RegisterViewModel();
		BTNReg.IsEnabled = false;        		
    }

    private void CBTC_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
		if(CBTC.IsChecked == true)
		{
			BTNReg.IsEnabled = true;
		}
		else
		{
			BTNReg.IsEnabled = false;
		}
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync(nameof(TCPage));
    }
}