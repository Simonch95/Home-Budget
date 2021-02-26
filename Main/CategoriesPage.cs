using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using CapturePicture.DataBase;
using Com.Akaita.Android.Circularseekbar;
using Google.Type;
using DateTime = System.DateTime;

namespace CapturePicture
{
    [Activity(Label = "Kategorie")]
    public class CategoriesPage : AppCompatActivity
    {
        public ImageButton foodButton, carButton, cosmeticButton, transportButton, houseButton, entertainmentButton,
                    giftsButton, clothesButton, pharmacyButton, anotherButton, nextMonthButton, previousMonthButton;
        public CircularSeekBar progressBar;
        public Button saveButton, cancelButton, dateButton;
        public TextView foodLbl, carLbl, cosmeticLbl, transportLbl, houseLbl, entertainmentLbl,
                    giftsLbl, clothesLbl, pharmacyLbl, anotherLbl, nextMonthLbl, previousMonthLbl;
        public EditText accountTextEditText;
        public DateTime CurrentDate;
        public string[] months = { "styczeń","luty","marzec","kwiecień","maj","czerwiec","lipiec", "sierpień","wrzesień", "październik", "listopad","grudzień" };
        public List<TextView> listOfLabelMoney = new List<TextView>();
        public List<string> listofCategoryName = new List<string>();
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.categories_page);
            
            InicializeGUI();
            AddListCategoryName();
             string dateFromCalendar = Intent.GetStringExtra("date");
            CurrentDate = Convert.ToDateTime(dateFromCalendar);
            int idmonth = CurrentDate.Month;
            dateButton.Text = months[idmonth - 1];
            PrintSpendedMoney();
            
            foodButton.Click += AddNewReceipt_OnClick;
            carButton.Click += AddNewReceipt_OnClick;
            cosmeticButton.Click += AddNewReceipt_OnClick;
            transportButton.Click += AddNewReceipt_OnClick;
            houseButton.Click += AddNewReceipt_OnClick;
            entertainmentButton.Click += AddNewReceipt_OnClick;
            giftsButton.Click += AddNewReceipt_OnClick;
            clothesButton.Click += AddNewReceipt_OnClick;
            pharmacyButton.Click += AddNewReceipt_OnClick;
            anotherButton.Click += AddNewReceipt_OnClick;
            previousMonthButton.Click += PreviousMonth_OnClick;
            nextMonthButton.Click += NextMonth_OnClick;
           


        }

        private void PrintSpendedMoney()
        {
            double total = 0;
            for (int i = 0; i < listOfLabelMoney.Count; i++)
            {
                int id = DatabaseController.SelectIdCategory(listofCategoryName[i]); 
                double spendedMoney = DatabaseController.SelectSpendedMoneyOfCategory(CurrentDate.Year, CurrentDate.Month, id);
                total += spendedMoney;
                listOfLabelMoney[i].Text = Convert.ToString(spendedMoney);
            }
            progressBar.ProgressText = Convert.ToString(total);
        }
        public void AddListCategoryName()
        {
            listofCategoryName.Add("Food");
            listofCategoryName.Add("Car");
            listofCategoryName.Add("Cosmetic");
            listofCategoryName.Add("Transport");
            listofCategoryName.Add("House");
            listofCategoryName.Add("Entertainment");
            listofCategoryName.Add("Clothes");
            listofCategoryName.Add("Pharmacy");
            listofCategoryName.Add("Gifts");
            listofCategoryName.Add("Another");
        }

        public void InicializeGUI()
        {
            progressBar = FindViewById<CircularSeekBar>(Resource.Id.ProgressBar);
            foodButton = FindViewById<ImageButton>(Resource.Id.FoodBtn);
            carButton = FindViewById<ImageButton>(Resource.Id.CarBtn);
            cosmeticButton = FindViewById<ImageButton>(Resource.Id.CosmeticsBtn);
            transportButton = FindViewById<ImageButton>(Resource.Id.TransportBtn);
            houseButton = FindViewById<ImageButton>(Resource.Id.HouseBtn);
            entertainmentButton = FindViewById<ImageButton>(Resource.Id.EntertainmentBtn);
            giftsButton = FindViewById<ImageButton>(Resource.Id.GiftsBtn);
            clothesButton = FindViewById<ImageButton>(Resource.Id.ClothesBtn);
            pharmacyButton = FindViewById<ImageButton>(Resource.Id.PharmacyBtn);
            anotherButton = FindViewById<ImageButton>(Resource.Id.AnotherBtn);
            nextMonthButton = FindViewById<ImageButton>(Resource.Id.NextMonthBtn);
            previousMonthButton = FindViewById<ImageButton>(Resource.Id.PreviousMonthBtn);
            dateButton = FindViewById<Button>(Resource.Id.DateBtn);
            foodLbl = FindViewById<TextView>(Resource.Id.FoodMoneyLbl);
            carLbl = FindViewById<TextView>(Resource.Id.CarMoneyLbl);
            cosmeticLbl = FindViewById<TextView>(Resource.Id.CosmeticMoneyLbl);
            transportLbl = FindViewById<TextView>(Resource.Id.TransportMoneyLbl);
            houseLbl = FindViewById<TextView>(Resource.Id.HouseMoneyLbl);
            entertainmentLbl = FindViewById<TextView>(Resource.Id.EntertainmentMoneyLbl);
            giftsLbl = FindViewById<TextView>(Resource.Id.GiftsMoneyLbl);
            clothesLbl = FindViewById<TextView>(Resource.Id.ClothesMoneyLbl);
            pharmacyLbl = FindViewById<TextView>(Resource.Id.PharmacyMoneyLbl);
            anotherLbl = FindViewById<TextView>(Resource.Id.AnotherMoneyLbl);
            nextMonthLbl = FindViewById<TextView>(Resource.Id.FoodMoneyLbl);
            previousMonthLbl = FindViewById<TextView>(Resource.Id.FoodMoneyLbl);
            listOfLabelMoney.Add(foodLbl);
            listOfLabelMoney.Add(carLbl); 
            listOfLabelMoney.Add(cosmeticLbl);
            listOfLabelMoney.Add(transportLbl);           
            listOfLabelMoney.Add(houseLbl);
            listOfLabelMoney.Add(entertainmentLbl);
            listOfLabelMoney.Add(clothesLbl);          
            listOfLabelMoney.Add(pharmacyLbl);
            listOfLabelMoney.Add(giftsLbl);
            listOfLabelMoney.Add(anotherLbl);
           
        }
        public void SetSpendedMoney()
        {
            double money = DatabaseController.SelectSpendedMoney(CurrentDate.Year, CurrentDate.Month);
            progressBar.ProgressText = Convert.ToString(money);            
        }

        private void AddNewReceipt_OnClick(object sender, EventArgs e)
        {
           ImageButton obj = (ImageButton)sender;
            var id = obj.TooltipText;
            switch (obj.TooltipText)
            {
                case "Food":
                    {
                        Toast.MakeText(this, "Spożywcze", ToastLength.Long).Show();
                        SendCategory(obj.TooltipText);
                        break;
                    }
                case "Car":
                    {
                        Toast.MakeText(this, "Samochód", ToastLength.Long).Show();
                        SendCategory(obj.TooltipText);
                        break;
                    }
                case "Cosmetic":
                    {
                        Toast.MakeText(this, "Kosmetyki", ToastLength.Long).Show();
                        SendCategory(obj.TooltipText);
                        break;
                    }
                case "Transport":
                    {
                        Toast.MakeText(this, "Transport", ToastLength.Long).Show();
                        SendCategory(obj.TooltipText);
                        break;
                    }
                case "House":
                    {
                        Toast.MakeText(this, "Dom", ToastLength.Long).Show();
                        SendCategory(obj.TooltipText);
                        break;
                    }
                case "Entertainment":
                    {
                        Toast.MakeText(this, "Rozrywka", ToastLength.Long).Show();
                        SendCategory(obj.TooltipText);
                        break;
                    }
                case "Clothes":
                    {
                        Toast.MakeText(this, "Odzież", ToastLength.Long).Show();
                        SendCategory(obj.TooltipText);
                        break;
                    }
                case "Pharmacy":
                    {
                        Toast.MakeText(this, "Apteka", ToastLength.Long).Show();
                        SendCategory(obj.TooltipText);
                        break;
                    }
                case "Gifts":
                    {
                        Toast.MakeText(this,"Prezenty", ToastLength.Long).Show();
                        SendCategory(obj.TooltipText);
                        break;
                    }
                case "Another":
                    {
                        Toast.MakeText(this, "Inne", ToastLength.Long).Show();
                        SendCategory(obj.TooltipText);
                        break;
                    }
            }

        }

        private void SendCategory(string categoryName)
        {
            Intent intent = new Intent(this, typeof(ReceiptPage));
            intent.PutExtra("Category", categoryName);
            this.StartActivity(intent);
        }
        private void NextMonth_OnClick(object sender, EventArgs e)
        {

            CurrentDate = CurrentDate.AddMonths(1);
            SetDay(CurrentDate); 
        }
        private void PreviousMonth_OnClick(object sender, EventArgs e)
        {

            CurrentDate = CurrentDate.AddMonths(-1);
            SetDay(CurrentDate);           
        }
        private void SetDay(DateTime currentDate)
        {
            int idmonth = currentDate.Month;
            dateButton.Text = months[idmonth - 1];
            PrintSpendedMoney();

        }
    }
}