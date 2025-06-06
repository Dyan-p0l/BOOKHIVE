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
using MahApps.Metro.IconPacks;


namespace LIBRARY_MANAGEMENT_SYSTEM
{
    /// <summary>
    /// Interaction logic for UserDashboard.xaml
    /// </summary>
    public partial class UserDashboard : Window
    {

        public UserDashboard()
        {
            InitializeComponent();
            MainContentFrame.Navigate(new UsersUI.UserHomepage(MainContentFrame)); 
            SetActiveButton(HomeButton); 
        }
        
        private void SetActiveButton(Button activeButton)
        {
            var buttons = new List<Button> { HomeButton, AccountButton, BooksButton, HistoryButton, LogoutButton };

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

        private void ManageBooks_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new UsersUI.UserBooks());
            SetActiveButton(BooksButton);
        }

        private void accountClick(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new UsersUI.UserAccount());
            SetActiveButton(AccountButton);
        }

        private void historyClick(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new UsersUI.UserHistory());
            SetActiveButton(HistoryButton);
        }

        private void homeClick(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new UsersUI.UserHomepage(MainContentFrame));
            SetActiveButton(HomeButton);
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

    }
}
