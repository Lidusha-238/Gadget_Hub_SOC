using System;
using System.Data;
using System.Data.SqlClient;

namespace Data_Access
{
    public class Database
    {
        public readonly string conn_string = "Data Source=DESKTOP-T0DGFL0;Initial Catalog=Gadget_Hub;Integrated Security=True;";

        public int exeQuery(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(conn_string))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    if (parameters != null && parameters.Length > 0)
                        cmd.Parameters.AddRange(parameters);

                    return cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }

        public DataSet exeSelectQuery(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(conn_string))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    if (parameters != null && parameters.Length > 0)
                        cmd.Parameters.AddRange(parameters);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
