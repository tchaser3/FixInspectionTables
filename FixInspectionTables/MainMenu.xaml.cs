/* Title:           Main Menu
 * Date:            2-15-17
 * Author:          Terry Holmes
 * 
 * Description:     This is the main menu */

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

namespace FixInspectionTables
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();

        public MainMenu()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //this will close the program
            TheMessagesClass.CloseTheProgram();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutBox AboutBox = new AboutBox();
            AboutBox.ShowDialog();
        }

        private void btnFixDailyInspections_Click(object sender, RoutedEventArgs e)
        {
            FixDailyInspections FixDailyInspections = new FixDailyInspections();
            FixDailyInspections.Show();
            this.Close();
        }

        private void btnFixSignOut_Click(object sender, RoutedEventArgs e)
        {
            FixDailySignOut FixDailySignOut = new FixDailySignOut();
            FixDailySignOut.Show();
            this.Close();
        }

        private void btnFixDOTForm_Click(object sender, RoutedEventArgs e)
        {
            FixDOTForm FixDOTForm = new FixDOTForm();
            FixDOTForm.Show();
            this.Close();
        }

        private void btnFixWeeklyReports_Click(object sender, RoutedEventArgs e)
        {
            FixWeeklyInspections FixWeeklyInspections = new FixWeeklyInspections();
            FixWeeklyInspections.Show();
            this.Close();
        }
    }
}
