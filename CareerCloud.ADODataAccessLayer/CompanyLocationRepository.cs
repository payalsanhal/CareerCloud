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
    public class CompanyLocationRepository : IDataRepository<CompanyLocationPoco>
    {
        protected readonly string _connStr;
        public CompanyLocationRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params CompanyLocationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (CompanyLocationPoco item in items)
                {
                    comm.CommandText = @"INSERT INTO [dbo].[Company_Locations]
                                           ([Id]
                                           ,[Company]
                                           ,[Country_Code]
                                           ,[State_Province_Code]
                                           ,[Street_Address]
                                           ,[City_Town]
                                           ,[Zip_Postal_Code])
                                     VALUES
                                           (@Id
                                           ,@Company
                                           ,@Country_Code
                                           ,@State_Province_Code
                                           ,@Street_Address
                                           ,@City_Town
                                           ,@Zip_Postal_Code)";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Company", item.Company);
                    comm.Parameters.AddWithValue("@Country_Code", item.CountryCode);
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

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<CompanyLocationPoco> GetAll(params Expression<Func<CompanyLocationPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT  [Id]
                                  ,[Company]
                                  ,[Country_Code]
                                  ,[State_Province_Code]
                                  ,[Street_Address]
                                  ,[City_Town]
                                  ,[Zip_Postal_Code]
                                  ,[Time_Stamp]
                              FROM [JOB_PORTAL_DB].[dbo].[Company_Locations]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                CompanyLocationPoco[] pocos = new CompanyLocationPoco[500];
                while (sqlReader.Read())
                {
                    CompanyLocationPoco poco = new CompanyLocationPoco();
                    poco.Id = sqlReader.GetGuid(0);
                    poco.Company = sqlReader.GetGuid(1);
                    poco.CountryCode = sqlReader.GetString(2);
                    poco.Province = sqlReader.GetString(3);
                    poco.Street = sqlReader.GetString(4);
                    if (!sqlReader.IsDBNull(5))
                    {
                        poco.City = sqlReader.GetString(5);
                    }
                    if (!sqlReader.IsDBNull(6))
                    {
                        poco.PostalCode = sqlReader.GetString(6);
                    }
                    poco.TimeStamp = (byte[])sqlReader[7];
                    pocos[index] = poco;
                    index++;
                }
                connection.Close();
                return pocos.ToList();
            }
        }

        public IList<CompanyLocationPoco> GetList(Expression<Func<CompanyLocationPoco, bool>> where, params Expression<Func<CompanyLocationPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyLocationPoco GetSingle(Expression<Func<CompanyLocationPoco, bool>> where, params Expression<Func<CompanyLocationPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyLocationPoco> pocos = GetAll().AsQueryable();
            // return pocos.Where(where).FirstOrDefault();
            CompanyLocationPoco item = new CompanyLocationPoco();
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

        public void Remove(params CompanyLocationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (CompanyLocationPoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[Company_Locations]
                                          WHERE [Id]= @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params CompanyLocationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (CompanyLocationPoco item in items)
                {
                    comm.CommandText = @"UPDATE[dbo].[Company_Locations]
                                   SET [Company] = @Company
                                      ,[Country_Code] = @Country_Code
                                      ,[State_Province_Code] = @State_Province_Code
                                      ,[Street_Address] = @Street_Address
                                      ,[City_Town] = @City_Town
                                      ,[Zip_Postal_Code] = @Zip_Postal_Code
                                       WHERE [Id]= @Id";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Company", item.Company);
                    comm.Parameters.AddWithValue("@Country_Code", item.CountryCode);
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
    }
}
