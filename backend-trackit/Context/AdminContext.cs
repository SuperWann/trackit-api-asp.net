using backend_trackit.Helper;
using backend_trackit.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public List<OrderCustomerProcessed> getDataOrderProcessedByResi(string no_resi)
        {
            List<OrderCustomerProcessed> orders = new List<OrderCustomerProcessed>();

            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"select * from ""Order"" o 
                            join paket p on o.id_order = p.order_id_order 
                            JOIN pegawai pp ON p.pegawai_id_pegawai = pp.id_pegawai
                            where no_resi = @no_resi";

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@no_resi", no_resi);

                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new OrderCustomerProcessed()
                    {
                        id_order = int.Parse(reader["id_order"].ToString()),
                        no_resi = reader["no_resi"].ToString(),
                        id_customer_order = int.Parse(reader["customer_id_customer"].ToString()),
                        id_jenis_paket = int.Parse(reader["jenis_paket_id_jenis_paket"].ToString()),
                        id_status_paket = int.Parse(reader["status_paket_id_status"].ToString()),
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
                        nama_kurir = reader["nama_pegawai"].ToString()
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

        public List<OrderCustomerProcessed> getDataOrderOnProcessByKecamatan(int id_kecamatan)
        {
            List<OrderCustomerProcessed> orders = new List<OrderCustomerProcessed>();

            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"select * from ""Order"" o 
                            join paket p on o.id_order = p.order_id_order 
                            JOIN pegawai pp ON p.pegawai_id_pegawai = pp.id_pegawai
                            where id_kecamatan_pengirim = @id_kecamatan or id_kecamatan_penerima = @id_kecamatan";

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_kecamatan", id_kecamatan);

                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new OrderCustomerProcessed()
                    {
                        id_order = int.Parse(reader["id_order"].ToString()),
                        no_resi = reader["no_resi"].ToString(),
                        id_customer_order = int.Parse(reader["customer_id_customer"].ToString()),
                        id_jenis_paket = int.Parse(reader["jenis_paket_id_jenis_paket"].ToString()),
                        id_status_paket = int.Parse(reader["status_paket_id_status"].ToString()),
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
                        nama_kurir = reader["nama_pegawai"].ToString()
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

        public bool processOrderAccepted(OrderAccepted data)
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

        public bool createTrackingHistoryAccepted(OrderAccepted data)
        {
            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"insert into tracking_history(paket_no_resi, timestamp, deskripsi_lokasi) values(@resi, @waktu, @deskripsi)";
            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@resi", data.no_resi);
                cmd.Parameters.AddWithValue("@waktu", data.waktu);
                cmd.Parameters.AddWithValue("@deskripsi", data.deskripsi);

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

        public bool createTrackingHistoryAfterAccepted(OrderProcessed data)
        {
            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"insert into tracking_history(paket_no_resi, timestamp, deskripsi_lokasi) values(@resi, @waktu, @deskripsi)";
            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@resi", data.no_resi);
                cmd.Parameters.AddWithValue("@waktu", data.waktu);
                cmd.Parameters.AddWithValue("@deskripsi", data.deskripsi);

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

        public bool createTrackingHistorySendKecamatanPenerima(OrderSendKecamatanPenerima data)
        {
            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"insert into tracking_history(paket_no_resi, timestamp, deskripsi_lokasi) values(@resi, @waktu, @deskripsi)";
            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@resi", data.no_resi);
                cmd.Parameters.AddWithValue("@waktu", data.waktu);
                cmd.Parameters.AddWithValue("@deskripsi", data.deskripsi);

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

        public bool changeStatusPaket(string no_resi, int id_status)
        {
            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"update paket set status_paket_id_status = @id_status where no_resi = @no_resi";
            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_status", id_status);
                cmd.Parameters.AddWithValue("@no_resi", no_resi);

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

        public bool changeKurirPaket(string no_resi, int id_pegawai)
        {
            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"update paket set pegawai_id_pegawai = @id_pegawai where no_resi = @no_resi";
            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_pegawai", id_pegawai);
                cmd.Parameters.AddWithValue("@no_resi", no_resi);

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

        public bool createAkunKurir(DataKurir data)
        {
            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"insert into pegawai(nama_pegawai, email, password, kecamatan_id_kecamatan, peran_pegawai_id_peran) 
                            values (@nama, @email, @password, @id_kecamatan, 2)";

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@nama", data.nama);
                cmd.Parameters.AddWithValue("@email", data.email);
                cmd.Parameters.AddWithValue("@password", data.password);
                cmd.Parameters.AddWithValue("@id_kecamatan", data.id_kecamatan);

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

        public List<DataKurir> getDataKurirKecamatan(int id_kecamatan)
        {
            List<DataKurir> dataKurirs = new List<DataKurir>();
            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"select * from pegawai where kecamatan_id_kecamatan = @id_kecamatan and peran_pegawai_id_peran = 2";

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_kecamatan", id_kecamatan);

                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dataKurirs.Add(new DataKurir()
                    {
                        nama = reader["nama_pegawai"].ToString(),
                        email = reader["email"].ToString(),
                        id_kecamatan = int.Parse(reader["kecamatan_id_kecamatan"].ToString()),
                    });
                }

                cmd.Dispose();
                db.closeConnection();
            }
            catch(Exception ex)
            {
                _errMsg = ex.Message;
            }

            return dataKurirs;
        }

        public bool deleteAkunKurir(int id_kurir)
        {
            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"delete from pegawai where id_pegawai = @id_pegawai";

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_pegawai", id_kurir);

                int rowsAffected = cmd.ExecuteNonQuery();

                cmd.Dispose();
                db.closeConnection();

                return rowsAffected > 0;
            }catch (Exception ex)
            {
                _errMsg = ex.Message;
                return false;
            }
        }
    }
}
