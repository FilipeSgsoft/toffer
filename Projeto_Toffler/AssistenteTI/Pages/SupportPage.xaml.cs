using AssistenteTI.Util;
using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
    /// Interação lógica para SupportPage.xam
    /// </summary>
    public partial class SupportPage : Page
    {
        public SupportPage()
        {
            InitializeComponent();
            LoadProgram();
        }
        /// <summary>
        /// Método para carregar lista de problemas
        /// </summary>
        private void LoadProgram()
        {
            try
            {
                DataTable tableProgram = AtendimentoBLL.Instance.GetProblem();
                cbxTipoTratativa.DataContext = tableProgram;
            }
            catch (Exception)
            {
             

            }

        }
        private async void ResolverCaso_Click(object sender, RoutedEventArgs e)
        {
            //Obter procesimento
            //Validação de Campos
            if (await Methods.ValidationInput(cbxTipoTratativa.Text, "problema")) return;
            //Armazenar valores em variaveis
            string cod = ((DataRowView)cbxTipoTratativa.SelectedItem).Row.ItemArray[0].ToString();
            string urlProblem = ((DataRowView)cbxTipoTratativa.SelectedItem).Row.ItemArray[2].ToString();

            //Execute método
            Methods.ExecuteThread(GridBackground, GridLoading, () => resolveProblem(cod, urlProblem)).Start();
        }
        /// <summary>
        /// Método para resolver problema de suporte
        /// </summary>
        /// <param name="problem"></param>
        /// <param name="urlProblem"></param>
        private void resolveProblem(string cod, string urlProblem)
        {
            try
            {
                
                string waySystems = Environment.CurrentDirectory + "\\" + cod + "\\" + cod + ".exe";
                string way = Environment.CurrentDirectory + "\\instalador\\" + cod;
                //Atualizar Label
                Methods.UpdateLoading(txtLoading, "Aguarde estou tentando resolver!");

                //Verificar se sistema está na máquina
                if (!Directory.Exists(way))
                {
                    DirectoryInfo di = Directory.CreateDirectory(way);
                }

                FileInfo file = new FileInfo(waySystems);
                if (file.Exists)
                {
                    System.Diagnostics.Process.Start(waySystems);
                }
                else
                {
                    //Baixar Programa
                    WebClient client = new WebClient();
                    client.DownloadFile(urlProblem, way + "\\support" + cod + ".exe");
                    //Instalar Programa
                    Methods.UpdateLoading(txtLoading, "Executanto procedimento padrão");
                    InstallProgramSilent("support" + cod + ".exe", way + "\\");
                }
                Methods.PopUpInfoMethod(GridBackground, GridLoading, "PRONTO! Por favor, verifique se o problema foi resolvido!", "ModalAlertSupport");
                //Carregar tela de Feedback
                Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(async () =>
                {
                    CardIdenticacaoProblema.Visibility = Visibility.Hidden;
                    CardFeedbackProblema.Visibility = Visibility.Visible;
                }));
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Método para instalação silenciosa
        /// </summary>
        /// <param name="nomeArquivo"></param>
        /// <param name="caminho"></param>
        public void InstallProgramSilent(string nomeArquivo, string caminho)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = System.IO.Path.GetFullPath(System.IO.Path.Combine(caminho, nomeArquivo));
                startInfo.Arguments = "/S";
                startInfo.WorkingDirectory = Environment.CurrentDirectory;
                startInfo.WindowStyle = ProcessWindowStyle.Normal;
                startInfo.UseShellExecute = true;
                Process exeProcess = Process.Start(startInfo);
                exeProcess.EnableRaisingEvents = true;
                while (exeProcess.HasExited == false)
                {
                    exeProcess.WaitForExit(90000);
                    System.Threading.Thread.Sleep(250);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void ConfirmaFeedBack_Click(object sender, RoutedEventArgs e)
        {
            CardIdenticacaoProblema.Visibility = Visibility.Visible;
            CardFeedbackProblema.Visibility = Visibility.Hidden;
        }
    }
}
