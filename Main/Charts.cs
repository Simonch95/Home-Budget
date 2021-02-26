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
using Microcharts.Droid;
using Microcharts;
using System.Collections.Generic;
using Microcharts;
using SkiaSharp;
using Android.Views;
using CapturePicture.DataBase;

namespace CapturePicture.Main
{
    [Activity(Label = "Charts")]
    public class Charts : Activity
    {
        DateTime CurrentDate;
        ChartView chartview;
        public Button dateButton;
        public ImageButton nextMonthButton, previousMonthButton;
        //   TextView chartoptionsText;
        public List<string> listofCategoryName = new List<string>();
        public List<double> listOfSpendedMoney = new List<double>();
        public string[] months = { "styczeń", "luty", "marzec", "kwiecień", "maj", "czerwiec", "lipiec", "sierpień", "wrzesień", "październik", "listopad", "grudzień" };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.charts);
            chartview = (ChartView)FindViewById(Resource.Id.chartView);
            nextMonthButton = FindViewById<ImageButton>(Resource.Id.NextMonthButton);
            previousMonthButton = FindViewById<ImageButton>(Resource.Id.PreviousMonthButton);
            dateButton = FindViewById<Button>(Resource.Id.MonthButton);
            nextMonthButton.Click += NextMonth_OnClick;
            previousMonthButton.Click += PreviousMonth_OnClick;

            string dateFromCalendar = Intent.GetStringExtra("date");
            CurrentDate = Convert.ToDateTime(dateFromCalendar);
            int idmonth = CurrentDate.Month;
            dateButton.Text = months[idmonth - 1];
            AddListCategoryName();
            GetSpendedMoney();
          

            DrawChart("DonutChart");


        }
        public void GetSpendedMoney()
        {
            int id = 0;
            listOfSpendedMoney.Clear();
            foreach (var categoryName in listofCategoryName)
            {
                id = DatabaseController.SelectIdCategory(categoryName);
                listOfSpendedMoney.Add(DatabaseController.SelectSpendedMoneyOfCategory(CurrentDate.Year,CurrentDate.Month, id));
            }
            DrawChart("DonutChart");
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

      public  void DrawChart(string charttype)
        {

            List<Entry> DataList = new List<Entry>();
            DataList.Add(new Entry((float)listOfSpendedMoney[2])
            {
                Label = "Kosmetyki",
                ValueLabel = Convert.ToString(listOfSpendedMoney[2]),
                Color = SKColor.Parse("#10e7eb")
            });
            DataList.Add(new Entry((float)listOfSpendedMoney[3])
            {
                Label = "Transport",
                ValueLabel = Convert.ToString(listOfSpendedMoney[3]),
                Color = SKColor.Parse("#1068eb")
            });

           
          
           
           
        

            DataList.Add(new Entry((float)listOfSpendedMoney[4])
            {
                Label = "Dom",
                ValueLabel = Convert.ToString(listOfSpendedMoney[4]),
                Color = SKColor.Parse("#eb0e33")
            });
          

          
            DataList.Add(new Entry((float)listOfSpendedMoney[7])
            {
                Label = "Ubrania",
                ValueLabel = Convert.ToString(listOfSpendedMoney[7]),
                Color = SKColor.Parse("#cc00cc")
            });
            DataList.Add(new Entry((float)listOfSpendedMoney[8])
            {
                Label = "Apteka",
                ValueLabel = Convert.ToString(listOfSpendedMoney[8]),
                Color = SKColor.Parse("#0e0e0e")
            });
            DataList.Add(new Entry((float)listOfSpendedMoney[5])
            {
                Label = "Rozrywka",
                ValueLabel = Convert.ToString(listOfSpendedMoney[5]),
                Color = SKColor.Parse("#e3d320")
            });
            DataList.Add(new Entry((float)listOfSpendedMoney[1])
            {
                Label = "Samochód",
                ValueLabel = Convert.ToString(listOfSpendedMoney[1]),
                Color = SKColor.Parse("#0ccf40")
            });
            DataList.Add(new Entry((float)listOfSpendedMoney[9])
            {
                Label = "Inne",
                ValueLabel = Convert.ToString(listOfSpendedMoney[9]),
                Color = SKColor.Parse("#ff9900")
            });
           
            DataList.Add(new Entry((float)listOfSpendedMoney[6])
            {
                Label = "Prezenty",
                ValueLabel = Convert.ToString(listOfSpendedMoney[6]),
                Color = SKColor.Parse("#c2c2c2")
            });
            DataList.Add(new Entry((float)listOfSpendedMoney[0])
            {
                Label = "Spożywcze",
                ValueLabel = Convert.ToString(listOfSpendedMoney[0]),
                Color = SKColor.Parse("#266489")
            });



            if (charttype == "DonutChart")
            {
                var chart = new DonutChart() { Entries = DataList, LabelTextSize = 28f };
                chartview.Chart = chart;
            }
           
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
            GetSpendedMoney();

        }

    }
}