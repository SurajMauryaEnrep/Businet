using System;
using System.Data;
using System.Data.Common;

//using System.Web.Configuration;


namespace EnRepMobileWeb.UTILITIES
{
    public class DatabaseHelper : IDisposable
    {

        private string strConnectionString;
        private DbConnection objConnection;
        private DbCommand objCommand;
        private DbProviderFactory objFactory = null;
        private bool boolHandleErrors;
        private string strLastError;
        private bool boolLogError;


        public DatabaseHelper()
        {
            //strConnectionString = WebConfigurationManager.ConnectionStrings["FiddleFitDBConnection"].ConnectionString;

            objFactory = DbProviderFactories.GetFactory("System.Data.SqlClient");

            objConnection = objFactory.CreateConnection();
            objCommand = objFactory.CreateCommand();

            objConnection.ConnectionString = strConnectionString;
            objCommand.Connection = objConnection;
        }


        public bool HandleErrors
        {
            get
            {
                return boolHandleErrors;
            }
            set
            {
                boolHandleErrors = value;
            }
        }

        public string LastError
        {
            get
            {
                return strLastError;
            }
        }

        public bool LogErrors
        {
            get
            {
                return boolLogError;
            }
            set
            {
                boolLogError = value;
            }
        }

        public DbParameter CreateParameter()
        {
            return objFactory.CreateParameter();
        }

        /// <summary>
        /// AddParameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int AddParameter(string name, object value)
        {
            DbParameter p = objFactory.CreateParameter();
            p.ParameterName = name;
            p.Value = value;
            return objCommand.Parameters.Add(p);
        }

        /// <summary>
        /// AddParameter
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public int AddParameter(DbParameter parameter)
        {
            return objCommand.Parameters.Add(parameter);
        }

        public DbCommand Command
        {
            get
            {
                return objCommand;
            }
        }

        public void BeginTransaction()
        {
            if (objConnection.State == System.Data.ConnectionState.Closed)
            {
                objConnection.Open();
            }
            objCommand.Transaction = objConnection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            objCommand.Transaction.Commit();
            objConnection.Close();
        }

        public void RollbackTransaction()
        {
            objCommand.Transaction.Rollback();
            objConnection.Close();
        }

        /// <summary>
        /// Execute Non Query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string query)
        {
            return ExecuteNonQuery(query, CommandType.Text, ConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Execute Non Query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string query, CommandType commandtype)
        {
            return ExecuteNonQuery(query, commandtype, ConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Execute Non Query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connectionstate"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string query, ConnectionState connectionstate)
        {
            return ExecuteNonQuery(query, CommandType.Text, connectionstate);
        }

        /// <summary>
        /// Execute Non Query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="commandtype"></param>
        /// <param name="connectionstate"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string query, CommandType commandtype, ConnectionState connectionstate)
        {
            objCommand.CommandText = query;
            objCommand.CommandType = commandtype;
            int i = -1;
            try
            {
                if (objConnection.State == System.Data.ConnectionState.Closed)
                {
                    objConnection.Open();
                }
                i = objCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            finally
            {
                objCommand.Parameters.Clear();
                if (connectionstate == ConnectionState.CloseOnExit)
                {
                    objConnection.Close();
                }
            }

            return i;
        }

        /// <summary>
        /// Execute Scalar
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public object ExecuteScalar(string query)
        {
            return ExecuteScalar(query, CommandType.Text, ConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Execute Scalar
        /// </summary>
        /// <param name="query"></param>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        public object ExecuteScalar(string query, CommandType commandtype)
        {
            return ExecuteScalar(query, commandtype, ConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Execute Scalar
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connectionstate"></param>
        /// <returns></returns>
        public object ExecuteScalar(string query, ConnectionState connectionstate)
        {
            return ExecuteScalar(query, CommandType.Text, connectionstate);
        }

        /// <summary>
        /// Execute Scalar
        /// </summary>
        /// <param name="query"></param>
        /// <param name="commandtype"></param>
        /// <param name="connectionstate"></param>
        /// <returns></returns>
        public object ExecuteScalar(string query, CommandType commandtype, ConnectionState connectionstate)
        {
            objCommand.CommandText = query;
            objCommand.CommandType = commandtype;
            object o = null;
            try
            {
                if (objConnection.State == System.Data.ConnectionState.Closed)
                {
                    objConnection.Open();
                }
                o = objCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            finally
            {
                objCommand.Parameters.Clear();
                if (connectionstate == ConnectionState.CloseOnExit)
                {
                    objConnection.Close();
                }
            }

            return o;
        }

        /// <summary>
        /// Execute Reader
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string query)
        {
            return ExecuteReader(query, CommandType.Text, ConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Execute Reader
        /// </summary>
        /// <param name="query"></param>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string query, CommandType commandtype)
        {
            return ExecuteReader(query, commandtype, ConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Execute Reader
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connectionstate"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string query, ConnectionState connectionstate)
        {
            return ExecuteReader(query, CommandType.Text, connectionstate);
        }

        /// <summary>
        /// Execute Reader
        /// </summary>
        /// <param name="query"></param>
        /// <param name="commandtype"></param>
        /// <param name="connectionstate"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string query, CommandType commandtype, ConnectionState connectionstate)
        {
            objCommand.CommandText = query;
            objCommand.CommandType = commandtype;
            DbDataReader reader = null;
            try
            {
                if (objConnection.State == System.Data.ConnectionState.Closed)
                {
                    objConnection.Open();
                }
                if (connectionstate == ConnectionState.CloseOnExit)
                {
                    reader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                }
                else
                {
                    reader = objCommand.ExecuteReader();
                }

            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            finally
            {
                objCommand.Parameters.Clear();
            }

            return reader;
        }

        /// <summary>
        /// Execute DataSet
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string query)
        {
            return ExecuteDataSet(query, CommandType.Text, ConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Execute DataSet
        /// </summary>
        /// <param name="query"></param>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string query, CommandType commandtype)
        {
            return ExecuteDataSet(query, commandtype, ConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Execute DataSet
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connectionstate"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string query, ConnectionState connectionstate)
        {
            return ExecuteDataSet(query, CommandType.Text, connectionstate);
        }

        /// <summary>
        /// Execute DataSet
        /// </summary>
        /// <param name="query"></param>
        /// <param name="commandtype"></param>
        /// <param name="connectionstate"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string query, CommandType commandtype, ConnectionState connectionstate)
        {
            DbDataAdapter adapter = objFactory.CreateDataAdapter();
            objCommand.CommandText = query;
            objCommand.CommandType = commandtype;
            adapter.SelectCommand = objCommand;
            DataSet ds = new DataSet();
            try
            {
                adapter.Fill(ds);
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            finally
            {
                objCommand.Parameters.Clear();
                if (connectionstate == ConnectionState.CloseOnExit)
                {
                    if (objConnection.State == System.Data.ConnectionState.Open)
                    {
                        objConnection.Close();
                    }
                }
            }
            return ds;
        }

        /// <summary>
        /// Handle Exceptions
        /// </summary>
        /// <param name="ex"></param>
        private void HandleExceptions(Exception ex)
        {
            if (LogErrors)
            {
                WriteToLog(ex.Message);
            }
            if (HandleErrors)
            {
                strLastError = ex.Message;
            }
            else
            {
                throw ex;
            }
        }

        private void WriteToLog(string msg)
        {
        }

        public void Dispose()
        {
            objConnection.Close();
            objConnection.Dispose();
            objCommand.Dispose();
        }

    }

    public enum Providers
    {
        SqlServer, OleDb, Oracle, ODBC, ConfigDefined
    }

    public enum ConnectionState
    {
        KeepOpen, CloseOnExit
    }
}
