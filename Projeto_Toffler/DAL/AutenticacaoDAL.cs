using DAL.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Serializable]
    public class AutenticacaoDAL : DBAHelper
    {
        #region Singleton

        private static AutenticacaoDAL _instance;

        public static AutenticacaoDAL Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(AutenticacaoDAL))
                    {
                        if (_instance == null)
                        {
                            _instance = new AutenticacaoDAL();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        private AutenticacaoDAL() : base(Encryption.Decrypt(ConfigurationManager.ConnectionStrings["ASIMOV"].ConnectionString), Provider.SqlServer) { }

        /// <summary>
        /// Método para obter carga horária do usuario
        /// </summary>
        /// <param name="loginPlataform"></param>
        /// <returns></returns>
        public DataTable CheckJourneyTime(string loginPlataform)
        {
            try
            {
                var param = new List<SqlParameter>
                {
                    new SqlParameter("@login",loginPlataform),
                };

               return ExecuteDataTable("SP_OBTER_JORNADA", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
