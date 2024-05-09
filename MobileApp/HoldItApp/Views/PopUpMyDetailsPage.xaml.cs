using CommunityToolkit.Maui.Views;
using HoldItApp.Models;
using HoldItApp.ViewModels;

namespace HoldItApp.Views;

public partial class PopUpMyDetailsPage : Popup
{
    public PopUpMyDetailsPage(PostModel pm)
    {
        InitializeComponent();
        this.BindingContext = new PopUpDetailsViewModel(pm);
    }
}