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
    public class SecurityLoginRepository : IDataRepository<SecurityLoginPoco>
    {
        protected readonly string _connStr;
        public SecurityLoginRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params SecurityLoginPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (SecurityLoginPoco item in items)
                {
                    comm.CommandText = @"INSERT INTO [dbo].[Security_Logins]
                                           ([Id],[Login],[Password],[Created_Date],[Password_Update_Date],[Agreement_Accepted_Date],[Is_Locked]
                                           ,[Is_Inactive],[Email_Address],[Phone_Number],[Full_Name],[Force_Change_Password],[Prefferred_Language])
                                     VALUES
                                           (@Id,@Login,@Password,@Created_Date,@Password_Update_Date,@Agreement_Accepted_Date,@Is_Locked,
                                            @Is_Inactive,@Email_Address,@Phone_Number,@Full_Name,@Force_Change_Password,@Prefferred_Language)";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Login", item.Login);
                    comm.Parameters.AddWithValue("@Password", item.Password);
                    comm.Parameters.AddWithValue("@Created_Date", item.Created);
                    comm.Parameters.AddWithValue("@Password_Update_Date", item.PasswordUpdate);
                    comm.Parameters.AddWithValue("@Agreement_Accepted_Date", item.AgreementAccepted);
                    comm.Parameters.AddWithValue("@Is_Locked", item.IsLocked);
                    comm.Parameters.AddWithValue("@Is_Inactive", item.IsInactive);
                    comm.Parameters.AddWithValue("@Email_Address", item.EmailAddress);
                    comm.Parameters.AddWithValue("@Phone_Number", item.PhoneNumber);
                    comm.Parameters.AddWithValue("@Full_Name", item.FullName);
                    comm.Parameters.AddWithValue("@Force_Change_Password", item.ForceChangePassword);
                    comm.Parameters.AddWithValue("@Prefferred_Language", item.PrefferredLanguage);

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

        public IList<SecurityLoginPoco> GetAll(params Expression<Func<SecurityLoginPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT  [Id]
                                  ,[Login]
                                  ,[Password]
                                  ,[Created_Date]
                                  ,[Password_Update_Date]
                                  ,[Agreement_Accepted_Date]
                                  ,[Is_Locked]
                                  ,[Is_Inactive]
                                  ,[Email_Address]
                                  ,[Phone_Number]
                                  ,[Full_Name]
                                  ,[Force_Change_Password]
                                  ,[Prefferred_Language]
                                  ,[Time_Stamp]
                              FROM [JOB_PORTAL_DB].[dbo].[Security_Logins]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                SecurityLoginPoco[] pocos = new SecurityLoginPoco[500];
                while (sqlReader.Read())
                {
                    SecurityLoginPoco poco = new SecurityLoginPoco();
                    poco.Id = sqlReader.GetGuid(0);
                    poco.Login = sqlReader.GetString(1);
                    if (!sqlReader.IsDBNull(2))
                    {
                        poco.Password = sqlReader.GetString(2);
                    }
                    if (!sqlReader.IsDBNull(3))
                    {
                        poco.Created= sqlReader.GetDateTime(3);
                    }
                    if (!sqlReader.IsDBNull(4))
                    {
                        poco.PasswordUpdate = sqlReader.GetDateTime(4);
                    }
                    if (!sqlReader.IsDBNull(5))
                    {
                        poco.AgreementAccepted = sqlReader.GetDateTime(5);
                    }

                    poco.IsLocked = sqlReader.GetBoolean(6); 
                    poco.IsInactive = sqlReader.GetBoolean(7);
                    if (!sqlReader.IsDBNull(8))
                    {
                        poco.EmailAddress = sqlReader.GetString(8);
                    }
                    if (!sqlReader.IsDBNull(9))
                    {
                        poco.PhoneNumber = sqlReader.GetString(9);
                    }
                    if (!sqlReader.IsDBNull(10))
                    {
                        poco.FullName = sqlReader.GetString(10);
                    }
                    poco.ForceChangePassword = sqlReader.GetBoolean(11);
                    if (!sqlReader.IsDBNull(12))
                    {
                        poco.PrefferredLanguage = sqlReader.GetString(12);
                    }
                    poco.TimeStamp = (byte[])sqlReader[13];
                    pocos[index] = poco;
                    index++;
                }
                connection.Close();
                return pocos.ToList();
            }
        }

        public IList<SecurityLoginPoco> GetList(Expression<Func<SecurityLoginPoco, bool>> where, params Expression<Func<SecurityLoginPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SecurityLoginPoco GetSingle(Expression<Func<SecurityLoginPoco, bool>> where, params Expression<Func<SecurityLoginPoco, object>>[] navigationProperties)
        {
            IQueryable<SecurityLoginPoco> pocos = GetAll().AsQueryable();
            //return pocos.Where(where).FirstOrDefault();
            SecurityLoginPoco item = new SecurityLoginPoco();
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

        public void Remove(params SecurityLoginPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (SecurityLoginPoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[Security_Logins]
                                          WHERE [Id]= @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params SecurityLoginPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (SecurityLoginPoco item in items)
                {
                    comm.CommandText = @"UPDATE [dbo].[Security_Logins]
                                       SET [Login] = @Login
                                          ,[Password] = @Password
                                          ,[Created_Date] = @Created_Date
                                          ,[Password_Update_Date] = @Password_Update_Date
                                          ,[Agreement_Accepted_Date] = @Agreement_Accepted_Date
                                          ,[Is_Locked] = @Is_Locked
                                          ,[Is_Inactive] = @Is_Inactive
                                          ,[Email_Address] = @Email_Address
                                          ,[Phone_Number] = @Phone_Number
                                          ,[Full_Name] = @Full_Name
                                          ,[Force_Change_Password] = @Force_Change_Password
                                          ,[Prefferred_Language] = @Prefferred_Language
                                        WHERE [Id]= @Id";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Login", item.Login);
                    comm.Parameters.AddWithValue("@Password", item.Password);
                    comm.Parameters.AddWithValue("@Created_Date", item.Created);
                    comm.Parameters.AddWithValue("@Password_Update_Date", item.PasswordUpdate);
                    comm.Parameters.AddWithValue("@Agreement_Accepted_Date", item.AgreementAccepted);
                    comm.Parameters.AddWithValue("@Is_Locked", item.IsLocked);
                    comm.Parameters.AddWithValue("@Is_Inactive", item.IsInactive);
                    comm.Parameters.AddWithValue("@Email_Address", item.EmailAddress);
                    comm.Parameters.AddWithValue("@Phone_Number", item.PhoneNumber);
                    comm.Parameters.AddWithValue("@Full_Name", item.FullName);
                    comm.Parameters.AddWithValue("@Force_Change_Password", item.ForceChangePassword);
                    comm.Parameters.AddWithValue("@Prefferred_Language", item.PrefferredLanguage);

                    connection.Open();
                    int count = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
