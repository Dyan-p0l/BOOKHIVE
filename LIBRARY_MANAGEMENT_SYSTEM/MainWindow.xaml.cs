using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;
using LIBRARY_MANAGEMENT_SYSTEM.backend;

namespace LIBRARY_MANAGEMENT_SYSTEM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void openReg_Click(object sender, RoutedEventArgs e)
        {
            Registration reg = new Registration();
            reg.Show();
            this.Close();
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            dbConnection db = new dbConnection();
            using (SqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();

                    string query = "SELECT UserType FROM Users WHERE Username = @username AND Password = @password";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        object userTypeObj = cmd.ExecuteScalar();

                        if (userTypeObj != null)
                        {
                            string? userType = userTypeObj?.ToString();

                            if (!string.IsNullOrEmpty(userType))
                            {
                                if (userType == "Admin")
                                {
                                    MessageBox.Show("Welcome Admin!");
                                    AdminDashboard adminDashboard = new AdminDashboard();
                                    adminDashboard.Show();
                                    this.Close();
                                }
                                else if (userType == "User")
                                {
                                    MessageBox.Show("Welcome " + username + "!");
                                    UserDashboard userDashboard = new UserDashboard();
                                    userDashboard.Show();
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Unrecognized user type.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid username or password.");
                            }

                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Database error: " + ex.Message);
                }
            }
        }

    }
}