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
    public class CompanyDescriptionRepository : IDataRepository<CompanyDescriptionPoco>
    {
        protected readonly string _connStr;

        public CompanyDescriptionRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params CompanyDescriptionPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (CompanyDescriptionPoco item in items)
                {
                    comm.CommandText = @"INSERT INTO [dbo].[Company_Descriptions]
                                           ( [Id] 			 
                                            ,[Company] 		
                                            ,[LanguageID] 		
                                            ,[Company_Name]		
                                            ,[Company_Description])
                                     VALUES
                                           (@Id
                                            ,@Company
                                            ,@LanguageID
                                           , @Company_Name
                                           , @Company_Description)";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Company", item.Company);
                    comm.Parameters.AddWithValue("@LanguageID", item.LanguageId);
                    comm.Parameters.AddWithValue("@Company_Name", item.CompanyName);
                    comm.Parameters.AddWithValue("@Company_Description", item.CompanyDescription);

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

        public IList<CompanyDescriptionPoco> GetAll(params Expression<Func<CompanyDescriptionPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT 
                                    [Id] 			 
                                ,[Company] 		
                                ,[LanguageID] 		
                                ,[Company_Name]		
                                ,[Company_Description]
                                ,[Time_Stamp]
                          FROM [dbo].[Company_Descriptions]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                CompanyDescriptionPoco[] pocos = new CompanyDescriptionPoco[700];
                while (sqlReader.Read())
                {
                    CompanyDescriptionPoco poco = new CompanyDescriptionPoco();
                    poco.Id = sqlReader.GetGuid(0);
                    poco.Company = sqlReader.GetGuid(1);
                    poco.LanguageId = sqlReader.GetString(2);
                    poco.CompanyName = sqlReader.GetString(3);
                    poco.CompanyDescription = sqlReader.GetString(4);
                    poco.TimeStamp = (byte[])sqlReader[5];
                    pocos[index] = poco;
                    index++;
                }
                connection.Close();
                return pocos.ToList();
            }
        }

        public IList<CompanyDescriptionPoco> GetList(Expression<Func<CompanyDescriptionPoco, bool>> where, params Expression<Func<CompanyDescriptionPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyDescriptionPoco GetSingle(Expression<Func<CompanyDescriptionPoco, bool>> where, params Expression<Func<CompanyDescriptionPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyDescriptionPoco> pocos = GetAll().AsQueryable();
            // return pocos.Where(where).FirstOrDefault();
            CompanyDescriptionPoco item = new CompanyDescriptionPoco();
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

        public void Remove(params CompanyDescriptionPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (CompanyDescriptionPoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[Company_Descriptions]
                                          WHERE [Id]= @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params CompanyDescriptionPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (CompanyDescriptionPoco item in items)
                {
                    comm.CommandText = @"Update [dbo].[Company_Descriptions]
                           SET [Company] =@Company
                              ,[LanguageID] =@LanguageID
                              ,[Company_Name]=@Company_Name
                              ,[Company_Description]=@Company_Description
                        WHERE [Id]= @Id";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Company", item.Company);
                    comm.Parameters.AddWithValue("@LanguageID", item.LanguageId);
                    comm.Parameters.AddWithValue("@Company_Name", item.CompanyName);
                    comm.Parameters.AddWithValue("@Company_Description", item.CompanyDescription);

                    connection.Open();
                    int count = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
