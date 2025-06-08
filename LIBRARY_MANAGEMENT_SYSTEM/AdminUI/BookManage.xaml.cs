using LIBRARY_MANAGEMENT_SYSTEM.AdminUI.adminActions;
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
using System.Windows.Threading;

namespace LIBRARY_MANAGEMENT_SYSTEM.AdminUI
{


    public partial class BookManage : Page
    {

        private DispatcherTimer clockTimer;

        public BookManage()
        {
            InitializeComponent();
            SetupClock();
        }

        private void SetupClock()
        {
            clockTimer = new DispatcherTimer();
            clockTimer.Interval = TimeSpan.FromSeconds(1);
            clockTimer.Tick += (sender, args) =>
            {
                ClockText.Text = DateTime.Now.ToString("hh:mm:ss tt");
            };
            clockTimer.Start();
        }

        private void showAdd(object sender, RoutedEventArgs e)
        {
            BookAdd addBook = new BookAdd();
            addBook.Show();
        }

        private void showRemove(object sender, RoutedEventArgs e)
        {
            BookRemove removeBook = new BookRemove();
            removeBook.Show();
        }

        private void showBooks(object sender, RoutedEventArgs e)
        {
            Books books = new Books();
            books.Show();
        }

        private void showBookLog(object sender, RoutedEventArgs e)
        {
            BookLog bookLog = new BookLog();
            bookLog.Show();
        }

    }
}
