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
using SQLite;

namespace CapturePicture.Model
{
    [Table("Receipt")]
    public class Receipt
    {
        private int id_Receipt;
        [PrimaryKey, AutoIncrement]
        public int Id_Receipt //PK
        {
            get { return id_Receipt; }
            set { id_Receipt = value; }
        }
        private int id_Category; //FK
        
        
        public int Id_Category
        {
            get { return id_Category; }
            set { id_Category = value; }
        }
        private double price;
        [Column("Price")]
        public double Price
        {
            get { return price; }
            set { price = value; }
        }
     
        private DateTime date;
       [Column("Date")]
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
        public Receipt()
        {

        }
        public Receipt( int idCategory, double price, DateTime date)
        {            
            this.Id_Category = idCategory;
           // this.ReceiptName = receiptName;
            this.Price = price;
            this.Date = date;

        }

        public Receipt(int idReceipt, int idCategory, double price, DateTime date)
        {
            this.Id_Receipt = idReceipt;
            this.Id_Category = idCategory;
            this.Price = price;
            this.Date = date;

        }

    }
}