using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Environment = System.Environment;
using System.IO;
using Android.Util;
using CapturePicture.Model;

namespace CapturePicture.DataBase
{
    public static class DatabaseController
    {
      
        public static string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
     
        public static bool CreateDatebase()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "HomeBudgetDB.db")))
                {
                    connection.CreateTable<Category>();
                    connection.CreateTable<Receipt>();
                    connection.CreateTable<CategoryLimit>();
                }

                return true;
            }
            catch (SQLiteException ex)
            {
                Log.Info("Error",ex.Message);
                return false;
                throw;
            }

        }




        #region INSERT
        public static bool InsertReceipt(Receipt receipt)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "HomeBudgetDB.db")))
                {
                    connection.Insert(receipt);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLEx", ex.Message);
                return false;
                throw;
            }
        }
        public static void InsertCategory(Category category)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "HomeBudgetDB.db")))
                {
                    connection.Insert(category);
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLEx", ex.Message);

                throw;
            }

        }

    

        public static bool InsertCategoryLimit(CategoryLimit categoryLimit)
        {
            try
            {
                Log.Info("Dodaj limit", categoryLimit.Date.ToString("dd/MM/yyyy"));
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "HomeBudgetDB.db")))
                {
                    connection.Insert(categoryLimit);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLEx", ex.Message);
                return false;
                throw;
            }
        }
        #endregion

        #region SELECT
        public static bool ExistLimit(DateTime date, CategoryLimit categoryLimit)
        {
            DateTime dateTime = new DateTime(date.Year,date.Month,1);
            Log.Info("Czy istnieje limit?", dateTime.ToString("dd/MM/yyyy"));

            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "HomeBudgetDB.db")))
                {
                    var list = connection.Query<CategoryLimit>("SELECT * FROM CategoryLimit WHERE Date >? AND Date <? AND Id_Category=? ", dateTime.Ticks, dateTime.AddMonths(1).Ticks,categoryLimit.Id_Category); //1
                    if (list.Count > 0)
                        return true;
                    else return false;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLEx", ex.Message);
                return false;
                throw;
            }
        }
        public static bool IsEmptyTable()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "HomeBudgetDB.db")))
                {

                    var list = connection.Query<Category>("SELECT * FROM Category");
                    if (list.Count == 0)
                    {
                        return true;
                    }
                    return false;

                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLEx", ex.Message);
                return false;
                throw;
            }
        }
        public static int SelectIdCategoryLimit(CategoryLimit categoryLimit)
        {
            DateTime dateTime = new DateTime(categoryLimit.Date.Year, categoryLimit.Date.Month, 1);
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "HomeBudgetDB.db")))
                {
                    var list = connection.Query<CategoryLimit>("SELECT Id_Limit FROM CategoryLimit WHERE Date >? AND Date <? AND Id_Category=? ", dateTime.Ticks, dateTime.AddMonths(1).Ticks, categoryLimit.Id_Category); //1
                    if (list.Count > 0)
                        return list[0].Id_Limit;
                    else return 0;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLEx", ex.Message);
                return 0;
                throw;
            }
        }
        public static List<Receipt> SelectReceipts()
        {
            List<Receipt> listOfReceipt = new List<Receipt>();
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "HomeBudgetDB.db")))
                {
                    listOfReceipt = connection.Query<Receipt>("SELECT * FROM Receipt ");
                    return listOfReceipt;
                }

            }
            catch (Exception ex)
            {

                Log.Info("SQLEx", ex.Message);
                return null;
                throw;
            }
        }
        public static List<Receipt> SelectReceipts(DateTime dateFrom, DateTime dateTo)
        {
          
            List<Receipt> listOfReceipt = new List<Receipt>();
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "HomeBudgetDB.db")))
                {
                    listOfReceipt = connection.Query<Receipt>("SELECT * FROM Receipt WHERE Date >? AND Date <?  ORDER BY Date DESC", dateFrom.Ticks, dateTo.AddDays(1).Ticks);
                    return listOfReceipt;
                }

            }
            catch (Exception ex)
            {

                Log.Info("SQLEx", ex.Message);
                return null;
                throw;
            }
        }
        public static double SelectSpendedMoneyOfCategory(int year, int month,int id_category)
        {
            double spended = 0;
            try
            {
                DateTime date = new DateTime(year, month, 1);

                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "HomeBudgetDB.db")))
                {
                  
                    //Działa   
                    var valueList = connection.Query<Receipt>($"SELECT Price FROM Receipt WHERE Date >? AND Date <? AND Id_Category =?", date.Ticks, date.AddMonths(1).Ticks, id_category);
                    foreach (var receipt in valueList)
                    {
                        
                        spended += receipt.Price;
                    }
                    if (spended > 0)
                    {
                        return spended;
                    }
                    return spended;
                }

            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLEx", ex.Message);
                throw;
            }
        }
        public static double SelectSpendedMoney(int year, int month)
        {
            double spended = 0;
            try
            {
                DateTime date = new DateTime(year, month, 1);

                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "HomeBudgetDB.db")))
                {
                    var valueList = connection.Query<Receipt>($"SELECT Price FROM Receipt WHERE Date >? AND Date <?", date.Ticks, date.AddMonths(1).Ticks);//AND Id_Category =?
                    foreach (var receipt in valueList)
                    {
                            spended += receipt.Price;
                    }
                    if (spended > 0)
                    {
                        return spended;
                    }
                    return spended;
                }

            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLEx", ex.Message);
                throw;
            }
        }
        public static int SelectIdCategory(string categoryName)
        {
            int id = 0;
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "HomeBudgetDB.db")))
                {

                    var list = connection.Query<Category>("SELECT Id_Category FROM Category Where CategoryName=?", categoryName);
                    if (list.Count == 1) // before >1
                    {
                        id = list[0].Id_Category;
                    }
                    return id;
                }

            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLEx", ex.Message);
                return 0;
                throw;
            }
        }
      
        public static double SelectAmountLimit(int year, int month,int id) //Pobierz wszystkie limity budżetu na dany miesiąc i zsumuj
        {
            DateTime date = new DateTime(year, month, 1);
         
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "HomeBudgetDB.db")))
                {
                    var list = connection.Query<CategoryLimit>("SELECT AmountLimit FROM CategoryLimit Where Id_Category=? AND Date > ? AND Date <?", id, date.Ticks, date.AddMonths(1).Ticks); // Date
                                                                                                                                                                                               // var list = connection.Query<CategoryLimit>($"SELECT AmountLimit FROM CategoryLimit WHERE Id_Category IN(SELECT Id_Category FROM Category WHERE CategoryName=?,{categoryName}) " +
                                                                                                                                                                                               //      $"Date > ? AND Date <?", "id catgori", date.Ticks, date.AddMonths(1).Ticks);
                    if (list.Count > 0)
                    {
                        return list[0].AmountLimit;
                    }
                    else return 0;

                       
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLEx", ex.Message);
                return 0;
                throw;
            }
        }
        public static List<CategoryLimit> GetLimitBudget(DateTime date)
        {
            DateTime dateTime = new DateTime(date.Year, date.Month, 1);
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "HomeBudgetDB.db")))
                {
                    var listofCategoryLimit = connection.Query<CategoryLimit>("SELECT * FROM CategoryLimit WHERE Date >? AND Date <? ", dateTime.Ticks, dateTime.AddMonths(1).Ticks);
                    return listofCategoryLimit;
                }

            }
            catch (Exception ex)
            {
                return null;
                throw;
            }
        }
        public static string SelectCategoryName(int id_Category)
        {
            string categoryName = "";
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "HomeBudgetDB.db")))
            {

                
                var list = connection.Query<Category>("SELECT CategoryName FROM Category Where Id_Category=?", id_Category);
                if (list.Count == 1) // before >1
                {
                    categoryName = list[0].CategoryName;
                }
                return categoryName;
            }

            return "";
        }
        #endregion

        #region UPDATE
        public static bool UpdateLimitOfCategory(CategoryLimit categoryLimit)
        {
            DateTime dateTime = new DateTime(categoryLimit.Date.Year, categoryLimit.Date.Month, 1);
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "HomeBudgetDB.db")))
                {
                    var list = connection.Query<CategoryLimit>("UPDATE CategoryLimit SET AmountLimit=?, Active=? WHERE Date >? AND Date <? AND Id_Category=? ", categoryLimit.AmountLimit, categoryLimit.Active, dateTime.Ticks, dateTime.AddMonths(1).Ticks,categoryLimit.Id_Category); //Date=?, categoryLimit.Date.Ticks, 
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLEx", ex.Message);
                return false;
                throw;
            }
        }

        public static bool UpdateTableCategory(Category category)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "HomeBudgetDB.db")))
                {   //  connection.Query<Category>("UPDATE Receipt set Id_Category=?, Category_Name=?,AmountLimit=?,Id_Receipt=? Where Id=?",category.Id_Category,category.CategoryName,category.AmountLimit,category.Id_Receipt);
                    return true;
                }

            }
            catch (SQLiteException ex)
            {

                Log.Info("SQLEx", ex.Message);
                return false;
                throw;
            }
        }
        #endregion

        #region DELETE
        public static bool DeleteTableCategory(Category category)
        {
            try
            {

                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "HomeBudgetDB.db")))
                {
                    connection.Delete(category);
                    return true;
                }


            }
            catch (SQLiteException ex)
            {

                Log.Info("SQLEx", ex.Message);
                return false;
                throw;
            }
        }
        public static bool DeleteReceipt(Receipt receipt)
        {
            try
            {

                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "HomeBudgetDB.db")))
                {
                    connection.Delete(receipt);
                    return true;
                }


            }
            catch (SQLiteException ex)
            {

                Log.Info("SQLEx", ex.Message);
                return false;
                throw;
            }
        }
        #endregion


















    }
}