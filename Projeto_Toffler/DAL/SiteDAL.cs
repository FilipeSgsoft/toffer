using DAL.Util;
using System;
using System.Configuration;
using System.Data;

namespace DAL
{
    public class SiteDAL : DBAHelper
    {
        #region Singleton

        private static SiteDAL _instance;

        public static SiteDAL Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(SiteDAL))
                    {
                        if (_instance == null)
                        {
                            _instance = new SiteDAL();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
        private SiteDAL() : base(Encryption.Decrypt(ConfigurationManager.ConnectionStrings["TEL_ADMIN"].ConnectionString), Provider.SqlServer) { }
        /// <summary>
        /// Método para obter lista de sites
        /// </summary>
        /// <returns></returns>
        public DataTable GetSite()
        {
            try
            {
                #region query
                string query = @"
                        SELECT
            	               SITE_NR_SEQUENCIA
                              ,SITE_TX_DESCRICAO
                        FROM   [SITE] WITH(NOLOCK)
                        ";
                #endregion

                return ExecuteDataTable(query, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
