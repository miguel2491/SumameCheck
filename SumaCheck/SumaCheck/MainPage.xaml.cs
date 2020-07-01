using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SumaCheck.Views;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace SumaCheck
{
    public partial class MainPage : ContentPage
    {
        private MediaFile _image;
        public string filename;
        public string foto_;

        public MainPage()
        {
            InitializeComponent();
            //Scanner();
            CameraButton.Clicked += CameraButton_Clicked;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                lblTime.Text = DateTime.Now.ToString("HH:mm:ss")
                );
                return true;
            });
        }

        private async void CameraButton_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera soportada.", "OK");
                return;
            }
            _image = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Check_",
                Name = foto_ + ".jpg",
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 1000,
                DefaultCamera = CameraDevice.Front
            });
            if (_image == null)
                return;
            await DisplayAlert("File Location", _image.Path, "OK");
            PhotoImage.Source = ImageSource.FromStream(() =>
            {
                return _image.GetStream();
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _scanView.IsScanning = true;
            
        }

        private void OpenScanner(object sender, EventArgs e)
        {
            //Scanner();
            //ScannerV();
        }

        public async void ScannerV()
        {
            
        }
        public void Handle_OnScanResult(ZXing.Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                //await DisplayAlert("Scanned result", result.Text, "OK");
                var hra_llegada = lblTime.Text;
                
                string[] hora_g = hra_llegada.Split(':');
                var h_ = 0;
                var m_ = 0; 
                foreach (var hr in hora_g)
                {
                    var index = Array.IndexOf(hora_g, hr);
                    if (index == 0)
                    {
                        h_ = Convert.ToInt32(hr);
                    }
                    else if (index == 1)
                    {
                        m_ = Convert.ToInt16(hr);
                    }
                }
                var retardo = "";
                var imgRetardo = "";
                var color = "";
                if (h_ < 9)
                {
                    if (m_ < 36)
                    {
                        retardo = "PUNTUAL";
                        imgRetardo = "check.png";
                        color = "#4D7356";
                        txt_retardo.TextColor = Color.Black;
                    }
                    else {
                        retardo = "RETARDO";
                        imgRetardo = "uncheck.png";
                        color = "#BF2604";
                        txt_retardo.TextColor = Color.Red;
                    }
                }
                else {
                    retardo = "RETARDO";
                    imgRetardo = "uncheck.png";
                    color = "#BF2604";
                    txt_retardo.TextColor = Color.Red;
                }
                img_retardo.IsVisible = true;
                img_retardo.Source = imgRetardo;
                txt_noticia.Text = "Bienvenido " + result.Text;
                //txt_retardo.TextColor = color;
                txt_retardo.Text = retardo;
                ActivarCam();
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

        public async void ActivarCam()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera soportada.", "OK");
                return;
            }
            _image = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Check_",
                Name = foto_ + ".jpg",
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 1000,
                DefaultCamera = CameraDevice.Front
            });
            if (_image == null)
                return;
            
            PhotoImage.Source = ImageSource.FromStream(() =>
            {
                return _image.GetStream();
            });
            //NavigationPage page = App.Current.MainPage as NavigationPage;
            //await Navigation.PushAsync(new Reconocimiento());
            /*await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera soportada.", "OK");
                return;
            }
            
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
