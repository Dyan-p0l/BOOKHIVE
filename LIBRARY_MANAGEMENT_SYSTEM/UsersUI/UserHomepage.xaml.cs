using Azure;
using LIBRARY_MANAGEMENT_SYSTEM.AdminUI.adminActions;
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

    public partial class UserHomepage : Page
    {

        private Frame _parentFrame;
        private string currentUsername = dataStore.CurrentUsername;
        public List<Notify> notifyList { get; set; } = new List<Notify>();

        public UserHomepage(Frame parentFrame)
        {            
            InitializeComponent();
            _parentFrame = parentFrame;
            LoadUserNotif();
            this.DataContext = this;
        }

        private void NotificationIcon_Click(object sender, MouseButtonEventArgs e)
        {
            // Toggle visibility
            NotificationPanel.Visibility = NotificationPanel.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;

            // Prevent the event from bubbling further
            e.Handled = true;
        }

        private void LoadUserNotif()
        {
            try
            {
                dbConnection db = new dbConnection();
                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT Title, Action, Status FROM AdminLogs WHERE Username = @Username AND Status != 'Pending' ORDER BY DateAdded DESC";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", currentUsername);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                notifyList.Add(new Notify
                                {
                                    title = reader["Title"].ToString(),
                                    action = reader["Action"].ToString(),   
                                    status = reader["Status"].ToString()
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


        private void showAot1(object sender, MouseButtonEventArgs e)
        {
            _parentFrame.Navigate(new Aot(_parentFrame));
        }

        private void showControlsys(object sender, MouseButtonEventArgs e)
        {
            _parentFrame.Navigate(new homeBooks.Controlsys(_parentFrame));
        }

        private void showPotter1(object sender, MouseButtonEventArgs e)
        {
            _parentFrame.Navigate(new Potter1(_parentFrame));
        }

        private void showCsharp(object sender, MouseButtonEventArgs e)
        {
            _parentFrame.Navigate(new Csharp(_parentFrame));
        }

        private void showAot2(object sender, MouseButtonEventArgs e)
        {
            _parentFrame.Navigate(new homeBooks.Aot2(_parentFrame));
        }

        private void showPotter2(object sender, MouseButtonEventArgs e)
        {
            _parentFrame.Navigate(new homeBooks.Potter2(_parentFrame));
        }
        
        private void showCpp(object sender, MouseButtonEventArgs e)
        {
            _parentFrame.Navigate(new Cpp(_parentFrame));
        }
       
        private void showJava(object sender, MouseButtonEventArgs e)
        {
            _parentFrame.Navigate(new Java(_parentFrame));
        }
            
    }

    public class Notify
    {
        public string title { get; set; }
        public string action { get; set; }
        public string status { get; set; }


        public string Message
        {
            get
            {
                string shortTitle = title.Length > 13 ? title.Substring(0, 12) + "..." : title;

                return status.ToLower() switch
                {
                    "approved" => $"\"{action}\" Request: \"{shortTitle}\" APPROVED!",
                    "declined" => $"\"{action}\" Request: \"{shortTitle}\" DECLINED!",
                    _ => $"You have an action on the book titled \"{shortTitle}\"."
                };
            }
        }


    }


}
