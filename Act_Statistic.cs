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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.layout_Statistics);

            List_sales = FindViewById<ListView>(Resource.Id.list_sales);
            BuildListClientsProducts();
            BuildSalesList();
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


        }
        private void BuildSalesList()
        {

            string Query = "SELECT * FROM Sales";


            SALES = new List<Cl_sales>();
            DataTable data_sales = Cl_Manager.ExeQuery(Query);
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