using System;
using Android.Hardware.Camera2;

namespace SumaCheck.Droid.Renderers.Camera
{
    public class CameraCaptureListener : CameraCaptureSession.CaptureCallback
    {
        public event EventHandler PhotoComplete;

        public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request, TotalCaptureResult result)
        {
            PhotoComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}