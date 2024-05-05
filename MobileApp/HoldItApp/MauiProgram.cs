using CommunityToolkit.Maui;
using FFImageLoading.Maui;
using Microsoft.Extensions.Logging;
using Camera.MAUI;
using Microsoft.Maui.LifecycleEvents;

namespace HoldItApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseFFImageLoading()
                .UseMauiCameraView()
                .UseMauiCommunityToolkitMediaElement()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


#if DEBUG
    		builder.Logging.AddDebug();
#endif
//            builder.ConfigureLifecycleEvents(events =>
//            {
//#if ANDROID
//                events.AddAndroid(android => android.OnCreate((activity, bundle) => MakeStatusBarTranslucent(activity)));

//                static void MakeStatusBarTranslucent(Android.App.Activity activity)
//                {
//                    activity.Window.SetFlags(Android.Views.WindowManagerFlags.LayoutNoLimits, Android.Views.WindowManagerFlags.LayoutNoLimits);

//                    activity.Window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);

//                    activity.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
//                }
//#endif
//            });
            return builder.Build();
        }
    }
}
