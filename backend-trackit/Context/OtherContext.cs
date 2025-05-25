using backend_trackit.Helper;
using backend_trackit.Models;
using Npgsql;

namespace backend_trackit.Context
{
    public class OtherContext
    {
        private string _constr;
        private string _errMsg;

        public OtherContext(string conn)
        {
            _constr = conn;
        }

        public List<JenisPaket> getDataJenisPaket()
        {
            List<JenisPaket> jenis = new List<JenisPaket>();

            string query = @"select * from jenis_paket";

            DBSQLhelper db = new DBSQLhelper(this._constr);

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                NpgsqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    jenis.Add(new JenisPaket
                    {
                        id_jenis = int.Parse(reader["id_jenis_paket"].ToString()),
                        nama_jenis = reader["nama_jenis"].ToString()
                    });
                }

                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
            }

            return jenis;
        }

        public List<Kecamatan> GetDataKecamatanByKabupaten(string nama_kabupaten)
        {
            List<Kecamatan> dataKecamatan = new List<Kecamatan>();

            string query = @"select id_kecamatan, nama_kecamatan
                            from kecamatan JOIN kabupaten ON kecamatan.kabupaten_id_kabupaten = kabupaten.id_kabupaten
                            where nama_kabupaten = @nama_kabupaten";

            DBSQLhelper db = new DBSQLhelper(this._constr);

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@nama_kabupaten", nama_kabupaten);

                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dataKecamatan.Add(new Kecamatan
                    {
                        id_kecamatan = int.Parse(reader["id_kecamatan"].ToString()),
                        nama_kecamatan = reader["nama_kecamatan"].ToString()
                    });
                }

                cmd.Dispose();
                db.closeConnection();
            }
            catch(Exception ex)
            {
                _errMsg = ex.Message;
            }

            return dataKecamatan;
        }
    }
}
