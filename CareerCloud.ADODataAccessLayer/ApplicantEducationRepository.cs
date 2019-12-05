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
    public class ApplicantEducationRepository : IDataRepository<ApplicantEducationPoco>
    {
        protected readonly string _connStr;

        public ApplicantEducationRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params ApplicantEducationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (ApplicantEducationPoco item in items)
                {
                    comm.CommandText = @"INSERT INTO [dbo].[Applicant_Educations]
                    ( [Id], [Applicant], [Major], [Certificate_Diploma], [Start_Date], [Completion_Date], [Completion_Percent] )
              VALUES( @Id, @Applicant, @Major, @Certificate_Diploma, @Start_Date, @Completion_Date, @Completion_Percent )";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Applicant", item.Applicant);
                    comm.Parameters.AddWithValue("@Major", item.Major);
                    comm.Parameters.AddWithValue("@Certificate_Diploma", item.CertificateDiploma);
                    comm.Parameters.AddWithValue("@Start_Date", item.StartDate);
                    comm.Parameters.AddWithValue("@Completion_Date", item.CompletionDate);
                    comm.Parameters.AddWithValue("@Completion_Percent", item.CompletionPercent);
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public IList<ApplicantEducationPoco> GetAll(params Expression<Func<ApplicantEducationPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT [Id], [Applicant], [Major], [Certificate_Diploma], [Start_Date], [Completion_Date], [Completion_Percent], [Time_Stamp]
                                    FROM [dbo].[Applicant_Educations]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                ApplicantEducationPoco[] pocos = new ApplicantEducationPoco[500];
                while (sqlReader.Read())
                {
                    ApplicantEducationPoco poco = new ApplicantEducationPoco();
                    poco.Id = sqlReader.GetGuid(0);
                    poco.Applicant = sqlReader.GetGuid(1);
                    poco.Major = sqlReader.GetString(2);
                    poco.CertificateDiploma = sqlReader.GetString(3);
                    if (!sqlReader.IsDBNull(4))
                    {
                        poco.StartDate = sqlReader.GetDateTime(4);
                    }
                    if (!sqlReader.IsDBNull(5))
                    {
                        poco.CompletionDate = (DateTime?)sqlReader.GetDateTime(5);
                    }
                    if (!sqlReader.IsDBNull(6))
                    {
                        poco.CompletionPercent = (byte?)sqlReader[6];
                    }
                    poco.TimeStamp = (byte[])sqlReader[7];

                    pocos[index] = poco;
                    index++;
                }
                connection.Close();
                return pocos.ToList();
            }
        }

        public ApplicantEducationPoco GetSingle(Expression<Func<ApplicantEducationPoco, bool>>
            where, params Expression<Func<ApplicantEducationPoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantEducationPoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
            //ApplicantEducationPoco item = new ApplicantEducationPoco();
            //try
            //{
            //    item = pocos.Where(where).FirstOrDefault();
            //}
            //catch
            //{
            //    return null;
            //}
            //return item;
        }

        public void Remove(params ApplicantEducationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (ApplicantEducationPoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[Applicant_Educations]
                                          WHERE [Id]= @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params ApplicantEducationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (ApplicantEducationPoco item in items)
                {
                    comm.CommandText = @"UPDATE [dbo].[Applicant_Educations]
                        SET [Applicant] = @Applicant
                      ,[Major] = @Major
                      ,[Certificate_Diploma] = @Certificate_Diploma
                      ,[Start_Date] = @Start_Date
                      ,[Completion_Date] = @Completion_Date
                      ,[Completion_Percent] = @Completion_Percent 
                        WHERE [Id]= @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Applicant", item.Applicant);
                    comm.Parameters.AddWithValue("@Major", item.Major);
                    comm.Parameters.AddWithValue("@Certificate_Diploma", item.CertificateDiploma);
                    comm.Parameters.AddWithValue("@Start_Date", item.StartDate);
                    comm.Parameters.AddWithValue("@Completion_Date", item.CompletionDate);
                    comm.Parameters.AddWithValue("@Completion_Percent", item.CompletionPercent);

                    connection.Open();
                    int count = comm.ExecuteNonQuery();
                    connection.Close();
                }

            }
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<ApplicantEducationPoco> GetList(Expression<Func<ApplicantEducationPoco, bool>> where, params Expression<Func<ApplicantEducationPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }
    }
}
