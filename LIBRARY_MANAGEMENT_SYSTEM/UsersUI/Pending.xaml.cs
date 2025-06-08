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
    /// Interaction logic for Pending.xaml
    /// </summary>
    public partial class Pending : Window
    {

        public List<BookLogs> bookLogs { get; set; } = new List<BookLogs>();
        private string currentUsername = dataStore.CurrentUsername;

        public Pending()
        {
            InitializeComponent();
            LoadBookLog();
            this.DataContext = this;
        }
        private void backBtn(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadBookLog()
        {
            try
            {
                dbConnection db = new dbConnection();
                using (SqlConnection connection = db.GetConnection())
                {
                    string query = "SELECT Action, Title, BookID, DateAdded FROM AdminLogs WHERE Username = @Username AND Status = 'Pending'" ;
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Username", currentUsername);
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        bookLogs.Add(new BookLogs
                        {
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
        public string action { get; set; }
        public string title { get; set; }
        public int bookID { get; set; }
        public DateTime dateAdded { get; set; }
        public string Message =>
              action.ToLower() switch
              {
                  "borrow" => $"Borrow Request: \"{title}\"[\"{bookID}\"]",
                  "return" => $"Return Request: \"{title}\"[\"{bookID}\"]",
                  "extend" => $"Extend Request: \"{title}\"[\"{bookID}\"]",
                  _ => $"You have an action on the book titled \"{title}\"."
              };
    }
}
