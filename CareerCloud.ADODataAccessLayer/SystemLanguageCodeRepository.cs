using CareerCloud.DataAccessLayer;
using CareerCloud.Pocos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace CareerCloud.ADODataAccessLayer
{
    public class SystemLanguageCodeRepository : IDataRepository<SystemLanguageCodePoco>
    {
        protected readonly string _connStr;
        public SystemLanguageCodeRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params SystemLanguageCodePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (SystemLanguageCodePoco item in items)
                {
                    comm.CommandText = @"INSERT INTO [dbo].[System_Language_Codes]
                                       ([LanguageID]
                                       ,[Name]
                                       ,[Native_Name])
                                 VALUES
                                       (@LanguageID
                                       ,@Name
                                       ,@Native_Name)";
                    comm.Parameters.AddWithValue("@LanguageID", item.LanguageID);
                    comm.Parameters.AddWithValue("@Name", item.Name);
                    comm.Parameters.AddWithValue("@Native_Name", item.NativeName);
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<SystemLanguageCodePoco> GetAll(params Expression<Func<SystemLanguageCodePoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT  [LanguageID]
                                  ,[Name]
                                  ,[Native_Name]
                              FROM [JOB_PORTAL_DB].[dbo].[System_Language_Codes]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                SystemLanguageCodePoco[] pocos = new SystemLanguageCodePoco[500];
                while (sqlReader.Read())
                {
                    SystemLanguageCodePoco poco = new SystemLanguageCodePoco();
                    poco.LanguageID = sqlReader.GetString(0);
                    poco.Name = sqlReader.GetString(1);
                    poco.NativeName = sqlReader.GetString(2);
                   
                    pocos[index] = poco;
                    index++;
                }
                connection.Close();
                return pocos.ToList();
            }
        }

        public IList<SystemLanguageCodePoco> GetList(Expression<Func<SystemLanguageCodePoco, bool>> where, params Expression<Func<SystemLanguageCodePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SystemLanguageCodePoco GetSingle(Expression<Func<SystemLanguageCodePoco, bool>> where, params Expression<Func<SystemLanguageCodePoco, object>>[] navigationProperties)
        {
            IQueryable<SystemLanguageCodePoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params SystemLanguageCodePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (SystemLanguageCodePoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[System_Language_Codes]
                                          WHERE [LanguageID]= @LanguageID";
                    comm.Parameters.AddWithValue("@LanguageID", item.LanguageID);
                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params SystemLanguageCodePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (SystemLanguageCodePoco item in items)
                {
                    comm.CommandText = @"UPDATE [dbo].[System_Language_Codes]
                       SET [Name] = @Name
                          ,[Native_Name] = @Native_Name
                           WHERE [LanguageID]= @LanguageID";
                    comm.Parameters.AddWithValue("@LanguageID", item.LanguageID);
                    comm.Parameters.AddWithValue("@Name", item.Name);
                    comm.Parameters.AddWithValue("@Native_Name", item.NativeName);

                    connection.Open();
                    int count = comm.ExecuteNonQuery();
                    connection.Close();
                }

            }
        }
    }
}
