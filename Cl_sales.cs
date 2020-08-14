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
   public class Cl_sales
    {
        public string Name_Client { get; set; }
        public string Name_Product { get; set; }
        public int Price_Product { get; set; }
        public int Amount { get; set; }
        public int Price_Total { get; set; }

    }
}