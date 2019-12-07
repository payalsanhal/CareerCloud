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
    public class CompanyJobEducationRepository : IDataRepository<CompanyJobEducationPoco>
    {
        protected readonly string _connStr;

        public CompanyJobEducationRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params CompanyJobEducationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (CompanyJobEducationPoco item in items)
                {
                    comm.CommandText = @"INSERT INTO [dbo].[Company_Job_Educations]
                                       ([Id]
                                       ,[Job]
                                       ,[Major]
                                       ,[Importance])
                                 VALUES
                                       (@Id
                                       ,@Job
                                       ,@Major
                                       ,@Importance)";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Job", item.Job);
                    comm.Parameters.AddWithValue("@Major", item.Major);
                    comm.Parameters.AddWithValue("@Importance", item.Importance);

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

        public IList<CompanyJobEducationPoco> GetAll(params Expression<Func<CompanyJobEducationPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT 
                                       [Id]
                                      ,[Job]
                                      ,[Major]
                                      ,[Importance]
                                      ,[Time_Stamp]
                          FROM [dbo].[Company_Job_Educations]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                CompanyJobEducationPoco[] pocos = new CompanyJobEducationPoco[1500];
                while (sqlReader.Read())
                {
                    CompanyJobEducationPoco poco = new CompanyJobEducationPoco();
                    poco.Id = sqlReader.GetGuid(0);
                    poco.Job = sqlReader.GetGuid(1);
                    poco.Major = sqlReader.GetString(2);
                    poco.Importance = (short)sqlReader[3];
                    poco.TimeStamp = (byte[])sqlReader[4];
                    pocos[index] = poco;
                    index++;
                }
                connection.Close();
                return pocos.ToList();
            }
        }

        public IList<CompanyJobEducationPoco> GetList(Expression<Func<CompanyJobEducationPoco, bool>> where, params Expression<Func<CompanyJobEducationPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyJobEducationPoco GetSingle(Expression<Func<CompanyJobEducationPoco, bool>> where, params Expression<Func<CompanyJobEducationPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyJobEducationPoco> pocos = GetAll().AsQueryable();
            // return pocos.Where(where).FirstOrDefault();
            CompanyJobEducationPoco item = new CompanyJobEducationPoco();
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

        public void Remove(params CompanyJobEducationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (CompanyJobEducationPoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[Company_Job_Educations]
                                          WHERE [Id]= @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params CompanyJobEducationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (CompanyJobEducationPoco item in items)
                {
                    comm.CommandText = @"UPDATE [dbo].[Company_Job_Educations]
                                       SET [Job] = @Job	
                                       ,[Major] = @Major	
                                       ,[Importance] = @Importance
                                  WHERE [Id]= @Id";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Job", item.Job);
                    comm.Parameters.AddWithValue("@Major", item.Major);
                    comm.Parameters.AddWithValue("@Importance", item.Importance);

                    connection.Open();
                    int count = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
