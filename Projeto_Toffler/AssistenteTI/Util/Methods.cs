using AssistenteTI.Modal;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AssistenteTI.Util
{
    public static class Methods
    {
        /// <summary>
        /// Método para validar preenchimento de campo
        /// </summary>
        /// <param name="param"></param>
        /// <param name="campo"></param>
        /// <returns></returns>
        public static async Task<bool> ValidationInput(string param, string campo)
        {
            if (param == "")
            {
                InfoNotificationMessage msg = new InfoNotificationMessage();
                msg.Message = "Por favor, preenchar o campo " + campo + "!";

                await DialogHost.Show(msg, "ModalAlert");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Método para chamar pop up
        /// </summary>
        /// <param name="background"></param>
        /// <param name="loading"></param>
        /// <param name="message"></param>
        public static void PopUpInfoMethod(Grid background, Grid loading, string message, string popup)
        {
            Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(async () =>
            {
                background.Visibility = Visibility.Hidden;
                loading.Visibility = Visibility.Hidden;

                DialogHost.CloseDialogCommand.Execute(new object(), null);
               
                try
                {
                    //Em caso de informação
                    InfoNotificationMessage msg = new InfoNotificationMessage();
                    msg.Message = message;
                    await DialogHost.Show(msg, popup);
                }
                catch (Exception)
                {


                }

                
            }));
        }
        /// <summary>
        /// Método para carregar text Loading
        /// </summary>
        /// <param name="control"></param>
        /// <param name="msg"></param>
        public static void UpdateLoading(TextBlock control, string msg)
        {
            Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(async () =>
            {
                control.Text = msg;
            }));
        }
        /// <summary>
        /// Método para executar uma Thread
        /// </summary>
        /// <param name="background"></param>
        /// <param name="loading"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static Thread ExecuteThread(Grid background, Grid loading, Action method)
        {
            Thread thread = new Thread(() => {
                try
                {
                    Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(async () =>
                    {
                        //Apresentar Grid
                        background.Visibility = Visibility.Visible;
                        loading.Visibility = Visibility.Visible;
                    }));

                    method();

                    Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(async () =>
                    {
                        //Apresentar Grid
                        background.Visibility = Visibility.Hidden;
                        loading.Visibility = Visibility.Hidden;
                    }));


                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(async () =>
                    {
                        //Ocultar Grid
                        background.Visibility = Visibility.Hidden;
                        loading.Visibility = Visibility.Hidden;
                    }));
                    //Exibir pop up de Error
                    PopUpInfoMethod(background, loading, ex.Message.ToString(), "Error");
                }

            });
            return thread;
        }
    }
}
