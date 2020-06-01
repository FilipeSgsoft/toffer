using AssistenteTI.Pages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AssistenteTI
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            NotifyIcon nIcon = new NotifyIcon();
            nIcon.ShowBalloonTip(5000, "Olá", "Bem vindo ao Toffler! :)", ToolTipIcon.Info);
            FramePrincipal.Content = new LoginPage();
        }

        private void ButtonMinimized_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void FocarJanela()
        {

         

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
