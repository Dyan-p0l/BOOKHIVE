using LIBRARY_MANAGEMENT_SYSTEM.backend;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for BookAdd.xaml
    /// </summary>
    public partial class BookAdd : Window
    {
        public BookAdd()
        {
            InitializeComponent();
        }
        
        private void backBtn(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addBookClick(object sender, EventArgs e)
        {
            Random random = new Random();
            bool idExists = true;
            int idNumber = 0;
            string title = txtTitle.Text.Trim();
            string author = txtAuthor.Text.Trim();
            string Action = "";
            DateTime currentDateTime = DateTime.Now;

            var selectedGenres = genreListBox.SelectedItems
                .Cast<ListBoxItem>()
                .Select(item => item.Content.ToString())
                .ToList();
            string genre = string.Join(", ", selectedGenres);
            
            if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(author))
            {
                MessageBox.Show("All fields are required. Please fill in the form.");
                return;
            }
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author))
            {
                MessageBox.Show("Please complete all fields.");
                return;
            }

            try
            {
                dbConnection dbCon = new dbConnection();
                using (SqlConnection connection = dbCon.GetConnection())
                {
                    connection.Open();

                    while (idExists)
                    {
                        idNumber = random.Next(10000, 100000); 
                        string checkQuery = "SELECT COUNT(*) FROM Books WHERE BookID = @ID";
                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection))
                        {
                            checkCmd.Parameters.AddWithValue("@ID", idNumber);
                            int count = (int)checkCmd.ExecuteScalar();
                            idExists = (count > 0); 
                        }
                    }

<<<<<<< HEAD
                    string insertQuery = "INSERT INTO Books (Title, BookID, Author, Genre) VALUES (@Title, @IDNum, @Author, @Genre)";
=======
                    string insertQuery = "INSERT INTO Books (Title, BookID, Author, genre) VALUES (@Title, @IDNum, @Author, @Genre)";
>>>>>>> 73eccd690b166c17be51c0de25c2151b239f76c6
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@Title", title);
                        insertCmd.Parameters.AddWithValue("@IDNum", idNumber);
                        insertCmd.Parameters.AddWithValue("@Author", author); 
                        insertCmd.Parameters.AddWithValue("@Genre", genre);
                        int result = insertCmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Book Succesfully added with Book Id: " + idNumber + "!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            txtTitle.Clear();
                            txtAuthor.Clear();
                            genreListBox.SelectedItems.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Book Add failed. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    Action = $"Book Added: {title}, with ID:[{idNumber}]";

                    string actionQuery = "INSERT INTO AdminActions (actions, date) VALUES (@Action, @Date)";
                    using (SqlCommand insertCmd = new SqlCommand(actionQuery, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@Action", Action);
                        insertCmd.Parameters.AddWithValue("@Date", currentDateTime);
                        int result = insertCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"An error occurred:\n{ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
    }
}
