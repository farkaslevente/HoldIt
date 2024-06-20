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
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(PPCatalogPage), typeof(PPCatalogPage));
            Routing.RegisterRoute(nameof(TimelinePage), typeof(TimelinePage));            
            Routing.RegisterRoute(nameof(SearchResultPage), typeof(SearchResultPage));
            Routing.RegisterRoute(nameof(FollowedPage), typeof(FollowedPage));
            Routing.RegisterRoute(nameof(FollowedTimelinePage), typeof(FollowedTimelinePage));
            Routing.RegisterRoute(nameof(SupportPage), typeof(SupportPage));            
            Routing.RegisterRoute(nameof(ResetPwdCodePage), typeof(ResetPwdCodePage));
            Routing.RegisterRoute(nameof(ResetPwdFinalStagePage), typeof(ResetPwdFinalStagePage));            
            Routing.RegisterRoute(nameof(PopUpUploadPage), typeof(PopUpUploadPage));
            Routing.RegisterRoute(nameof(PrivateTimelinePageViewModel), typeof(PrivateTimelinePageViewModel));
            Routing.RegisterRoute(nameof(SupportPage), typeof(SupportPage));
            Routing.RegisterRoute(nameof(TCPage), typeof(TCPage));
            Routing.RegisterRoute(nameof(IncognitoSupportPage), typeof(IncognitoSupportPage));
        }       
    }
}
