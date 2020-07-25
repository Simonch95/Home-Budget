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

namespace CapturePicture
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class AddReceipt : AppCompatActivity
    {
       public Button caputreButton;
        ImageView imageView;
        private TextView textResult;
        public object AuthImplicit(string projectId)
        {
            // If you don't specify credentials when constructing the client, the
            // client library will look for credentials in the environment.
            var credential = GoogleCredential.GetApplicationDefault();
            var storage = StorageClient.Create(credential);
            // Make an authenticated API request.
            var buckets = storage.ListBuckets(projectId);
            foreach (var bucket in buckets)
            {
                caputreButton.Text = bucket.Name;
                //Console.WriteLine(bucket.Name);
            }
            return null;
        }

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
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.add_receipt); 
            caputreButton = FindViewById<Button>(Resource.Id.captureButton);
            imageView = FindViewById<ImageView>(Resource.Id.image);
            textResult = FindViewById<TextView>(Resource.Id.textResult);
            caputreButton.Click += CaptureButton_Click;
            RequestPermissions(permissionGroup,0);

            //TextRecognizer txtRecognizer = new TextRecognizer.Builder(ApplicationContext).Build();
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
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 40,
                Name="newimage.jpg",
                Directory = "sample",
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
            if(!textR.IsOperational)
            {
                Log.Error("Error", "asdasadasda");
            }
            else
            {
                Frame frame = new Frame.Builder().SetBitmap(bitmap).Build();
                SparseArray items = textR.Detect(frame);
                StringBuilder strBuilder = new StringBuilder();
                for (int i = 0; i < items.Size(); i++)
                {
                    TextBlock item = (TextBlock) items.ValueAt(i);
                    strBuilder.Append(item.Value);
                    strBuilder.Append("\n");
                }

                Toast.MakeText(this, strBuilder.ToString(), ToastLength.Long).Show();
                textResult.Text = strBuilder.ToString();
            }
            // Image img = Image.FromBytes(imageArray);


            // DetectText(img);

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        private void DetectText(Image image)
        {
            // [START vision_text_detection]
            // [START vision_text_detection_gcs]
            var client = ImageAnnotatorClient.Create();
            var response = client.DetectText(image);
            foreach (var annotation in response)
            {
                if (annotation.Description != null)
                {
                    caputreButton.Text = annotation.Description;
                } // Console.WriteLine(annotation.Description);
                  
            }
            // [END vision_text_detection_gcs]
            // [END vision_text_detection]
          //  return 0;
        }
    }
}