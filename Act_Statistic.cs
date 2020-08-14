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
    [Activity(Label = "Act_Statistic")]
    public class Act_Statistic : Activity
    {
        ListView List_sales;
        List<Cl_sales> SALES;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.layout_Statistics);

            List_sales = FindViewById<ListView>(Resource.Id.list_sales);
            BuildSalesList();
        }

        private void BuildSalesList()
        {

        }
    }
}