using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using CapturePicture.DataBase;
using Android.Graphics;
using System.Collections.ObjectModel;
using Android.Support.Design.Widget;
using Android.Util;
using CapturePicture.Model;

namespace CapturePicture
{
    public class ReceiptAdapter : RecyclerView.Adapter
    {
        public Activity _activity;
        public event EventHandler<int> ItemClick;
        public int selectedPosition = -1;
        public static int listPosition;        
       

        private string[] category =
         {
            "", "Spożywcze", "Samochód", "Kosmetyki", "Transport", "Dom", "Rozrywka", "Ubrania", "Apteka", "Prezenty",
            "Inne", 
        };

        public List<Receipt> listOfReceipt;
     
        public ReceiptAdapter(List<Receipt> list)
        {
            this.listOfReceipt = list;
        }
      
        void OnClick(int position)
        {
            if (ItemClick != null)
                 ItemClick(this, position);           
        }


        public override int ItemCount
        {
            
            get {return DatabaseController.SelectReceipts().Count; } 
        }
     
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            //2

            ReceiptViewHolder rh = holder as ReceiptViewHolder;
            if(listOfReceipt.Count>position)
            {
                rh.priceTextView.Text = (Convert.ToString(listOfReceipt[position].Price)) + "  PLN";
                rh.categoryNameTextView.Text = category[listOfReceipt[position].Id_Category-1];
                rh.dateTextView.Text = Convert.ToString(listOfReceipt[position].Date.ToString("dd/MM/yyyy"));
                if (selectedPosition == position)
                {
                    rh.ItemView.SetBackgroundColor(Color.Yellow);
                }
                else rh.ItemView.SetBackgroundColor(Color.White);
                rh.ItemView.Click += (sender, e) =>
                {
                    selectedPosition = position;
                    NotifyDataSetChanged();
                };
            }         

        }

      

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

           View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.receipt_items, parent, false);
            
            // Create a ViewHolder to hold view references inside the CardView:
            ReceiptViewHolder rh = new ReceiptViewHolder(itemView, OnClick);
            return rh;
        }
     

    }
}