using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using System.Windows.Controls;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.Defaults;
using LIBRARY_MANAGEMENT_SYSTEM.UsersUI.actions;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using Microsoft.Data.SqlClient;
using System.Windows;
using LIBRARY_MANAGEMENT_SYSTEM.backend;

namespace LIBRARY_MANAGEMENT_SYSTEM.AdminUI
{
    public partial class AdminHomepage : Page
    {
        public IEnumerable<ISeries> Series { get; set; }
        public IEnumerable<ISeries> ColumnSeries { get; set; }
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }

        public AdminHomepage()
        {
            InitializeComponent();

            // Example values - replace with actual DB queries
            int totalBooks = 100; // You should get this from the database
            int borrowedBooks = 40;
            int availableBooks = totalBooks - borrowedBooks;


            int fictionCount = 0;
            int engineeringCount = 0;
            int scienceCount = 0;
            int romanceCount = 0;
            int programmingCount = 0;
            int sciFiCount = 0;
            int historyCount = 0;

            try
            {
                dbConnection db = new dbConnection();
                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = @"
                    SELECT
                        SUM(CASE WHEN Genre LIKE '%Fiction%' THEN 1 ELSE 0 END) AS Fiction,
                        SUM(CASE WHEN Genre LIKE '%Science%' THEN 1 ELSE 0 END) AS Science,
                        SUM(CASE WHEN Genre LIKE '%Romance%' THEN 1 ELSE 0 END) AS Romance,
                        SUM(CASE WHEN Genre LIKE '%Computer Programming%' THEN 1 ELSE 0 END) AS Computer_Programming,
                        SUM(CASE WHEN Genre LIKE '%Engineering%' THEN 1 ELSE 0 END) AS Engineering,
                        SUM(CASE WHEN Genre LIKE '%Science Fiction%' THEN 1 ELSE 0 END) Scifi
                    FROM Books";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                fictionCount = reader["Fiction"] != DBNull.Value ? Convert.ToInt32(reader["Fiction"]) : 0;
                                engineeringCount = reader["Engineering"] != DBNull.Value ? Convert.ToInt32(reader["Engineering"]) : 0;
                                scienceCount = reader["Science"] != DBNull.Value ? Convert.ToInt32(reader["Science"]) : 0;
                                romanceCount = reader["Romance"] != DBNull.Value ? Convert.ToInt32(reader["Romance"]) : 0;
                                programmingCount = reader["Computer_Programming"] != DBNull.Value ? Convert.ToInt32(reader["Computer_Programming"]) : 0;
                                sciFiCount = reader["Scifi"] != DBNull.Value ? Convert.ToInt32(reader["Scifi"]) : 0;
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error loading chart data: " + ex.Message);
            }

            numAcc.Text = getAccCount().ToString();
            numBook.Text = getBookCount().ToString();

            Series = new ISeries[]
            {
                new PieSeries<int> { Values = new[] { borrowedBooks }, Name = "Books with borrowed copies"},
                new PieSeries<int> { Values = new[] { availableBooks }, Name = "Books without borrowed copies"}
            };

            var bookCounts = new List<ObservableValue>
            {
                new ObservableValue(fictionCount),
                new ObservableValue(scienceCount),
                new ObservableValue(historyCount),
                new ObservableValue(engineeringCount),
                new ObservableValue(sciFiCount),
                new ObservableValue(programmingCount)
            };

            string[] genres = new[]{"Fiction", "Science", "History", "Engineering", "Sci-Fi", "Computer Programming"};

            SKColor[] colors = new[]
            {
                SKColor.Parse("#FF5C5C"),
                SKColor.Parse("#02C4A1"),
                SKColor.Parse("#FFD32C"),
                SKColor.Parse("#6A5ACD"),
                SKColor.Parse("#FFA500"),
                SKColor.Parse("#40E0D0")
            };

            var whiteTextPaint = new SolidColorPaint(SKColors.White);

            ColumnSeries = genres.Select((genre, index) => new ColumnSeries<ObservableValue>
            {
                Values = new[] { bookCounts[index] },
                Name = genre,
                Fill = new SolidColorPaint(colors[index]),
                MaxBarWidth = 60,
                DataLabelsPaint = whiteTextPaint,
                DataLabelsSize = 11,
                DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top
            }).ToArray();


            XAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Genres",
                    TextSize = 10,
                    UnitWidth = 1,
                    MinStep = 1,
                    ForceStepToMin = true
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

        private int getAccCount()
        {
            int count = 0;

            try
            {
                dbConnection db = new dbConnection();
                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE UserType = 'User'"; // Replace with your table name

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        count = (int)cmd.ExecuteScalar(); // Efficient way to get a single value
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error counting rows: " + ex.Message);
            }

            return count;
        }

        private int getBookCount()
        {
            int count = 0;

            try
            {
                dbConnection db = new dbConnection();
                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Books"; // Replace with your table name

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        count = (int)cmd.ExecuteScalar(); // Efficient way to get a single value
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error counting rows: " + ex.Message);
            }

            return count;
        }


    }
}
