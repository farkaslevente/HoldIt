using HoldItApp.ViewModels;
using HoldItApp.Views;
namespace HoldItApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {

            InitializeComponent();
            ShellViewModel shellViewModel = new ShellViewModel();
            this.BindingContext = shellViewModel;
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            Routing.RegisterRoute(nameof(ForgottenPwdPage), typeof(ForgottenPwdPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(PPCatalogPage), typeof(PPCatalogPage));
            Routing.RegisterRoute(nameof(TimelinePage), typeof(TimelinePage));            
            Routing.RegisterRoute(nameof(SearchResultPage), typeof(SearchResultPage));
            Routing.RegisterRoute(nameof(FollowedPage), typeof(FollowedPage));                        
            Routing.RegisterRoute(nameof(SupportPage), typeof(SupportPage));            
            Routing.RegisterRoute(nameof(ResetPwdCodePage), typeof(ResetPwdCodePage));
            Routing.RegisterRoute(nameof(ResetPwdFinalStagePage), typeof(ResetPwdFinalStagePage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));


        }       
    }
}
