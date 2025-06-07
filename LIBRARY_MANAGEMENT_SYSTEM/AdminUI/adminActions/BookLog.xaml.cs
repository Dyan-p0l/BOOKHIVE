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
using static System.Reflection.Metadata.BlobBuilder;

namespace LIBRARY_MANAGEMENT_SYSTEM.AdminUI.adminActions
{
    /// <summary>
    /// Interaction logic for BookLog.xaml
    /// </summary>
    public partial class BookLog : Window
    {

        public List<BookLogs> BookLogsList { get; set; } = new List<BookLogs>();

        public BookLog()
        {
            InitializeComponent();
            LoadBookLog();
            this.DataContext = this;
        }

        private void backBtn(object sender, EventArgs e)
        {
            this.Close();
        }

        private void approveClick(object sender, EventArgs e)
        {
            DateTime currentDateTime = DateTime.Now;
            if (sender is Button btn && btn.Tag is BookLogs log)
            {
                try
                {
                    dbConnection db = new dbConnection();
                    using (SqlConnection connection = db.GetConnection())
                    {
                        string updateQuery = "UPDATE AdminLogs SET Status = 'Approved' WHERE BookID = @BookID AND Action = @Action AND Username = @Username AND Status = 'Pending'";
                        SqlCommand cmd = new SqlCommand(updateQuery, connection);
                        cmd.Parameters.AddWithValue("@BookID", log.bookID);
                        cmd.Parameters.AddWithValue("@Action", log.action);
                        cmd.Parameters.AddWithValue("@Username", log.username);

                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            BookLogsList.Remove(log);
                            // Refresh the UI
                            BookLogsList = new List<BookLogs>(BookLogsList);
                            DataContext = null;
                            DataContext = this;
                        }
                        string Action = $"APPROVED A {log.Message}";
                        string actionQuery = "INSERT INTO AdminActions (actions, date) VALUES (@Action, @Date)";
                        using (SqlCommand insertCmd = new SqlCommand(actionQuery, connection))
                        {
                            insertCmd.Parameters.AddWithValue("@Action", Action);
                            insertCmd.Parameters.AddWithValue("@Date", currentDateTime);
                            int result = insertCmd.ExecuteNonQuery();
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error approving request: " + ex.Message);
                }
            }
        }

        private void declineClick(object sender, EventArgs e)
        {
            DateTime currentDateTime = DateTime.Now;
            if (sender is Button btn && btn.Tag is BookLogs log)
            {
                try
                {
                    dbConnection db = new dbConnection();
                    using (SqlConnection connection = db.GetConnection())
                    {
                        string updateQuery = "UPDATE AdminLogs SET Status = 'Declined' WHERE BookID = @BookID AND Action = @Action AND Username = @Username AND Status = 'Pending'";
                        SqlCommand cmd = new SqlCommand(updateQuery, connection);
                        cmd.Parameters.AddWithValue("@BookID", log.bookID);
                        cmd.Parameters.AddWithValue("@Action", log.action);
                        cmd.Parameters.AddWithValue("@Username", log.username);

                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            BookLogsList.Remove(log);
                            // Refresh the UI
                            BookLogsList = new List<BookLogs>(BookLogsList);
                            DataContext = null;
                            DataContext = this;
                        }
                        string Action = $"DECLINED A {log.Message}";
                        string actionQuery = "INSERT INTO AdminActions (actions, date) VALUES (@Action, @Date)";
                        using (SqlCommand insertCmd = new SqlCommand(actionQuery, connection))
                        {
                            insertCmd.Parameters.AddWithValue("@Action", Action);
                            insertCmd.Parameters.AddWithValue("@Date", currentDateTime);
                            int result = insertCmd.ExecuteNonQuery();
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error approving request: " + ex.Message);
                }
            }
        }

        private void LoadBookLog()
        {
            try
            {
                dbConnection db = new dbConnection();
                using (SqlConnection connection = db.GetConnection())
                {
                    string query = "SELECT Username, Action, Title, BookID, DateAdded FROM AdminLogs WHERE Status = 'Pending'";
                    SqlCommand cmd = new SqlCommand(query, connection);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        BookLogsList.Add(new BookLogs
                        {
                            username = reader["Username"].ToString(),
                            action = reader["Action"].ToString(),
                            title = reader["Title"].ToString(),
                            bookID = Convert.ToInt32(reader["BookID"]),
                            dateAdded = Convert.ToDateTime(reader["DateAdded"])
                        });
                    }

                    reader.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user logs: " + ex.Message);
            }
        }

    }

    public class BookLogs
    {
       public string username { get; set; }
       public string action { get; set; }
       public string title { get; set; }
       public int bookID { get; set; }
       public DateTime dateAdded { get; set; }
       public string Message =>
             action.ToLower() switch
             {
                 "borrow" => $"Borrow Request: \"{username}\" → \"{title}\"[\"{bookID}\"]",
                 "return" => $"Return Request: \"{username}\" → \"{title}\"[\"{bookID}\"]",
                 "extend" => $"Extend Request: \"{username}\" → \"{title}\"[\"{bookID}\"]",
                 _ => $"You have an action on the book titled \"{title}\"."
                };
    }

    
}
