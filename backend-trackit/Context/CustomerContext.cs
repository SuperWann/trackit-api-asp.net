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

            string query = @"SELECT id_customer, nama_customer, no_telepon, pin, nama_kecamatan, nama_kabupaten, detail_alamat FROM customer c 
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
                        kabupaten = reader["nama_kabupaten"].ToString(),
                        detail_alamat = reader["detail_alamat"].ToString()
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

        public bool orderPengiriman(OrderCustomer order)
        {
            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"insert into ""Order""(customer_id_customer, berat_paket, nama_pengirim, no_telepon_pengirim, detail_alamat_pengirim, 
                            jenis_paket_id_jenis_paket, nama_penerima, no_telepon_penerima, detail_alamat_penerima, isaccepted, catatan_kurir, waktu_order)
                            values( @id_customer, @berat_paket, @nama_pengirim, @telepon_pengirim, @detail_alamat_pengirim, @id_jenis_paket, 
                            @nama_penerima, @telepon_penerima, @detail_alamat_penerima, false, @catatan, @waktu_order)";
            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_customer", order.id_customer_order);
                cmd.Parameters.AddWithValue("@berat_paket", order.berat);
                cmd.Parameters.AddWithValue("@nama_pengirim", order.nama_pengirim);
                cmd.Parameters.AddWithValue("@telepon_pengirim", order.telepon_pengirim);
                cmd.Parameters.AddWithValue("@detail_alamat_pengirim", order.detail_alamat_pengirim);
                cmd.Parameters.AddWithValue("@id_jenis_paket", order.id_jenis_paket);
                cmd.Parameters.AddWithValue("@nama_penerima", order.nama_penerima);
                cmd.Parameters.AddWithValue("@telepon_penerima", order.telepon_penerima);
                cmd.Parameters.AddWithValue("@detail_alamat_penerima", order.detail_alamat_penerima);
                cmd.Parameters.AddWithValue("@catatan", order.catatan_kurir);
                cmd.Parameters.AddWithValue("@waktu_order", order.waktu_order);

                int rowsAffected = cmd.ExecuteNonQuery();

                cmd.Dispose();
                db.closeConnection();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
                return false;
            }
        }

        /*public List<OrderCustomer> getDataOrderByStatus(int id)*/
    }
}
