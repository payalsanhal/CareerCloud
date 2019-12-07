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
    public class CompanyJobRepository : IDataRepository<CompanyJobPoco>
    {
        protected readonly string _connStr;

        public CompanyJobRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params CompanyJobPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (CompanyJobPoco item in items)
                {
                    comm.CommandText = @"INSERT INTO [dbo].[Company_Jobs]
                                       ([Id]
                                       ,[Company]
                                       ,[Profile_Created]
                                       ,[Is_Inactive]
                                       ,[Is_Company_Hidden])
                                 VALUES
                                       (@Id
		                                ,@Company
			                            ,@Profile_Created
			                            ,@Is_Inactive
			                            ,@Is_Company_Hidden)";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Company", item.Company);
                    comm.Parameters.AddWithValue("@Profile_Created", item.ProfileCreated);
                    comm.Parameters.AddWithValue("@Is_Inactive", item.IsInactive);
                    comm.Parameters.AddWithValue("@Is_Company_Hidden", item.IsCompanyHidden);


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

        public IList<CompanyJobPoco> GetAll(params Expression<Func<CompanyJobPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT 
                                        [Id]
                                       ,[Company]
                                       ,[Profile_Created]
                                       ,[Is_Inactive]
                                       ,[Is_Company_Hidden]
                                      ,[Time_Stamp]
                          FROM [dbo].[Company_Jobs]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                CompanyJobPoco[] pocos = new CompanyJobPoco[1500];
                while (sqlReader.Read())
                {
                    CompanyJobPoco poco = new CompanyJobPoco();
                    poco.Id = sqlReader.GetGuid(0);
                    poco.Company = sqlReader.GetGuid(1);
                    poco.ProfileCreated = sqlReader.GetDateTime(2);
                    poco.IsInactive = sqlReader.GetBoolean(3);
                    poco.IsCompanyHidden = sqlReader.GetBoolean(4);
                    poco.TimeStamp = (byte[])sqlReader[5];
                    pocos[index] = poco;
                    index++;
                }
                connection.Close();
                return pocos.ToList();
            }
        }

        public IList<CompanyJobPoco> GetList(Expression<Func<CompanyJobPoco, bool>> where, params Expression<Func<CompanyJobPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyJobPoco GetSingle(Expression<Func<CompanyJobPoco, bool>> where, params Expression<Func<CompanyJobPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyJobPoco> pocos = GetAll().AsQueryable();
            // return pocos.Where(where).FirstOrDefault();
            CompanyJobPoco item = new CompanyJobPoco();
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

        public void Remove(params CompanyJobPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (CompanyJobPoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[Company_Jobs]
                                          WHERE [Id]= @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params CompanyJobPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (CompanyJobPoco item in items)
                {
                    comm.CommandText = @"UPDATE [dbo].[Company_Jobs]
                                       SET [Company] = @Company	
                                       ,[Profile_Created] = @Profile_Created	
                                       ,[Is_Inactive] = @Is_Inactive
                                       ,[Is_Company_Hidden]= @Is_Inactive
                                  WHERE [Id]= @Id";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Company", item.Company);
                    comm.Parameters.AddWithValue("@Profile_Created", item.ProfileCreated);
                    comm.Parameters.AddWithValue("@Is_Inactive", item.IsInactive);
                    comm.Parameters.AddWithValue("@Is_Company_Hidden", item.IsCompanyHidden);


                    connection.Open();
                    int count = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}