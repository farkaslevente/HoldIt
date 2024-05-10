
using HoldItApp.Models;
using HoldItApp.ViewModels;
using HoldItApp.Views;
namespace HoldItApp.Views;

public partial class PPCatalogPage : ContentPage
{    
    public static string source;
    public PPCatalogPage()
		{				
		InitializeComponent();		
	}

    private void SelectedSource()
    {
        source = ppCW.SelectedItem.ToString();        
    }
}