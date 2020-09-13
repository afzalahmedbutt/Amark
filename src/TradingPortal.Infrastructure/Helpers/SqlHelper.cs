using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace TradingPortal.Infrastructure.Helpers
{
    public class SqlHelper
    {
        /// <summary>
        /// Creates Sql Parameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        
        public static SqlParameter Parameter(string name, object value, SqlDbType type, ParameterDirection direction = ParameterDirection.Input)
        {
            return new SqlParameter { ParameterName = name, Value = value, SqlDbType = type, Direction = direction};
        }

        /// <summary>
        /// Creates Sql Parameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        
        public static SqlParameter Parameter(string name, string value)
        {
            return Parameter(name, value, SqlDbType.NVarChar);
        }

        /// <summary>
        /// Creates Sql Parameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        
        public static SqlParameter Parameter(string name, int value)
        {
            return Parameter(name, value, SqlDbType.Int);
        }

        /// <summary>
        /// Creates Sql Parameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        
        public static SqlParameter Parameter(string name, bool value)
        {
            return Parameter(name, value ? 1 : 0, SqlDbType.TinyInt);
        }

        /// <summary>
        /// Creates Sql Parameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        
        public static SqlParameter Parameter(string name, object value)
        {

            return new SqlParameter { ParameterName = name, Value = value ?? DBNull.Value, Direction = ParameterDirection.Input, IsNullable = value == null };
            ;
        }
        public static SqlParameter Parameter(string name, object value, SqlDbType sqlType)
        {

            return new SqlParameter { ParameterName = name, Value = value ?? DBNull.Value, Direction = ParameterDirection.Input, IsNullable = value == null, SqlDbType = sqlType };
            ;
        }

        public static SqlParameter Parameter(string name, object value, SqlDbType sqlType,string typeName)
        {

            return new SqlParameter { ParameterName = name, Value = value ?? DBNull.Value, Direction = ParameterDirection.Input, IsNullable = value == null, SqlDbType = sqlType,TypeName = typeName };
            ;
        }

        public static SqlParameter Parameter(string name, DateTime value)
        {
            return new SqlParameter
            {
                ParameterName = name,
                Value = value,
                Direction = ParameterDirection.Input,
                DbType = DbType.DateTime
            };
        }

        /// <summary>
        /// Creates SqlConnection
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        ///<author>Muzammil H</author>
        public static SqlConnection Connection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        /// 4.5 working
        ///// <summary>
        ///// Creates SqlConnection
        ///// </summary>
        ///// <param name="connectionString"></param>
        ///// <param name="credential"></param>
        ///// <returns></returns>
        /////<author>Muzammil H</author>
        //public static SqlConnection Connection(string connectionString,SqlCredential credential)
        //{
        //    return new SqlConnection(connectionString,credential);
        //}

        ////need to test 4.5 wokring
        /////  <summary>
        /////  Creates SqlConnection
        /////  </summary>
        /////  <param name="connectionString"></param>
        /////  <param name="userId"></param>
        /////  <param name="password"></param>
        ///// <param name="isReadOnly"></param>
        ///// <returns></returns>
        ///// <author>Muzammil H</author>
        //public static SqlConnection Connection(string connectionString,string userId,string password,bool isReadOnly=true)
        //{
        //    var credential = new SqlCredential(userId, password.ToSecureString(isReadOnly));
        //    return new SqlConnection(connectionString,credential);
        //}

        ///// <summary>
        ///// Convert password to SecureString
        ///// </summary>
        ///// <param name="password"></param>
        ///// <returns></returns>
        /////<author>Muzammil H</author>
        //private static SecureString ConvertToSecureString(string password)
        //{
        //    var secureString = new SecureString();
        //    if (password.Length > 0)
        //    {
        //        foreach (var c in password.ToCharArray()) secureString.AppendChar(c);
        //    }
        //    return secureString;
        //}
    }
}
