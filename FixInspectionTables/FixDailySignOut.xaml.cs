/* Title:           Fix Sign Out Table
 * Date:            2-15-17
 * Author:          Terry Holmes
 * 
 * Description:     This form is used to fix the sign out table */

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
using NewEventLogDLL;
using InspectionsDLL;
using DataValidationDLL;

namespace FixInspectionTables
{
    /// <summary>
    /// Interaction logic for FixDailySignOut.xaml
    /// </summary>
    public partial class FixDailySignOut : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        InspectionsClass TheInspectionsClass = new InspectionsClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();

        VehicleSignedOutDataSet TheVehiclesSignedOut;

        int gintNumberOfRecords;

        public FixDailySignOut()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //this will close the program
            TheMessagesClass.CloseTheProgram();
        }

        private void btnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainMenu MainMenu = new MainMenu();
            MainMenu.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this will load up the data set
            try
            {
                TheVehiclesSignedOut = TheInspectionsClass.GetVehicleSignedOutInfo();

                gintNumberOfRecords = TheVehiclesSignedOut.vehiclesignedout.Rows.Count - 1;

                dgrResults.ItemsSource = TheVehiclesSignedOut.vehiclesignedout;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Fix Inspection Tables Fix Daily Sign Out Window Loaded " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            string strTransactionDate;
            bool blnFatalError = false;
            int intCounter;
            PleaseWait PleaseWait = new PleaseWait();
            PleaseWait.Show();

            try
            {
                //beginning loop
                for(intCounter = 0; intCounter <= gintNumberOfRecords; intCounter++)
                {
                    strTransactionDate = Convert.ToString(TheVehiclesSignedOut.vehiclesignedout[intCounter].Date);

                    blnFatalError = TheDataValidationClass.VerifyDateData(strTransactionDate);

                    if(blnFatalError == true)
                    {
                        TheVehiclesSignedOut.vehiclesignedout[intCounter].Date = TheVehiclesSignedOut.vehiclesignedout[intCounter - 1].Date;
                        TheInspectionsClass.UpdateVehicleSignedOutDB(TheVehiclesSignedOut);
                    }
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Fix Inspection Tables Fix Daily Sign Out Process Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }

            PleaseWait.Close();
        }
    }
}
