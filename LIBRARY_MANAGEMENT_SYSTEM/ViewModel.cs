using System;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;

namespace LIBRARY_MANAGEMENT_SYSTEM
{
    public class ViewModel
    {
        public ISeries[] PieSeries { get; set; }

        public ViewModel()
        {
            PieSeries = new ISeries[]
            {
            new PieSeries<double> { Values = new double[] { 45 }, Name = "Books", InnerRadius = 40 },
            new PieSeries<double> { Values = new double[] { 30 }, Name = "Magazines", InnerRadius = 40 },
            new PieSeries<double> { Values = new double[] { 25 }, Name = "Journals", InnerRadius = 40 }
            };
        }

    }
}
