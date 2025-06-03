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
using LIBRARY_MANAGEMENT_SYSTEM.backend;
using Microsoft.Data.SqlClient;

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
            string password = txtPassword.Password;
            string confpassword = txtconfPassword.Password;
            string userType = "User";

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

            try
            {
                dbConnection db = new dbConnection();
                using (SqlConnection connection = db.GetConnection())
                {
                    connection.Open();

                    // Check if username already exists
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@Username", username);
                        int userExists = (int)checkCmd.ExecuteScalar();
                        if (userExists > 0)
                        {
                            MessageBox.Show("Username already exists. Please choose another one.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    // Insert new user
                    string insertQuery = "INSERT INTO Users (FullName, Username, Password, UserType) VALUES (@Fullname, @Username, @Password, @UserType)";
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@Fullname", fullname);
                        insertCmd.Parameters.AddWithValue("@Username", username);
                        insertCmd.Parameters.AddWithValue("@Password", password); // For production, hash passwords!
                        insertCmd.Parameters.AddWithValue("@UserType", userType);
                        int result = insertCmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            MainWindow login = new MainWindow();
                            login.Show();
                            this.Close();
                            MainWindow main = new MainWindow();
                            main.Show();
                            this.Close(); // or redirect to login screen
                        }
                        else
                        {
                            MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred:\n{ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
