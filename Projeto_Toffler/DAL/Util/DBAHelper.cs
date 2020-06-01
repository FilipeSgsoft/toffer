using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.Util
{
    public abstract class DBAHelper
    {
        /// <summary>
        ///     verifica o Cliente se é SQLserver,oracle,MySQL,postGre
        /// </summary>
        public enum Provider
        {
            [ValueEnumerator("System.Data.SqlClient")]
            SqlServer,

            [ValueEnumerator("System.Data.OracleClient")]
            Oracle,

            [ValueEnumerator("System.Data.MySqlClient")]
            MySql,

            [ValueEnumerator("Npgsql")]
            PostGreSql
        }

        private DbCommand _command;
        private DbConnection _connection;
        //Metodo generico de conexão com qualquer banco de dados.
        //seja Oracle,SQL Server,MySQl entre outros.

        private DbProviderFactory _factory;

        protected DBAHelper()
        {
        }

        protected DBAHelper(string conexaoString, Provider provider)
        {
            ConfiguringAccessBank(conexaoString, provider);
        }

        public List<DbParameter> ListParameter { get; private set; }

        public DbParameter CreateDbParameter(string parameterName, object valueParam)
        {
            var aux = _factory.CreateParameter();

            if (aux == null)
                return null;

            aux.Value = valueParam;
            aux.ParameterName = parameterName;

            return aux;
        }

        /// <summary>
        ///     Seta configurações padrão para do servidor
        /// </summary>
        /// <param name="enumeratorProvider"></param>
        /// <returns></returns>
        private string ClipValueEnumerator(Enum enumeratorProvider)
        {
            var type = enumeratorProvider.GetType();

            var fieldInfo = type.GetField(enumeratorProvider.ToString());

            var attributes = fieldInfo.GetCustomAttributes(typeof(ValueEnumerator), false)
                as ValueEnumerator[];

            if (attributes != null)
                return attributes.Length > 0 ? attributes[0].Value : null;

            return null;
        }

        /// <summary>
        ///     Onde configura o Acesso ao Banco.
        /// </summary>
        /// <param name="stringConnection"></param>
        /// <param name="provider"></param>
        private void ConfiguringAccessBank(string stringConnection, Provider provider)
        {
            try
            {
                // aqui pega o tipo do provider (Caso escolha: 'Provider.PostGreSQL' adicione o provider no webconfig.)
                var providerData = ClipValueEnumerator(provider);

                // associa o provider a fabrica
                _factory = DbProviderFactories.GetFactory(providerData);

                // a fabrica entao cria o objeto de conexao
                _connection = _factory.CreateConnection();

                // associa a string de conexao a ele
                if (_connection != null)
                {
                    _connection.ConnectionString = stringConnection;

                    //.. e comando
                    _command = _connection.CreateCommand();
                }

                // cria a lista de parametros
                ListParameter = new List<DbParameter>();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao configurar banco de dados: " + ex.Message);
            }
        }

        /// <summary>
        ///     Abre Conexão
        /// </summary>
        private void OpenConnection()
        {
            if (_connection == null) return;
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        /// <summary>
        ///     Feixa conexão
        /// </summary>
        private void CloseConnection()
        {
            if (_connection == null) return;
            if (_command.Transaction != null) return;
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        /// <summary>
        ///     Limpa a lista de paramentros
        /// </summary>
        private void ClearParameters()
        {
            if (ListParameter.Count > 0)
                ListParameter.Clear();

            if (_command.Parameters.Count > 0)
                _command.Parameters.Clear();
        }

        /// <summary>
        ///     Adiciona a lista de paramentro
        /// </summary>
        /// <param name="nameParameter"></param>
        /// <param name="valueParameter"></param>
        /// <param name="parameterExit"></param>
        protected void AddParameters(string nameParameter, object valueParameter, bool parameterExit)
        {
            var par = _factory.CreateParameter();

            if (par == null) return;

            par.ParameterName = nameParameter;
            par.Value = valueParameter ?? DBNull.Value;

            // se for do tipo saida...
            if (parameterExit)
                par.Direction = ParameterDirection.InputOutput;

            ListParameter.Add(par);
        }

        protected void AddParameters(string nameParameter, object valueParameter, bool parameterExit, SqlCommand commandtranacional)
        {
            var par = new SqlParameter
            {
                ParameterName = nameParameter,
                Value = valueParameter ?? DBNull.Value
            };

            // se for do tipo saida...
            if (parameterExit)
                par.Direction = ParameterDirection.InputOutput;

            commandtranacional.Parameters.Add(par);
        }

        /// <summary>
        ///     Executa um query e retorna um datatable
        /// </summary>
        /// <param name="query"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected DataTable ExecuteDataTable(string query, CommandType type)
        {
            var objDataTable = new DataTable();
            try
            {
                if (_command != null)
                {
                    _command.CommandText = query;
                    _command.CommandType = type;

                    using (var adapter = _factory.CreateDataAdapter())
                    {
                        // se existir parametro...
                        if (ListParameter.Count > 0)
                        {
                            // percorre a lista e adiciona no command
                            ListParameter.ForEach(p => _command.Parameters.Add(p));
                        }

                        if (adapter != null)
                        {
                            adapter.SelectCommand = _command;
                            adapter.Fill(objDataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar dados: " + ex.Message);
            }
            finally
            {
                ClearParameters();
                CloseConnection();
            }

            return objDataTable;
        }

        protected DataTable ExecuteDataTable(string query, CommandType type, SqlCommand commandTransacional)
        {
            var objDataTable = new DataTable();

            try
            {
                if (commandTransacional != null)
                {
                    commandTransacional.CommandText = query;
                    commandTransacional.CommandType = type;

                    using (var adaptertransaction = new SqlDataAdapter(commandTransacional))
                    {
                        adaptertransaction.Fill(objDataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar dados: " + ex.Message);
            }
            finally
            {
                ClearParameters();
                CloseConnection();
            }

            return objDataTable;
        }

        /// <summary>
        ///     Executa um query e retorna um datatable
        /// </summary>
        /// <param name="query"></param>
        /// <param name="timeoutSegundos"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected DataTable ExecuteDataTable(string query, int timeoutSegundos, CommandType type)
        {
            var objDataTable = new DataTable();
            try
            {
                if (_command != null)
                {
                    _command.CommandText = query;
                    _command.CommandType = type;
                    _command.CommandTimeout = timeoutSegundos;

                    using (var adapter = _factory.CreateDataAdapter())
                    {
                        // se existir parametro...
                        if (ListParameter.Count > 0)
                        {
                            // percorre a lista e adiciona no command
                            ListParameter.ForEach(p => _command.Parameters.Add(p));
                        }

                        if (adapter != null)
                        {
                            adapter.SelectCommand = _command;
                            adapter.Fill(objDataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar dados: " + ex.Message);
            }
            finally
            {
                ClearParameters();
                CloseConnection();
            }

            return objDataTable;
        }

        /// <summary>
        ///     Retorna um datatable e a conexao e compartilhada.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="type"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        protected DataTable ExecuteDataTable(string query, CommandType type, List<DbParameter> parametros)
        {
            var objDataTable = new DataTable();

            try
            {
                //Instancia um novo comando segundo a abstracao
                using (var comando = _connection.CreateCommand())
                {
                    comando.CommandText = query;
                    comando.CommandType = type;

                    using (var adapter = _factory.CreateDataAdapter())
                    {
                        // se existir parametro...
                        if (parametros.Count > 0)
                        {
                            foreach (var item in parametros)
                            {
                                //Caso o valor seja NULL Converte para DBNull.value (equivalente a NULL no banco)
                                if (item.Value == null)
                                {
                                    item.Value = DBNull.Value;
                                }

                                comando.Parameters.Add(item);
                            }
                        }

                        if (adapter != null)
                        {
                            adapter.SelectCommand = comando;
                            adapter.Fill(objDataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar dados: " + ex.Message);
            }

            return objDataTable;
        }

        /// <summary>
        ///     Retorna um datatable e a conexao e compartilhada.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="type"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        protected DataTable ExecuteDataTable(string query, CommandType type, List<SqlParameter> parametros)
        {
            var objDataTable = new DataTable();

            try
            {
                //Instancia um novo comando segundo a abstracao
                using (var comando = _connection.CreateCommand())
                {
                    comando.CommandText = query;
                    comando.CommandType = type;
                    comando.CommandTimeout = 1200;

                    using (var adapter = _factory.CreateDataAdapter())
                    {
                        // se existir parametro...
                        if (parametros.Count > 0)
                        {
                            foreach (var item in parametros)
                            {
                                //Caso o valor seja NULL Converte para DBNull.value (equivalente a NULL no banco)
                                if (item.Value == null)
                                {
                                    item.Value = DBNull.Value;
                                }

                                comando.Parameters.Add(item);
                            }
                        }

                        if (adapter != null)
                        {
                            adapter.SelectCommand = comando;
                            OpenConnection();
                            adapter.Fill(objDataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar dados: " + ex.Message);
            }
            finally
            {
                ClearParameters();
                CloseConnection();
            }

            return objDataTable;
        }


        /// <summary>
        ///     Executa uma query com base na conexao do contrutor
        /// </summary>
        /// <param name="query"></param>
        /// <param name="type"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        protected int ExecuteQuery(string query, CommandType type, List<DbParameter> parametros)
        {
            int _return;

            try
            {
                //Instancia um novo comando segundo a abstracao
                using (var comando = _connection.CreateCommand())
                {
                    comando.CommandText = query;
                    comando.CommandType = type;

                    // se existir parametro...
                    if (parametros.Count > 0)
                    {
                        foreach (var item in parametros)
                        {
                            if (item.Value == null)
                            {
                                item.Value = DBNull.Value;
                            }

                            comando.Parameters.Add(item);
                        }
                    }


                    OpenConnection();
                    _return = comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar comando SQL: " + ex.Message);
            }
            finally
            {
                ClearParameters();
                CloseConnection();
            }

            return _return;
        }

        /// <summary>
        ///     Executa uma query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected int ExecuteQuery(string query, CommandType type)
        {
            var _return = 0;

            try
            {
                if (_command != null)
                {
                    _command.CommandText = query;
                    _command.CommandType = type;

                    if (ListParameter.Count > 0)
                    {
                        // percorre a lista e adiciona no command
                        ListParameter.ForEach(p => _command.Parameters.Add(p));
                    }

                    OpenConnection();
                    _return = _command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar comando SQL: " + ex.Message);
            }
            finally
            {
                ClearParameters();
                CloseConnection();
            }

            return _return;
        }

        protected int ExecuteQuery(string query, CommandType type, SqlCommand commandTransactional)
        {
            var _return = 0;

            try
            {
                if (commandTransactional != null)
                {
                    commandTransactional.CommandText = query;
                    commandTransactional.CommandType = type;

                    OpenConnection();
                    _return = commandTransactional.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar comando SQL: " + ex.Message);
            }
            finally
            {
                ClearParameters();
                CloseConnection();
            }

            return _return;
        }

        /// <summary>
        ///     Executa uma query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="timeoutSegundos"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected int ExecuteQuery(string query, int timeoutSegundos, CommandType type)
        {
            var _return = 0;

            try
            {
                if (_command != null)
                {
                    _command.CommandText = query;
                    _command.CommandType = type;
                    _command.CommandTimeout = timeoutSegundos;

                    if (ListParameter.Count > 0)
                    {
                        // percorre a lista e adiciona no command
                        ListParameter.ForEach(p => _command.Parameters.Add(p));
                    }

                    OpenConnection();
                    _return = _command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar comando SQL: " + ex.Message);
            }
            finally
            {
                ClearParameters();
                CloseConnection();
            }

            return _return;
        }


        protected int ExecuteQuery(string query, int timeoutSegundos, CommandType type, List<SqlParameter> parametros)
        {
            int _return;

            try
            {
                //Instancia um novo comando segundo a abstracao
                using (var comando = _connection.CreateCommand())
                {
                    comando.CommandText = query;
                    comando.CommandType = type;
                    comando.CommandTimeout = timeoutSegundos;

                    // se existir parametro...
                    if (parametros.Count > 0)
                    {
                        foreach (var item in parametros)
                        {
                            if (item.Value == null)
                            {
                                item.Value = DBNull.Value;
                            }

                            comando.Parameters.Add(item);
                        }
                    }

                    OpenConnection();
                    _return = comando.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao executar comando SQL: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar comando SQL: " + ex.Message);
            }
            finally
            {
                ClearParameters();
                CloseConnection();
            }
            return _return;
        }

        /// <summary>
        ///     Executa uma query com base na conexao do contrutor
        /// </summary>
        /// <param name="query"></param>
        /// <param name="type"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        protected int ExecuteQuery(string query, CommandType type, List<SqlParameter> parametros)
        {
            int _return;

            try
            {
                //Instancia um novo comando segundo a abstracao
                using (var comando = _connection.CreateCommand())
                {
                    comando.CommandText = query;
                    comando.CommandType = type;

                    // se existir parametro...
                    if (parametros.Count > 0)
                    {
                        foreach (var item in parametros)
                        {
                            if (item.Value == null)
                            {
                                item.Value = DBNull.Value;
                            }

                            comando.Parameters.Add(item);


                        }
                    }

                    OpenConnection();
                    _return = comando.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao executar comando SQL: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar comando SQL: " + ex.Message);
            }
            finally
            {
                ClearParameters();
                CloseConnection();
            }

            return _return;
        }

        /// <summary>
        ///     é mas ultil para Procedures
        /// </summary>
        /// <param name="query"></param>
        /// <param name="type"></param>
        /// <param name="commandTransacional"></param>
        /// <returns></returns>
        protected object ExecuteScalar(string query, CommandType type, SqlCommand commandTransacional)
        {
            object _return = null;
            var nameParameter = string.Empty;

            try
            {
                if (commandTransacional != null)
                {
                    commandTransacional.CommandText = query;
                    commandTransacional.CommandType = type;

                    //OpenConnection();
                    commandTransacional.ExecuteScalar();

                    // bom utilizar com SP que retornem resultados do insert ou update por exemplo...
                    // aqui retorna o valor vinda de uma SP, ex: SET @Codigo = SCOPE_IDENTITY()
                    // nesse caso retorna o codigo apos fazer o insert no banco
                    if (nameParameter != string.Empty)
                        _return = _command.Parameters[nameParameter].Value;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar comando SQL: " + ex.Message);
            }
            finally
            {
                ClearParameters();
                CloseConnection();
            }

            return _return;
        }

        protected object ExecuteScalar(string query, CommandType type, List<SqlParameter> parametros)
        {
            object _return;
            //string nameParameter = string.Empty;

            try
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = type;
                    //command.Parameters[nameParameter].Direction = ParameterDirection.InputOutput;

                    // se existir parametro...
                    if (parametros.Count > 0)
                    {
                        foreach (var item in parametros)
                        {
                            if (item.Value == null)
                            {
                                item.Value = DBNull.Value;
                            }

                            command.Parameters.Add(item);
                        }
                    }

                    OpenConnection();
                    return command.ExecuteScalar();

                    // bom utilizar com SP que retornem resultados do insert ou update por exemplo...
                    // aqui retorna o valor vinda de uma SP, ex: SET @Codigo = SCOPE_IDENTITY()
                    // nesse caso retorna o codigo apos fazer o insert no banco
                    //if (nameParameter != null)
                    //    _return = _command.Parameters[nameParameter].Value;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar comando SQL: " + ex.Message);
            }
            finally
            {
                ClearParameters();
                CloseConnection();
            }

            return _return;
        }

        protected object ExecuteScalar(string query, CommandType type)
        {
            object _return;

            try
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = type;

                    OpenConnection();
                    return command.ExecuteScalar();

                    // bom utilizar com SP que retornem resultados do insert ou update por exemplo...
                    // aqui retorna o valor vinda de uma SP, ex: SET @Codigo = SCOPE_IDENTITY()
                    // nesse caso retorna o codigo apos fazer o insert no banco
                    //if (nameParameter != null)
                    //    _return = _command.Parameters[nameParameter].Value;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar comando SQL: " + ex.Message);
            }
            finally
            {
                ClearParameters();
                CloseConnection();
            }
        }

        /// <summary>
        ///     Obtém do webconfig o linkedServer integrado ao vocalcom
        /// </summary>
        /// <param name="banco"></param>
        /// <param name="tabela"></param>
        /// <returns></returns>
        public string GetTableNameLikedServerHermes(string banco, string tabela)
        {
            var linkServer = (string)new AppSettingsReader().GetValue("LinkedServerVocalcom", typeof(string));
            var linkServerUser = (string)new AppSettingsReader().GetValue("LinkedServerVocalcomUser", typeof(string));

            return linkServer + "." + banco + "." + linkServerUser + "." + tabela;
        }

        private class ValueEnumerator : Attribute
        {
            public ValueEnumerator(string _value)
            {
                Value = _value;
            }

            public string Value { get; }
        }
    }
}
