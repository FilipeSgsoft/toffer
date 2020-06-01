using AssistenteTI.Modal;
using AssistenteTI.Util;
using BLL;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interação lógica para LoginPage.xam
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            //Carregar lista de sites
            LoadSite();
        }
        /// <summary>
        /// Método para carregar lista de sites
        /// </summary>
        private void LoadSite()
        {
            try
            {
                
                DataTable tableSites = SiteBLL.Instance.GetSite();
                cbxSites.DataContext = tableSites;
            }
            catch (Exception)
            {
                //Em caso de falha para obter lista de sites, o usuário deve analisar pra ver se está conectado na VPN
                //Instruções para o usuário
                Methods.PopUpInfoMethod(GridBackground, GridLoading,
                    "Ops! Você está sem acesso a rede! \n\nPor favor, siga as instruções abaixo: \n\n  1. Verifique se realmente está conectado na internet \n  2. Verique se está conectado na Open VPN \n  2.1 Em casos de falha na VPN entre em contato com suporte através do WhatsApp"
                     , "ModalAlertLogin");

            }
            
        }
        /// <summary>
        /// Método para autenticar no sistemas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Acessar_Click(object sender, RoutedEventArgs e)
        {
            //Validação de Campos
            if (await Methods.ValidationInput(cbxSites.Text, "site")) return;
            if (await Methods.ValidationInput(txtLogin.Text, "login")) return;

            //Armazenar valores em variaveis
            string site = ((DataRowView)cbxSites.SelectedItem).Row.ItemArray[0].ToString();;
            string login = txtLogin.Text;

            //Bloquear campos de entrada
            BlockFields();
            //Exibir modal Loading
            GridBackground.Visibility = Visibility.Visible;
            GridLoading.Visibility = Visibility.Visible;

            //Verificar se usuário existe
            if (!AutenticacaoBLL.Instance.CheckUserExists(login, site))
            {
                Methods.PopUpInfoMethod(GridBackground, GridLoading, "Usuário não encontrado, por favor verifique o login informado!", "ModalAlert");
                UnlockFields();
                return;
            }

            //Verificar se usuário esta dentro da jornada de trabalho
            if (!AutenticacaoBLL.Instance.CheckJourneyTime(login))
            {
                Methods.PopUpInfoMethod(GridBackground, GridLoading, "Você está fora da sua jornada de trabalho!", "ModalAlert");
                UnlockFields();
                return;
            }

            //Acessar sistemas
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Content = new GuidePage(login);
            
        }

        /// <summary>
        /// Método para bloquear campos
        /// </summary>
        private void BlockFields()
        {

            cbxSites.IsEnabled = false;
            txtLogin.IsEnabled = false;
            Acessar.IsEnabled = false;
            Acessar.Content = "Autenticando...";
        }
        /// <summary>s
        /// Método para desbloquear campos
        /// </summary>
        private void UnlockFields()
        {
            cbxSites.IsEnabled = true;
            txtLogin.IsEnabled = true;
            Acessar.IsEnabled = true;
            Acessar.Content = "Acessar";
        }

        private void btnConnection_Click(object sender, RoutedEventArgs e)
        {
            this.LoadSite();
        }
    }
}
