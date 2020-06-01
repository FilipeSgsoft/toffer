using DAL.Util;
using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
namespace DAL
{
    public class FuncionarioDAL : DBAHelper
    {
        
        public FuncionarioDAL() : base(Encryption.Decrypt(ConfigurationManager.ConnectionStrings["TEL_ADMIN"].ConnectionString), Provider.SqlServer) { }

        public DataTable GetUser(string loginPlataform, string site)
        {
            try
            {
                var query = @"SELECT MATRICULA, NOME, LOGIN_REDE_TEL, LOGIN_PLATAFORMA, LOGIN_TELEFONIA , [STATUS]
                            FROM dbo.FUNCIONARIO 
                            WHERE LOGIN_REDE_TEL LIKE  '%'+	@login+'%' 
                            AND  SITE_NR_SEQUENCIA = @site";

                var param = new List<SqlParameter>
                {
                    new SqlParameter("@login",loginPlataform),
                    new SqlParameter("@site",site),
                };

                return ExecuteDataTable(query, CommandType.Text, param);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
