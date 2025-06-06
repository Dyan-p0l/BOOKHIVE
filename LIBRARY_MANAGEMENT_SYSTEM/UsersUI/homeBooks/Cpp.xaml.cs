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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LIBRARY_MANAGEMENT_SYSTEM.UsersUI
{
    /// <summary>
    /// Interaction logic for Cpp.xaml
    /// </summary>
    public partial class Cpp : Page
    {
        Frame _parentFrame;
        public Cpp(Frame parentFrame)
        {
            InitializeComponent();
            _parentFrame = parentFrame;
        }

        private void backBtn(object sender, RoutedEventArgs e)
        {
            _parentFrame.Navigate(new UserHomepage(_parentFrame));
        }
    }
}
