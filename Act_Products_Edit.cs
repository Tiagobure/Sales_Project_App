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
    [Activity(Label = "Act_Products_Edit")]
    public class Act_Products_Edit : Activity
    {

        Button Cmd_Record_Product;
        Button Cmd_Cancel_Product;
        EditText Name_Product;
        EditText Price_Product;

        Cl_Products Products;
        int Id_Product = 0;
        bool Edit = false;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(savedInstanceState);

           


            // Create your application here
            SetContentView(Resource.Layout.layout_edit_Product);

                Cmd_Record_Product = FindViewById<Button>(Resource.Id.cmd_Product_record);
            Cmd_Cancel_Product = FindViewById<Button>(Resource.Id.cmd_Product_cancel);
            Name_Product = FindViewById<EditText>(Resource.Id.edit_name_Product);
            Price_Product = FindViewById<EditText>(Resource.Id.edit_Price_Unit);


            Cmd_Cancel_Product.Click += delegate { this.Finish(); };
            Cmd_Record_Product.Click += Cmd_Record_Product_Click;

            //edit Products or no
            if (this.Intent.GetStringExtra("Id_Product") != null)
            {
                Id_Product = int.Parse(this.Intent.GetStringExtra("Id_Product"));
                PresentProduct();
                Edit = true;
            }
        }
        private void PresentProduct()
        {
            DataTable data = Cl_Manager.ExeQuery("SELECT * FROM Products WHERE Id_Product = " + Id_Product);
            Name_Product.Text = data.Rows[0]["Name_product"].ToString();
            Price_Product.Text = data.Rows[0]["Price"].ToString();
        }
        private void Cmd_Record_Product_Click(object sender, EventArgs e)
        {
            if (Name_Product.Text == "" || Name_Product.Text == null || Price_Product.Text == "" || Price_Product.Text == null)
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
                lparameters.Add(new SQLparameters("@Id_Product", Cl_Manager.ID_AVAILABLE("Products", "Id_Product")));
            }
            else
            {
                lparameters.Add(new SQLparameters("@Id_Product", Id_Product));
            }
            lparameters.Add(new SQLparameters("@Name_product", Name_Product.Text));
            lparameters.Add(new SQLparameters("@Price", Price_Product.Text));
            lparameters.Add(new SQLparameters("@Update_Info_Product", DateTime.Now));

            //NEW Products
            if (!Edit)
            {
                DataTable data = Cl_Manager.ExeQuery("SELECT Name_product FROM Products WHERE Name_product = @Name_product", lparameters);
                if(data.Rows.Count != 0)
                {
                    AlertDialog.Builder box = new AlertDialog.Builder(this);
                    box.SetTitle("ERROR");
                    box.SetMessage("There is already a Product with the same name!");
                    box.SetPositiveButton("OK", delegate { });
                    box.Show();
                    return;
                }
                Cl_Manager.ExeNonQuery("INSERT INTO Products VALUES (" +
                   "@Id_Product, @Name_product, @Price, @Update_Info_Product)", lparameters);


                Intent i = this.Intent;
                SetResult(Result.Ok, i);
                Finish();
            }
            else
            {
                //EDIT Products
                DataTable data = Cl_Manager.ExeQuery("SELECT Name_product FROM Products WHERE Name_product = @Name_product AND Id_Product <> @Id_Product", lparameters);
                if (data.Rows.Count != 0)
                {
                    AlertDialog.Builder box = new AlertDialog.Builder(this);
                    box.SetTitle("ERROR");
                    box.SetMessage("There is already a Product with the same name!");
                    box.SetPositiveButton("OK", delegate { });
                    box.Show();
                    return;
                }
                Cl_Manager.ExeNonQuery(
                    "UPDATE Products SET " +
                    "Name_product = @Name_product, " +
                    "Price = @Price, " +
                    "Update_Info_Product = @Update_Info_Product " +
                    "WHERE Id_Product = @Id_Product", lparameters);
                
                Intent i = this.Intent;
                SetResult(Result.Ok, i);
                Finish();
            }
        }
    }
}