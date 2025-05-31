using backend_trackit.Helper;
using backend_trackit.Models;
using Npgsql;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace backend_trackit.Context
{
    public class AdminContext
    {
        private string _constr;
        private string _errMsg;

        public AdminContext(string conn)
        {
            _constr = conn;
        }

        public List<OrderCustomer> getDataOrderNotAcceptedByKecamatan(int id)
        {
            List<OrderCustomer> orders = new List<OrderCustomer>();

            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"select * from ""Order"" where isaccepted = false and id_kecamatan_pengirim = @id_kecamatan";

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_kecamatan", id);

                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new OrderCustomer()
                    {
                        id_order = int.Parse(reader["id_order"].ToString()),
                        id_customer_order = int.Parse(reader["customer_id_customer"].ToString()),
                        id_jenis_paket = int.Parse(reader["jenis_paket_id_jenis_paket"].ToString()),
                        berat = Convert.ToDouble(reader["berat_paket"]),
                        nama_pengirim = reader["nama_pengirim"].ToString(),
                        telepon_pengirim = reader["no_telepon_pengirim"].ToString(),
                        detail_alamat_pengirim = reader["detail_alamat_pengirim"].ToString(),
                        nama_penerima = reader["nama_penerima"].ToString(),
                        telepon_penerima = reader["no_telepon_penerima"].ToString(),
                        detail_alamat_penerima = reader["detail_alamat_penerima"].ToString(),
                        isAccepted = bool.Parse(reader["isaccepted"].ToString()),
                        waktu_order = DateTime.Parse(reader["waktu_order"].ToString()),
                        catatan_kurir = reader["catatan_kurir"].ToString(),
                        id_kecamatan_pengirim = int.Parse(reader["id_kecamatan_pengirim"].ToString()),
                        id_kecamatan_penerima = int.Parse(reader["id_kecamatan_penerima"].ToString()),
                    });
                }

                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
            }

            return orders;
        }

        public bool processOrderAccepted(ProcessOrderAccepted data)
        {
            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"insert into paket(no_resi, order_id_order, pegawai_id_pegawai, status_paket_id_status) values(@resi, @id_order, @id_pegawai, 1)";
            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@resi", data.no_resi);
                cmd.Parameters.AddWithValue("@id_order", data.id_order);
                cmd.Parameters.AddWithValue("@id_pegawai", data.id_kurir);

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

        public bool changeStatusOrder(int id_order)
        {
            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"update ""Order"" set isaccepted = not isaccepted where id_order = @id_order";
            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_order", id_order);

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
    }
}
