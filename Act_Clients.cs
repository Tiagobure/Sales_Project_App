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
            List<Cl_Clients> CLIENTS = new List<Cl_Clients>();
            DataTable data = Cl_Manager.ExeQuery("SELECT * FROM Clients");
            foreach (DataRow line in data.Rows)
            {
                CLIENTS.Add(new Cl_Clients()
                {
                    Id_Client = Convert.ToInt16(line["Id_Client"]),
                    Name = line["Name"].ToString(),
                    Telephone = line["Telephone"].ToString()
                });

            }

            List<string> names = new List<string>();
            foreach(Cl_Clients cl in CLIENTS)
            {
                names.Add(cl.Name);
            }

            ArrayAdapter<string> AdapTer = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, names);

            List_clients.Adapter = AdapTer;

            PresentTotalCustomers(CLIENTS.Count);

        }
        private void PresentTotalCustomers(int total_customers)
        {
            
            Number_clients.Text = "Registered customers: " + total_customers;
        }
    }
}