using Accessit.Exchange.DroitDeconnexion.Logic.Helpers;
using Accessit.Exchange.DroitDeconnexion.Presentation.Components;
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

namespace Accessit.Exchange.DroitDeconnexion.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            TextBoxOutputter outputter = new TextBoxOutputter(ConsoleTextBlock);
            Console.SetOut(outputter);
        }

        private void Button_Deploy_Solution_Click(object sender, RoutedEventArgs e)
        {
            DeployerHelper helper = new DeployerHelper();
            helper.Deploy();
        }

        private void Button_Retract_Solution_Click(object sender, RoutedEventArgs e)
        {
            DeployerHelper helper = new DeployerHelper();
            helper.Retract();
        }

        private void Button_Enable_Solution_Click(object sender, RoutedEventArgs e)
        {
            ActivatorHelper helper = new ActivatorHelper();
            helper.Enable();
        }

        private void Button_Disable_Solution_Click(object sender, RoutedEventArgs e)
        {
            ActivatorHelper helper = new ActivatorHelper();
            helper.Disable();
        }

        private void Button_Update_Status_Click(object sender, RoutedEventArgs e)
        {
            StatusHelper helper = new StatusHelper(Status);
            helper.GetStatus();
        }

        private void Button_Get_All_Quarantined_Click(object sender, RoutedEventArgs e)
        {
            QuarantinedMailHelper helper = new QuarantinedMailHelper();
            helper.GetAllQuarantinedMails();
        }

        private void Button_Release_All_Quarantined_Click(object sender, RoutedEventArgs e)
        {
            QuarantinedMailHelper helper = new QuarantinedMailHelper();
            helper.ReleaseAllMails();
        }
    }
}
