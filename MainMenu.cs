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

namespace CapturePicture
{
    [Activity(Label = "MainMenu", MainLauncher = true)]
    public class MainMenu : Activity
    {
        public Button addButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main_menu);
            // Create your application here
            addButton = FindViewById<Button>(Resource.Id.AddReceiptBtn);
            addButton.Click += AddButton_Click;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
           StartActivity(typeof(AddReceipt));
        }
    }
}