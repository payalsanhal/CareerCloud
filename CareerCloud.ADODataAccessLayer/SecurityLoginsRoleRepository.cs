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
    public class SecurityLoginsRoleRepository : IDataRepository<SecurityLoginsRolePoco>
    {
        protected readonly string _connStr;
        public SecurityLoginsRoleRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params SecurityLoginsRolePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (SecurityLoginsRolePoco item in items)
                {
                    comm.CommandText = @"INSERT INTO [dbo].[Security_Logins_Roles]
                                           ([Id]
                                           ,[Login]
                                           ,[Role])
                                     VALUES
                                           (@Id
                                           ,@Login
                                           ,@Role)";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Login", item.Login);
                    comm.Parameters.AddWithValue("@Role", item.Role);

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

        public IList<SecurityLoginsRolePoco> GetAll(params Expression<Func<SecurityLoginsRolePoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT  [Id]
                                  ,[Login]
                                  ,[Role]
                                  ,[Time_Stamp]
                              FROM [JOB_PORTAL_DB].[dbo].[Security_Logins_Roles]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                SecurityLoginsRolePoco[] pocos = new SecurityLoginsRolePoco[500];
                while (sqlReader.Read())
                {
                    SecurityLoginsRolePoco poco = new SecurityLoginsRolePoco();
                    poco.Id = sqlReader.GetGuid(0);
                    poco.Login = sqlReader.GetGuid(1);
                    poco.Role = sqlReader.GetGuid(2);
                    poco.TimeStamp = (byte[])sqlReader[3];
                    pocos[index] = poco;
                    index++;
                }
                connection.Close();
                return pocos.ToList();
            }
        }

        public IList<SecurityLoginsRolePoco> GetList(Expression<Func<SecurityLoginsRolePoco, bool>> where, params Expression<Func<SecurityLoginsRolePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SecurityLoginsRolePoco GetSingle(Expression<Func<SecurityLoginsRolePoco, bool>> where, params Expression<Func<SecurityLoginsRolePoco, object>>[] navigationProperties)
        {
            IQueryable<SecurityLoginsRolePoco> pocos = GetAll().AsQueryable();
            // return pocos.Where(where).FirstOrDefault();
            SecurityLoginsRolePoco item = new SecurityLoginsRolePoco();
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

        public void Remove(params SecurityLoginsRolePoco[] items)
        {

            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (SecurityLoginsRolePoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[Security_Logins_Roles]
                                          WHERE [Id]= @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params SecurityLoginsRolePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (SecurityLoginsRolePoco item in items)
                {
                    comm.CommandText = @"UPDATE [dbo].[Security_Logins_Roles]
                                    SET [Login] = @Login
                                        ,[Role] = @Role
                                  WHERE [Id]= @Id";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Login", item.Login);
                    comm.Parameters.AddWithValue("@Role", item.Role);

                    connection.Open();
                    int count = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
