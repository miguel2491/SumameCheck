using System;
using Plugin.Media.Abstractions;

using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;
using SumaCheck.Renderers;
using System.Reflection;
using System.IO;

namespace SumaCheck
{
    public partial class MainPage : ContentPage
    {
        private MediaFile _image;
        public string filename;
        public string foto_;
        public static event EventHandler<ImageSource> PhotoCapturedEvent;
        

        public MainPage()
        {
            InitializeComponent();
            Scanner();
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                lblTime.Text = DateTime.Now.ToString("HH:mm:ss")
                );
                return true;
            });

            PhotoCapturedEvent += (sender, source) =>
            {
                PhotoCaptured.Source = source;
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //_scanView.IsScanning = true;
            
        }

        private void OpenScanner(object sender, EventArgs e)
        {
            //Scanner();
            //ScannerV();
        }

        public static void OnPhotoCaptured(ImageSource src)
        {
            PhotoCapturedEvent?.Invoke(new MainPage(), src);
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
                DependencyService.Get<IAudio>().PlayAudioFile("beep.mp3");
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
                            color = hra_llegada;
                            txt_retardo.TextColor = Color.Black;
                        }
                        else
                        {
                            retardo = "RETARDO : "+ hra_llegada;
                            imgRetardo = "uncheck.png";
                            color = "#BF2604";
                            txt_retardo.TextColor = Color.Red;
                        }
                    }
                    else
                    {
                        retardo = "RETARDO : "+ hra_llegada;
                        imgRetardo = "uncheck.png";
                        color = "#BF2604";
                        txt_retardo.TextColor = Color.Red;
                    }
                    img_retardo.IsVisible = true;
                    img_retardo.Source = imgRetardo;
                    txt_noticia.Text = result.Text;
                    //txt_retardo.TextColor = color;
                    txt_retardo.Text = retardo;
                    //DisplayAlert("Bienvenido: ", result.Text, "OK");
                    DependencyService.Get<IAudio>().PlayAudioFile("beep.mp3");
                    MessagingCenter.Send<object>(this, "A");
                    Ocultar();
                    //Reiniciar();
                });

            };

            await Navigation.PushAsync(ScannerPage);
        }

        public void ActivarCam()
        {
            //_scanView.IsVisible = false;

        }

        public async void Ocultar()
        {
            await Task.Delay(5000);
            CamaraPre.IsVisible = false;
            Reiniciar();
        }

        public async void Reiniciar() {
            await Task.Delay(10000);
            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage = new NavigationPage(new MainPage());
            });
        }

    }
}
