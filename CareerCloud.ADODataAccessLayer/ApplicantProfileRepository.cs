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
    public class ApplicantProfileRepository : IDataRepository<ApplicantProfilePoco>
    {
        protected readonly string _connStr;

        public ApplicantProfileRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params ApplicantProfilePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (ApplicantProfilePoco item in items)
                {
                    comm.CommandText = @"	INSERT INTO [dbo].[Applicant_Profiles]
			   ([Id]
			   ,[Login]
			   ,[Current_Salary]
			   ,[Current_Rate]
			   ,[Currency]
			   ,[Country_Code]
			   ,[State_Province_Code]
			   ,[Street_Address]
			   ,[City_Town]
			   ,[Zip_Postal_Code])
		        VALUES
			   (@Id,@Login,@Current_Salary,@Current_Rate,@Currency,@Country_Code,@State_Province_Code,@Street_Address,@City_Town,@Zip_Postal_Code)";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Login", item.Login);
                    comm.Parameters.AddWithValue("@Current_Salary", item.CurrentSalary);
                    comm.Parameters.AddWithValue("@Current_Rate", item.CurrentRate);
                    comm.Parameters.AddWithValue("@Currency", item.Currency);
                    comm.Parameters.AddWithValue("@Country_Code", item.Country);
                    comm.Parameters.AddWithValue("@State_Province_Code", item.Province);
                    comm.Parameters.AddWithValue("@Street_Address", item.Street);
                    comm.Parameters.AddWithValue("@City_Town", item.City);
                    comm.Parameters.AddWithValue("@Zip_Postal_Code", item.PostalCode);

                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public IList<ApplicantProfilePoco> GetAll(params Expression<Func<ApplicantProfilePoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT [Id]
                                  ,[Login]
                                  ,[Current_Salary]
                                  ,[Current_Rate]
                                  ,[Currency]
                                  ,[Country_Code]
                                  ,[State_Province_Code]
                                  ,[Street_Address]
                                  ,[City_Town]
                                  ,[Zip_Postal_Code]
                                  ,[Time_Stamp]
                              FROM [JOB_PORTAL_DB].[dbo].[Applicant_Profiles]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                ApplicantProfilePoco[] pocos = new ApplicantProfilePoco[500];
                while (sqlReader.Read())
                {
                    ApplicantProfilePoco poco = new ApplicantProfilePoco();
                    poco.Id = sqlReader.GetGuid(0);
                    poco.Login = sqlReader.GetGuid(1);
                    if (!sqlReader.IsDBNull(2))
                    {
                        poco.CurrentSalary = sqlReader.GetDecimal(2);
                    }
                    if (!sqlReader.IsDBNull(3))
                    {
                        poco.CurrentRate = sqlReader.GetDecimal(3);
                    }
                    poco.Currency = sqlReader.GetString(4);
                    poco.Country = sqlReader.GetString(5);
                    poco.Province = sqlReader.GetString(6);
                    poco.Street = sqlReader.GetString(7);
                    poco.City = sqlReader.GetString(8);
                    poco.PostalCode = sqlReader.GetString(9);
                    poco.TimeStamp = (byte[])sqlReader[10];
                    pocos[index] = poco;
                    index++;
                }
                connection.Close();
                return pocos.ToList();
            }
        }

        public ApplicantProfilePoco GetSingle(Expression<Func<ApplicantProfilePoco, bool>> where, params Expression<Func<ApplicantProfilePoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantProfilePoco> pocos = GetAll().AsQueryable();
            //return pocos.Where(where).FirstOrDefault();

            ApplicantProfilePoco item = new ApplicantProfilePoco();
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

        public void Remove(params ApplicantProfilePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (ApplicantProfilePoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[Applicant_Profiles]
                                          WHERE [Id]= @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params ApplicantProfilePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (ApplicantProfilePoco item in items)
                {
                    comm.CommandText = @"UPDATE [dbo].[Applicant_Profiles]
                       SET [Login] = @Login
                          ,[Current_Salary] = @Current_Salary
                          ,[Current_Rate] = @Current_Rate
                          ,[Currency] = @Currency
                          ,[Country_Code] = @Country_Code
                          ,[State_Province_Code] = @State_Province_Code
                          ,[Street_Address] = @Street_Address
                          ,[City_Town] = @City_Town
                          ,[Zip_Postal_Code] = @Zip_Postal_Code
                           WHERE [Id]= @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Login", item.Login);
                    comm.Parameters.AddWithValue("@Current_Salary", item.CurrentSalary);
                    comm.Parameters.AddWithValue("@Current_Rate", item.CurrentRate);
                    comm.Parameters.AddWithValue("@Currency", item.Currency);
                    comm.Parameters.AddWithValue("@Country_Code", item.Country);
                    comm.Parameters.AddWithValue("@State_Province_Code", item.Province);
                    comm.Parameters.AddWithValue("@Street_Address", item.Street);
                    comm.Parameters.AddWithValue("@City_Town", item.City);
                    comm.Parameters.AddWithValue("@Zip_Postal_Code", item.PostalCode);

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

        public IList<ApplicantProfilePoco> GetList(Expression<Func<ApplicantProfilePoco, bool>> where, params Expression<Func<ApplicantProfilePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

    }
}
