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
    [Table("CategoryLimit")]
    public class CategoryLimit
    {
        [PrimaryKey, AutoIncrement]
        public int Id_Limit //PK
        {
            get; set;
        }
        public int Id_Category { get;  set; }
        public double AmountLimit   
        {
            get;set;
        }
        public bool Active { get; set; }
        [Column("Date")]
        public DateTime Date
        {
            get;set;
        }
        public CategoryLimit()
        {

        }
        public CategoryLimit(int idCategory, double amountLimit, bool active, DateTime date)
        {

            this.Id_Category = idCategory;
            this.AmountLimit = amountLimit;
            this.Date = date;
            this.Active = active;
        }
     
        public CategoryLimit(int idLimit, int idCategory, double amountLimit, bool active, DateTime date)
        {
            this.Id_Limit = idLimit;
            this.Id_Category = idCategory;
            this.AmountLimit = amountLimit;
            this.Date = date;
            this.Active = active;
        }
       
    }
}