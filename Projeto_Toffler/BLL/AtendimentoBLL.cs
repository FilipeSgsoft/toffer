using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    [Serializable]
    public class AtendimentoBLL
    {
        #region Singleton

        private static AtendimentoBLL _instance;

        public static AtendimentoBLL Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(AtendimentoBLL))
                    {
                        if (_instance == null)
                        {

                            _instance = new AtendimentoBLL();


                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
        //Entidades utilizadas por essa classe
        private readonly AtendimentoDAL _atendimentoDal;

        /// <summary>
        ///     Construtor Estatico
        ///     Faz parte do Singleton. Inicialize aqui os objetos uteis
        /// </summary>
        private AtendimentoBLL()
        {
            _atendimentoDal = AtendimentoDAL.Instance;
        }

        /// <summary>
        /// Método para obter lista de problemas do Home Office
        /// </summary>
        /// <returns></returns>
        public DataTable GetProblem()
        {
            return _atendimentoDal.GetProblem();
        }

        /// <summary>
        /// Método para obter lista de sistemas do usuario
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public DataTable GetSystems(string login)
        {
            return _atendimentoDal.GetSystems(login);
        }
    }
}
