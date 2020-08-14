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
    [Activity(Label = "Act_Products")]
    public class Act_Products : Activity
    {
        Button Cmd_add_Products;
        ListView List_Products;
        TextView Number_Products;
        List<Cl_Products> PRODUCTS;
        List<string> NAMES;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Layout_Products);

            Cmd_add_Products = FindViewById<Button>(Resource.Id.cmd_add_Products);
            List_Products = FindViewById<ListView>(Resource.Id.list_Products);
            Number_Products = FindViewById<TextView>(Resource.Id.label_number_Products);


            BuildsProductsList();

            Cmd_add_Products.Click += Cmd_add_Products_Click;

            List_Products.ItemLongClick += List_Products_ItemLongClick;
        }

       
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if(resultCode == Result.Ok)
            {
                BuildsProductsList();
            }
        }



        private void List_Products_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            //select product
            Cl_Products products_select = PRODUCTS[e.Position];

            AlertDialog.Builder Box_Edit_Delete = new AlertDialog.Builder(this);
            Box_Edit_Delete.SetTitle("EDIT | DELETE");
            Box_Edit_Delete.SetMessage(products_select.Name_Product);
            

            Box_Edit_Delete.SetPositiveButton("Edit", delegate { EditProduct(products_select.Id_Products); });
            Box_Edit_Delete.SetNegativeButton("Delete", delegate { DeleteProduct(products_select.Id_Products); });

            Box_Edit_Delete.Show();


        }
        private void DeleteProduct(int id_product)
        {

            Cl_Manager.ExeNonQuery("DELETE FROM Products WHERE Id_Product = " + id_product);
            BuildsProductsList();

        }

        private void EditProduct(int id_product)
        {
            Intent i = new Intent(this, typeof(Act_Products_Edit));
            i.PutExtra("Id_Product", id_product.ToString());
            StartActivityForResult(i, 0);

        }
        private void Cmd_add_Products_Click(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(Act_Products_Edit));
            StartActivityForResult(i, 0);
        }






        private void BuildsProductsList()
        {
            DataTable data = Cl_Manager.ExeQuery("SELECT * FROM Products ORDER BY Name_product ASC");
            PRODUCTS = new List<Cl_Products>();

            foreach (DataRow line in data.Rows)
            {
                PRODUCTS.Add(new Cl_Products()
                {
                   Id_Products = Convert.ToInt16(line["Id_Product"]),
                    Name_Product = line["Name_product"].ToString(),
                    Price = Convert.ToInt32(line["Price"].ToString())
                });

            }

            NAMES = new List<string>();
            foreach (Cl_Products pr in PRODUCTS)
            {
                NAMES.Add(pr.Name_Product);
            }

            ArrayAdapter<string> AdapTer = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, NAMES);

            List_Products.Adapter = AdapTer;

            PresentTotalProducts(PRODUCTS.Count);

        }
        private void PresentTotalProducts(int total_products)
        {

            Number_Products.Text = "Registered customers: " + total_products;
        }
    }
}