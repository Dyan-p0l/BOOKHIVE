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
            string fullname = txtFullname.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;
            string confpassword = txtconfPassword.Password;
            string userType = "User";

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

                    // Check for existing username
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

                    // Insert into Users and retrieve inserted UserID
                    string insertQuery = @"
                INSERT INTO Users (FullName, Username, Password, UserType)
                VALUES (@Fullname, @Username, @Password, @UserType);
                SELECT SCOPE_IDENTITY();";

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@Fullname", fullname);
                        insertCmd.Parameters.AddWithValue("@Username", username);
                        insertCmd.Parameters.AddWithValue("@Password", password); // Consider hashing in production!
                        insertCmd.Parameters.AddWithValue("@UserType", userType);

                        // Execute and get the new UserID
                        object result = insertCmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int newUserId))
                        {
                            // Insert into AdminManageAccs
                            string adminInsertQuery = "INSERT INTO AdminManageAccs (UserID, FullName, Username, BorrowedBooks) VALUES (@UserID, @FullName, @Username, 0)";
                            using (SqlCommand adminCmd = new SqlCommand(adminInsertQuery, connection))
                            {
                                adminCmd.Parameters.AddWithValue("@UserID", newUserId);
                                adminCmd.Parameters.AddWithValue("@FullName", fullname);
                                adminCmd.Parameters.AddWithValue("@Username", username);
                                adminCmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            MainWindow main = new MainWindow();
                            main.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Registration failed. Could not retrieve user ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
