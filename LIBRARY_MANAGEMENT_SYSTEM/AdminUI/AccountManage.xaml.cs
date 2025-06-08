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

namespace LIBRARY_MANAGEMENT_SYSTEM.AdminUI
{
    /// <summary>
    /// Interaction logic for AccountManage.xaml
    /// </summary>
    public partial class AccountManage : Page
    {

        public AccountManage()
        {
            InitializeComponent();
            LoadAccountsFromDatabase();
        }

        private void LoadAccountsFromDatabase()
        {
            dbConnection db = new dbConnection();

            List<Account> acc = new List<Account>();

            using (SqlConnection connection = db.GetConnection())
            {
                string query = "SELECT UserID, FullName, UserName, Password FROM Users WHERE UserType = 'User'";
                SqlCommand cmd = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    acc.Add(new Account
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        fName = reader["FullName"].ToString(),
                        uName = reader["UserName"].ToString(),
                        pass = reader["Password"].ToString()
                    });
                }

                reader.Close();
            }

            AccountDataGrid.Columns.Clear(); // ✅ Clear previous columns including any auto-generated ones
            AccountDataGrid.AutoGenerateColumns = false; // ✅ Disable auto-generation

            // Manually add columns in the order you want
            AccountDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "UserID",
                Binding = new Binding("UserID")
            });
            AccountDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Full Name",
                Binding = new Binding("fName")
            });
            AccountDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Username",
                Binding = new Binding("uName")
            });
            AccountDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Password",
                Binding = new Binding("pass")
            });

            // ✅ Add Delete column last
            var deleteColumn = new DataGridTemplateColumn
            {
                Header = ""
            };

            var deleteTemplate = new DataTemplate();

            var buttonFactory = new FrameworkElementFactory(typeof(Button));
            buttonFactory.SetValue(Button.ContentProperty, "Delete");
            buttonFactory.SetValue(Button.BackgroundProperty, Brushes.Red);
            buttonFactory.SetValue(Button.ForegroundProperty, Brushes.White);
            buttonFactory.SetValue(Button.PaddingProperty, new Thickness(5, 2, 5, 2));
            buttonFactory.SetValue(Button.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            buttonFactory.AddHandler(Button.ClickEvent, new RoutedEventHandler(DeleteButton_Click));
            buttonFactory.SetBinding(Button.TagProperty, new Binding("UserID"));

            deleteTemplate.VisualTree = buttonFactory;
            deleteColumn.CellTemplate = deleteTemplate;

            AccountDataGrid.Columns.Add(deleteColumn);

            AccountDataGrid.ItemsSource = acc; // ✅ Set items last
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && int.TryParse(btn.Tag.ToString(), out int userId))
            {
                var result = MessageBox.Show($"Are you sure you want to delete user ID {userId}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    dbConnection db = new dbConnection();
                    using (SqlConnection conn = db.GetConnection())
                    {
                        string query = "DELETE FROM Users WHERE UserID = @UserID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    LoadAccountsFromDatabase(); // Refresh grid
                }
            }
        }


        private void AccountDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var headerColors = new[] {
                (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#e7a300"), // Dark Goldenrod (Very dark yellow-brown)
                (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#f0c200"), // Goldenrod (dark yellow)
                (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#f2db00"), // Gold (standard deep yellow)
                (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#f8e319")  // LemonChiffon (pale yellow)
            };

            var cellColors = new[] {
                (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#fff59d"), // Light Yellow
                (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#fff176"), // Lighter Gold
                (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#ffee58"), // Light Golden Yellow
                (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#ffeb3b")  // Standard Pale Yellow
            };

            for (int i = 0; i < AccountDataGrid.Columns.Count; i++)
            {

                AccountDataGrid.Columns[i].Width = new DataGridLength(1, DataGridLengthUnitType.Star);

                var headerStyle = new System.Windows.Style(typeof(System.Windows.Controls.Primitives.DataGridColumnHeader));
                headerStyle.Setters.Add(new Setter(System.Windows.Controls.Primitives.DataGridColumnHeader.BackgroundProperty, headerColors[i % headerColors.Length]));
                headerStyle.Setters.Add(new Setter(System.Windows.Controls.Primitives.DataGridColumnHeader.ForegroundProperty, System.Windows.Media.Brushes.Black));
                headerStyle.Setters.Add(new Setter(System.Windows.Controls.Primitives.DataGridColumnHeader.FontWeightProperty, FontWeights.Bold));
                headerStyle.Setters.Add(new Setter(System.Windows.Controls.Primitives.DataGridColumnHeader.HorizontalContentAlignmentProperty, HorizontalAlignment.Center));

                if (AccountDataGrid.Columns[i] is DataGridTextColumn textColumn)
                {

                    var cellStyle = new Style(typeof(DataGridCell));
                    cellStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, cellColors[i % cellColors.Length]));
                    cellStyle.Setters.Add(new Setter(DataGridCell.ForegroundProperty, System.Windows.Media.Brushes.Black));
                    cellStyle.Setters.Add(new Setter(DataGridCell.BorderThicknessProperty, new Thickness(0)));
                    cellStyle.Setters.Add(new Setter(DataGridCell.HorizontalContentAlignmentProperty, HorizontalAlignment.Center));
                    cellStyle.Setters.Add(new Setter(DataGridCell.VerticalContentAlignmentProperty, VerticalAlignment.Center));
                    textColumn.CellStyle = cellStyle;

                    var textStyle = new Style(typeof(TextBlock));
                    var headerName = AccountDataGrid.Columns[i].Header.ToString();

                    textStyle.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center));
                    textStyle.Setters.Add(new Setter(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center));
                    textStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));

                    textStyle.Setters.Add(new Setter(TextBlock.TextTrimmingProperty, TextTrimming.CharacterEllipsis));
                    textStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.NoWrap));
                    textColumn  .ElementStyle = textStyle;
                  
                }

                AccountDataGrid.Columns[i].HeaderStyle = headerStyle;
            }

            if (AccountDataGrid.Columns.Count > 0)
            {
                var deleteHeaderStyle = new Style(typeof(System.Windows.Controls.Primitives.DataGridColumnHeader));
                // Do NOT set a background color here — this will keep it default or transparent
                deleteHeaderStyle.Setters.Add(new Setter(System.Windows.Controls.Primitives.DataGridColumnHeader.ForegroundProperty, Brushes.Black));
                deleteHeaderStyle.Setters.Add(new Setter(System.Windows.Controls.Primitives.DataGridColumnHeader.FontWeightProperty, FontWeights.Bold));
                deleteHeaderStyle.Setters.Add(new Setter(System.Windows.Controls.Primitives.DataGridColumnHeader.HorizontalContentAlignmentProperty, HorizontalAlignment.Center));

                AccountDataGrid.Columns[^1].HeaderStyle = deleteHeaderStyle; // ^1 gets the last column (C# 8.0+)
            }
        }

    }

    public class Account
    {
        public int UserID { get; set; }
        public string fName { get; set; }
        public string uName { get; set; }
        public string pass { get; set; }

    }
}
