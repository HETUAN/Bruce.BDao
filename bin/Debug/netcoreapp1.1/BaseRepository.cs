/*
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Repositories
{
    public class BaseRepository
    {
        protected string ConnStr = "";//Common.ConfigurationManage.GetConfigString("ConnStr");
        protected SqlConnection OpenConnection()
        {
            // 
            try
            {
                SqlConnection connection = new SqlConnection(ConnStr);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected int ExecuteNonQuery(SqlConnection con, string sql, object param = null)
        {
            try
            {
                using (con)
                {
                    //修改数据
                    int row = con.Execute(sql, param);
                    return row;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        protected T QuerySingle<T>(SqlConnection con, string sql, object param = null)
        {
            try
            {
                using (con)
                {
                    return con.QueryFirst<T>(sql, param);
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        protected List<T> Query<T>(SqlConnection con, string sql, object param = null)
        {
            //
            try
            {
                using (con)
                {
                    return con.Query<T>(sql, param).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected T ExecuteScalar<T>(SqlConnection con, string sql, object param = null)
        {
            try
            {
                using (con)
                {
                    return con.ExecuteScalar<T>(sql, param);
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
        protected SqlMapper.GridReader QueryMultiple(SqlConnection con, string sql, object param = null)
        {
            try
            {
                using (con)
                {
                    return con.QueryMultiple(sql, param);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
*/