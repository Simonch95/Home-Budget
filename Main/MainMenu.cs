using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Database.Sqlite;
using Android.Icu.Text;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using CapturePicture.DataBase;
using CapturePicture.Main;
using CapturePicture.Model;
using Com.Akaita.Android.Circularseekbar;
using Environment = System.Environment;

namespace CapturePicture
{
    [Activity(Label = "Budżet domowy", MainLauncher = true )] 
    public class MainMenu : Activity, DatePickerDialog.IOnDateSetListener
    {
        public Button addButton, dateButton, checkHistoryButton, setBudgetButton, chartButton ;
        public ImageButton nextDayButton, previousDayButton;
        public CircularSeekBar progressBar;
        public TextView  budgetStatusTxtV, spendedStatusTxtV, leftStatusTxtV;
        public string money,date;
        public DateTime CurrentDay = DateTime.Now;
     
        public int check =0;
        public List<Category> listofCategory = new List<Category>();
        public List<CategoryLimit> listOfCategoryLimit = new List<CategoryLimit>();
       
       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main_menu);
            DatabaseController.CreateDatebase();
            string dateFromCalendar = Intent.GetStringExtra("date");
            DateTime date = Convert.ToDateTime(dateFromCalendar);
            if(date.Year!=1)
                CurrentDay= Convert.ToDateTime(dateFromCalendar); 
            InicializeGUI();
            InicializeCategoryTable();
           

            LoadData();

        }
   
        private void CheckLimit()
        {
            if (!DatabaseController.ExistLimit(CurrentDay, new CategoryLimit(1, 0, true, CurrentDay)))
            {               
                DatabaseController.InsertCategoryLimit(new CategoryLimit( 1, 0, true, CurrentDay));
                SetRestBudget(0, 0);             
            }            
        }
        private void InicializeCategoryTable()
        {
            
           
            if (DatabaseController.IsEmptyTable())
            {
                List<Category> listofCategory = new List<Category>();
                listofCategory.Add(new Category("OverallBudget"));
                listofCategory.Add(new Category("Food"));
                listofCategory.Add(new Category("Car"));
                listofCategory.Add(new Category("Cosmetic"));
                listofCategory.Add(new Category("Transport"));
                listofCategory.Add(new Category("House"));
                listofCategory.Add(new Category("Entertainment"));
                listofCategory.Add(new Category("Gifts"));
                listofCategory.Add(new Category("Clothes"));
                listofCategory.Add(new Category("Pharmacy"));               
                listofCategory.Add(new Category("Another"));
                foreach (var category in listofCategory)
                {
                    DatabaseController.InsertCategory(category);
                }
            }
          
        }

        private void InicializeGUI()
        {
            addButton = FindViewById<Button>(Resource.Id.AddReceiptBtn);
       
            previousDayButton = FindViewById<ImageButton>(Resource.Id.PreviousDayBtn);
            nextDayButton = FindViewById<ImageButton>(Resource.Id.NextDayBtn);
            dateButton = FindViewById<Button>(Resource.Id.Date);
               
            checkHistoryButton = FindViewById<Button>(Resource.Id.CheckHistoryBudgetBtn);
            setBudgetButton = FindViewById<Button>(Resource.Id.SetBudgetBtn);

            budgetStatusTxtV = FindViewById<TextView>(Resource.Id.BudgetStatus);
           spendedStatusTxtV = FindViewById<TextView>(Resource.Id.BudgetSpendedStatus);
           leftStatusTxtV = FindViewById<TextView>(Resource.Id.BudgetLeftStatus);

            chartButton = FindViewById<Button>(Resource.Id.ChartsBtn);
            chartButton.Click += ShowChart;
            dateButton.Click += DateSelect_OnClick;
            addButton.Click += AddReceipt_Click;
            nextDayButton.Click += NextDay_OnClick;
            previousDayButton.Click += PreviousDay_OnClick;
            setBudgetButton.Click += OpenSettings_OnClick;
            checkHistoryButton.Click += OpenReviewOfReceipts_OnClick;
          
        }

        private void ShowChart(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Charts));
            intent.PutExtra("date", dateButton.Text);
            this.StartActivity(intent);
        }

        private void OpenReviewOfReceipts_OnClick(object sender, EventArgs e)
        {
            StartActivity(typeof(ReviewOfReceipts));
        }

        private void LoadData() 
        {
            CheckLimit();

            double overallBudget = 0;
           
            double spendedmoney = 0;
            listOfCategoryLimit = DatabaseController.GetLimitBudget(CurrentDay); 
            spendedmoney = DatabaseController.SelectSpendedMoney(CurrentDay.Year, CurrentDay.Month);
            if (listOfCategoryLimit.Count>=1)
            {                
                int i = 0;
                if(listOfCategoryLimit[i].Active)
                {                    
                    if(listOfCategoryLimit[i].AmountLimit==0)                    
                        ShowDialog(overallBudget,spendedmoney);
                           
                    overallBudget = DatabaseController.SelectAmountLimit(CurrentDay.Year, CurrentDay.Month, listOfCategoryLimit[i].Id_Category);
                    SetRestBudget(overallBudget, spendedmoney);
                }
                else
                {
                    
                    foreach (var limit in listOfCategoryLimit)
                    {
                       if(limit.Active)
                         overallBudget += DatabaseController.SelectAmountLimit(CurrentDay.Year, CurrentDay.Month, limit.Id_Category);                                  
                    }
                    SetRestBudget(overallBudget, spendedmoney);
                }
            }            
            else       
                ShowDialog(overallBudget,spendedmoney);            
           
            SetDay(CurrentDay);        
        }

        private void ShowDialog(double overallBudget,double spenedmoney)
        {
            AlertDialog.Builder alertDiag = new AlertDialog.Builder(this);
            alertDiag.SetTitle("Budżet");
            alertDiag.SetMessage("Czy ustawić budżet na nowy miesiąc?");
            alertDiag.SetPositiveButton("TAK", (senderAlert, args) =>
            {
                OpenChangeAccountStatus();
                alertDiag.Dispose();


            });
            alertDiag.SetNegativeButton("NIE", (senderAlert, args) =>
            {
                SetRestBudget(0, 0);
                alertDiag.Dispose();
            });
            Dialog diag = alertDiag.Create();
          
            diag.Show();
                      
        }

        private void SetRestBudget(double overallBudget, double spendedmoney)
        {
            money = Convert.ToString(overallBudget);
            budgetStatusTxtV.Text = money + "  PLN";
            spendedStatusTxtV.Text = Convert.ToString(spendedmoney) + "  PLN";
            leftStatusTxtV.Text = Convert.ToString(overallBudget - spendedmoney) + "  PLN";
        }

        private void OpenSettings_OnClick(object sender, EventArgs e)
        {
            AlertDialog.Builder alertDiag = new AlertDialog.Builder(this);
            alertDiag.SetTitle("Budżet na ten miesiąc");
            alertDiag.SetMessage("Czy chcesz ustawić budżet według kategorii?");
            alertDiag.SetPositiveButton("Tak", (senderAlert, args) =>
            {
                OpenBudgetOfCategories();
                //Toast.MakeText(this, "Deleted", ToastLength.Short).Show();
            });
            alertDiag.SetNegativeButton("Nie", (senderAlert, args) => {
                OpenChangeAccountStatus();
                //Głownu budżets
                //Toast.MakeText(this, "Deleted", ToastLength.Short).Show();
            });
            alertDiag.SetNeutralButton("Cancel", (senderAlert, args) => {
                alertDiag.Dispose();
            });
            Dialog diag = alertDiag.Create();
            diag.Show();
        }

        private void OpenChangeAccountStatus()
        {
            Log.Info("ustaw budżet ogólny", CurrentDay.ToString("dd/MM/yyyy"));
            Intent intent = new Intent(this, typeof(ChangeAccountStatus));
         
            intent.PutExtra("money", money);
            intent.PutExtra("date", dateButton.Text);
            this.StartActivity(intent);
          
        }
        
        private void OpenBudgetOfCategories()
        {
            Intent intent = new Intent(this, typeof(BudgetOfCategoriesPage));        
            intent.PutExtra("date", dateButton.Text);
            this.StartActivity(intent);
          
         
        }

//      
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
      

        private void AddReceipt_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(CategoriesPage));       
            intent.PutExtra("date", dateButton.Text);
            this.StartActivity(intent);
         
        }

      
        public void OnDateSet(DatePicker view, int Year, int Month, int dayOfMonth)
        {
           
            CurrentDay = new DateTime(Year, Month + 1, dayOfMonth);
            Log.Info("Data z kalendarza", CurrentDay.ToString("dd/MM/yyyy"));
            SetDay(CurrentDay);
            LoadData();

          
        }
        private void NextDay_OnClick(object sender, EventArgs e)
        {
           
            CurrentDay = CurrentDay.AddDays(1);
            SetDay(CurrentDay);
            LoadData();
           

          
        }
        private void PreviousDay_OnClick(object sender, EventArgs e)
        {
           
            CurrentDay = CurrentDay.AddDays(-1);
            SetDay(CurrentDay);
            LoadData();      
            
            

        
        }
        private void SetDay(DateTime currentDate)
        {

            dateButton.Text = CurrentDay.ToString("dd/MM/yyyy");
        }
      
    }
}