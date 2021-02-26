using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using CapturePicture.DataBase;
using CapturePicture.Model;
using static Android.InputMethodServices.Keyboard;

namespace CapturePicture
{

    [Activity(Label = "ReviewOfReceipts")]
    public class ReviewOfReceipts : Activity, DatePickerDialog.IOnDateSetListener
    {
        
        public Button date1Button, date2Button,removeButton,backButton;
    
        public RecyclerView recyclerView;
      
        public RecyclerView.LayoutManager rLayoutManager;
        public RecyclerView.ViewHolder holder;
        public ReceiptAdapter rAdapter;
        public DateTime DateFrom= new DateTime(2000,1,1) , DateTo = DateTime.Now;
        public int dateId=0;

      
       
        public int selectedItem = -1;

        public List<Receipt> listOfReceipt = new List<Receipt>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.review_of_receipts);
           
            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            removeButton = FindViewById<Button>(Resource.Id.RemoveReceiptBtn);
            backButton = FindViewById<Button>(Resource.Id.BackBtn);

            date1Button = FindViewById<Button>(Resource.Id.Calendar1Btn);
            date2Button = FindViewById<Button>(Resource.Id.Calendar2Btn);
            //Receipt receipt3 = new Receipt(4, 13.4, new DateTime(2021, 01, 02));
            //Receipt receipt4 = new Receipt(5, 23.15, new DateTime(2021, 01, 02));
            //Receipt receipt5 = new Receipt(6, 35, new DateTime(2021, 01, 05));
            //Receipt receipt6 = new Receipt(2, 40, new DateTime(2021, 01, 08));
            //Receipt receipt = new Receipt(3, 50, new DateTime(2021, 01, 12));
            //Receipt receipt2 = new Receipt(5, 19.99, new DateTime(2021, 01, 13));
            //Receipt receipt7 = new Receipt(8, 24.45, new DateTime(2021, 01, 13));
            //Receipt receipt8 = new Receipt(9, 90.83, new DateTime(2021, 01, 14));
            //Receipt receipt9 = new Receipt(11, 74.35, new DateTime(2021, 01, 15));
            //Receipt receipt10 = new Receipt(10, 13.5, new DateTime(2021, 01, 18));
            //Receipt receipt11 = new Receipt(8, 50.25, new DateTime(2021, 01, 20));
            //Receipt receipt12 = new Receipt(4, 58.76, new DateTime(2021, 01, 21));
            //Receipt receipt13 = new Receipt(5, 43.52, new DateTime(2021, 01, 22));
            //Receipt receipt14 = new Receipt(8, 60.74, new DateTime(2021, 01, 23));
            //Receipt receipt15 = new Receipt(2, 34.32, new DateTime(2021, 01, 25));
            //Receipt receipt16 = new Receipt(3, 23.15, new DateTime(2021, 01, 25));
            //Receipt receipt17 = new Receipt(8, 25.5, new DateTime(2021, 01, 28));
            //Receipt receipt18 = new Receipt(7, 15.5, new DateTime(2021, 01, 30));
            //DatabaseController.InsertReceipt(receipt3);
            //DatabaseController.InsertReceipt(receipt4);
            //DatabaseController.InsertReceipt(receipt5);
            //DatabaseController.InsertReceipt(receipt6);
            //DatabaseController.InsertReceipt(receipt);
            //DatabaseController.InsertReceipt(receipt2);
            //DatabaseController.InsertReceipt(receipt7);
            //DatabaseController.InsertReceipt(receipt8);
            //DatabaseController.InsertReceipt(receipt9);
            //DatabaseController.InsertReceipt(receipt10);
            //DatabaseController.InsertReceipt(receipt11);
            //DatabaseController.InsertReceipt(receipt12);
            //DatabaseController.InsertReceipt(receipt13);
            //DatabaseController.InsertReceipt(receipt14);
            //DatabaseController.InsertReceipt(receipt15);
            //DatabaseController.InsertReceipt(receipt16);
            //DatabaseController.InsertReceipt(receipt17);
            //DatabaseController.InsertReceipt(receipt18);
            date1Button.Text= DateFrom.ToString("dd/MM/yyyy");
            date2Button.Text = DateTo.ToString("dd/MM/yyyy");
            date1Button.Click += DateSelect_OnClick;
            date2Button.Click += DateSelect2_OnClick;
            removeButton.Click += RemoveReceipt;
            backButton.Click += BackToMenu;
            

            listOfReceipt = DatabaseController.SelectReceipts(DateFrom, DateTo);
            rAdapter = new ReceiptAdapter(listOfReceipt);
            rAdapter.ItemClick += OnItemClick;
            rLayoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(rLayoutManager);
            recyclerView.SetAdapter(rAdapter);
            DatabaseController.SelectReceipts(DateFrom, DateTo);
        
        }
        public void ReloadData()
        {
            listOfReceipt.Clear();
            listOfReceipt = DatabaseController.SelectReceipts(DateFrom,DateTo);        
            rAdapter = new ReceiptAdapter(listOfReceipt);
            rLayoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(rLayoutManager);
            recyclerView.SetAdapter(rAdapter);
            DatabaseController.SelectReceipts(DateFrom, DateTo);
        }
        private void BackToMenu(object sender, EventArgs e)
        {
            Finish();
        }


        private void RemoveReceipt(object sender, EventArgs e)
        {
           if(selectedItem>=0)
           {
                Receipt receipt = listOfReceipt[selectedItem];
                Toast.MakeText(this, $"Usunięto" , ToastLength.Short).Show();
                DatabaseController.DeleteReceipt(receipt);
                ReloadData();
               
           }
        }
        public void OnItemClick(object sender, int position)
        {
            var item = sender;
            int photoNum = position + 1;
            selectedItem = position;    
        }
      
        private void DateSelect_OnClick(object sender, EventArgs e)
        {
            dateId = 1;
        
            ShowDialog(dateId);
        }
        private void DateSelect2_OnClick(object sender, EventArgs e)
        {
           dateId = 2;

            ShowDialog(dateId);
        }
        protected override Dialog OnCreateDialog(int id)
        {
            if (id >= 1)
            {
                DateTime currently = DateTime.Now;
                return  new DatePickerDialog(this, this, currently.Year, currently.Month - 1, currently.Day);
                
            }


            return null;
        }
        public void OnDateSet(DatePicker view, int Year, int Month, int dayOfMonth)
        {
          
            if (dateId == 1)
            {
                DateFrom = new DateTime(Year, Month + 1, dayOfMonth);              
            }                
            else if( dateId == 2)
            {
                DateTo = new DateTime(Year, Month + 1, dayOfMonth);              
            }
            CheckDate(DateFrom, DateTo);

            DatabaseController.SelectReceipts(DateFrom, DateTo);
        }

        private void CheckDate(DateTime dateFrom, DateTime dateTo)
        {
           if(dateTo != null && dateFrom != null)
           {
                DateFrom = (dateTo > dateFrom) ? dateFrom : dateTo;
                SetDate();
           }
        }

        private void SetDate()
        {
            date1Button.Text = DateFrom.ToString("dd/MM/yyyy");
            date2Button.Text = DateTo.ToString("dd/MM/yyyy");
            ReloadData();
        }


       }
}