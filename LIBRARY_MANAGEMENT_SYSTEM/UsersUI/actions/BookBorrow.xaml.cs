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

namespace LIBRARY_MANAGEMENT_SYSTEM.UsersUI.actions
{
    /// <summary>
    /// Interaction logic for BookBorrow.xaml
    /// </summary>
    public partial class BookBorrow : Window
    {
        public BookBorrow()
        {
            InitializeComponent();

        }

        private void backBtn(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
