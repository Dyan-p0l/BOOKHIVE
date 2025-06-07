using MahApps.Metro.IconPacks;
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

namespace LIBRARY_MANAGEMENT_SYSTEM
{
    /// <summary>
    /// Interaction logic for AdminDashboard.xaml
    /// </summary>
    public partial class AdminDashboard : Window
    {
        
        public AdminDashboard()
        {
            InitializeComponent();
            AdminContentFrame.Navigate(new AdminUI.AdminHomepage());
            SetActiveButton(home);
        }

        private void SetActiveButton(Button activeButton)
        {
            var buttons = new List<Button> { home, accountsBtn, booksBtn, historyBtn, logoutBtn};

            foreach (var btn in buttons)
            {
                bool isActive = (btn == activeButton);

                btn.Background = isActive
                    ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD32C"))
                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00DDDDDD"));

                if (btn.Content is StackPanel sp)
                {
                    var textBlocks = FindVisualChildren<TextBlock>(sp);
                    var icons = FindVisualChildren<PackIconFontAwesome>(sp);
                    foreach (var tb in textBlocks)
                    {
                        tb.Foreground = isActive
                            ? new SolidColorBrush(Colors.Black)
                            : new SolidColorBrush(Colors.White);
                    }
                    foreach (var icon in icons)
                    {
                        icon.Foreground = isActive
                            ? new SolidColorBrush(Colors.Black)
                            : new SolidColorBrush(Colors.White);
                    }

                }
            }
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T t)
                    {
                        yield return t;
                    }

                    if (child != null)
                    {
                        foreach (T childOfChild in FindVisualChildren<T>(child))
                        {
                            yield return childOfChild;
                        }
                    }
                }
            }
        }

        private void homeButton(object sender, RoutedEventArgs e)
        {
            AdminContentFrame.Navigate(new AdminUI.AdminHomepage());
            SetActiveButton(home);
        }   
        
        private void accountManage(object sender, RoutedEventArgs e)
        {
            AdminContentFrame.Navigate(new AdminUI.AccountManage());
            SetActiveButton(accountsBtn);
        }

        private void bookManage(object sender, RoutedEventArgs e)
        {
            AdminContentFrame.Navigate(new AdminUI.BookManage());
            SetActiveButton(booksBtn);
        }

        private void historyManage(object sender, RoutedEventArgs e)
        {
            AdminContentFrame.Navigate(new AdminUI.HistoryManage());
            SetActiveButton(historyBtn);
        }
        private void logOut(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
            "Are you sure you want to log out?",
            "Confirm Logout",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question
);

            if (result == MessageBoxResult.Yes)
            {
                MainWindow login = new MainWindow();
                login.Show();
                this.Close();
            }
        }

        private void AdminContentFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
    }
}
