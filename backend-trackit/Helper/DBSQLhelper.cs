using System.Data;
using Npgsql;

namespace backend_trackit.Helper
{
    public class DBSQLhelper
    {
        private NpgsqlConnection connection;
        private string __constr;

        public DBSQLhelper(string pConstr)
        {
            __constr = pConstr;
            connection = new NpgsqlConnection(pConstr);
            connection.ConnectionString = __constr;
        }

        public NpgsqlCommand GetNpgsqlCommand(string query)
        {
            connection.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        public void closeConnection()
        {
            connection.Close();
        }
    }
}
