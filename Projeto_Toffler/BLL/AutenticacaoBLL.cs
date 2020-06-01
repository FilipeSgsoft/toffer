using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{

    [Serializable]
    public class AutenticacaoBLL
    {
        #region Singleton

        private static AutenticacaoBLL _instance;

        public static AutenticacaoBLL Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(AutenticacaoBLL))
                    {
                        if (_instance == null)
                        {

                            _instance = new AutenticacaoBLL();


                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
        //Entidades utilizadas por essa classe
        private readonly AutenticacaoDAL _autenticacaoDal;
        private readonly FuncionarioDAL _funcionarioDal;

        /// <summary>
        ///     Construtor Estatico
        ///     Faz parte do Singleton. Inicialize aqui os objetos uteis
        /// </summary>
        private AutenticacaoBLL()
        {
            _autenticacaoDal = AutenticacaoDAL.Instance;
            _funcionarioDal = new FuncionarioDAL();
        }

        /// <summary>
        /// Método para validar se o usuário informa está correto
        /// </summary>
        /// <param name="loginPlataform"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public bool CheckUserExists(string loginPlataform, string site)
        {
            try
            {
                //Obter dados do usuário
                DataTable data = _funcionarioDal.GetUser(loginPlataform, site);
                //Verficar se usuário está cadastrado no banco
                if(data.Rows.Count == 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Método para verificar se usuario está dentro da carga horária
        /// </summary>
        /// <param name="loginPlataforma"></param>
        /// <returns></returns>
        public bool CheckJourneyTime(string loginPlataform)
        {
            try
            {
                //Obter jornada do usuario
                DataTable data = _autenticacaoDal.CheckJourneyTime(loginPlataform);

                if(data.Rows.Count == 0)
                {
                    return false;
                }

                var startJourney= DateTime.ParseExact(data.Rows[0]["ini_jornada"].ToString(), "HH:mm", CultureInfo.InvariantCulture);
                var endJourney = DateTime.ParseExact(data.Rows[0]["fim_jornada"].ToString(), "HH:mm", CultureInfo.InvariantCulture);
                var startInterval = DateTime.ParseExact(data.Rows[0]["ini_intervalo"].ToString(), "HH:mm", CultureInfo.InvariantCulture);
                var endInterval = DateTime.ParseExact(data.Rows[0]["fim_intervalo"].ToString(), "HH:mm", CultureInfo.InvariantCulture);
                var hora_atual_jornada = DateTime.ParseExact(data.Rows[0]["hora_atual"].ToString(), "HH:mm", CultureInfo.InvariantCulture);

                // Hora Atual - É pego diretamente do  banco de dados para evitar fraude alterando o horário da máquina
                // Foi solicitado que existisse uma "sobra", então foi definido 5 minutos a mais para o usuário efetuar o login
                TimeSpan ts_hora_atual = new TimeSpan(hora_atual_jornada.Hour, hora_atual_jornada.AddMinutes(+5).Minute, 00);
                TimeSpan ts_ini_jornada = new TimeSpan(startJourney.Hour, startJourney.Minute, 00);
                TimeSpan ts_fim_jornada = new TimeSpan(endJourney.Hour, endJourney.Minute, 00);

                // Intervalo
                TimeSpan ts_ini_intervalo = new TimeSpan(startInterval.Hour, startInterval.Minute, 00);
                TimeSpan ts_fim_intervalo = new TimeSpan(endInterval.Hour, endInterval.Minute, 00);


                // Verifica se está dentro de sua jornada de trabalho
                if (ts_hora_atual >= ts_ini_jornada) return true;

                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
