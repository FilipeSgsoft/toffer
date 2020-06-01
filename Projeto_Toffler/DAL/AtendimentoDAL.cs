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
    public class AtendimentoDAL : DBAHelper
    {
        #region Singleton

        private static AtendimentoDAL _instance;

        public static AtendimentoDAL Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(AtendimentoDAL))
                    {
                        if (_instance == null)
                        {
                            _instance = new AtendimentoDAL();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        private AtendimentoDAL() : base(Encryption.Decrypt(ConfigurationManager.ConnectionStrings["ASIMOV"].ConnectionString), Provider.SqlServer) { }

        /// <summary>
        /// Método para obter lista de problemas do Home Office
        /// </summary>
        /// <returns></returns>
        public DataTable GetProblem()
        {
            try
            {
                var query = @"SELECT * FROM Hackaton.dbo.tipo_problema (nolock)";


             

                return ExecuteDataTable(query, CommandType.Text);

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// Método para obter lista de sistemas do usuario
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public DataTable GetSystems(string login)
        {
            try
            {
                var query = @"SELECT s.nome,s.url,'web' as tipo
FROM Asimov.dbo.usuario AS u (NOLOCK)
LEFT JOIN Asimov.dbo.grupo_sistema AS gs (NOLOCK) ON gs.codgrupo = u.codgrupo
LEFT JOIN Asimov.dbo.sistema AS s (NOLOCK) ON s.codsistema = gs.codsistema
WHERE u.[login] = @login

UNION ALL 
SELECT nome, url, 'Desktop' as tipo  from Hackaton.dbo.desktopSystems (nolock)
                           ";


                var param = new List<SqlParameter>
                {
                    new SqlParameter("@login",login),
                };

                return ExecuteDataTable(query, CommandType.Text, param);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
