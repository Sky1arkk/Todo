using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;

namespace Todo.Droid
{
    [Activity(Label = "Todo", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            //test branch
            base.OnCreate(bundle);

            App.ScreenWidth = (Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);
            App.ScreenHeight = (Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);

            if (App.ScreenHeight > 800)
            {
                SetTheme(Resource.Style.TodoThemeHighSize);
            }

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}

