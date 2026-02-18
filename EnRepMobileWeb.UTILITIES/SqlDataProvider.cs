using System.Data.SqlClient;
using System.Data;

namespace EnRepMobileWeb.UTILITIES
{
    public class SqlDataProvider
    {
        public SqlDataProvider()
        {
        }

        /// <summary>
        /// Create Parameter
        /// </summary>
        /// <param name="strParameterName"></param>
        /// <param name="objType"></param>
        /// <returns></returns>
        public SqlParameter CreateParameter(string strParameterName, DbType objType)
        {
            SqlParameter objParameter = new SqlParameter();
            objParameter.ParameterName = strParameterName;

            switch (objType)
            {
                case DbType.Int32:
                    {
                        objParameter.SqlDbType = SqlDbType.Int;
                        break;
                    }
                case DbType.Single:
                    {
                        objParameter.SqlDbType = SqlDbType.Real;
                        break;
                    }
                case DbType.Boolean:
                    {
                        objParameter.SqlDbType = SqlDbType.Bit;
                        break;
                    }
                case DbType.String:
                    {
                        objParameter.SqlDbType = SqlDbType.NVarChar;
                        break;
                    }
                case DbType.DateTime:
                    {
                        objParameter.SqlDbType = SqlDbType.DateTime;
                        break;
                    }
                case DbType.Int64:
                    {
                        objParameter.SqlDbType = SqlDbType.BigInt;
                        break;
                    }
                case DbType.Currency:
                    {
                        objParameter.SqlDbType = SqlDbType.Money;
                        break;
                    }
                case DbType.Object:
                    {
                        objParameter.SqlDbType = SqlDbType.Text;
                        break;
                    }

            }

            return objParameter;
        }

        /// <summary>
        /// Create Parameter
        /// </summary>
        /// <param name="strParameterName"></param>
        /// <param name="objType"></param>
        /// <param name="intSize"></param>
        /// <returns></returns>
        public SqlParameter CreateParameter(string strParameterName, DbType objType, int intSize)
        {
            SqlParameter objParameter = (SqlParameter)CreateParameter(strParameterName, objType);
            objParameter.Size = intSize;
            return objParameter;

        }

        /// <summary>
        /// CreateInitialized Parameter
        /// </summary>
        /// <param name="strParameterName"></param>
        /// <param name="objType"></param>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public SqlParameter CreateInitializedParameter(string strParameterName, DbType objType, object objValue)
        {
            SqlParameter objParameter = (SqlParameter)CreateParameter(strParameterName, objType);
            objParameter.Value = objValue;
            return objParameter;

        }
        /// <summary>
        /// Create initialized table type parameter
        /// </summary>
        /// <param name="strParameterName"></param>
        /// <param name="objType"></param>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public SqlParameter CreateInitializedParameterTableType(string strParameterName, SqlDbType objType, object objValue)
        {
            SqlParameter objParameter = new SqlParameter(strParameterName, objValue);
            objParameter.SqlDbType = objType;
            return objParameter;

        }

        /// <summary>
        /// CreateInitialized Parameter
        /// </summary>
        /// <param name="strParameterName"></param>
        /// <param name="objType"></param>
        /// <param name="intSize"></param>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public SqlParameter CreateInitializedParameter(string strParameterName, DbType objType, int intSize, object objValue)
        {
            SqlParameter objParameter = (SqlParameter)CreateParameter(strParameterName, objType, intSize);
            objParameter.Value = objValue;
            return objParameter;
        }
    
    }
}
