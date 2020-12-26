using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
namespace TrangChuWebsite.Models
{
    public class Database
    {
        string StringCon = ConfigurationManager.ConnectionStrings["connection"].ToString();
        public SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(StringCon);
            return conn;
        }
        public DataTable GetTable(string sql)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = GetConnection();
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }
        public void Execute(string sql)
        {
            SqlConnection conn = GetConnection();
            using (conn)
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}