using System.Data.SqlClient;
using System.Collections.Generic;
using System;
using System.Data;

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
        public static Dictionary<string, string>[] GetInquiries()
        {
            string sql = "SELECT [Data] FROM [dbo].[Inquiries] WHERE Reviewed=0 Order By RequestDate DESC";
            string[] jsons = ExecuteScalars(sql, "Data");

            List<Dictionary<string, string>> inquiries = new List<Dictionary<string, string>>();
            foreach (string json in jsons)
                inquiries.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(json));

            return inquiries.ToArray();
        }

        internal static string DisplayInquiries(Dictionary<string, string>[] dictionaries)
        {
            string result = string.Empty;
            foreach (Dictionary<string, string> dictionary in dictionaries)
            {
                foreach (KeyValuePair<string, string> item in dictionary)
                    result += item.Key + ": " + item.Value + "\r\n";

                result += "\r\n";
            }
            return result;
        }

        private static void ExecuteNonQuery(string sql)
        {
            SqlConnection cn = new SqlConnection(HackerDevsConnectionstring());
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            cm.ExecuteNonQuery();
            cn.Close();
        }
        private static string ExecuteScalar(string sql)
        {
            string result = string.Empty;
            SqlConnection cn = new SqlConnection(HackerDevsConnectionstring());
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            object obj = cm.ExecuteScalar();
            if (obj != null)
                result = obj.ToString();
            cn.Close();
            return result;
        }
        private static string[] ExecuteScalars(string sql, string field)
        {
            List<string> result = new List<string>();
            SqlConnection cn = new SqlConnection(HackerDevsConnectionstring());
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader sdr = cm.ExecuteReader();
            while (sdr.Read())
            {
                result.Add(sdr[field].ToString());
            }
            cn.Close();
            return result.ToArray();
        }        
        private static string HackerDevsConnectionstring()
        {
            return @"data source=.\sqlexpress; initial catalog=HackerDevs; user id=sql_app_user; password=$en$road222!";
        }

        public static string JsonObjectToHTML(Dictionary<string, object> jsonObj)
        {
            string result = "<ul>";

            foreach (string key in jsonObj.Keys)
            {
                result += "<li>" + key;

                string str = jsonObj[key] != null ? jsonObj[key].ToString() : string.Empty;

                if (str.StartsWith("{"))
                    result += JsonObjectToHTML(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(str));
                else if (str.StartsWith("["))
                    result += JsonArrayToHTML(Newtonsoft.Json.JsonConvert.DeserializeObject<object[]>(str), key);
                else
                    result += " : " + str;

                result += "</li>";
            }

            result += "</ul>";
            return result;

        }

        public static string JsonArrayToHTML(object[] jsonStrings, string key)
        {
            string result = "<ul>";
            int count = 1;
            foreach (object obj in jsonStrings)
            {
                string str = obj != null ? obj.ToString() : string.Empty;

                result += "<li>" + key + " - " + count;

                if (str.StartsWith("{"))
                    result += JsonObjectToHTML(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(str));
                else if (str.StartsWith("["))
                    result += JsonArrayToHTML(Newtonsoft.Json.JsonConvert.DeserializeObject<object[]>(str), str + " - " + count);
                else
                    result += " : " + str;

                result += "</li>";

                count++;
            }

            result += "</ul>";
            return result;
        }

    }
}
