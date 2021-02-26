using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using CapturePicture.DataBase;
using CapturePicture.Model;
using Google.Type;
using DateTime = System.DateTime;

namespace CapturePicture
{
    [Activity(Label = "Limit na obecny miesiąc")]
    public class ChangeAccountStatus : AppCompatActivity
    {

      
        public Button saveButton, cancelButton;
        public EditText accountTextEditText;
        public DateTime date;
     
        protected override void OnCreate(Bundle savedInstanceState)
        {
           
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.change_account_status);

            saveButton = FindViewById<Button>(Resource.Id.SaveBtn);
            cancelButton = FindViewById<Button>(Resource.Id.CancelBtn);
            accountTextEditText = FindViewById<EditText>(Resource.Id.AccountText);
            saveButton.Click += Save_OnClick;
            cancelButton.Click += Cancel_OnClick;
            string dateFromCalendar = Intent.GetStringExtra("date");
            date = Convert.ToDateTime(dateFromCalendar);
            accountTextEditText.Text = Intent.GetStringExtra("money");
            if(accountTextEditText.Text.Equals("0"))
            {
                accountTextEditText.Text = "";
            }
           
         
        }

        private void Cancel_OnClick(object sender, EventArgs e)
        {
            Finish();
        }

        private void Save_OnClick(object sender, EventArgs e)
        {
            
           
            if (CheckLimit(accountTextEditText.Text))
            {
                double moneyLimit = Convert.ToDouble(accountTextEditText.Text);
                CategoryLimit categoryLimit = new CategoryLimit();
                categoryLimit.Id_Category = 1;
                categoryLimit.AmountLimit = moneyLimit;
                categoryLimit.Active = true;
                categoryLimit.Date = date;
                int id = DatabaseController.SelectIdCategoryLimit(categoryLimit);
      
                if (id >0)
                {
                    categoryLimit.Id_Limit = id;
                    if( DatabaseController.UpdateLimitOfCategory(categoryLimit))
                    {
                        TurnOffAcitivicyLimit(date);
                        Intent intent = new Intent(this, typeof(MainMenu));
                        intent.PutExtra("date", date.ToString("dd/MM/yyyy"));
                        intent.PutExtra("money", accountTextEditText.Text);
                        Toast.MakeText(this, "Budżet na ten miesiąc został zapisany", ToastLength.Long).Show();
                        this.StartActivity(intent);

                    }
                    else Toast.MakeText(this, "Nieoczekiwany błąd", ToastLength.Long).Show();
                }
                else if (DatabaseController.InsertCategoryLimit(categoryLimit))
                {
          
                    Intent intent = new Intent(this, typeof(MainMenu));
                    intent.PutExtra("date", date.ToString("dd/MM/yyyy"));
                    intent.PutExtra("money", accountTextEditText.Text);
                    Toast.MakeText(this, "Budżet na ten miesiąc został zapisany", ToastLength.Long).Show();
                    this.StartActivity(intent);


                }
               
            }
            //Intent intent = new Intent(this, typeof(BudgetOfCategoriesPage));
            //intent.PutExtra("date", dateButton.Text);
            //
          
           
           // StartActivity(typeof(MainMenu)); // na próbę
            
        }

        private void TurnOffAcitivicyLimit(DateTime date)
        {
            List<CategoryLimit> categoryLimit = DatabaseController.GetLimitBudget(date);
            foreach (var item in categoryLimit)
            {
                if(item.Id_Category!=1)
                {
                    item.Active = false;
                    DatabaseController.UpdateLimitOfCategory(item);

                }
            }
        }

        public bool CheckLimit(string accountTextEditText)
        {
            string price = "";
            foreach (var item in accountTextEditText)
            {
                if (char.IsDigit(item))
                    price += Convert.ToString(item);
                else
                {
                    Toast.MakeText(this, "Nie właściwe dane lub format", ToastLength.Long).Show();
                    return false;
                }
            }

            return true;
        }
    }
}