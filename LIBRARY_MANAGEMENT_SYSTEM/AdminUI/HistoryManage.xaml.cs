using LIBRARY_MANAGEMENT_SYSTEM.backend;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// <summary>
    /// Interaction logic for HistoryManage.xaml
    /// </summary>
    public partial class HistoryManage : Page
    {

        public ObservableCollection<AdminNotification> Notifications { get; set; } = new ObservableCollection<AdminNotification>();

        public HistoryManage()
        {
            InitializeComponent();
            this.DataContext = this;
            LoadAdminAction();
        }



        private void LoadAdminAction()
        {
            try
            {
                dbConnection db = new dbConnection();
                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT actions, date  FROM AdminActions ORDER BY date DESC";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Notifications.Add(new AdminNotification
                                {
                                    action = reader["actions"].ToString(),
                                    DateAdded = Convert.ToDateTime(reader["date"]),
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user logs: " + ex.Message);
            }
        }
    }

    public class AdminNotification
    {
        public string action { get; set; }
        public DateTime DateAdded { get; set; }

    }
}
