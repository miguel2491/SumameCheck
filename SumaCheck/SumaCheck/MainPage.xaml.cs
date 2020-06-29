﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace SumaCheck
{
    public partial class MainPage : ContentPage
    {
        //private MediaFile _image;
        public string filename;
        public string foto_;

        public MainPage()
        {
            InitializeComponent();
            //Scanner();
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                lblTime.Text = DateTime.Now.ToString("HH:mm:ss")
                );
                return true;
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _scanView.IsScanning = true;
            
        }

        private void OpenScanner(object sender, EventArgs e)
        {
            Scanner();
            //ScannerV();
        }

        public async void ScannerV()
        {
            
        }
        public void Handle_OnScanResult(ZXing.Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Scanned result", result.Text, "OK");
                txt_noticia.Text = "Bienvenido " + result.Text;
            });
        }
        public async void Scanner()
        {
            var options = new MobileBarcodeScanningOptions
            {
                AutoRotate = false,
                UseFrontCameraIfAvailable = true,
                TryHarder = false,
                DisableAutofocus = false,
                UseNativeScanning = true
            };
            options.PossibleFormats.Add(ZXing.BarcodeFormat.QR_CODE);

            var overlay = new ZXingDefaultOverlay
            {
                ShowFlashButton = false,
                TopText = "Coloca tu Código QR Frente de la Camara"
            };
            var ScannerPage = new ZXingScannerPage(options, overlay);
            ScannerPage.WidthRequest = 10;
            ScannerPage.AutoFocus();
            ScannerPage.OnScanResult += (result) =>
            {
                ScannerPage.IsScanning = false;
                ScannerPage.IsAnalyzing = false;
                if (ScannerPage.IsScanning) {
                    ScannerPage.AutoFocus();
                }


                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopAsync();
                    txt_noticia.Text = "Bienvenido "+result.Text;
                    DisplayAlert("Bienvenido: ", result.Text, "OK");
                    
                    //ActivarCam();
                });

            };

            await Navigation.PushAsync(ScannerPage);
        }

        public  void ActivarCam()
        {
            /*await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera soportada.", "OK");
                return;
            }
            _image = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Auto_",
                Name = foto_ + ".jpg"
            });
            if (_image == null)
                return;
            // await DisplayAlert("File Location Error", "Error parece que hubo un problema con la camara, confirme espacio en memoria o notifique a sistemas", "OK");
            var xlocal = _image.Path;
            img_a.Source = ImageSource.FromStream(() => {

                return _image.GetStream();


            });*/
        }
    }
}
