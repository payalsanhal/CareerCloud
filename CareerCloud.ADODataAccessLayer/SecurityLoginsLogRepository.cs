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
    public class SecurityLoginsLogRepository : IDataRepository<SecurityLoginsLogPoco>
    {
        protected readonly string _connStr;
        public SecurityLoginsLogRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params SecurityLoginsLogPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (SecurityLoginsLogPoco item in items)
                {
                  comm.CommandText = @"INSERT INTO [dbo].[Security_Logins_Log]
                               ([Id]
                               ,[Login]
                               ,[Source_IP]
                               ,[Logon_Date]
                               ,[Is_Succesful])
                         VALUES
                               (@Id
                               ,@Login
                               ,@Source_IP
                               ,@Logon_Date
                               ,@Is_Succesful)";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Login", item.Login);
                    comm.Parameters.AddWithValue("@Source_IP", item.SourceIP);
                    comm.Parameters.AddWithValue("@Logon_Date", item.LogonDate);
                    comm.Parameters.AddWithValue("@Is_Succesful", item.IsSuccesful);


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

        public IList<SecurityLoginsLogPoco> GetAll(params Expression<Func<SecurityLoginsLogPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT  [Id]
                      ,[Login]
                      ,[Source_IP]
                      ,[Logon_Date]
                      ,[Is_Succesful]
                  FROM [JOB_PORTAL_DB].[dbo].[Security_Logins_Log]";
                 connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                SecurityLoginsLogPoco[] pocos = new SecurityLoginsLogPoco[2000];
                while (sqlReader.Read())
                {
                    SecurityLoginsLogPoco poco = new SecurityLoginsLogPoco();
                    poco.Id = sqlReader.GetGuid(0);
                    poco.Login = sqlReader.GetGuid(1);
                    poco.SourceIP = sqlReader.GetString(2);
                    poco.LogonDate = sqlReader.GetDateTime(3);
                    poco.IsSuccesful = sqlReader.GetBoolean(4);
                    pocos[index] = poco;
                    index++;
                }
                connection.Close();
                return pocos.ToList();
            }
        }

        public IList<SecurityLoginsLogPoco> GetList(Expression<Func<SecurityLoginsLogPoco, bool>> where, params Expression<Func<SecurityLoginsLogPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SecurityLoginsLogPoco GetSingle(Expression<Func<SecurityLoginsLogPoco, bool>> where, params Expression<Func<SecurityLoginsLogPoco, object>>[] navigationProperties)
        {
            IQueryable<SecurityLoginsLogPoco> pocos = GetAll().AsQueryable();
            // return pocos.Where(where).FirstOrDefault();
            SecurityLoginsLogPoco item = new SecurityLoginsLogPoco();
            try
            {
                item = pocos.Where(where).FirstOrDefault();
            }
            catch
            {
                return null;
            }
            return item;
        }

        public void Remove(params SecurityLoginsLogPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (SecurityLoginsLogPoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[Security_Logins_Log]
                                          WHERE [Id]= @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params SecurityLoginsLogPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (SecurityLoginsLogPoco item in items)
                {
                    comm.CommandText = @"UPDATE [dbo].[Security_Logins_Log]
                                   SET [Login] = @Login
                                      ,[Source_IP] = @Source_IP
                                      ,[Logon_Date] = @Logon_Date
                                      ,[Is_Succesful] = @Is_Succesful
                                  WHERE [Id]= @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Login", item.Login);
                    comm.Parameters.AddWithValue("@Source_IP", item.SourceIP);
                    comm.Parameters.AddWithValue("@Logon_Date", item.LogonDate);
                    comm.Parameters.AddWithValue("@Is_Succesful", item.IsSuccesful);

                    connection.Open();
                    int count = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
