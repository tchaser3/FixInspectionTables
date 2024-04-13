/* Title:           Fix Daily Inspections
 * Date:            2-16-17
 * Author:          Terry Holmes
 * 
 * Description:     This will allow the user to fix the Daily Inspection Table */

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
using DateSearchDLL;
using DataValidationDLL;

namespace FixInspectionTables
{
    /// <summary>
    /// Interaction logic for FixDailyInspections.xaml
    /// </summary>
    public partial class FixDailyInspections : Window
    {
        //setting up the class
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        InspectionsClass TheInspectionsClass = new InspectionsClass();
        DateSearchClass TheDateSearchClass = new DateSearchClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();

        //setting the data sets
        VehicleInventorySheetDataSet TheVehicleInventorySheetDataSet;

        //global variables
        int gintInspectionNumberOfRecords;

        public FixDailyInspections()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
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
            //this will load the data set
            PleaseWait PleaseWait = new PleaseWait();
            PleaseWait.Show();

            try
            {
                TheVehicleInventorySheetDataSet = TheInspectionsClass.GetVehicleInventorySheetInfo();

                gintInspectionNumberOfRecords = TheVehicleInventorySheetDataSet.vehicleinventorysheet.Rows.Count - 1;

                dgvResults.ItemsSource = TheVehicleInventorySheetDataSet.vehicleinventorysheet;
            }
            catch(Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Fix Inspection Tables Fix Daily Inspections Window Loaded " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }

            PleaseWait.Close();
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            int intCounter;
            PleaseWait PleaseWait = new PleaseWait();
            bool blnUpdateRecord;
            bool blnFatalError;
            string strDateTest;

            PleaseWait.Show();

            try
            {
                //beginning loop
                for(intCounter = 0; intCounter <= gintInspectionNumberOfRecords; intCounter++)
                {
                    blnUpdateRecord = false;

                    strDateTest = Convert.ToString(TheVehicleInventorySheetDataSet.vehicleinventorysheet[intCounter].Date);

                    blnFatalError = TheDataValidationClass.VerifyDateData(strDateTest);

                    if(blnFatalError == true)
                    {
                        TheVehicleInventorySheetDataSet.vehicleinventorysheet[intCounter].Date = TheVehicleInventorySheetDataSet.vehicleinventorysheet[intCounter - 1].Date;
                        blnUpdateRecord = true;
                    }

                    if(TheVehicleInventorySheetDataSet.vehicleinventorysheet[intCounter].IsOdometerReadingNull() == true)
                    {
                        TheVehicleInventorySheetDataSet.vehicleinventorysheet[intCounter].OdometerReading = 0;
                        blnUpdateRecord = true;
                    }
                    if(TheVehicleInventorySheetDataSet.vehicleinventorysheet[intCounter].IsNotesNull() == true)
                    {
                        TheVehicleInventorySheetDataSet.vehicleinventorysheet[intCounter].Notes = "NONE REPORTED";
                        blnUpdateRecord = true;
                    }
                    if(TheVehicleInventorySheetDataSet.vehicleinventorysheet[intCounter].IsProblemCriticalNull() == true)
                    {
                        TheVehicleInventorySheetDataSet.vehicleinventorysheet[intCounter].ProblemCritical = "NO";
                        blnUpdateRecord = true;
                    }
                    if(TheVehicleInventorySheetDataSet.vehicleinventorysheet[intCounter].IsProblemReportedNull() == true)
                    {
                        TheVehicleInventorySheetDataSet.vehicleinventorysheet[intCounter].ProblemReported = "NO";
                        blnUpdateRecord = true;
                    }
                    if(TheVehicleInventorySheetDataSet.vehicleinventorysheet[intCounter].IsWorkOrderCreatedNull() == true)
                    {
                        TheVehicleInventorySheetDataSet.vehicleinventorysheet[intCounter].WorkOrderCreated = "NO";
                        blnUpdateRecord = true;
                    }

                    if(blnUpdateRecord == true)
                    {
                        TheInspectionsClass.UpdateVehicleInventorySheetDB(TheVehicleInventorySheetDataSet);
                    }
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Fix Inspection Tables Fix Daily Inspections " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }

            PleaseWait.Close();
        }
    }
}
