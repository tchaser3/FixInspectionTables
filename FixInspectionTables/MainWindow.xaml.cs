/* Title:           Logon
 * Date:            2-10-17
 * Author:          Terry Holmes
 * 
 * Description:     This is the logon form fixing the inspection tables */

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
using NewEventLogDLL;
using NewEmployeeDLL;
using DataValidationDLL;

namespace FixInspectionTables
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();

        //setting up the data set
        public static VerifyLogonDataSet TheVerifyLogonDataSet = new VerifyLogonDataSet();

        int gintNumberOfMisses;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.CloseTheProgram();
        }
        private void LogonFailed()
        {
            gintNumberOfMisses++;

            if(gintNumberOfMisses == 3)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Fix inspection Tables There Have Been Three Attempts To Log In");

                TheMessagesClass.ErrorMessage("There Have Been Three Attempts to Log In, The Application will Close");

                Application.Current.Shutdown();
            }
            else
            {
                TheMessagesClass.InformationMessage("The Login Information is not Correct,/nOr You Are Not A Administrator");

                return;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gintNumberOfMisses = 0;
        }

        private void btnLogon_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            int intEmployeeID = 0;
            string strValueForValidation;
            string strLastName;
            bool blnFatalError = false;
            bool blnThereIsAProblem = false;
            string strErrorMessage = "";
            int intRecordsReturned;

            try
            {
                //data validation
                strValueForValidation = pbxEmployeeID.Password;
                strLastName = txtLastName.Text;
                blnFatalError = TheDataValidationClass.VerifyIntegerData(strValueForValidation);
                if(blnFatalError == true)
                {
                    blnThereIsAProblem = true;
                    strErrorMessage = strErrorMessage + "The Employee ID Entered is not an Integer\n";
                }
                else
                {
                    intEmployeeID = Convert.ToInt32(strValueForValidation);
                }
                if(strLastName == "")
                {
                    blnThereIsAProblem = true;
                    strErrorMessage = strErrorMessage + "The Last Name Was Not Entered\n";
                }
                if(blnThereIsAProblem == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }

                TheVerifyLogonDataSet = TheEmployeeClass.VerifyLogon(intEmployeeID, strLastName);

                intRecordsReturned = TheVerifyLogonDataSet.VerifyLogon.Rows.Count;

                if(intRecordsReturned == 0)
                {
                    LogonFailed();
                }
                else
                {
                    if (TheVerifyLogonDataSet.VerifyLogon[0].EmployeeGroup != "ADMIN")
                    {
                        LogonFailed();
                    }
                    else
                    {
                        TheMessagesClass.InformationMessage("You Have Logged Into Fix Inspection Tables");

                        MainMenu MainMenu = new MainMenu();
                        MainMenu.Show();
                        this.Hide();
                    }
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Fix Inspection Tables Main Window Logon " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
    }
}
