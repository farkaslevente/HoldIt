using CommunityToolkit.Maui.Views;
using HoldItApp.Models;
using HoldItApp.ViewModels;

namespace HoldItApp.Views;

public partial class PopUpDetailsPage : Popup
{    

    public PopUpDetailsPage(PostModel pm)
	{
        InitializeComponent();
        this.BindingContext = new PopUpDetailsViewModel(pm);
    }
}