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

namespace FruitSales
{
    [Activity(Label = "Fruits", Theme = "@style/AppTheme", MainLauncher = true, Icon = "@drawable/pineapple")]
    public class Act_Menu_main : Activity
    {

        readonly string[] Permission =
        {
            Android.Manifest.Permission.Internet,
            Android.Manifest.Permission.WriteExternalStorage,

        };
        const int RequestId = 0;

        Button cmd_client;
        Button cmd_products;
        Button cmd_sales;
        Button cmd_statistics;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            RequestPermissions(Permission, RequestId);

            // Create your application here
            SetContentView(Resource.Layout.Layout_Menu_main);


            cmd_client = FindViewById<Button>(Resource.Id.cmd_client);
            cmd_products = FindViewById<Button>(Resource.Id.cmd_products);
            cmd_sales = FindViewById<Button>(Resource.Id.cmd_sales);
            cmd_statistics = FindViewById<Button>(Resource.Id.cmd_Statistics);

            cmd_client.Click += Cmd_client_Click;
            cmd_products.Click += Cmd_products_Click;
            cmd_sales.Click += Cmd_sales_Click;
            cmd_statistics.Click += Cmd_statistics_Click;
        }

        private void Cmd_client_Click(object sender, EventArgs e)
        {
            //boxclient
            StartActivity(typeof(Act_Clients));
        }

        private void Cmd_products_Click(object sender, EventArgs e)
        {
            //boxproducts 
            StartActivity(typeof(Act_Products));
          

           
        }

        private void Cmd_sales_Click(object sender, EventArgs e)
        {
            //boxsales
        }

        private void Cmd_statistics_Click(object sender, EventArgs e)
        {
            //boxstatistics
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            Cl_Manager.MainApplication();

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}