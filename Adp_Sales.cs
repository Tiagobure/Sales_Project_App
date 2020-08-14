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
    class Adp_Sales : BaseAdapter<Cl_sales>
    {
        private List<Cl_sales> SALES;
        private Context context;
        public Adp_Sales(Context context, List<Cl_sales> sales)
        {
            this.context = context;
            SALES = sales;


        }
        public override Cl_sales this[int position] { 
        
        get { return SALES[position]; }
        }

        public override int Count
        {
            get { return SALES.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            View line = convertView;
            if (line == null)
            {
                line = LayoutInflater.From(context).Inflate(Resource.Layout.layout_list_sales, null, false);
            }
                TextView name_client = line.FindViewById<TextView>(Resource.Id.text_name_client);
                TextView name_product = line.FindViewById<TextView>(Resource.Id.List_Name_product_sales);
                TextView price_amount = line.FindViewById<TextView>(Resource.Id.List_Price_amount_sales);
                TextView total = line.FindViewById<TextView>(Resource.Id.List_Total_sales);

                name_client.Text = SALES[position].Name_Client;
                name_product.Text = SALES[position].Name_Product;
                price_amount.Text = SALES[position].Price_Product + " x " + SALES[position].Amount;
                total.Text = SALES[position].Price_Total.ToString();

                return line;

            
        }
    }
}