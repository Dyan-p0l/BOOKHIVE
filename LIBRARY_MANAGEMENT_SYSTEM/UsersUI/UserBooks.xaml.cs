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

       
        public UserBooks()
        {
            InitializeComponent();

            var whiteTextPaint = new SolidColorPaint(SKColors.White);


            Series = new ISeries[]
            {
                new ColumnSeries<ObservablePoint>
                {
                    Values = new List<ObservablePoint> { new ObservablePoint(0, 120) },
                    Name = "Borrowed",
                    Fill = new SolidColorPaint(SKColor.Parse("#FF5C5C")),
                    DataLabelsPaint = whiteTextPaint,
                    DataLabelsSize = 11,
                    DataLabelsPosition = DataLabelsPosition.Top,
                    MaxBarWidth = 60
                },
                new ColumnSeries<ObservablePoint>
                {
                    Values = new List<ObservablePoint> { new ObservablePoint(1, 95) },
                    Name = "Returned",
                    Fill = new SolidColorPaint(SKColor.Parse("#02C4A1")),
                    DataLabelsPaint = whiteTextPaint,
                    DataLabelsSize = 11,
                    DataLabelsPosition = DataLabelsPosition.Top,
                    MaxBarWidth = 60
                },
                new ColumnSeries<ObservablePoint>
                {
                    Values = new List<ObservablePoint> { new ObservablePoint(2, 30) },
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

        
    }
}
