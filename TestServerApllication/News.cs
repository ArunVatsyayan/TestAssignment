using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TestServerApllication
{
    public class News
    {
        public struct NewsStruct
        {
            public long ID { get; set; }
            public string Headline { get; set; }
            public DateTime DateTimeUpdate { get; set; }
            public string FullNewsLink { get; set; }
            public string NewsContent { get; set; }
            public string Source { get; set; }
        }
        public static List<NewsStruct> selectAll()
        {
            List<NewsStruct> SDML = new List<NewsStruct>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GET_NEWS";

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    NewsStruct SDM = new NewsStruct();
                    SDM.ID = (long)reader["ID"];
                    SDM.Headline = (string)reader["Headline"];
                    SDM.FullNewsLink = (string)reader["FullNewsLink"];
                    SDM.NewsContent = (string)reader["NewsContent"];
                    SDM.Source = (string)reader["Source"];
                    SDM.DateTimeUpdate = (DateTime)reader["DateTimeUpdate"];
                    SDML.Add(SDM);

                }



                connection.Close();

            }
            return SDML;
        }
    }
}