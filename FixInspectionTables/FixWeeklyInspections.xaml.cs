/* Title:           Fix Weekly Inspections
 * Date:            2-17-17
 * Author:          Terry Holmes
 * 
 * Description:     This is for fixing the weekly inspection */

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
using InspectionsDLL;
using NewEventLogDLL;
using DataValidationDLL;

namespace FixInspectionTables
{
    /// <summary>
    /// Interaction logic for FixWeeklyInspections.xaml
    /// </summary>
    public partial class FixWeeklyInspections : Window
    {
        //setting the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        InspectionsClass TheInspectionsClass = new InspectionsClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();

        WeeklyVehicleInspectionsDataSet TheWeeklyVehicleInspectionsDataSet = new WeeklyVehicleInspectionsDataSet();
        int gintNumberOfRecords;

        public FixWeeklyInspections()
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
            //this will load up the data set
            try
            {
                TheWeeklyVehicleInspectionsDataSet = TheInspectionsClass.GetWeeklyVehicleInspectionsInfo();

                gintNumberOfRecords = TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections.Rows.Count - 1;

                dgrResults.ItemsSource = TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Fix Inspection Tables Fix Weekly Inspections Window Loaded " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            //this will process the data
            //setting local variables
            int intCounter;
            bool blnIsNotDate;
            string strValueForValidation;
            bool blnUpdateRecord;

            PleaseWait PleaseWait = new PleaseWait();
            PleaseWait.Show();

            try
            {
                //loop
                for(intCounter = 0; intCounter <= gintNumberOfRecords; intCounter++)
                {
                    blnUpdateRecord = false;

                    if(TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].IsInspectionDateNull() == true)
                    {
                        blnIsNotDate = true;
                    }
                    else
                    {
                        strValueForValidation = Convert.ToString(TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].InspectionDate);

                        blnIsNotDate = TheDataValidationClass.VerifyDateData(strValueForValidation);
                    }

                    if(blnIsNotDate == true)
                    {
                        TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].InspectionDate = TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter - 1].InspectionDate;
                        blnUpdateRecord = true;
                    }
                    if(TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].IsInspectionNotesNull() == true)
                    {
                        TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].InspectionNotes = "NOT PROVIDED";
                        blnUpdateRecord = true;
                    }
                    else if(TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].InspectionNotes == "")
                    {
                        TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].InspectionNotes = "NOT PROVIDED";
                        blnUpdateRecord = true;
                    }
                    if(TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].IsPPEInspectedNull() == true)
                    {
                        TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].PPEInspected = "YES";
                        blnUpdateRecord = true;
                    }
                    else if(TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].PPEInspected == "")
                    {
                        TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].PPEInspected = "YES";
                        blnUpdateRecord = true;
                    }
                    if(TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].IsVehicleCleanlinessNull() == true)
                    {
                        TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].ToolsInspected = "YES";
                        blnUpdateRecord = true;
                    }
                    else if(TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].ToolsInspected == "")
                    {
                        TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].ToolsInspected = "YES";
                        blnUpdateRecord = true;
                    }
                    if(TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].IsVehicleCleanlinessNull() == true)
                    {
                        TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].VehicleCleanliness = "YES";
                        blnUpdateRecord = true;
                    }
                    else if(TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].VehicleCleanliness == "")
                    {
                        TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].VehicleCleanliness = "YES";
                        blnUpdateRecord = true;
                    }
                    if(TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].IsVehicleServiceabilityNull() == true)
                    {
                        TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].VehicleServiceability = "YES";
                        blnUpdateRecord = true;
                    }
                    else if(TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].VehicleServiceability == "")
                    {
                        TheWeeklyVehicleInspectionsDataSet.WeeklyVehicleInspections[intCounter].VehicleServiceability = "YES";
                        blnUpdateRecord = true;
                    }

                    if(blnUpdateRecord == true)
                    {
                        TheInspectionsClass.UpdateWeeklyVehicleInspectionsDB(TheWeeklyVehicleInspectionsDataSet);
                    }
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Fix Inspection Tables Fix Weekly Inspections Process Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.Message);
            }

            PleaseWait.Close();
        }
    }
}
