using Android.Content;
using Android.Views;
using Android.Util;
using Java.Lang;

namespace SumaCheck.Droid.Renderers.Camera
{
    public class CameraTextureView : TextureView
    {
        private int _mRationWidth;
        private int _mRationHeight;

        public CameraTextureView(Context context) : base(context, null)
        {
        }

        public CameraTextureView(Context context, IAttributeSet attrs) :
            base(context, attrs, 0)
        {
        }

        public CameraTextureView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
        }

        public void SetAspectRatio(int width, int height)
        {
            if (width < 0 || height < 0)
                throw new IllegalArgumentException("Tamaño no puede ser negativo");

            if (_mRationWidth == width && _mRationHeight == height)
                return;
            _mRationWidth = width;
            _mRationHeight = height;
            RequestLayout();
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            var width = MeasureSpec.GetSize(widthMeasureSpec);
            var height = MeasureSpec.GetSize(heightMeasureSpec);

            if (_mRationWidth == 0 || _mRationHeight == 0)
            {
                SetMeasuredDimension(width, height);
            }
            else
            {
                if (width < height * _mRationWidth / _mRationHeight)
                {
                    SetMeasuredDimension(width, width * _mRationHeight / _mRationWidth);
                }
                else
                {
                    SetMeasuredDimension(height * _mRationWidth / _mRationHeight, height);
                }
            }
        }

    }
}