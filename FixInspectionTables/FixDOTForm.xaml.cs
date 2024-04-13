/* Title:           Fix DOT Form
 * Date:            2-15-17
 * Author:          Terry Holmes
 * 
 * Description:     Fix DOT Form */

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
    /// Interaction logic for FixDOTForm.xaml
    /// </summary>
    public partial class FixDOTForm : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        InspectionsClass TheInspectionsClass = new InspectionsClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();

        VehicleDailyInspectionDataSet TheDOTFormDataSet = new VehicleDailyInspectionDataSet();

        int gintNumberOfRecords;

        public FixDOTForm()
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
            //this will load the data set
            try
            {
                TheDOTFormDataSet = TheInspectionsClass.GetDailyVehicleInspectionInfo();

                gintNumberOfRecords = TheDOTFormDataSet.VehicleDailyInspection.Rows.Count - 1;

                dgrResults.ItemsSource = TheDOTFormDataSet.VehicleDailyInspection;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Fix Inspection Tables Fix DOT Form Window Loaded " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            int intCounter;
            bool blnFatalError = false;
            bool blnUpdateItem = false;
            string strValueForValidation;

            PleaseWait PleaseWait = new PleaseWait();
            PleaseWait.Show();

            try
            {
                for(intCounter = 0; intCounter <= gintNumberOfRecords; intCounter++)
                {
                    blnUpdateItem = false;

                    //checking the date
                    strValueForValidation = Convert.ToString(TheDOTFormDataSet.VehicleDailyInspection[intCounter].Date);

                    blnFatalError = TheDataValidationClass.VerifyDateData(strValueForValidation);

                    if(blnFatalError == true)
                    {
                        TheDOTFormDataSet.VehicleDailyInspection[intCounter].Date = TheDOTFormDataSet.VehicleDailyInspection[intCounter - 1].Date;
                        blnUpdateItem = true;
                    }

                    if (TheDOTFormDataSet.VehicleDailyInspection[intCounter].IsNotesNull() == true)
                    {
                        TheDOTFormDataSet.VehicleDailyInspection[intCounter].Notes = "NOT PROVIDED";
                        blnUpdateItem = true;
                    }

                    if(blnUpdateItem == true)
                    {
                        TheInspectionsClass.UpdateVehicleDailyInspectionDB(TheDOTFormDataSet);
                    }
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Fix Inspection Tables Fix DOT Form Process Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }

            PleaseWait.Close();
        }
    }
}
