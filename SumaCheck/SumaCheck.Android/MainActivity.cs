using System;
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Support.Design.Widget;


namespace SumaCheck.Droid
{
    [Activity(Label = "SumaCheck", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public const int CameraPermissionsCode = 1;

        public static readonly string[] CameraPermissions =
        {
            Manifest.Permission.Camera
        };

        public static event EventHandler CameraPermissionGranted;
        private View _layout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}