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
   public class Category
    {
        private int id_Category;
        [PrimaryKey, AutoIncrement]
        public int Id_Category 
        {
            get { return id_Category; }
            set { id_Category = value; }
        }
       
     
        private string categoryName;
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }
     
        public Category()
        {

        }
        public Category( string categoryName)
        {
            this.CategoryName = categoryName;
           
        }
        public Category(int idCategory, string categoryName)
        {
            this.Id_Category = idCategory;
            this.CategoryName = categoryName;
        }
       

    }
}