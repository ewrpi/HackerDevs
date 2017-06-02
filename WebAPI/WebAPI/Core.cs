using System.Data.SqlClient;
namespace WebAPI
{
    public class Core
    {
        public static void AddInquiry(string whoAreYou, string whatDoYouWant, string ipAddress)
        {
            string sql = @"INSERT INTO [dbo].[Inquiries]
                           ([WhoAreYou]
                           ,[WhatDoYouWant]
                           ,[RequestDate]
                           ,[IpAddress])
                     VALUES
                           ('" + whoAreYou + @"'
                           ,'" + whatDoYouWant + @"'
                           ,GetDate()
                           ,'" + ipAddress + @"')";

            ExecuteNonQuery(sql);
        }
        public static void AddInquiry(string data, string ipAddress)
        {
            string sql = @"INSERT INTO [dbo].[Inquiries]
                           ([Data]                           
                           ,[RequestDate]
                           ,[IpAddress])
                     VALUES
                           ('" + data + @"'                           
                           ,GetDate()
                           ,'" + ipAddress + @"')";

            ExecuteNonQuery(sql);
        }

        private static void ExecuteNonQuery(string sql)
        {
            SqlConnection cn = new SqlConnection(HackerDevsConnectionstring());
            cn.Open();
            SqlCommand cm = new SqlCommand(sql.Replace("'", "''"), cn);
            cm.ExecuteNonQuery();
            cn.Close();
        }
        private static string HackerDevsConnectionstring()
        {
            return @"data source=.\sqlexpress; initial catalog=HackerDevs; user id=sql_app_user; password=$en$road222!";
        }
    }
}
