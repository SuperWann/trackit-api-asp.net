using backend_trackit.Helper;
using backend_trackit.Models;
using Npgsql;

namespace backend_trackit.Context
{
    public class CustomerContext
    {
        private string _constr;
        private string _errMsg;

        public CustomerContext(string conn)
        {
            _constr = conn;
        }

        public bool cariNomorTelepon(string no_telepon)
        {
            bool isExist = false;
            string query = @"SELECT COUNT(*) FROM customer WHERE no_telepon = @no_telepon";
            DBSQLhelper db = new DBSQLhelper(this._constr);

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@no_telepon", no_telepon);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                isExist = (count > 0);

                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
            }

            return isExist;
        }

        public bool checkLoginCustomer(string no_telepon, string pin)
        {
            bool isExist = false;
            string query = @"SELECT COUNT(*) FROM customer WHERE no_telepon = @no_telepon and pin = @pin";
            DBSQLhelper db = new DBSQLhelper(this._constr);

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@no_telepon", no_telepon);
                cmd.Parameters.AddWithValue("@pin", pin);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                isExist = (count > 0);

                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
                Console.WriteLine($"no_telepon = {no_telepon}, pin = {pin}");
            }

            return isExist;
        }

        public List<Customer> getDataCustomerLogin(string no_telepon, string pin)
        {
            List<Customer> customer = new List<Customer>();

            string query = @"SELECT id_customer, nama_customer, no_telepon, pin, nama_kecamatan, nama_kabupaten FROM customer c 
                            join kecamatan k on c.kecamatan_id_kecamatan = k.id_kecamatan
                            join kabupaten kk on kk.id_kabupaten = k.kabupaten_id_kabupaten
                            WHERE no_telepon = @no_telepon and pin = @pin";

            DBSQLhelper db = new DBSQLhelper(this._constr);

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@no_telepon", no_telepon);
                cmd.Parameters.AddWithValue("@pin", pin);

                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customer.Add(new Customer()
                    {
                        id = int.Parse(reader["id_customer"].ToString()),
                        nama = reader["nama_customer"].ToString(),
                        no_telepon = reader["no_telepon"].ToString(),
                        pin = reader["pin"].ToString(),
                        kecamatan = reader["nama_kecamatan"].ToString(),
                        kabupaten = reader["nama_kabupaten"].ToString()
                    });
                }

                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
            }

            return customer;
        }
    }
}
