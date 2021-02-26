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

namespace CapturePicture
{
    public class Calkulator
    {
        public static bool CheckPrice(string value)
        {
            double price = 0;
            try
            {             
                price = Convert.ToDouble(value);
            }
            catch (Exception e)
            {
                return false;
                throw;
            }
            return true;
        }
     
    }
}