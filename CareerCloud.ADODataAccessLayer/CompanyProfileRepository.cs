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
    public class CompanyProfileRepository : IDataRepository<CompanyProfilePoco>
    {
        protected readonly string _connStr;
        public CompanyProfileRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params CompanyProfilePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (CompanyProfilePoco item in items)
                {
                    comm.CommandText = @"INSERT INTO [dbo].[Company_Profiles]
                                       ([Id]
                                       ,[Registration_Date]
                                       ,[Company_Website]
                                       ,[Contact_Phone]
                                       ,[Contact_Name]
                                       ,[Company_Logo])
                                 VALUES
                                       (@Id
                                       ,@Registration_Date
                                       ,@Company_Website
                                       ,@Contact_Phone
                                       ,@Contact_Name
                                       ,@Company_Logo)";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Registration_Date", item.RegistrationDate);
                    comm.Parameters.AddWithValue("@Company_Website", item.CompanyWebsite);
                    comm.Parameters.AddWithValue("@Contact_Phone", item.ContactPhone);
                    comm.Parameters.AddWithValue("@Contact_Name", item.ContactName);
                    comm.Parameters.AddWithValue("@Company_Logo", item.CompanyLogo);

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

        public IList<CompanyProfilePoco> GetAll(params Expression<Func<CompanyProfilePoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT [Id]
                                  ,[Registration_Date]
                                  ,[Company_Website]
                                  ,[Contact_Phone]
                                  ,[Contact_Name]
                                  ,[Company_Logo]
                                  ,[Time_Stamp]
                              FROM [JOB_PORTAL_DB].[dbo].[Company_Profiles]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                CompanyProfilePoco[] pocos = new CompanyProfilePoco[500];
                while (sqlReader.Read())
                {
                    CompanyProfilePoco poco = new CompanyProfilePoco();
                    poco.Id = sqlReader.GetGuid(0);
                    poco.RegistrationDate = sqlReader.GetDateTime(1);
                    if (!sqlReader.IsDBNull(2))
                    {
                        poco.CompanyWebsite = sqlReader.GetString(2);
                    }
                    if (!sqlReader.IsDBNull(3))
                    {
                        poco.ContactPhone = sqlReader.GetString(3);
                    }
                    if (!sqlReader.IsDBNull(4))
                    {
                        poco.ContactName = sqlReader.GetString(4);
                    }
                    if (!sqlReader.IsDBNull(5))
                    {
                        poco.CompanyLogo = (byte[])sqlReader[5];
                    }
                    poco.TimeStamp = (byte[])sqlReader[6];
                    pocos[index] = poco;
                    index++;
                }
                connection.Close();
                return pocos.ToList();
            }
        }

        public IList<CompanyProfilePoco> GetList(Expression<Func<CompanyProfilePoco, bool>> where, params Expression<Func<CompanyProfilePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyProfilePoco GetSingle(Expression<Func<CompanyProfilePoco, bool>> where, params Expression<Func<CompanyProfilePoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyProfilePoco> pocos = GetAll().AsQueryable();
            // return pocos.Where(where).FirstOrDefault();
            CompanyProfilePoco item = new CompanyProfilePoco();
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

        public void Remove(params CompanyProfilePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (CompanyProfilePoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[Company_Profiles]
                                          WHERE [Id]= @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params CompanyProfilePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (CompanyProfilePoco item in items)
                {
                    comm.CommandText = @"UPDATE[dbo].[Company_Profiles]
                       SET[Registration_Date] = @Registration_Date
                          ,[Company_Website] =		@Company_Website
                          ,[Contact_Phone] =		@Contact_Phone
                          ,[Contact_Name] =			@Contact_Name
                          ,[Company_Logo] =			@Company_Logo
                            WHERE [Id]= @Id";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Registration_Date", item.RegistrationDate);
                    comm.Parameters.AddWithValue("@Company_Website", item.CompanyWebsite);
                    comm.Parameters.AddWithValue("@Contact_Phone", item.ContactPhone);
                    comm.Parameters.AddWithValue("@Contact_Name", item.ContactName);
                    comm.Parameters.AddWithValue("@Company_Logo", item.CompanyLogo);

                    connection.Open();
                    int count = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
