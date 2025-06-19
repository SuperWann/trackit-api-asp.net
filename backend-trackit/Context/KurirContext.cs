using backend_trackit.Helper;
using backend_trackit.Models;
using Npgsql;

namespace backend_trackit.Context
{
    public class KurirContext
    {
        private string _constr;
        private string _errMsg;

        public KurirContext(string conn)
        {
            _constr = conn;
        }

        public List<DropdownKurir> getDataKurir(int id_kecamatan)
        {
            List<DropdownKurir> kurir = new List<DropdownKurir>();

            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"select * from pegawai p 
                            join peran_pegawai pp on pp.id_peran = p.peran_pegawai_id_peran
	                        where id_peran = 2 and kecamatan_id_kecamatan = @id_kecamatan";

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_kecamatan", id_kecamatan);

                NpgsqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    kurir.Add(new DropdownKurir
                    {
                        id_kurir = int.Parse(reader["id_pegawai"].ToString()),
                        nama = reader["nama_pegawai"].ToString(),
                        id_kecamatan = int.Parse(reader["kecamatan_id_kecamatan"].ToString())
                    });
                }

                cmd.Dispose();
                db.closeConnection();
            }
            catch(Exception ex)
            {
                _errMsg = ex.Message;
            }

            return kurir;
        }

        public List<ListPengirimanKurir> getDataPengirimanByKurir(int id_kurir)
        {
            List<ListPengirimanKurir> listPengiriman = new List<ListPengirimanKurir>();

            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"select * from ""Order"" o 
                            join paket p on o.id_order = p.order_id_order 
                            where pegawai_id_pegawai = @id_kurir";

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_kurir", id_kurir);

                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listPengiriman.Add(new ListPengirimanKurir()
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
            return listPengiriman;
        }
    }
}
