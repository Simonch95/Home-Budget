package mono.com.akaita.android.circularseekbar;


public class CircularSeekBar_OnCenterClickedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.akaita.android.circularseekbar.CircularSeekBar.OnCenterClickedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCenterClicked:(Lcom/akaita/android/circularseekbar/CircularSeekBar;F)V:GetOnCenterClicked_Lcom_akaita_android_circularseekbar_CircularSeekBar_FHandler:Com.Akaita.Android.Circularseekbar.CircularSeekBar/IOnCenterClickedListenerInvoker, EDMTDev\n" +
			"";
		mono.android.Runtime.register ("Com.Akaita.Android.Circularseekbar.CircularSeekBar+IOnCenterClickedListenerImplementor, EDMTDev", CircularSeekBar_OnCenterClickedListenerImplementor.class, __md_methods);
	}


	public CircularSeekBar_OnCenterClickedListenerImplementor ()
	{
		super ();
		if (getClass () == CircularSeekBar_OnCenterClickedListenerImplementor.class)
			mono.android.TypeManager.Activate ("Com.Akaita.Android.Circularseekbar.CircularSeekBar+IOnCenterClickedListenerImplementor, EDMTDev", "", this, new java.lang.Object[] {  });
	}


	public void onCenterClicked (com.akaita.android.circularseekbar.CircularSeekBar p0, float p1)
	{
		n_onCenterClicked (p0, p1);
	}

	private native void n_onCenterClicked (com.akaita.android.circularseekbar.CircularSeekBar p0, float p1);

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
