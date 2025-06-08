using LIBRARY_MANAGEMENT_SYSTEM.backend;
using Microsoft.Data.SqlClient;
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

namespace LIBRARY_MANAGEMENT_SYSTEM.UsersUI
{
    /// <summary>
    /// Interaction logic for UserAccount.xaml
    /// </summary>
    public partial class UserAccount : Page
    {
        private string currentUsername = dataStore.CurrentUsername;
        private int currentUserID;
        private UserDashboard _dashboard;

        public UserAccount(UserDashboard dashboard)
        {
            InitializeComponent();
            _dashboard = dashboard;
            LoadUserInfo();
        }
        

        private void LoadUserInfo()
        {
            try
            {
                dbConnection db = new dbConnection();
                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM Users WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", currentUsername);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                currentUserID = Convert.ToInt32(reader["UserID"]);
                                string fullName = reader["FullName"].ToString();
                                string username = reader["Username"].ToString();
                                string password = reader["Password"].ToString();

                                lblFullname.Text = fullName.ToUpper();
                                lblUserID.Text = $"UID: {currentUserID}";

                                txtFullname.Text = fullName;
                                txtUsername.Text = username;
                                txtPassword.Password = password;
                                txtconfPass.Password = password;
                            }
                            else
                            {
                                MessageBox.Show("User not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load user data: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string newFullName = txtFullname.Text.Trim();
            string newUsername = txtUsername.Text.Trim();
            string newPassword = txtPassword.Password.Trim();
            string confirmPass = txtconfPass.Password.Trim();

            if (string.IsNullOrWhiteSpace(newFullName) || string.IsNullOrWhiteSpace(newUsername))
            {
                MessageBox.Show("Full name and username cannot be empty.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (newPassword != confirmPass)
            {
                MessageBox.Show("Passwords do not match.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                dbConnection db = new dbConnection();
                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    // Store the old username to update AdminLogs
                    string oldUsername = dataStore.CurrentUsername;

                    // Update Users table
                    string updateUserQuery = @"
                UPDATE Users 
                SET FullName = @FullName, Username = @Username, Password = @Password 
                WHERE UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(updateUserQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@FullName", newFullName);
                        cmd.Parameters.AddWithValue("@Username", newUsername);
                        cmd.Parameters.AddWithValue("@Password", newPassword);
                        cmd.Parameters.AddWithValue("@UserID", currentUserID);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            // Update AdminLogs table to reflect the new username
                            string updateLogsQuery = @"
                        UPDATE AdminLogs
                        SET Username = @NewUsername
                        WHERE Username = @OldUsername";

                            using (SqlCommand logCmd = new SqlCommand(updateLogsQuery, conn))
                            {
                                logCmd.Parameters.AddWithValue("@NewUsername", newUsername);
                                logCmd.Parameters.AddWithValue("@OldUsername", oldUsername);
                                logCmd.ExecuteNonQuery();
                            }

                            if (rows > 0)
                            {
                                // Update AdminLogs table to reflect the new username
                                string updateFaveQuery = @"
                        UPDATE Favorites
                        SET UserName = @NewUsername
                        WHERE UserName = @OldUsername";

                                using (SqlCommand logCmd = new SqlCommand(updateFaveQuery, conn))
                                {
                                    logCmd.Parameters.AddWithValue("@NewUsername", newUsername);
                                    logCmd.Parameters.AddWithValue("@OldUsername", oldUsername);
                                    logCmd.ExecuteNonQuery();
                                }

                                // Update current session username
                                dataStore.CurrentUsername = newUsername;
                                _dashboard.UpdateWelcomeMessage(newUsername);
                                lblFullname.Text = newFullName.ToUpper();

                                MessageBox.Show("Account updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                MessageBox.Show("Update failed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating account: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            LoadUserInfo(); // reload original values
        }
    }
}
