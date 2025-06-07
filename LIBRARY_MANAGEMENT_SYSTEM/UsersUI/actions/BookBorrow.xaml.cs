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

namespace LIBRARY_MANAGEMENT_SYSTEM.UsersUI.actions
{
    /// <summary>
    /// Interaction logic for BookBorrow.xaml
    /// </summary>
    public partial class BookBorrow : Window
    {
        private string currentUsername = dataStore.CurrentUsername;
        public BookBorrow()
        {
            InitializeComponent();

        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string title = txtBookTitle.Text.Trim();
            string bookIdText = txtBookID.Text.Trim();  // From your TextBox
            string author = txtBookAuthor.Text.Trim();
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

                    // INSERT INTO Users-Actions
                    string insertUserAction = @"INSERT INTO [Users-Actions] (Username, Action, Date, Status)
                                        VALUES (@Username, @Action, @Date, @Status)";

                    using (SqlCommand cmd = new SqlCommand(insertUserAction, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", currentUsername);
                        cmd.Parameters.AddWithValue("@Action", "Borrow");
                        cmd.Parameters.AddWithValue("@Date", now);
                        cmd.Parameters.AddWithValue("@Status", "Pending");
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Borrow request submitted successfully.");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }


        private void backBtn(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
