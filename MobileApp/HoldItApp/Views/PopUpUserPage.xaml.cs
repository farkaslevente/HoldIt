using CommunityToolkit.Maui.Views;
using HoldItApp.ViewModels;
using HoldItApp.Models;

namespace HoldItApp.Views;

public partial class PopUpUserPage : Popup
{
	public PopUpUserPage(UserModel pm)
	{
        this.BindingContext = new PopUpUserViewModel(pm);
        InitializeComponent();
		
	}
}