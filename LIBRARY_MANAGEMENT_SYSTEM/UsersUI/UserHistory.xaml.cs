using LIBRARY_MANAGEMENT_SYSTEM.backend;
using MahApps.Metro.IconPacks;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public partial class UserHistory : Page
    {
        public ObservableCollection<UserNotification> Notifications { get; set; } = new ObservableCollection<UserNotification>();
        private string currentUsername = dataStore.CurrentUsername;

        public UserHistory()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUserLogs();
        }

        private void LoadUserLogs()
        {
            try
            {
                dbConnection db = new dbConnection();
                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT Title, DateAdded, Action FROM AdminLogs WHERE Username = @Username ORDER BY DateAdded DESC";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", currentUsername);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Notifications.Add(new UserNotification
                                {
                                    Title = reader["Title"].ToString(),
                                    DateAdded = Convert.ToDateTime(reader["DateAdded"]),
                                    Action = reader["Action"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user logs: " + ex.Message);
            }
        }
    }
}
