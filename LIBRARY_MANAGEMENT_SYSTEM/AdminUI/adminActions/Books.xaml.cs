using LIBRARY_MANAGEMENT_SYSTEM.backend;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace LIBRARY_MANAGEMENT_SYSTEM.AdminUI.adminActions
{
    public partial class Books : Window
    {
        public Books()
        {
            InitializeComponent();
            LoadBooksFromDatabase();
        }

        private void backBtn(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadBooksFromDatabase()
        {
            dbConnection db = new dbConnection();
            List<Book> books = new List<Book>();

            using (SqlConnection connection = db.GetConnection())
            {
                string query = "SELECT Title, BookID, Author, Genre FROM Books";
                SqlCommand cmd = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    books.Add(new Book
                    {
                        Title = reader["Title"].ToString(),
                        BookID = Convert.ToInt32(reader["BookID"]),
                        Author = reader["Author"].ToString(),
                        Genre = reader["Genre"].ToString()
                    });
                }

                reader.Close();
            }

            BookDataGrid.ItemsSource = books;

        }

        private void BookDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var headerColors = new[] {
                (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#e7a300"), 
                (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#f0c200"), 
                (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#f2db00"), 
                (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#f8e319")  
            };

            var cellColors = new[] {
                (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#fff59d"), 
                (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#fff176"), 
                (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#ffee58"), 
                (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#ffeb3b")  
            };

            for (int i = 0; i < BookDataGrid.Columns.Count; i++)
            {

                BookDataGrid.Columns[i].Width = new DataGridLength(1, DataGridLengthUnitType.Star);

                var headerStyle = new System.Windows.Style(typeof(System.Windows.Controls.Primitives.DataGridColumnHeader));
                headerStyle.Setters.Add(new Setter(System.Windows.Controls.Primitives.DataGridColumnHeader.BackgroundProperty, headerColors[i % headerColors.Length]));
                headerStyle.Setters.Add(new Setter(System.Windows.Controls.Primitives.DataGridColumnHeader.ForegroundProperty, System.Windows.Media.Brushes.Black));
                headerStyle.Setters.Add(new Setter(System.Windows.Controls.Primitives.DataGridColumnHeader.FontWeightProperty, FontWeights.Bold));
                headerStyle.Setters.Add(new Setter(System.Windows.Controls.Primitives.DataGridColumnHeader.HorizontalContentAlignmentProperty, HorizontalAlignment.Center));

                if (BookDataGrid.Columns[i] is DataGridTextColumn textColumn)
                {

                    var cellStyle = new Style(typeof(DataGridCell));
                    cellStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, cellColors[i % cellColors.Length]));
                    cellStyle.Setters.Add(new Setter(DataGridCell.ForegroundProperty, System.Windows.Media.Brushes.Black));
                    cellStyle.Setters.Add(new Setter(DataGridCell.BorderThicknessProperty, new Thickness(0)));
                    cellStyle.Setters.Add(new Setter(DataGridCell.HorizontalContentAlignmentProperty, HorizontalAlignment.Center));
                    cellStyle.Setters.Add(new Setter(DataGridCell.VerticalContentAlignmentProperty, VerticalAlignment.Center));
                    textColumn.CellStyle = cellStyle;
                    
                    var textStyle = new Style(typeof(TextBlock));
                    var headerName = BookDataGrid.Columns[i].Header.ToString();

                    if (headerName == "Author" || headerName == "BookID")
                    {
                        textStyle.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center));
                        textStyle.Setters.Add(new Setter(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center));
                        textStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
                        textColumn.ElementStyle = textStyle;
                    }


                    if (headerName == "Title" || headerName == "Genre")
                    {
                        textStyle.Setters.Add(new Setter(TextBlock.TextTrimmingProperty, TextTrimming.CharacterEllipsis));
                        textStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.NoWrap));
                        textColumn.ElementStyle = textStyle;
                    }
                }

                BookDataGrid.Columns[i].HeaderStyle = headerStyle;
            }
        }

    }

    public class Book
    {
        public string Title { get; set; }
        public int BookID { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }

    }
}
