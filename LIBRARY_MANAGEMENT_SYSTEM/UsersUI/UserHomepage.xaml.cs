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
        public List<Books> bookList { get; set; } = new List<Books>();

        public UserHomepage(Frame parentFrame)
        {            
            InitializeComponent();
            _parentFrame = parentFrame;
            LoadUserNotif();
            this.DataContext = this;
        }

        private void NotificationIcon_Click(object sender, MouseButtonEventArgs e)
        {
            NotificationPanel.Visibility = NotificationPanel.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;

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
                            int count = 0;
                            while (reader.Read())
                            {
                                notifyList.Add(new Notify
                                {
                                    title = reader["Title"].ToString(),
                                    action = reader["Action"].ToString(),   
                                    status = reader["Status"].ToString()
                                });
                                count++;
                            }

                            notifCount.Text = count.ToString();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user logs: " + ex.Message);
            }
        }

        private void LoadBooksFromDatabase(string searchTerm = "")
        {
            dbConnection db = new dbConnection();
            List<Book> books = new List<Book>();
            bookList.Clear();

            using (SqlConnection connection = db.GetConnection())
            {
                
                string query = @"SELECT Title, BookID, Author FROM Books WHERE Title LIKE @search OR Author LIKE @search";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@search", "%" + searchTerm + "%");
                
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    bookList.Add(new Books
                    {
                        title = reader["Title"].ToString(),
                        bookID = Convert.ToInt32(reader["BookID"]),
                        author = reader["Author"].ToString()
                    });
                }

                reader.Close();
            }
            BookItemsControl.ItemsSource = null;
            BookItemsControl.ItemsSource = bookList;
        }

        private void searchBook(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearch.Text.Trim();
            LoadBooksFromDatabase(searchText);
            resultExander.IsExpanded = true;
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

    public class Books
    {
        public string title { get; set; }
        public string author { get; set; }
        public int bookID { get; set; }
    }


}
