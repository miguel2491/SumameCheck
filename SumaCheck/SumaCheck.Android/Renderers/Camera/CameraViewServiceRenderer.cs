﻿using Android;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Graphics;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using SumaCheck.Droid.Renderers.Camera;
using SumaCheck.Renderers;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CameraView), typeof(CameraViewServiceRenderer))]
namespace SumaCheck.Droid.Renderers.Camera
{
    public class CameraViewServiceRenderer: ViewRenderer<CameraView, CameraDroid>
    {
        private CameraDroid _camera;
        private readonly Context _context;
        private CameraOptions stateCam;

        public CameraViewServiceRenderer(Context context) : base(context)
        {
            _context = context;
            MessagingCenter.Subscribe<object>(this, "A", (e) =>
            {
                AbreCam();
            });
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CameraView> e)
        {
            base.OnElementChanged(e);

            var permissions = CameraPermissions();
            _camera = new CameraDroid(Context);

            CameraOptions CameraOption = e.NewElement?.Camera ?? CameraOptions.Rear;
            stateCam = e.NewElement?.Camera ?? CameraOptions.Rear;
            if (Control == null)
            {
                if (permissions)
                {
                    _camera.OpenCamera(CameraOption);
                    SetNativeControl(_camera);
                }
                else
                {
                    MainActivity.CameraPermissionGranted += (sender, args) =>
                    {
                        _camera.OpenCamera(CameraOption);

                        SetNativeControl(_camera);
                    };
                }
            }

            if (e.NewElement != null && _camera != null)
            {
                _camera.Photo += OnPhoto;
            }
        }

        public async void AbreCam()
        {
            await Task.Delay(1000);
            _camera.OpenCamera(stateCam);
            SetNativeControl(_camera);
            CapFoto();
        }

        public async void CapFoto()
        {
            await Task.Delay(4000);
            _camera.TakePhoto();
        }

        private async void OnPhoto(object sender, ImageSource imgSource)
        {
            var imageData = await RotateImageToPortrait(imgSource);

            Device.BeginInvokeOnMainThread(() =>
            {
                MainPage.OnPhotoCaptured(imageData);
            });
        }

        protected override void Dispose(bool disposing)
        {
            _camera.Photo -= OnPhoto;

            base.Dispose(disposing);
        }

        private bool CameraPermissions()
        {
            const string permission = Manifest.Permission.Camera;

            if ((int)Build.VERSION.SdkInt < 23 || ContextCompat.CheckSelfPermission(Android.App.Application.Context, permission) == Permission.Granted)
            {
                return true;
            }

            ActivityCompat.RequestPermissions((MainActivity)_context, MainActivity.CameraPermissions, MainActivity.CameraPermissionsCode);

            return false;
        }

        // ReSharper disable once UnusedMember.Local
        private async Task<ImageSource> RotateImageToPortrait(ImageSource imgSource)
        {
            var imagesourceHandler = new StreamImagesourceHandler();
            var photoTask = imagesourceHandler.LoadImageAsync(imgSource, _context);

            var photo = await photoTask;

            var matrix = new Matrix();

            matrix.PreRotate(-90);
            photo = Bitmap.CreateBitmap(photo, 0, 0, photo.Width, photo.Height, matrix, false);
            matrix.Dispose();

            var stream = new MemoryStream();
            photo.Compress(Bitmap.CompressFormat.Jpeg, 50, stream);
            stream.Seek(0L, SeekOrigin.Begin);

            return ImageSource.FromStream(() => stream);
        }
    }
}