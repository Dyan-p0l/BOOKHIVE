using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace LIBRARY_MANAGEMENT_SYSTEM
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            // Inputs
            string fullname = txtFullname.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confpassword = txtconfPassword.Text;

            // Check if all fields are empty
            if (string.IsNullOrWhiteSpace(fullname) &&
                string.IsNullOrWhiteSpace(username) &&
                string.IsNullOrWhiteSpace(password) &&
                string.IsNullOrWhiteSpace(confpassword))
            {
                MessageBox.Show("All fields are required. Please fill in the form.");
                return;
            }

            
            if (string.IsNullOrWhiteSpace(fullname) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confpassword))
            {
                MessageBox.Show("Please complete all fields.");
                return;
            }

            if (confpassword != password)
            {
                MessageBox.Show("Passwords do not match. Please try again!");
                return;
            }

            string connectionString = @"Data Source=Jaydee\SQLEXPRESS;Initial Catalog=bookhiveDB;Integrated Security=True;TrustServerCertificate=True;";
            string query = "INSERT INTO Users (Fullname, Username, Password) VALUES (@Fullname, @Username, @Password)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Fullname", fullname);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password); 

                    connection.Open();
                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Registration successful!");
                    }
                    else
                    {
                        MessageBox.Show("Registration failed.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


    }
}
