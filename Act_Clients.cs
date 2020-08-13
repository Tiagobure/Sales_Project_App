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
        List<Cl_Clients> CLIENTS;
        List<string> NAMES;
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

            List_clients.ItemLongClick += List_clients_ItemLongClick;
        }

        private void List_clients_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            Cl_Clients Clients_select = CLIENTS[e.Position];

            AlertDialog.Builder Box_Edit_Delete = new AlertDialog.Builder(this);
            Box_Edit_Delete.SetTitle("EDIT | DELETE");
            Box_Edit_Delete.SetMessage(Clients_select.Name);
            

            Box_Edit_Delete.SetPositiveButton("Edit", delegate { /*BACK MAN, EDIT*/});
            Box_Edit_Delete.SetNegativeButton("Delete", delegate { DeleteCustomer(Clients_select.Id_Client); });

            Box_Edit_Delete.Show();


        }
        private void DeleteCustomer(int id_client)
        {

            Cl_Manager.ExeNonQuery("DELETE FROM Clients WHERE Id_Client = " + id_client);
            BuildsCustomerList();

        }

        private void Cmd_add_client_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(Act_Clients_Edit));
        }

       

        private void BuildsCustomerList()
        {
            DataTable data = Cl_Manager.ExeQuery("SELECT * FROM Clients");
            CLIENTS = new List<Cl_Clients>();

            foreach (DataRow line in data.Rows)
            {
                CLIENTS.Add(new Cl_Clients()
                {
                    Id_Client = Convert.ToInt16(line["Id_Client"]),
                    Name = line["Name"].ToString(),
                    Telephone = line["Telephone"].ToString()
                });

            }

            NAMES = new List<string>();
            foreach (Cl_Clients cl in CLIENTS)
            {
                NAMES.Add(cl.Name);
            }

            ArrayAdapter<string> AdapTer = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, NAMES);

            List_clients.Adapter = AdapTer;

            PresentTotalCustomers(CLIENTS.Count);

        }
        private void PresentTotalCustomers(int total_customers)
        {

            Number_clients.Text = "Registered customers: " + total_customers;
        }
    }
}