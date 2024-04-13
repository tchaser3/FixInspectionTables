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
    /// Interaction logic for AboutBox.xaml
    /// </summary>
    public partial class AboutBox : Window
    {
        public AboutBox()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            string strAbout;

            strAbout = "If You Have Any Questions, Please Contact Terry Holmes\nAt tchaser3@roadrunner.com";

            tblAbout.Text = strAbout;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //this will close the program
            this.Close();
        }
    }
}
