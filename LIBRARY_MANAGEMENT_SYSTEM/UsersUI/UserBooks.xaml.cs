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
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WPF;
using System.Collections.Generic;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.Defaults;
using LIBRARY_MANAGEMENT_SYSTEM.UsersUI.actions;
using LIBRARY_MANAGEMENT_SYSTEM.backend;
using Microsoft.Data.SqlClient;

namespace LIBRARY_MANAGEMENT_SYSTEM.UsersUI
{
    /// <summary>
    /// Interaction logic for UserBooks.xaml
    /// </summary>
    public partial class UserBooks : Page
    {
        public ISeries[] Series { get; set; }
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }

        private string currentUsername = dataStore.CurrentUsername;

        public UserBooks()
        {
            InitializeComponent();
            LoadChartData();
        }

        private void showPending(object sender, EventArgs e) 
        { 
            Pending pending = new Pending();
            pending.Show();
        }

        private void LoadChartData()
        {
            int borrowedCount = 0;
            int returnedCount = 0;
            int overdueCount = 0;

            try
            {
                dbConnection db = new dbConnection();
                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = @"
                    SELECT
                        SUM(CASE WHEN Action = 'Borrow' THEN 1 ELSE 0 END) AS Borrowed,
                        SUM(CASE WHEN Action = 'Return' THEN 1 ELSE 0 END) AS Returned,
                        SUM(CASE WHEN Status = 'Overdue' THEN 1 ELSE 0 END) AS Overdue
                    FROM AdminLogs
                    WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", currentUsername);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                borrowedCount = reader["Borrowed"] != DBNull.Value ? Convert.ToInt32(reader["Borrowed"]) : 0;
                                returnedCount = reader["Returned"] != DBNull.Value ? Convert.ToInt32(reader["Returned"]) : 0;
                                overdueCount = reader["Overdue"] != DBNull.Value ? Convert.ToInt32(reader["Overdue"]) : 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading chart data: " + ex.Message);
            }

            var whiteTextPaint = new SolidColorPaint(SKColors.White);

            Series = new ISeries[]
            {
            new ColumnSeries<ObservablePoint>
            {
                Values = new List<ObservablePoint> { new ObservablePoint(0, borrowedCount) },
                Name = "Borrowed",
                Fill = new SolidColorPaint(SKColor.Parse("#FF5C5C")),
                DataLabelsPaint = whiteTextPaint,
                DataLabelsSize = 11,
                DataLabelsPosition = DataLabelsPosition.Top,
                MaxBarWidth = 60
            },
            new ColumnSeries<ObservablePoint>
            {
                Values = new List<ObservablePoint> { new ObservablePoint(1, returnedCount) },
                Name = "Returned",
                Fill = new SolidColorPaint(SKColor.Parse("#02C4A1")),
                DataLabelsPaint = whiteTextPaint,
                DataLabelsSize = 11,
                DataLabelsPosition = DataLabelsPosition.Top,
                MaxBarWidth = 60
            },
            new ColumnSeries<ObservablePoint>
            {
                Values = new List<ObservablePoint> { new ObservablePoint(2, overdueCount) },
                Name = "Overdue",
                Fill = new SolidColorPaint(SKColor.Parse("#FFD32C")),
                DataLabelsPaint = whiteTextPaint,
                DataLabelsSize = 11,
                DataLabelsPosition = DataLabelsPosition.Top,
                MaxBarWidth = 60
            }
            };

            XAxes = new Axis[]
            {
            new Axis
            {
                Labels = new[] { "Borrowed", "Returned", "Overdue" },
                LabelsPaint = whiteTextPaint,
                UnitWidth = 3
            }
            };

            YAxes = new Axis[]
            {
            new Axis
            {
                Name = "Books",
                LabelsPaint = whiteTextPaint
            }
            };

            DataContext = this;
        }

        private void bookBorrow(object sender, RoutedEventArgs e)
        {
            BookBorrow bookBorrowWindow = new BookBorrow();
            bookBorrowWindow.Show();
        }

        private void bookReturn(object sender, RoutedEventArgs e)
        {
            BookReturn bookReturnWindow = new BookReturn();
            bookReturnWindow.Show();
        }
    }
}
