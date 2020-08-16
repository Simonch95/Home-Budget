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
using Com.Akaita.Android.Circularseekbar;

namespace CapturePicture
{
    [Activity(Label = "MainMenu", MainLauncher = true)]
    public class MainMenu : Activity, DatePickerDialog.IOnDateSetListener
    {
        public Button addButton;
        public CircularSeekBar progressBar;
        public Button date;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main_menu);
            // Create your application here
            addButton = FindViewById<Button>(Resource.Id.AddReceiptBtn);
            addButton.Click += AddButton_Click;
            date = FindViewById<Button>(Resource.Id.Date);
           date.Click += DateSelect_OnClick;
            progressBar = FindViewById<CircularSeekBar>(Resource.Id.ProgressBar);
            progressBar.ProgressText = "70";
        }

        private void DateSelect_OnClick(object sender, EventArgs e)
        {
            int dateId = 1;
            ShowDialog(dateId);
        }

        protected override Dialog OnCreateDialog(int id)
        {
            if (id == 1)
            {
                DateTime currently = DateTime.Now;
                return new DatePickerDialog(this, this, currently.Year, currently.Month-1, currently.Day);
            }
               
            return null;
        }
      

        private void AddButton_Click(object sender, EventArgs e)
        {
           StartActivity(typeof(AddReceipt));
        }

        private int year, month, day;
        public void OnDateSet(DatePicker view, int Year, int Month, int dayOfMonth)
        {
            var currentlyDate = new DateTime(Year, Month, dayOfMonth);

            date.Text = currentlyDate.ToString("MM/dd/yyyy");
            // Toast.MakeText(this, "dzisiaj mamyv "+(month)+""+"",ToastLength.Long).Show();
        }
    }
}