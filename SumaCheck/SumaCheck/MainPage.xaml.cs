using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
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
            Scanner();
        }

        private void OpenScanner(object sender, EventArgs e)
        {
            Scanner();
        }

        public async void Scanner()
        {
            var ScannerPage = new ZXingScannerPage();

            ScannerPage.OnScanResult += (result) =>
            {
                ScannerPage.IsScanning = false;

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
