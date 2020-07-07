package md5e03c2ab233e792209749a093a6684b46;


public class ImageAvailableListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.media.ImageReader.OnImageAvailableListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onImageAvailable:(Landroid/media/ImageReader;)V:GetOnImageAvailable_Landroid_media_ImageReader_Handler:Android.Media.ImageReader/IOnImageAvailableListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("SumaCheck.Droid.Renderers.Camera.ImageAvailableListener, SumaCheck.Android", ImageAvailableListener.class, __md_methods);
	}


	public ImageAvailableListener ()
	{
		super ();
		if (getClass () == ImageAvailableListener.class)
			mono.android.TypeManager.Activate ("SumaCheck.Droid.Renderers.Camera.ImageAvailableListener, SumaCheck.Android", "", this, new java.lang.Object[] {  });
	}


	public void onImageAvailable (android.media.ImageReader p0)
	{
		n_onImageAvailable (p0);
	}

	private native void n_onImageAvailable (android.media.ImageReader p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
