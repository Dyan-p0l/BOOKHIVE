using Azure;
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

namespace LIBRARY_MANAGEMENT_SYSTEM.UsersUI
{

    public partial class UserHomepage : Page
    {

        private Frame _parentFrame;

        public UserHomepage(Frame parentFrame)
        {            
            InitializeComponent();
            _parentFrame = parentFrame;
        }

        private void showAot1(object sender, MouseButtonEventArgs e)
        {
            _parentFrame.Navigate(new Aot(_parentFrame));
        }

        private void showControlsys(object sender, MouseButtonEventArgs e)
        {
            _parentFrame.Navigate(new homeBooks.Controlsys(_parentFrame));
        }

        private void showPotter1(object sender, MouseButtonEventArgs e)
        {
            _parentFrame.Navigate(new Potter1(_parentFrame));
        }

        private void showCsharp(object sender, MouseButtonEventArgs e)
        {
            _parentFrame.Navigate(new Csharp(_parentFrame));
        }

        private void showAot2(object sender, MouseButtonEventArgs e)
        {
            _parentFrame.Navigate(new homeBooks.Aot2(_parentFrame));
        }

        private void showPotter2(object sender, MouseButtonEventArgs e)
        {
            _parentFrame.Navigate(new homeBooks.Potter2(_parentFrame));
        }
        
        private void showCpp(object sender, MouseButtonEventArgs e)
        {
            _parentFrame.Navigate(new Cpp(_parentFrame));
        }
       
        private void showJava(object sender, MouseButtonEventArgs e)
        {
            _parentFrame.Navigate(new Java(_parentFrame));
        }


    }
}
