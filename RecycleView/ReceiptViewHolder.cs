using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace CapturePicture.Model
{
   public class ReceiptViewHolder : RecyclerView.ViewHolder
    {
       
        public TextView priceTextView { get; set; }
        public TextView categoryNameTextView { get; set; }
        public TextView dateTextView { get; set; }
        public ReceiptViewHolder(View itemView, Action<int> listener):base(itemView)
        {
         
            priceTextView = itemView.FindViewById<TextView>(Resource.Id.ReceiptPirceTxt);
            categoryNameTextView = itemView.FindViewById<TextView>(Resource.Id.ReceiptCategoryTxt);
            dateTextView = itemView.FindViewById<TextView>(Resource.Id.ReceiptDateTxt);
            itemView.Click += (sender, e) => listener(base.LayoutPosition);
        }
    }

}