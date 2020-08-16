package mono.com.akaita.android.circularseekbar;


public class CircularSeekBar_OnCircularSeekBarChangeListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.akaita.android.circularseekbar.CircularSeekBar.OnCircularSeekBarChangeListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onProgressChanged:(Lcom/akaita/android/circularseekbar/CircularSeekBar;FZ)V:GetOnProgressChanged_Lcom_akaita_android_circularseekbar_CircularSeekBar_FZHandler:Com.Akaita.Android.Circularseekbar.CircularSeekBar/IOnCircularSeekBarChangeListenerInvoker, EDMTDev\n" +
			"n_onStartTrackingTouch:(Lcom/akaita/android/circularseekbar/CircularSeekBar;)V:GetOnStartTrackingTouch_Lcom_akaita_android_circularseekbar_CircularSeekBar_Handler:Com.Akaita.Android.Circularseekbar.CircularSeekBar/IOnCircularSeekBarChangeListenerInvoker, EDMTDev\n" +
			"n_onStopTrackingTouch:(Lcom/akaita/android/circularseekbar/CircularSeekBar;)V:GetOnStopTrackingTouch_Lcom_akaita_android_circularseekbar_CircularSeekBar_Handler:Com.Akaita.Android.Circularseekbar.CircularSeekBar/IOnCircularSeekBarChangeListenerInvoker, EDMTDev\n" +
			"";
		mono.android.Runtime.register ("Com.Akaita.Android.Circularseekbar.CircularSeekBar+IOnCircularSeekBarChangeListenerImplementor, EDMTDev", CircularSeekBar_OnCircularSeekBarChangeListenerImplementor.class, __md_methods);
	}


	public CircularSeekBar_OnCircularSeekBarChangeListenerImplementor ()
	{
		super ();
		if (getClass () == CircularSeekBar_OnCircularSeekBarChangeListenerImplementor.class)
			mono.android.TypeManager.Activate ("Com.Akaita.Android.Circularseekbar.CircularSeekBar+IOnCircularSeekBarChangeListenerImplementor, EDMTDev", "", this, new java.lang.Object[] {  });
	}


	public void onProgressChanged (com.akaita.android.circularseekbar.CircularSeekBar p0, float p1, boolean p2)
	{
		n_onProgressChanged (p0, p1, p2);
	}

	private native void n_onProgressChanged (com.akaita.android.circularseekbar.CircularSeekBar p0, float p1, boolean p2);


	public void onStartTrackingTouch (com.akaita.android.circularseekbar.CircularSeekBar p0)
	{
		n_onStartTrackingTouch (p0);
	}

	private native void n_onStartTrackingTouch (com.akaita.android.circularseekbar.CircularSeekBar p0);


	public void onStopTrackingTouch (com.akaita.android.circularseekbar.CircularSeekBar p0)
	{
		n_onStopTrackingTouch (p0);
	}

	private native void n_onStopTrackingTouch (com.akaita.android.circularseekbar.CircularSeekBar p0);

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
