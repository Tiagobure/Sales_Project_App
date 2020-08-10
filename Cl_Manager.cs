using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mono.Data.Sqlite;

namespace FruitSales
{
    public static class Cl_Manager
    {
        static string Data_Folder;
        static string Data_Base;

        public static void MainApplication()
        {
            Data_Folder = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "SALES");
            if (!Directory.Exists(Data_Folder))
            {
                Directory.CreateDirectory(Data_Folder);
            }

            Data_Base = Path.Combine(Data_Folder + @"/data.db");

            if (!File.Exists(Data_Base))
            {
                //File
                SqliteConnection.CreateFile(Data_Base);

                SqliteConnection LinkBD = new SqliteConnection("Data source = " + Data_Base);
                LinkBD.Open();

                SqliteCommand command = new SqliteCommand();
                command.Connection = LinkBD;

                //created client
                command.CommandText =
                    "CREATE TABLE Clients (" +
                    "Id_Client              INTEGER NOT NULL PRIMARY KEY, " +
                    "Name                   NVARCHAR(50), " +
                    "Telephone              NVARCHAR(20), " +
                    "Update_Info            DATETIME)";
                command.ExecuteNonQuery();

                //created product
                command.CommandText =
                    "CREATE TABLE Products (" +
                    "Id_Product             INTEGER NOT NULL PRIMARY KEY, " +
                    "Name_product           NVARCHAR(30), " +
                    "Price                  INTEGER, " +
                    "Update_Info_Product    DATETIME)";
                command.ExecuteNonQuery();

                //created sale
                command.CommandText =
                    "CREATE TABLE Sales (" +
                    "Id_Sales             INTEGER NOT NULL PRIMARY KEY, " +
                    "Id_client            INTEGER, " +
                    "Id_Product           INTEGER, " +
                    "Amount               INTEGER, " +
                    "Update_Info_Sales    DATETIME, " +
                    "FOREIGN KEY(Id_Client) REFERENCES Clients(Id_client) ON DELETE CASCADE, " +
                    "FOREIGN KEY(Id_Product) REFERENCES Products(Id_Product) ON DELETE CASCADE)";
                command.ExecuteNonQuery();

                LinkBD.Close();
                LinkBD.Dispose();
            }
        }


        public static void ExeNonQuery(string query, List<SQLparameters> parametersA)
        {
            //performs insertion, update or  deletion
            SqliteConnection connection = new SqliteConnection("Data source = " + Data_Base);
            connection.Open();


            SqliteCommand command = new SqliteCommand(query, connection);


            foreach (SQLparameters Pmeters in parametersA)
            {
                command.Parameters.Add(new SqliteParameter(Pmeters.Name, Pmeters.ValueA));
            }

            //communication with the database
            command.ExecuteNonQuery();

            command.Dispose();


            connection.Close();
            connection.Dispose();
        }
        public static void ExeNonQuery(string query)
        {
            //performs insertion, update or  deletion
            SqliteConnection connection = new SqliteConnection("Data source = " + Data_Base);
            connection.Open();


            SqliteCommand command = new SqliteCommand(query, connection);
                     
            //communication with the database
            command.ExecuteNonQuery();

            command.Dispose();


            connection.Close();
            connection.Dispose();
        }

        public static DataTable ExeQuery(string query, List<SQLparameters> parametersB)
        {
            //reading data(SELECT)
            SqliteConnection connection = new SqliteConnection("Data source = " + Data_Base);
            connection.Open();

            SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection);


            foreach (SQLparameters Pmeters in parametersB)
            {

                adapter.SelectCommand.Parameters.Add(new SqliteParameter(Pmeters.Name, Pmeters.ValueA));
            }

            DataTable data = new DataTable();

            adapter.Fill(data);

            return data;
        }
        public static DataTable ExeQuery(string query)
        {
            //reading data(SELECT)
            SqliteConnection connection = new SqliteConnection("Data source = " + Data_Base);
            connection.Open();

            SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection);
                     

            DataTable data = new DataTable();

            adapter.Fill(data);

            return data;
        }

        public static int ID_AVAILABLE(string table, string column)
        {
            //return available id
            int ValueId = 0;
            SqliteConnection connection = new SqliteConnection("Data source = " + Data_Base);
            connection.Open();

            string query = "SELECT MAX(" + column + ") AS MaxID FROM " + table; 

            DataTable data = new DataTable();
            SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection);
            adapter.Fill(data);

            if(data.Rows.Count != 0)
            {
                //check that the result is not null
                if (!DBNull.Value.Equals(data.Rows[0][0]))
                {
                    ValueId = Convert.ToInt32(data.Rows[0][0]) + 1;
                }
            }
            connection.Close();
            connection.Dispose();

            return ValueId;

        }

        //test
        public static void CLEAN_TABLE(string table)
        {
            ExeNonQuery("DELETE FROM " + table);
        }
    }
}