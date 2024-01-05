using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyManagenmentSystem
{
    // Adaptee

    public class DatabaseOperation
    {
        private SqlConnection con = new SqlConnection(@"Data Source=localhost;Initial Catalog=PharmacyManagmentSystem;Integrated Security=True");

        private SqlCommand cmd;
        private SqlDataReader dr;
        private SqlDataAdapter da;
        private DataTable dt;

        public bool Validation(string query)
        {
            con.Open();
            cmd = new SqlCommand(query, con);
            dr = cmd.ExecuteReader();
            bool result = dr.Read();
            con.Close();
            return result;
        }

        public string Execution(string query)
        {
            con.Open();
            cmd = new SqlCommand(query, con);
            int a = cmd.ExecuteNonQuery();
            con.Close();
            if (a > 0)
            {
                return "Execute Successfully";
            }
            else
            {
                return "Not Executed";
            }
        }

        public DataTable SearchDataTable(string query)
        {
            con.Open();
            dt = new DataTable();
            dt.Clear();
            da = new SqlDataAdapter(query, con);
            da.Fill(dt);
            con.Close();
            return dt;
        }


        public List<string> SearchList(string query)
        {
            List<string> items = new List<string>();
            con.Open();
            cmd = new SqlCommand(query, con);
            dr = cmd.ExecuteReader();
            items.Clear();
            while (dr.Read())
            {
                items.Add(dr[0].ToString());
            }
            con.Close();
            return items;
        }

        public string SearchText(string query)
        {
            string item = "";
            con.Open();
            cmd = new SqlCommand(query, con);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                item = dr[0].ToString();
            }
            con.Close();
            return item;
        }

    }
}

