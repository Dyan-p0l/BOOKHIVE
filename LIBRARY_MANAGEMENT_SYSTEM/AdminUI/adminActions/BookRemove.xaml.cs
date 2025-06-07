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

namespace LIBRARY_MANAGEMENT_SYSTEM.AdminUI.adminActions
{
    /// <summary>
    /// Interaction logic for BookRemove.xaml
    /// </summary>
    public partial class BookRemove : Window
    {
        public BookRemove()
        {
            InitializeComponent();
        }

        private void backBtn(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deleteBook(object sender, EventArgs e)
        {
            string title = txtTitle.Text.Trim();
            string author = txtAuthor.Text.Trim();
            string id = txtId.Text.Trim();
            string Action = "";
            DateTime currentDateTime = DateTime.Now;

            if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(author) && string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("All fields are required. Please fill in the form.");
                return;
            }

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author) || string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("Please complete all fields.");
                return;
            }

            try
            {
                dbConnection db = new dbConnection();

                using (SqlConnection connection = db.GetConnection())
                {
                    connection.Open();
<<<<<<< HEAD
                        
=======

>>>>>>> 73eccd690b166c17be51c0de25c2151b239f76c6
                    string checkQuery = "SELECT COUNT(*) FROM Books WHERE BookID = @IDNum";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@IDNum", id);
                        int bookExists = (int)checkCmd.ExecuteScalar();

                        if (bookExists == 0)
                        {
                            MessageBox.Show("No book found with that ID.", "Delete Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    string deleteQuery = "DELETE FROM Books WHERE BookID = @IDNum";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, connection))
                    {
                        deleteCmd.Parameters.AddWithValue("@IDNum", id);
                        int result = deleteCmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Book removed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            txtId.Clear();
                            txtAuthor.Clear();
                            txtTitle.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete the book. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    
                    Action = $"Book DELETED: {title}, with ID:[{id}]";

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
                MessageBox.Show($"An error occurred:\n{ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
