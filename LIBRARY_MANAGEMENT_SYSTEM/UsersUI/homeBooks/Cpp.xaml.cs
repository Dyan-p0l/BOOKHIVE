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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LIBRARY_MANAGEMENT_SYSTEM.UsersUI
{
    /// <summary>
    /// Interaction logic for Cpp.xaml
    /// </summary>
    public partial class Cpp : Page
    {
        Frame _parentFrame;
        private string currentUsername = dataStore.CurrentUsername;
        public Cpp(Frame parentFrame)
        {
            InitializeComponent();
            _parentFrame = parentFrame;
        }

        private void backBtn(object sender, RoutedEventArgs e)
        {
            _parentFrame.Navigate(new UserHomepage(_parentFrame));
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string title = "C++ Programming";
            string bookIdText = "80678";  
            DateTime now = DateTime.Now.Date;
            DateTime dueDate = now.AddDays(7);

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(bookIdText))
            {
                MessageBox.Show("Please enter the book title and book ID.");
                return;
            }

            if (!int.TryParse(bookIdText, out int bookId))
            {
                MessageBox.Show("Book ID must be a valid number.");
                return;
            }

            dbConnection db = new dbConnection();
            using (SqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();

                    // INSERT INTO AdminLogs
                    string insertAdminLog = @"INSERT INTO AdminLogs (Username, Action, Title, BookID, DateAdded, Status)
                              VALUES (@Username, @Action, @Title, @BookID, @DateAdded, @Status)";

                    using (SqlCommand cmd = new SqlCommand(insertAdminLog, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", currentUsername);
                        cmd.Parameters.AddWithValue("@Action", "Borrow");
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@BookID", bookId); // <-- new param
                        cmd.Parameters.AddWithValue("@DateAdded", now);
                        cmd.Parameters.AddWithValue("@Status", "Pending");
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Borrow request submitted successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }


    }
}
