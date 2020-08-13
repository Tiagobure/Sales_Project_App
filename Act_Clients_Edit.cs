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
    [Activity(Label = "Act_Clients_Edit")]
    public class Act_Clients_Edit : Activity
    {

        Button Cmd_gravar;
        Button Cmd_cancelar;
        EditText Name_client;
        EditText Phone_client;

        int Id_client = 0;
        bool Edit = false;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(savedInstanceState);

           


            // Create your application here
            SetContentView(Resource.Layout.layout_edit_client);

            Cmd_cancelar = FindViewById<Button>(Resource.Id.cmd_client_cancelar);
            Cmd_gravar = FindViewById<Button>(Resource.Id.cmd_client_gravar);
            Name_client = FindViewById<EditText>(Resource.Id.edit_name_client);
            Phone_client = FindViewById<EditText>(Resource.Id.edit_phone_client);


            Cmd_cancelar.Click += delegate { this.Finish(); };
            Cmd_gravar.Click += Cmd_gravar_Click;

            //edit client or no
            if (this.Intent.GetStringExtra("Id_Client") != null)
            {
                Id_client = int.Parse(this.Intent.GetStringExtra("Id_Client"));
                PresentCustomer();
                Edit = true;
            }
        }
        private void PresentCustomer()
        {
            DataTable data = Cl_Manager.ExeQuery("SELECT * FROM Clients WHERE Id_Client = " + Id_client);
            Name_client.Text = data.Rows[0]["Name"].ToString();
            Phone_client.Text = data.Rows[0]["Telephone"].ToString();
        }
        private void Cmd_gravar_Click(object sender, EventArgs e)
        {
            if (Name_client.Text == "" || Name_client.Text == null || Phone_client.Text == "" || Phone_client.Text == null)
            {
                AlertDialog.Builder box = new AlertDialog.Builder(this);
                box.SetTitle("ERROR!");
                box.SetMessage("Fill in all fields");
                box.SetPositiveButton("OK", delegate { });
                box.Show();
                return;
            }

            List<SQLparameters> lparameters = new List<SQLparameters>();


            if (!Edit)
            {
                lparameters.Add(new SQLparameters("@Id_Client", Cl_Manager.ID_AVAILABLE("Clients", "Id_Client")));
            }
            else
            {
                lparameters.Add(new SQLparameters("@Id_Client", Id_client));
            }
            lparameters.Add(new SQLparameters("@Name", Name_client.Text));
            lparameters.Add(new SQLparameters("@Telephone", Phone_client.Text));
            lparameters.Add(new SQLparameters("@Update_Info", DateTime.Now));

            //NEW CLIENT
            if (!Edit)
            {
                DataTable data = Cl_Manager.ExeQuery("SELECT Name FROM Clients WHERE Name = @Name", lparameters);
                if(data.Rows.Count != 0)
                {
                    AlertDialog.Builder box = new AlertDialog.Builder(this);
                    box.SetTitle("ERROR");
                    box.SetMessage("There is already a customer with the same name!");
                    box.SetPositiveButton("OK", delegate { });
                    box.Show();
                    return;
                }
                Cl_Manager.ExeNonQuery("INSERT INTO Clients VALUES (" +
                   "@Id_Client, @Name, @Telephone, @Update_Info)", lparameters);


                Intent i = this.Intent;
                SetResult(Result.Ok, i);
                Finish();
            }
            else
            {
                //EDIT CLIENT
                DataTable data = Cl_Manager.ExeQuery("SELECT Name FROM Clients WHERE Name = @Name AND Id_Client <> @Id_Client", lparameters);
                if (data.Rows.Count != 0)
                {
                    AlertDialog.Builder box = new AlertDialog.Builder(this);
                    box.SetTitle("ERROR");
                    box.SetMessage("There is already a customer with the same name!");
                    box.SetPositiveButton("OK", delegate { });
                    box.Show();
                    return;
                }
                Cl_Manager.ExeNonQuery(
                    "UPDATE Clients SET " +
                    "Name = @Name, " +
                    "Telephone = @Telephone, " +
                    "Update_Info = @Update_Info "+
                    "WHERE Id_Client = @Id_Client", lparameters);
                
                Intent i = this.Intent;
                SetResult(Result.Ok, i);
                Finish();
            }
        }
    }
}