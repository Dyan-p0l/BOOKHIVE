using System;
using Microsoft.Data.SqlClient;
using System.Windows;
using LIBRARY_MANAGEMENT_SYSTEM.backend;

namespace LIBRARY_MANAGEMENT_SYSTEM.UsersUI.actions
{
    public partial class BookReturn : Window
    {
        private string currentUsername = dataStore.CurrentUsername;

        public BookReturn()
        {
            InitializeComponent();
        }

        private void backBtn(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string title = txtBookTitle.Text.Trim();
            string bookIdText = txtBookID.Text.Trim();
            string author = txtBookAuthor.Text.Trim();
            DateTime now = DateTime.Now.Date;

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

                    // AdminLogs Insert
                    string insertAdminLog = @"INSERT INTO AdminLogs (Username, Action, Title, BookID, DateAdded, Status)
                                              VALUES (@Username, @Action, @Title, @BookID, @DateAdded, @Status)";

                    using (SqlCommand cmd = new SqlCommand(insertAdminLog, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", currentUsername);
                        cmd.Parameters.AddWithValue("@Action", "Return");
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@BookID", bookId);
                        cmd.Parameters.AddWithValue("@DateAdded", now);
                        cmd.Parameters.AddWithValue("@Status", "Pending");
                        cmd.ExecuteNonQuery();
                    }

                    // Users-Actions Insert
                    string insertUserAction = @"INSERT INTO [Users-Actions] (Username, Action, Date, Status)
                                                VALUES (@Username, @Action, @Date, @Status)";

                    using (SqlCommand cmd = new SqlCommand(insertUserAction, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", currentUsername);
                        cmd.Parameters.AddWithValue("@Action", "Return");
                        cmd.Parameters.AddWithValue("@Date", now);
                        cmd.Parameters.AddWithValue("@Status", "Pending");
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Return request submitted successfully.");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}
