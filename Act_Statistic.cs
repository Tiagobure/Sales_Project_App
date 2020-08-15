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
    [Activity(Label = "Act_Statistic")]
    public class Act_Statistic : Activity
    {
        ListView List_sales;
        List<Cl_Clients> CLIENTS;
        List<Cl_Products> PRODUCTS;
        List<Cl_sales> SALES;

        Spinner Combo_client;
        Spinner Combo_product;

        Button cmd_clients;
        Button cmd_products;
        Button cmd_total_sales;
        Button cmd_total;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.layout_Statistics);

            List_sales = FindViewById<ListView>(Resource.Id.list_sales);
            Combo_client = FindViewById<Spinner>(Resource.Id.Combo_clients_sta);
            Combo_product = FindViewById<Spinner>(Resource.Id.Combo_products_sta);

            cmd_clients = FindViewById<Button>(Resource.Id.cmd_client_sta);
            cmd_products = FindViewById<Button>(Resource.Id.cmd_product_sta);
            cmd_total = FindViewById<Button>(Resource.Id.Cmd_total_sta);
            cmd_total_sales = FindViewById<Button>(Resource.Id.cmd_tota_sales_sta);

            BuildListClientsProducts();
            BuildSalesList("SELECT * FROM Sales");
            PresentsSalesList();

            cmd_clients.Click += Cmd_clients_Click;
            cmd_products.Click += Cmd_products_Click;
            cmd_total.Click += Cmd_total_Click;
            cmd_total_sales.Click += Cmd_total_sales_Click;
        }

        private void Cmd_total_sales_Click(object sender, EventArgs e)
        {
            BuildSalesList("SELECT * FROM Sales");
            PresentsSalesList();

        }

        private void Cmd_total_Click(object sender, EventArgs e)
        {
            BuildSalesList("SELECT * FROM Sales");
            int total = 0;
            foreach(Cl_sales to in SALES)
            {
                total += to.Price_Total;
            }

            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("TOTAL SALES");
            builder.SetMessage("Total calculated: " + total);
            builder.SetCancelable(false);
            builder.SetPositiveButton("OK", delegate { });
            builder.Show();

        }

        private void Cmd_products_Click(object sender, EventArgs e)
        {
            int index = Combo_product.SelectedItemPosition;
            int id_product = PRODUCTS[index].Id_Products;

            BuildSalesList("SELECT * FROM Sales WHERE Id_Product = "+ id_product);
            PresentsSalesList();

        }

        private void Cmd_clients_Click(object sender, EventArgs e)
        {
            int index = Combo_client.SelectedItemPosition;
            int id_client = CLIENTS[index].Id_Client;

            BuildSalesList("SELECT * FROM Sales WHERE Id_Client = " + id_client);
            PresentsSalesList();
        }

        private void BuildListClientsProducts()
        {

            #region Build Clients
            DataTable Data_Clients_ = Cl_Manager.ExeQuery("SELECT * FROM Clients");
            CLIENTS = new List<Cl_Clients>();
            foreach (DataRow line in Data_Clients_.Rows)
            {
                Cl_Clients Neo = new Cl_Clients();
                Neo.Id_Client = Convert.ToInt16(line["Id_Client"]);
                Neo.Name = line["Name"].ToString();
                Neo.Telephone = line["Telephone"].ToString();
                CLIENTS.Add(Neo);
            }
            #endregion

            #region Build List Products

            DataTable Data_products_ = Cl_Manager.ExeQuery("SELECT * FROM Products");
            PRODUCTS = new List<Cl_Products>();
            foreach (DataRow line in Data_products_.Rows)
            {
                Cl_Products Neo = new Cl_Products();
                Neo.Id_Products = Convert.ToInt16(line["Id_Product"]);
                Neo.Name_Product = line["Name_product"].ToString();
                Neo.Price = Convert.ToInt16(line["Price"]);
                PRODUCTS.Add(Neo);
            }
            #endregion

            List<string> Name_Clients = new List<string>();
            foreach(Cl_Clients cl in CLIENTS)
            {
                Name_Clients.Add(cl.Name);
            }
            ArrayAdapter<string> adpter_clients = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, Name_Clients);
            Combo_client.Adapter = adpter_clients;
            
            List<string> Name_Products = new List<string>();
            foreach (Cl_Products pr in PRODUCTS)
            {
                Name_Products.Add(pr.Name_Product);
            }
            ArrayAdapter<string> adapter_products = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, Name_Products);
            Combo_product.Adapter = adapter_products;

        }
        private void BuildSalesList(string query)
        {

            


            SALES = new List<Cl_sales>();
            DataTable data_sales = Cl_Manager.ExeQuery(query);
            foreach(DataRow line in data_sales.Rows)
            {
                Cl_sales Neo = new Cl_sales();

                int Id_Client = Convert.ToInt16(line["Id_client"]);
                int Id_Product = Convert.ToInt16(line["Id_Product"]);
                int Amount = Convert.ToInt16(line["Amount"]);

                string Name_clients = CLIENTS.Where(i => i.Id_Client == Id_Client).FirstOrDefault().Name;
                var Products = PRODUCTS.Where(a => a.Id_Products == Id_Product).FirstOrDefault();
                string Name_Products = Products.Name_Product;
                int Price_Unit = Products.Price;


                Neo.Name_Client = Name_clients;
                Neo.Name_Product = Name_Products;
                Neo.Amount = Amount;
                Neo.Price_Product = Price_Unit;
                Neo.Price_Total = Amount * Neo.Price_Product;



                SALES.Add(Neo);

            }


        }

        private void PresentsSalesList()
        {
            Adp_Sales adapter = new Adp_Sales(this, SALES);
            List_sales.Adapter = adapter;

        }
    }
}