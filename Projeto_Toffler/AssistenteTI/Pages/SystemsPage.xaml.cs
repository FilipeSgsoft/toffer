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
using Ionic.Zip;

namespace AssistenteTI.Pages
{
    /// <summary>
    /// Interação lógica para SystemsPage.xam
    /// </summary>
    public partial class SystemsPage : Page
    {
        private string Usuario { get; set; }
        public SystemsPage(string usuario)
        {
            this.Usuario = usuario;
            InitializeComponent();
            LoadSystems();
        }
        /// <summary>
        /// Método para carregar sistemas
        /// </summary>
        private void LoadSystems()
        {
            try
            {
                DataTable table = AtendimentoBLL.Instance.GetSystems(Usuario);

                foreach (DataRow row in table.Rows)
                {
                    string icon = "";
                    if (row["tipo"].ToString().Contains("web"))
                    {
                        icon = @"\Images\icons\googlechrome.png";
                    }
                    else
                    {
                        icon = @"\Images\icons\"+ row["nome"] + ".png";
                    }
                    Button button = new Button()
                    {
                        Content = new StackPanel()
                        {
                            Children =
                        {
                            new Image() { HorizontalAlignment = HorizontalAlignment.Center,VerticalAlignment = VerticalAlignment.Center,Source = new BitmapImage(new Uri(icon, UriKind.Relative)), Height = 32, Width = 46 },
                            new Label() { HorizontalAlignment = HorizontalAlignment.Center,VerticalAlignment = VerticalAlignment.Center, FontSize = 8, Width = 42, Content=row["nome"].ToString(), Foreground = Brushes.White }
                        },
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center,

                        },
                        Background = Brushes.Transparent,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        BorderBrush = Brushes.Transparent,
                        Padding = new Thickness(10, 0, 10, 0),
                        Height = 70,
                        ToolTip = row["nome"].ToString()

                    };
                    button.Click += (sender, e) =>
                    {
                        Methods.ExecuteThread(GridBackground, GridLoading, () => InitializeSystems(row["nome"].ToString(), row["url"].ToString(), row["tipo"].ToString())).Start();
                    };

                    UniformGrid.Children.Add(button);
                }

               
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Método para inicialisar os sistemas
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="url"></param>
        /// <param name="type"></param>
        private void InitializeSystems(string nome ,string url, string type)
        {
            try
            {
                if (type == "web")
                {
                    System.Diagnostics.Process.Start("chrome.exe", url);
                }
                else
                {
                    string waySystems = Environment.CurrentDirectory + "\\"+ nome + "\\" + nome + ".exe";
                    string way = Environment.CurrentDirectory + "\\instalador\\" + nome;

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
                        Methods.UpdateLoading(txtLoading, "Baixando sistema "+ nome);
                        //Baixar Programa
                        WebClient client = new WebClient();
                        client.DownloadFile(url, way + "\\install" + nome + ".exe");
                        //Instalar Programa
                        Methods.UpdateLoading(txtLoading, "Instalação do sistema " + nome);
                        InstallProgramSilent("install" + nome + ".exe", way + "\\");
                        //Executa Programa
                        Methods.UpdateLoading(txtLoading, "Executando Sistema " + nome);
                        System.Diagnostics.Process.Start(waySystems);
                    }
                }
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
    }
}

