using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CustomerApp.Models;

namespace CustomerApp
{
    public partial class CustomerEntryForm : Form
    {
        public CustomerEntryForm()
        {
            InitializeComponent();
        }
        static string connectString = @"server=DESKTOP-83QIQD0\SQLEXPRESS; database=UMS5112; integrated security=true";

        SqlConnection con = new SqlConnection(connectString);

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                Customer customer = new Customer();
                customer.Name = nameTextBox.Text;
                customer.ContactNo = contactTextBox.Text;
                customer.District = numberTextBox.Text;
                customer.SubDistrict = addressTextBox.Text;
                bool isAdded = Add(customer);
                if (isAdded)
                {
                    nameTextBox.Text    = String.Empty;
                    contactTextBox.Text = String.Empty;
                    numberTextBox.Text  = String.Empty;
                    addressTextBox.Text = String.Empty;
                    MessageBox.Show("Data inserted Successfully");
                }
                else
                {
                    MessageBox.Show("Failed");
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private bool Add(Customer customer)
        {
           

            string query = @"INSERT INTO Customers VALUES('" + customer.Name + "','" + customer.ContactNo + "','" + customer.District + "','" + customer.SubDistrict + "')";

            SqlCommand command = new SqlCommand(query,con);

            con.Open();
            bool isAdded = command.ExecuteNonQuery() > 0;
            
            con.Close();
            return isAdded;

        }

       

        private void SearchButton_Click(object sender, EventArgs e)
        {
            string name = searchTextBox.Text;
            if (name == "")
            {
                MessageBox.Show("please input a name");
                return;
            }
            else
            {
                Customer customer = GetCustomer(name);
                nameTextBox.Text = customer.Name;
                contactTextBox.Text = customer.ContactNo;
                numberTextBox.Text = customer.District;
                addressTextBox.Text = customer.SubDistrict;
            }
           
            

        }

        private Customer GetCustomer(string name)
        {

            Customer customer = new Customer();
            string query = @"SELECT * FROM Customers WHERE Name='" + name + "'";
            SqlCommand command = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = command.ExecuteReader();
            if (dr.Read())
            {
                customer.Name = dr["Name"].ToString();
                customer.ContactNo = dr["ContactNo"].ToString();
                customer.District = dr["District"].ToString();
                customer.SubDistrict = dr["SubDistrict"].ToString();
            }
            else
            {
                MessageBox.Show("Customer not found");
            }
            return customer;
        }
    }
}
