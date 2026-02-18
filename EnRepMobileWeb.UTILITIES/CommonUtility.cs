using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace EnRepMobileWeb.UTILITIES
{
    public class CommonUtility<T>
    {
        List<T> listOfObject = new List<T>();

        /// <summary>
        /// Common Utility to set properties.
        /// </summary>
        /// <param name="reader_"></param>
        /// <param name="objectToFill_"></param>
        /// <returns></returns>
        public static List<T> SetProperties(SqlDataReader reader_, T objectToFill_)
        {
            List<T> objList = new List<T>();

            while (reader_.Read())
            {
                int propertyCounter = 0;
                T obj = (T)Activator.CreateInstance(objectToFill_.GetType());

                foreach (var property in objectToFill_.GetType().GetProperties())
                {
                    {
                        if (reader_.VisibleFieldCount > propertyCounter)
                        {
                            if (property.Name.Equals(reader_.GetName(propertyCounter), StringComparison.OrdinalIgnoreCase))
                            {
                                if (reader_.GetValue(propertyCounter) != DBNull.Value)
                                {
                                    property.SetValue(obj, Convert.ChangeType(reader_.GetValue(propertyCounter), property.PropertyType), null);

                                }
                                propertyCounter++;
                            }
                        }
                        else
                        {
                            objList.Add(obj);
                            obj = (T)Activator.CreateInstance(objectToFill_.GetType());
                            propertyCounter = 0;
                            break;
                        }
                    }
                }
            }
            return objList;
        }


        public List<string> DataSetToList(DataTable Dt)
        {
            List<string> _list = new List<string>();
            foreach (DataRow row in Dt.Rows)
            {

            }
            return _list;
        }
    }
}
