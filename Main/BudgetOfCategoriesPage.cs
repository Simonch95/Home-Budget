using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using CapturePicture.DataBase;
using CapturePicture.Model;

namespace CapturePicture
{
    [Activity(Label = "BudgetOfCategoriesPage")]


    public class BudgetOfCategoriesPage : Activity
    {
        public EditText foodET, carET, cosmeticET, transportET, houseET, entertainmentET, giftsET,
            clothesET, pharmacyET, anotherET;
        public TextView foodInfo, carInfo, cosmeticInfo, transportInfo, houseInfo, entertainmentInfo, giftsInfo,
            clothesInfo, pharmacyInfo, anotherInfo;
        public Button saveBtn;
        private InputMethodManager imm;
        private TextView totalTxT;
        public List<EditText> listofCategoryEditText = new List<EditText>();
        public List<string> listofCategoryName = new List<string>();
        public DateTime date;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.budget_of_categories);
            InitialEditText();
            InitialTexView();
            AddToListButton();
            AddToListCategoryName();

            totalTxT = FindViewById<TextView>(Resource.Id.TotalText);
            saveBtn = FindViewById<Button>(Resource.Id.SaveButton);
            string dateFromCalendar = Intent.GetStringExtra("date");
            date = Convert.ToDateTime(dateFromCalendar);
            //   foodET.KeyPress += GetMoney;
            saveBtn.Click += GetMoney;
            PrintLimit();
        }

        private void PrintLimit()
        {
           var listOfLimit= DatabaseController.GetLimitBudget(date);
            
            if (listOfLimit.Count > 1)
            {
                double total = 0;
                for (int i =1; i < listOfLimit.Count; i++)
                {
                    for (int k = 0; k <= listofCategoryEditText.Count+1; k++)
                    {
                        if (k == listOfLimit[i].Id_Category)
                        {
                            listofCategoryEditText[k - 2].Text = Convert.ToString(listOfLimit[i].AmountLimit);
                            total += listOfLimit[i].AmountLimit;
                            break;
                        }
                    }
                }
                SetTotalBudget(total);
            }          
        }

        private void SetTotalBudget(double total)
        {
            totalTxT.Text = Convert.ToString(total);
        }

        public void AddToListCategoryName()
        {
            listofCategoryName.Clear();
            listofCategoryName.Add("OverallBudget");
            listofCategoryName.Add("Food");
            listofCategoryName.Add("Car");
            listofCategoryName.Add("Cosmetic");
            listofCategoryName.Add("Transport");
            listofCategoryName.Add("House");
            listofCategoryName.Add("Entertainment");
            listofCategoryName.Add("Gifts");
            listofCategoryName.Add("Clothes");
            listofCategoryName.Add("Pharmacy");           
            listofCategoryName.Add("Another");
        }

        private void AddToListButton()
        {
            listofCategoryName.Clear();
            listofCategoryEditText.Add(foodET);            
            listofCategoryEditText.Add(carET);            
            listofCategoryEditText.Add(cosmeticET);           
            listofCategoryEditText.Add(transportET);           
            listofCategoryEditText.Add(houseET);            
            listofCategoryEditText.Add(entertainmentET);
            listofCategoryEditText.Add(giftsET);
            listofCategoryEditText.Add(clothesET);           
            listofCategoryEditText.Add(pharmacyET);                
        
           listofCategoryEditText.Add(anotherET);
        }

        private void InitialTexView()
        {
            foodInfo = FindViewById<TextView>(Resource.Id.FoodInfo);
            carInfo = FindViewById<TextView>(Resource.Id.CarInfo);
            cosmeticInfo = FindViewById<TextView>(Resource.Id.CosmeticInfo);
            transportInfo = FindViewById<TextView>(Resource.Id.TransportInfo);
            houseInfo = FindViewById<TextView>(Resource.Id.HouseInfo);
            entertainmentInfo = FindViewById<TextView>(Resource.Id.EntertainmentInfo);
            giftsInfo = FindViewById<TextView>(Resource.Id.GiftsInfo);
            clothesInfo = FindViewById<TextView>(Resource.Id.ClothesInfo);
            pharmacyInfo = FindViewById<TextView>(Resource.Id.PharmacyInfo);
           anotherInfo = FindViewById<TextView>(Resource.Id.AnotherInfo);
        }

        private void InitialEditText()
        {
            foodET = FindViewById<EditText>(Resource.Id.FoodEText);
            carET = FindViewById<EditText>(Resource.Id.CarEText);
            cosmeticET = FindViewById<EditText>(Resource.Id.CosmeticEText);
            transportET = FindViewById<EditText>(Resource.Id.TransportEText);
            houseET = FindViewById<EditText>(Resource.Id.HouseEText);
            entertainmentET = FindViewById<EditText>(Resource.Id.EntertainmentEText);
            giftsET = FindViewById<EditText>(Resource.Id.GiftsEText);
            clothesET = FindViewById<EditText>(Resource.Id.ClothesEText);
            pharmacyET = FindViewById<EditText>(Resource.Id.PharamcyEText);
            anotherET = FindViewById<EditText>(Resource.Id.AnotherEText);
        }

        private void GetMoney(object sender, System.EventArgs e)
        {
            HideKeyboard();
            List<CategoryLimit> listOfLimit = new List<CategoryLimit>();
            int i = 1, id = 0;
            double moneyLimit = 0;          

            foreach (EditText item in listofCategoryEditText)
            {
               
                if (IsDigitsOnly(item.Text))
                {
                    id= DatabaseController.SelectIdCategory(listofCategoryName[i]);
                    moneyLimit = Convert.ToDouble(item.Text);
                    listOfLimit.Add(new CategoryLimit(id, moneyLimit, true, date));
                   
                }
               
                ++i;
            }
            UpdateLimit(date, listOfLimit);
        
            StartActivity(typeof(MainMenu));
          
        }

        private void UpdateLimit(DateTime date,List<CategoryLimit> listOfLimit)
        {

            bool id = false;
            foreach (var categoryLimit in listOfLimit)
            {
                id = DatabaseController.ExistLimit(date, categoryLimit);
                if (id )
                {
                    categoryLimit.Id_Limit = DatabaseController.SelectIdCategoryLimit(categoryLimit);
                    if (DatabaseController.UpdateLimitOfCategory(categoryLimit))
                    {
                         double overBudgetLimit = DatabaseController.SelectAmountLimit(date.Year, date.Month, 1);
                        DatabaseController.UpdateLimitOfCategory(new CategoryLimit(1, overBudgetLimit, false, date));
                        Toast.MakeText(this, "Budżet na ten miesiąc został zapisany", ToastLength.Long).Show();
                       
                    }
                    else Toast.MakeText(this, "Nieoczekiwany błąd", ToastLength.Long).Show();
                }
                else if (DatabaseController.InsertCategoryLimit(categoryLimit))  
                {
                  
                    double overBudgetLimit = DatabaseController.SelectAmountLimit(date.Year, date.Month, 1);
                    DatabaseController.UpdateLimitOfCategory(new CategoryLimit(1, overBudgetLimit, false, date));             
                    Toast.MakeText(this, "Budżet na ten miesiąc został zapisany", ToastLength.Long).Show();
                  

                }
            }
           
        }

        public static bool IsDigitsOnly(string str)
        {
           
            foreach (char item in str)
            {
                string value="";
                if (char.IsDigit(item) || item.Equals('.') || item.Equals(','))
                    value += Convert.ToString(item);

            }
                return !string.IsNullOrEmpty(str) && str.All(char.IsDigit);
           

        }

      
        private void HideKeyboard()
        {
          
            View view = this.CurrentFocus;
            if(view!=null)
            {
                imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(view.WindowToken, 0);
            }
        }
      
    }
}