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
    public class ApplicantWorkHistoryRepository : IDataRepository<ApplicantWorkHistoryPoco>
    {
        protected readonly string _connStr;

        public ApplicantWorkHistoryRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params ApplicantWorkHistoryPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (ApplicantWorkHistoryPoco item in items)
                {
                    comm.CommandText = @"INSERT INTO [dbo].[Applicant_Work_History]
                                           ([Id], [Applicant]		
                                              ,[Company_Name]  
                                              ,[Country_Code]  
                                              ,[Location] 		
                                              ,[Job_Title]	
                                              ,[Job_Description]
                                              ,[Start_Month] 
                                              ,[Start_Year]   
                                              ,[End_Month]   
                                              ,[End_Year])
                                     VALUES
                                           (@Id,@Applicant	
                                            ,@Company_Name
                                            ,@Country_Code
                                            ,@Location
                                            ,@Job_Title
                                            ,@Job_Description
                                            ,@Start_Month
                                            ,@Start_Year
                                            ,@End_Month
                                            ,@End_Year)";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Applicant", item.Applicant);
                    comm.Parameters.AddWithValue("@Company_Name", item.CompanyName);
                    comm.Parameters.AddWithValue("@Country_Code", item.CountryCode);
                    comm.Parameters.AddWithValue("@Location", item.Location);
                    comm.Parameters.AddWithValue("@Job_Title", item.JobTitle);
                    comm.Parameters.AddWithValue("@Job_Description", item.JobDescription);
                    comm.Parameters.AddWithValue("@Start_Month", item.StartMonth);
                    comm.Parameters.AddWithValue("@Start_Year", item.StartYear);
                    comm.Parameters.AddWithValue("@End_Month", item.EndMonth);
                    comm.Parameters.AddWithValue("@End_Year", item.EndYear);

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

        public IList<ApplicantWorkHistoryPoco> GetAll(params Expression<Func<ApplicantWorkHistoryPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT [Id]
                              ,[Applicant]		
                              ,[Company_Name]  
                              ,[Country_Code]  
                              ,[Location] 		
                              ,[Job_Title]	
                              ,[Job_Description]
                              ,[Start_Month] 
                              ,[Start_Year]   
                              ,[End_Month]   
                              ,[End_Year]  
                              ,[Time_Stamp]
                          FROM [dbo].[Applicant_Work_History]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                ApplicantWorkHistoryPoco[] pocos = new ApplicantWorkHistoryPoco[500];
                while (sqlReader.Read())
                {
                    ApplicantWorkHistoryPoco poco = new ApplicantWorkHistoryPoco();
                    poco.Id = sqlReader.GetGuid(0);
                    poco.Applicant = sqlReader.GetGuid(1);
                    poco.CompanyName = sqlReader.GetString(2);
                    poco.CountryCode = sqlReader.GetString(3);
                    poco.Location = sqlReader.GetString(4);
                    poco.JobTitle = sqlReader.GetString(5);
                    poco.JobDescription = sqlReader.GetString(6);
                    poco.StartMonth = (short)sqlReader[7];
                    poco.StartYear = (int)sqlReader[8];
                    poco.EndMonth = (short)sqlReader[9];
                    poco.EndYear = (int)sqlReader[10];
                    poco.TimeStamp = (byte[])sqlReader[11];
                    pocos[index] = poco;
                    index++;
                }
                connection.Close();
                return pocos.ToList();
            }
        }

        public IList<ApplicantWorkHistoryPoco> GetList(Expression<Func<ApplicantWorkHistoryPoco, bool>> where, params Expression<Func<ApplicantWorkHistoryPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantWorkHistoryPoco GetSingle(Expression<Func<ApplicantWorkHistoryPoco, bool>> where, params Expression<Func<ApplicantWorkHistoryPoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantWorkHistoryPoco> pocos = GetAll().AsQueryable();
            // return pocos.Where(where).FirstOrDefault();
            ApplicantWorkHistoryPoco item = new ApplicantWorkHistoryPoco();
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

        public void Remove(params ApplicantWorkHistoryPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (ApplicantWorkHistoryPoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[Applicant_Work_History]
                                          WHERE [Id]= @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params ApplicantWorkHistoryPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (ApplicantWorkHistoryPoco item in items)
                {
                    comm.CommandText = @"UPDATE [dbo].[Applicant_Work_History]
                        SET   [Applicant] =	@Applicant	
                          ,[Company_Name] = @Company_Name
                          ,[Country_Code] = @Country_Code
                          ,[Location] =	@Location
                          ,[Job_Title] =@Job_Title
                          ,[Job_Description] = @Job_Description
                          ,[Start_Month] = @Start_Month
                          ,[Start_Year] = @Start_Year
                          ,[End_Month] = @End_Month
                          ,[End_Year] = @End_Year
                        WHERE [Id]= @Id";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Applicant", item.Applicant);
                    comm.Parameters.AddWithValue("@Company_Name", item.CompanyName);
                    comm.Parameters.AddWithValue("@Country_Code", item.CountryCode);
                    comm.Parameters.AddWithValue("@Location", item.Location);
                    comm.Parameters.AddWithValue("@Job_Title", item.JobTitle);
                    comm.Parameters.AddWithValue("@Job_Description", item.JobDescription);
                    comm.Parameters.AddWithValue("@Start_Month", item.StartMonth);
                    comm.Parameters.AddWithValue("@Start_Year", item.StartYear);
                    comm.Parameters.AddWithValue("@End_Month", item.EndMonth);
                    comm.Parameters.AddWithValue("@End_Year", item.EndYear);

                    connection.Open();
                    int count = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
