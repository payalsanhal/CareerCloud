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
    public class ApplicantSkillRepository : IDataRepository<ApplicantSkillPoco>
    {
        protected readonly string _connStr;

        public ApplicantSkillRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params ApplicantSkillPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (ApplicantSkillPoco item in items)
                {
                    comm.CommandText = @"INSERT INTO [dbo].[Applicant_Skills]
                                           ([Id]
                                           ,[Applicant]
                                           ,[Skill]
                                           ,[Skill_Level]
                                           ,[Start_Month]
                                           ,[Start_Year]
                                           ,[End_Month]
                                           ,[End_Year])
                                     VALUES
                                           (@Id
                                           ,@Applicant
                                           ,@Skill
                                           ,@Skill_Level
                                           ,@Start_Month
                                           ,@Start_Year
                                           ,@End_Month
                                           ,@End_Year)";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Applicant", item.Applicant);
                    comm.Parameters.AddWithValue("@Skill", item.Skill);
                    comm.Parameters.AddWithValue("@Skill_Level", item.SkillLevel);
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

        public IList<ApplicantSkillPoco> GetAll(params Expression<Func<ApplicantSkillPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT [Id]
                              ,[Applicant]
                              ,[Skill]
                              ,[Skill_Level]
                              ,[Start_Month]
                              ,[Start_Year]
                              ,[End_Month]
                              ,[End_Year]
                              ,[Time_Stamp]
                          FROM [dbo].[Applicant_Skills]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                ApplicantSkillPoco[] pocos = new ApplicantSkillPoco[500];
                while (sqlReader.Read())
                {
                    ApplicantSkillPoco poco = new ApplicantSkillPoco();
                    poco.Id = sqlReader.GetGuid(0);
                    poco.Applicant = sqlReader.GetGuid(1);
                    poco.Skill = sqlReader.GetString(2);
                    poco.SkillLevel = sqlReader.GetString(3);
                    poco.StartMonth = sqlReader.GetByte(4);
                    poco.StartYear= (int)sqlReader[5];
                    poco.EndMonth= sqlReader.GetByte(6);
                    poco.EndYear= (int)sqlReader[7];
                   
                    poco.TimeStamp = (byte[])sqlReader[8];
                    pocos[index] = poco;
                    index++;
                }
                connection.Close();
                return pocos.ToList();
            }
        }

        public IList<ApplicantSkillPoco> GetList(Expression<Func<ApplicantSkillPoco, bool>> where, params Expression<Func<ApplicantSkillPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantSkillPoco GetSingle(Expression<Func<ApplicantSkillPoco, bool>> where, params Expression<Func<ApplicantSkillPoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantSkillPoco> pocos = GetAll().AsQueryable();
            // return pocos.Where(where).FirstOrDefault();
            ApplicantSkillPoco item = new ApplicantSkillPoco();
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

        public void Remove(params ApplicantSkillPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (ApplicantSkillPoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[Applicant_Skills]
                                          WHERE [Id]= @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params ApplicantSkillPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (ApplicantSkillPoco item in items)
                {
                    comm.CommandText = @"UPDATE [dbo].[Applicant_Skills]
                        SET  [Applicant] =		 @Applicant
                              ,[Skill] =		 @Skill
                              ,[Skill_Level] =	 @Skill_Level
                              ,[Start_Month] =	 @Start_Month
                              ,[Start_Year] =	 @Start_Year
                              ,[End_Month] =	 @End_Month
                              ,[End_Year] =		 @End_Year
                        WHERE [Id]= @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Applicant", item.Applicant);
                    comm.Parameters.AddWithValue("@Skill", item.Skill);
                    comm.Parameters.AddWithValue("@Skill_Level", item.SkillLevel);
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
