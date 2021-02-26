using System;
using System.Drawing;
using Android;
using Android.App;
using Android.Gms.Vision;
using Android.Gms.Vision.Texts;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Plugin.Media;

using Bitmap = Android.Graphics.Bitmap;
using Google.Cloud.Vision.V1;
using Java.Lang;
using Image = Google.Cloud.Vision.V1.Image;
using AlertDialog = Android.App.AlertDialog;
using System.Runtime.CompilerServices;
using CapturePicture.Model;
using CapturePicture.DataBase;
using Android.Content;
using Tesseract.Droid;
using Tesseract;
using Com.Googlecode.Tesseract.Android;
using System.IO;

namespace CapturePicture
{
    [Activity(Label = "Dodaj paragon" )]
    public class ReceiptPage : AppCompatActivity
    {
       public Button captureButton, backButton, saveButton;
        ImageView imageView;
        private TextView resultText, categoryText;
        private EditText priceText;
        private string categoryName="";
        public DateTime date = DateTime.Now;

        private readonly string[] permissionGroup =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera
        };
      
        protected override void OnCreate(Bundle savedInstanceState)
        {
   
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
    
            SetContentView(Resource.Layout.add_receipt);
            captureButton = FindViewById<Button>(Resource.Id.captureBtn);
            imageView = FindViewById<ImageView>(Resource.Id.image);
            resultText = FindViewById<TextView>(Resource.Id.Result);
            backButton = FindViewById<Button>(Resource.Id.BackBtn);
            categoryText = FindViewById<TextView>(Resource.Id.Category);
            saveButton = FindViewById<Button>(Resource.Id.SaveBtn);
            priceText = FindViewById<EditText>(Resource.Id.PriceET);
            captureButton.Text = "Zrób zdjęcie";
       
            captureButton.Click += CaptureButton_Click;
            RequestPermissions(permissionGroup,0);
            categoryName = Intent.GetStringExtra("Category");
            categoryText.Text = categoryName;
            SetCategoryName(categoryName);
            backButton.Click += Close_OnClick;
             saveButton.Click += Save_OnClick;

            

        }

     
        private void Save_OnClick(object sender, EventArgs e)
        {
            string priceET = priceText.Text;
            double price = 0;

            if(priceET.Equals("")||priceET.Equals("0"))
            {
                Finish();
                Intent intent = new Intent(this, typeof(CategoriesPage));
                intent.PutExtra("date", Convert.ToString(date));
                this.StartActivity(intent);
            }
            else
            {
                int dot = 0;
                string temp = "";
                foreach (char item in priceET)
                {
                    if (dot < 2)
                    {
                        if (char.IsDigit(item))
                            temp += Convert.ToString(item);
                        else if (item.Equals('.') || item.Equals(','))
                        {
                            ++dot;
                            temp += Convert.ToString(item);

                        }
                        else if (!char.IsDigit(item))
                        {
                            temp = "0";
                            priceET = "0";
                            priceText.Text = temp;
                            break;
                        }
                    }
                    else break;
                }
                if (priceET.Contains('.'))
                {
                    priceET = priceET.Replace(".", ",");
                    if (Calkulator.CheckPrice(priceET))
                        price = Convert.ToDouble(priceET);

                    AddReceipt(categoryName, price);
                    Finish();
                    Intent intent = new Intent(this, typeof(CategoriesPage));
                    intent.PutExtra("date", Convert.ToString(date));
                    this.StartActivity(intent);
                }
                else
                {
                    price = Convert.ToDouble(priceET);

                    AddReceipt(categoryName, price);
                    Finish();
                    Intent intent = new Intent(this, typeof(CategoriesPage));
                    intent.PutExtra("date", Convert.ToString(date));
                    this.StartActivity(intent);
                }


            }
           

        }

        private void Close_OnClick(object sender, EventArgs e)
        {
            Finish();
            Intent intent = new Intent(this, typeof(CategoriesPage));
            intent.PutExtra("date",Convert.ToString(date));
            this.StartActivity(intent);
        }

        private void SetCategoryName(string categoryName)
        {
            switch (categoryName)
            {
                case "Food":
                    {
                        categoryText.Text = "Spożywcze: ";
                        break;
                    }
                case "Car":
                    {
                        categoryText.Text = "Samochód: ";
                        break;
                    }
                case "Cosmetic":
                    {
                        categoryText.Text = "Kosmetyki: ";
                        break;
                    }
                case "Transport":
                    {
                        categoryText.Text = "Transport: ";
                        break;
                    }
                case "House":
                    {
                        categoryText.Text = "Dom: ";
                        break;
                    }
                case "Entertainment":
                    {
                        categoryText.Text = "Rozrywka: ";
                        break;
                    }
                case "Clothes":
                    {
                        categoryText.Text = "Odzież: ";
                        break;
                    }
                case "Pharmacy":
                    {
                        categoryText.Text = "Apteka: ";
                        break;
                    }
                case "Gifts":
                    {
                        categoryText.Text = "Prezenty: ";
                        break;
                    }
                case "Another":
                    {
                        categoryText.Text = "Inne: ";
                        break;
                    }
            }
        }

        private void CaptureButton_Click(object sender, System.EventArgs e)
        {
            TakePhoto();          
        }

       async void TakePhoto()
        {
            await CrossMedia.Current.Initialize();
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
               
                RotateImage=false,
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 40,
                Name="recipt.jpg",
                Directory = "Home Budget",               
                SaveToAlbum = true
            });
            
            if (file == null)
            {
                return;
            }

            byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);
            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);

            imageView.SetImageBitmap(bitmap);
            TextRecognizer textR = new TextRecognizer.Builder(ApplicationContext).Build();
            string text = "";
            double price = 0;
            if (!textR.IsOperational)
            {
                Log.Error("Error", "Błąd");
            }
            else
            {               
                Frame frame = new Frame.Builder().SetBitmap(bitmap).Build();
                SparseArray items = textR.Detect(frame);
                StringBuilder strBuilder = new StringBuilder();
                for (int i = 0; i < items.Size(); i++)
                {
                    TextBlock item = (TextBlock)items.ValueAt(i);
                  
                    if (item.Value.Contains("SUMA PLN"))
                    {
                        for (int k = i; k < i + 2; k++)
                        {
                            item = (TextBlock)items.ValueAt(k);
                            strBuilder.Append(item.Value);
                           
                        }                     
                        text = CheckSigns(item.Value);
                         price = CheckTotal(text);
                        text = Convert.ToString(price);
                        AddReceipt(categoryName, price);
                        break;
                    }
                    else if (item.Value.Contains("PLN"))
                    {
                        
                        strBuilder.Append(item.Value);
                        text = Convert.ToString(strBuilder);
                        price = CheckTotal(text);
                        text = Convert.ToString(price);
                        AddReceipt(categoryName,price);
                        break;
                    }
                }
                if(text.Equals("")|| text.Equals("0"))
                {
                    Toast.MakeText(this, "Spórbuj ponownie", ToastLength.Long).Show();
                    resultText.Text = "Spróbuj ponownie";
                }

                else
                {
                    
                    resultText.Text = "Wydano: " + text + " PLN";
                }
               
            }
          

        }

        private void AddReceipt(string categoryName,double price)
        {
         
            int id = DatabaseController.SelectIdCategory(categoryName);
            Receipt receipt = new Receipt(id, price, date);
            DatabaseController.InsertReceipt(receipt);
        }

        private string CheckSigns(string value)
        {
            string price = "";
            value.TrimStart('P','L','N');
            int dot = 0;
            foreach (char item in value)
            {
                if (dot < 2)
                {
                    if (char.IsDigit(item))
                        price += Convert.ToString(item);
                    else if (item.Equals('.') || item.Equals(','))
                    {
                        ++dot;
                        price += Convert.ToString(item);

                    }
                    else if (!char.IsDigit(item))
                        break;
                }
                else break;
            }

            return price;
        }

      

        private double CheckTotal(string value)
        {
            double price = 0;
            try
            {
                if (value.Contains("PLN"))
                {
                    string[] priceText = value.Split("PLN");
                    value = priceText[1];
                    if (value.Contains('.'))
                        value = value.Replace(".", ",");
                    if (Calkulator.CheckPrice(value))
                        price = Convert.ToDouble(value);
                }
                else
                {
                    if (value.Contains('.'))
                        value = value.Replace(".", ",");
                    if (Calkulator.CheckPrice(value))
                        price = Convert.ToDouble(value);
                }

               
                
            }
            catch (System.Exception)
            {
                if (price == 0)
                    Toast.MakeText(this, "Coś poszło nie tak.\nSpróbuj ponownie", ToastLength.Long).Show();
                throw;
            }
            return price; 

             
            }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
      
      

       
    }
}