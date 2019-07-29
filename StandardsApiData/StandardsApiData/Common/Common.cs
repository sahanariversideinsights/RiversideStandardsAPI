using System.Data;
using System.Data.SqlClient;

namespace StandardsApiData.Common
{
    public class Common
    {

        public static string GetConnectionString()
        {
            AppConfiguration appConfiguration = new AppConfiguration();
            string cs = appConfiguration.ConnectionString;
            return cs;
        }


        public DataTable GetProviderSettings(string providerName)
        {
            DataTable dt = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(GetConnectionString()))
            {
                string sql = "select Keyname as ConfigKey,Value as ConfigValue from AppSettings where lower(providername) ='" + providerName.ToLower() + "' order by providername";

                using (SqlCommand cmd = new SqlCommand(sql, sqlConn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            return dt;
        }
    }      
}
