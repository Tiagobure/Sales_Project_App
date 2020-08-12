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
    [Activity(Label = "Act_Clients")]
    public class Act_Clients : Activity
    {
        Button Cmd_add_client;
        ListView List_clients;
        TextView Number_clients;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Layout_Client);

            Cmd_add_client = FindViewById<Button>(Resource.Id.cmd_add_client);
            List_clients = FindViewById<ListView>(Resource.Id.list_clients);
            Number_clients = FindViewById<TextView>(Resource.Id.label_number_clients);


            BuildsCustomerList();

            Cmd_add_client.Click += Cmd_add_client_Click;



        }

        private void Cmd_add_client_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BuildsCustomerList()
        {
            PresentTotalCustomers();

        }
        private void PresentTotalCustomers()
        {
            int Total_customers = 0;
            DataTable data = Cl_Manager.ExeQuery("SELECT  Id_Client FROM Clients");
            if(data.Rows.Count != 0)
            {
                Total_customers = data.Rows.Count;

            }
            Number_clients.Text = "Registered customers: " + Total_customers;
        }
    }
}