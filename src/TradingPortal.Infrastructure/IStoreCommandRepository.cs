using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Infrastructure.Extensions;
using TradingPortal.Infrastructure.Helpers;

namespace TradingPortal.Infrastructure
{
    public interface IStoreCommandRepository
    {
        IStoreCommandRepository RunSPCommand(string dbConnectionString, string spName);
        T ExecuteFirstOrDefaultSync<T>() where T : IDbDataReaderResult<T>, new();
        IStoreCommandRepository CreateStoreProcedureCommand(string spName);
        IStoreCommandRepository CreateStoreCommand(string sql);
        Task<ICollection<T>> ExecuteToListAsync<T>() where T : IDbDataReaderResult<T>, new();
        Task<T> ExecuteFirstOrDefault<T>() where T : IDbDataReaderResult<T>, new();
        IStoreCommandRepository AddParameter(string name, object value);
        IStoreCommandRepository AddParameter(string name, object value, System.Data.SqlDbType dbType);
        IStoreCommandRepository AddParameter(string name, object value, SqlDbType dbType, string typeName);
        IStoreCommandRepository AddParameter(string name, string value, System.Data.SqlDbType dbType, int size);
        IStoreCommandRepository AddParameter(string name, object value, System.Data.SqlDbType dbType, System.Data.ParameterDirection direction);
        Task<T> ExecuteScalarAsync<T>();
        Task<int> ExecuteNonQueryAsync();
        int ExecuteNonQuery();

        
    }

    public class StoreCommandRepository : IStoreCommandRepository
    {
        private readonly IConfiguration _config;
        private DbCommand cmd;
        private string StorProcedure = "";
        public StoreCommandRepository(Microsoft.Extensions.Configuration.IConfiguration config)
        {
            _config = config;
            
        }


        /// <summary>
        /// //YA[28 May, 2018] SAH-755 LeadScoring Changes from SQAH
        /// </summary>
        /// <param name="dbConnectionString"></param>
        /// <param name="spName"></param>
        /// <returns></returns>
        public IStoreCommandRepository RunSPCommand(string dbConnectionString, string spName)
        {
            if (string.IsNullOrEmpty(dbConnectionString))
                dbConnectionString = "DefaultConnection";
            StorProcedure = spName;
            var connectionString = _config["ConnectionStrings:" + dbConnectionString];
            cmd = new SqlConnection(connectionString).CreateCommand();
            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;
            return this;
        }


        public IStoreCommandRepository CreateStoreProcedureCommand(string spName)
        {
            StorProcedure = spName;
            var connectionString = _config["ConnectionStrings:DefaultConnection"];
            cmd = new SqlConnection(connectionString).CreateCommand();
            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;
            return this;
        }

        public IStoreCommandRepository CreateStoreCommand(string sql)
        {
            var connectionString = _config["ConnectionStrings:DefaultConnection"];
            cmd = new SqlConnection(connectionString).CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            return this;
        }
        public async Task<ICollection<T>> ExecuteToListAsync<T>() where T : IDbDataReaderResult<T>, new()
        {
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            try
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var data = reader.Select<T>().ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //throw new Microsoft.EntityFrameworkCore.Design.OperationException($@"Something bad happened while executing store Procedure {StorProcedure}", ex);
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }
        }

        public async Task<T> ExecuteFirstOrDefault<T>() where T : IDbDataReaderResult<T>, new()
        {
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            try
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var data = reader.Select<T>().FirstOrDefault();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //throw new Microsoft.EntityFrameworkCore.Design.OperationException($@"Something bad happened while executing store Procedure {StorProcedure}", ex);
            }

            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

        }

        public T ExecuteFirstOrDefaultSync<T>() where T : IDbDataReaderResult<T>, new()
        {
            if (cmd.Connection.State != ConnectionState.Open)
            {

                cmd.Connection.Open();
            }
            var reader = cmd.ExecuteReader();
            var data = reader.Select<T>().FirstOrDefault();
            if (cmd.Connection.State == ConnectionState.Open)
            {
                cmd.Connection.Close();
            }
            return data;
        }

        public IStoreCommandRepository AddParameter(string name, object value)
        {
            cmd.Parameters.Add(SqlHelper.Parameter(name, value));
            return this;
        }

        public IStoreCommandRepository AddParameter(string name, object value, SqlDbType dbType)
        {
            cmd.Parameters.Add(SqlHelper.Parameter(name, value ?? DBNull.Value, dbType));
            return this;
        }
        public IStoreCommandRepository AddParameter(string name, string value, SqlDbType dbType, int size)
        {
            cmd.Parameters.Add(SqlHelper.Parameter(name, value, dbType));
            return this;
        }
        public IStoreCommandRepository AddParameter(string name, object value, SqlDbType dbType, ParameterDirection direction)
        {
            cmd.Parameters.Add(SqlHelper.Parameter(name, value, dbType, direction));
            return this;
        }

        public IStoreCommandRepository AddParameter(string name, object value, SqlDbType dbType, string typeName)
        {
            cmd.Parameters.Add(SqlHelper.Parameter(name, value, dbType, typeName));
            return this;
        }

        public async Task<T> ExecuteScalarAsync<T>()
        {
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }

            try
            {
                var data = (T)await cmd.ExecuteScalarAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
                //throw new Microsoft.EntityFrameworkCore.Design.OperationException($@"Something bad happened while executing store Procedure {StorProcedure}", ex);
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

        }

        public async Task<int> ExecuteNonQueryAsync()
        {
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            try
            {
                var data = await cmd.ExecuteNonQueryAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
                //throw new Microsoft.EntityFrameworkCore.Design.OperationException($@"Something bad happened while executing store Procedure {StorProcedure}", ex);
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

        }

        public int ExecuteNonQuery()
        {
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            try
            {
                var data = cmd.ExecuteNonQuery();
                return data;

            }
            catch (Exception ex)
            {
                throw ex;
                //throw new Microsoft.EntityFrameworkCore.Design.OperationException($@"Something bad happened while executing store Procedure {StorProcedure}", ex);
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

        }
    }


    public interface IDbDataReaderResult<T>
    {
        T Convert(DbDataReader reader);
    }
}
