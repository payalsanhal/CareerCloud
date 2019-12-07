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
    public class CompanyJobSkillRepository : IDataRepository<CompanyJobSkillPoco>
    {
        protected readonly string _connStr;
        public CompanyJobSkillRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params CompanyJobSkillPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (CompanyJobSkillPoco item in items)
                {
                    comm.CommandText = @"INSERT INTO [dbo].[Company_Job_Skills]
                                           ([Id]
                                           ,[Job]
                                           ,[Skill]
                                           ,[Skill_Level]
                                           ,[Importance])
                                     VALUES
                                           (@Id
                                           ,@Job
                                           ,@Skill
                                           ,@Skill_Level
                                           ,@Importance)";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Job", item.Job);
                    comm.Parameters.AddWithValue("@Skill", item.Skill);
                    comm.Parameters.AddWithValue("@Skill_Level", item.SkillLevel);
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

        public IList<CompanyJobSkillPoco> GetAll(params Expression<Func<CompanyJobSkillPoco, object>>[] navigationProperties)
        {

            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT 
                                   [Id]
                                  ,[Job]
                                  ,[Skill]
                                  ,[Skill_Level]
                                  ,[Importance]
                                  ,[Time_Stamp]
                          FROM [dbo].[Company_Job_Skills]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                CompanyJobSkillPoco[] pocos = new CompanyJobSkillPoco[6000];
                while (sqlReader.Read())
                {
                    CompanyJobSkillPoco poco = new CompanyJobSkillPoco();
                    poco.Id = sqlReader.GetGuid(0);
                    poco.Job = sqlReader.GetGuid(1);
                    poco.Skill= sqlReader.GetString(2);
                    poco.SkillLevel= sqlReader.GetString(3);
                    poco.Importance = (int)sqlReader[4];
                    poco.TimeStamp = (byte[])sqlReader[5];
                    pocos[index] = poco;
                    index++;
                }
                connection.Close();
                return pocos.ToList();
            }
        }

        public IList<CompanyJobSkillPoco> GetList(Expression<Func<CompanyJobSkillPoco, bool>> where, params Expression<Func<CompanyJobSkillPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyJobSkillPoco GetSingle(Expression<Func<CompanyJobSkillPoco, bool>> where, params Expression<Func<CompanyJobSkillPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyJobSkillPoco> pocos = GetAll().AsQueryable();
            // return pocos.Where(where).FirstOrDefault();
            CompanyJobSkillPoco item = new CompanyJobSkillPoco();
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

        public void Remove(params CompanyJobSkillPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (CompanyJobSkillPoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[Company_Job_Skills]
                                          WHERE [Id]= @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params CompanyJobSkillPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (CompanyJobSkillPoco item in items)
                {
                    comm.CommandText = @"UPDATE [dbo].[Company_Job_Skills]
                                       SET [Job] =   @Job
                                          ,[Skill] = @Skill
                                          ,[Skill_Level] = @Skill_Level
                                          ,[Importance] = @Importance
                                  WHERE [Id]= @Id";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Job", item.Job);
                    comm.Parameters.AddWithValue("@Skill", item.Skill);
                    comm.Parameters.AddWithValue("@Skill_Level", item.SkillLevel);
                    comm.Parameters.AddWithValue("@Importance", item.Importance);

                    connection.Open();
                    int count = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}