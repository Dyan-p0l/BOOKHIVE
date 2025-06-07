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

namespace LIBRARY_MANAGEMENT_SYSTEM.AdminUI
{

    public partial class BookManage : Page
    {
        public BookManage()
        {
            InitializeComponent();
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

    }
}
