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

namespace AssistenteTI.Pages
{
    /// <summary>
    /// Interação lógica para GuidePage.xam
    /// </summary>
    public partial class GuidePage : Page
    {
        private string Usuario { get; set; }
        public GuidePage(string login)
        {
            Usuario = login;
            InitializeComponent();
            FrameSecundario.Content = new SystemsPage(Usuario);
        }

        private void Systems_Click(object sender, RoutedEventArgs e)
        {
            FrameSecundario.Content = new SystemsPage(Usuario);
            GridSupport.Background = Brushes.Transparent;
            GridSystems.Background = Brushes.BlueViolet;
           
        }

        private void Support_Click(object sender, RoutedEventArgs e)
        {
            FrameSecundario.Content = new SupportPage();
            GridSupport.Background = Brushes.BlueViolet;
            GridSystems.Background = Brushes.Transparent;
           
        }

       
    }
}
