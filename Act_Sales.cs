using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FruitSales
{
    [Activity(Label = "Act_Sales")]
    public class Act_Sales : Activity
    {
        Spinner Combo_clients;
        Spinner Combo_products;
        EditText Text_amount;
        Button Cmd_sales_cancel;
        Button Cmd_sales_record;
        TextView Text_info;
        TextView Text_price_total;

        List<Cl_Clients> CLIENTS;
        List<Cl_Products> PRODUCTS;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.layout_Sales);

            Combo_clients = FindViewById<Spinner>(Resource.Id.Combo_Clients);
            Combo_products = FindViewById<Spinner>(Resource.Id.Combo_Products);
            Text_amount = FindViewById<EditText>(Resource.Id.Edit_Amount);
            Cmd_sales_cancel = FindViewById<Button>(Resource.Id.cmd_sales_cancel);
            Cmd_sales_record = FindViewById<Button>(Resource.Id.cmd_sales_record);
            Text_info = FindViewById<TextView>(Resource.Id.text_Info);
            Text_price_total = FindViewById<TextView>(Resource.Id.text_Price_Total);

            Text_info.Text = "";

            LoadCombo();

            Cmd_sales_cancel.Click += delegate { this.Finish(); };
            Cmd_sales_record.Click += Cmd_sales_record_Click;
            Text_amount.TextChanged += Text_amount_TextChanged;
            Combo_products.ItemSelected += Combo_products_ItemSelected;

        }

        private void Combo_products_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            UpdatePriceTotal();
        }

        private void Text_amount_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            UpdatePriceTotal();
        }

        private void Cmd_sales_record_Click(object sender, EventArgs e)
        {
            if(Combo_clients.SelectedItemPosition == -1 || 
               Combo_products.SelectedItemPosition == -1 ||
               Text_amount.Text == "")
            {
                AlertDialog.Builder boxAmount = new AlertDialog.Builder(this);
                boxAmount.SetTitle("ERROR");
                boxAmount.SetMessage("Lack of information");
                boxAmount.Show();
                return;
            }
            int id_client = CLIENTS[Combo_clients.SelectedItemPosition].Id_Client;
            int id_product = PRODUCTS[Combo_products.SelectedItemPosition].Id_Products;
            int Amount = int.Parse(Text_amount.Text);

            int Id_Sales = Cl_Manager.ID_AVAILABLE("Sales", "Id_Sales");

            List<SQLparameters> para = new List<SQLparameters>()
            {
              new SQLparameters("@Id_Sales", Id_Sales),
               new SQLparameters("@Id_client", id_client),
               new SQLparameters("@Id_Product",id_product),
               new SQLparameters("@Amount", Amount),
                new SQLparameters("@Update_Info_Sales", DateTime.Now)

            };

           Cl_Manager.ExeNonQuery("INSERT INTO Sales VALUES(" +
                "@Id_Sales, " +
                "@Id_client, " +
                "@Id_Product, " +
                "@Amount," +
                "@Update_Info_Sales)", para);

            Text_info.Text = "SALES RECORDED SUCESSFULLY!";
            
            Text_amount.Text = "";

        }

        private void LoadCombo()
        {
            DataTable data_clients = Cl_Manager.ExeQuery("SELECT * FROM Clients ORDER BY Name ASC");
            DataTable data_products = Cl_Manager.ExeQuery("SELECT * FROM Products ORDER BY Name_product ASC");


            CLIENTS = new List<Cl_Clients>();
            foreach (DataRow line in data_clients.Rows)
            {
                CLIENTS.Add(new Cl_Clients
                {
                    Id_Client = Convert.ToInt16(line["Id_Client"]),
                    Name = line["Name"].ToString(),
                    Telephone = line["Telephone"].ToString(),

                });


            }
            PRODUCTS = new List<Cl_Products>();
            foreach (DataRow line in data_products.Rows)
            {
                PRODUCTS.Add(new Cl_Products
                {
                    Id_Products = Convert.ToInt16(line["Id_Product"]),
                    Name_Product = line["Name_product"].ToString(),
                    Price = Convert.ToInt16(line["Price"]),

                });


            }

            List<string> list_name_clients = new List<string>();
            List<string> list_name_products = new List<string>();
            foreach (Cl_Clients cli in CLIENTS) 
                list_name_clients.Add(cli.Name);
            foreach (Cl_Products pro in PRODUCTS)
                list_name_products.Add(pro.Name_Product);

            Combo_clients.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, list_name_clients);
            Combo_products.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, list_name_products);

            
        }

        private void UpdatePriceTotal()
        {
            //update price
            if (PRODUCTS.Count == 0)
                return;

            int Amount = -1;
            int Price_products = -1;
            if (Text_amount.Text != "")
            {
                if (int.Parse(Text_amount.Text) > 0)
                {
                    Amount = int.Parse(Text_amount.Text);
                    Price_products = PRODUCTS[Combo_products.SelectedItemPosition].Price;
                }
            }

            if (Amount == -1)
            {
                Text_price_total.Text = "";
            }
            else
            {
                Text_price_total.Text = "Total: " + (Amount * Price_products).ToString();
            }
        }
    }
}