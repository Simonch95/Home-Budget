package crc64b3f9853946985b33;


public class ReceiptViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("CapturePicture.Model.ReceiptViewHolder, CapturePicture", ReceiptViewHolder.class, __md_methods);
	}


	public ReceiptViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == ReceiptViewHolder.class)
			mono.android.TypeManager.Activate ("CapturePicture.Model.ReceiptViewHolder, CapturePicture", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
	}

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
