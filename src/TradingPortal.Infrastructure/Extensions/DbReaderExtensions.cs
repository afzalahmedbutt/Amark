using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Text;

namespace TradingPortal.Infrastructure.Extensions
{
    public static class DbReaderExtensions
    {
        public static string SafeGetString(this DbDataReader reader, string columnName)
        {
            try
            {
                var index = reader.GetOrdinal(columnName);

                return !reader.IsDBNull(index) ? reader.GetString(index) : string.Empty;
            }
            catch (Exception e)
            {
                throw new InvalidCastException("Invalid Cast for Column Name " + columnName + Environment.NewLine + " " + e.Message, e);
            }
        }

        public static int? SafeGetNullableInt(this DbDataReader reader, string columnName)
        {
            try
            {
                var index = reader.GetOrdinal(columnName);

                return !reader.IsDBNull(index) ? (int?)reader.GetInt32(index) : null;
            }
            catch (Exception e)
            {
                throw new InvalidCastException("Invalid Cast for Column Name " + columnName + Environment.NewLine + " " + e.Message, e);
            }
        }

        public static short? SafeGetNullableShort(this DbDataReader reader, string columnName)
        {
            try
            {
                var index = reader.GetOrdinal(columnName);

                return !reader.IsDBNull(index) ? (short?)reader.GetInt16(index) : null;
            }
            catch (Exception e)
            {
                throw new InvalidCastException("Invalid Cast for Column Name " + columnName + Environment.NewLine + " " + e.Message, e);
            }
        }
        public static decimal? SafeGetNullableDecimal(this DbDataReader reader, string columnName)
        {
            try
            {
                var index = reader.GetOrdinal(columnName);

                return !reader.IsDBNull(index) ? (decimal?)reader.GetDecimal(index) : null;
            }
            catch (Exception e)
            {
                throw new InvalidCastException("Invalid Cast for Column Name " + columnName + Environment.NewLine + " " + e.Message, e);
            }
        }
        public static long? SafeGetNullableLong(this DbDataReader reader, string columnName)
        {
            try
            {
                //var test =  reader.Select(x => x).ToList();
                var index = reader.GetOrdinal(columnName);

                return !reader.IsDBNull(index) ? (long?)reader.GetInt64(index) : null;
            }
            catch (Exception e)
            {
                throw new InvalidCastException("Invalid Cast for Column Name " + columnName + Environment.NewLine + " " + e.Message, e);
            }
        }
        public static byte? SafeGetNullableByte(this DbDataReader reader, string columnName)
        {
            try
            {
                var index = reader.GetOrdinal(columnName);

                return !reader.IsDBNull(index) ? (byte?)reader.GetByte(index) : null;
            }
            catch (Exception e)
            {
                throw new InvalidCastException("Invalid Cast for Column Name " + columnName + Environment.NewLine + " " + e.Message, e);
            }
        }

        public static Guid? SafeGetNullableGuid(this DbDataReader reader, string columnName)
        {
            try
            {
                var index = reader.GetOrdinal(columnName);

                return !reader.IsDBNull(index) ? (Guid?)reader.GetGuid(index) : null;
            }
            catch (Exception e)
            {
                throw new InvalidCastException("Invalid Cast for Column Name " + columnName + Environment.NewLine + " " + e.Message, e);
            }
        }

        public static DateTime? SafeGetNullableDateTime(this DbDataReader reader, string columnName)
        {
            try
            {
                var index = reader.GetOrdinal(columnName);

                return !reader.IsDBNull(index) ? (DateTime?)reader.GetDateTime(index) : null;
            }
            catch (Exception e)
            {
                throw new InvalidCastException("Invalid Cast for Column Name " + columnName + Environment.NewLine + " " + e.Message, e);
            }
        }

        public static bool? SafeGetNullableBool(this DbDataReader reader, string columnName)
        {
            try
            {
                var index = reader.GetOrdinal(columnName);

                return !reader.IsDBNull(index) ? (bool?)reader.GetBoolean(index) : null;
            }
            catch (Exception e)
            {
                throw new InvalidCastException("Invalid Cast for Column Name " + columnName + Environment.NewLine + " " + e.Message, e);
            }
        }

        public static Guid SafeGetGuid(this DbDataReader reader, string columnName)
        {
            try
            {
                var index = reader.GetOrdinal(columnName);

                return !reader.IsDBNull(index) ? reader.GetGuid(index) : Guid.Empty;
            }
            catch (Exception e)
            {
                throw new InvalidCastException("Invalid Cast for Column Name " + columnName + Environment.NewLine + " " + e.Message, e);
            }
        }

        public static T Fill<T>(this DbDataReader reader, T obj)
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(obj))
            {
                if (!prop.IsReadOnly)
                {
                    if (reader.HasColumn(prop.Name))
                    {
                        if (reader[prop.Name] is DBNull)
                            prop.SetValue(obj, null);
                        else
                            prop.SetValue(obj, reader[prop.Name]);
                    }
                }
            }

            return obj;
        }

        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (var i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;

        }

        public static IEnumerable<T> Select<T>(this DbDataReader reader, Func<DbDataReader, T> projection)
        {
            while (reader.Read())
            {
                yield return projection(reader);
            }
        }

        public static IEnumerable<T> Select<T>(this DbDataReader reader) where T : IDbDataReaderResult<T>, new()
        {
            while (reader.Read())
            {
                T obj = new T();
                yield return (obj as IDbDataReaderResult<T>).Convert(reader);
            }
        }
    }
}
