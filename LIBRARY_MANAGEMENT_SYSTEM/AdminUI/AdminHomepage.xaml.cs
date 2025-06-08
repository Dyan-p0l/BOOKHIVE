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

            numAcc.Text = getAccCount().ToString();
            numBook.Text = getBookCount().ToString();

            Series = new ISeries[]
            {
                new PieSeries<int> { Values = new[] { borrowedBooks }, Name = "Books with borrowed copies"},
                new PieSeries<int> { Values = new[] { availableBooks }, Name = "Books without borrowed copies"}
            };

            Dictionary<string, int> genreCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            try
            {
                dbConnection db = new dbConnection();
                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT Genre FROM Books"; // Ensure "Genre" is the correct column name

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string genreRaw = reader.GetString(0);
                            if (string.IsNullOrWhiteSpace(genreRaw)) continue;

                            // Split by comma, trim whitespace
                            var genres = genreRaw.Split(',')
                                                 .Select(g => g.Trim())
                                                 .Where(g => !string.IsNullOrEmpty(g));

                            foreach (var genre in genres)
                            {
                                if (genreCounts.ContainsKey(genre))
                                    genreCounts[genre]++;
                                else
                                    genreCounts[genre] = 1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading genre data: " + ex.Message);
                genreCounts = new Dictionary<string, int>(); // fallback
            }

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

            SKColor[] chartColors = new[]
            {
            SKColors.Red, SKColors.Green, SKColors.Blue,
            SKColors.Orange, SKColors.Purple, SKColors.Teal,
            SKColors.Yellow, SKColors.Pink, SKColors.Brown,
            SKColors.Gray
};

            int colorIndex = 0;

            Series = genreCounts.Select((kvp, index) => new PieSeries<int>
            {
                Values = new[] { kvp.Value },
                Name = kvp.Key,
                Fill = new SolidColorPaint(chartColors[index % chartColors.Length]),
                DataLabelsSize = 11,
                DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Outer,
                DataLabelsPaint = new SolidColorPaint(SKColors.White)
            }).ToArray();

            ColumnSeries = genreCounts.Select(kvp => new ColumnSeries<ObservableValue>
            {
                Values = new[] { new ObservableValue(kvp.Value) },
                Name = kvp.Key,
                Fill = new SolidColorPaint(chartColors[colorIndex++ % chartColors.Length]),
                MaxBarWidth = 60,
                DataLabelsPaint = whiteTextPaint,
                DataLabelsSize = 11,
                DataLabelsPosition = DataLabelsPosition.Top
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