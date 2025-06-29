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
        public List<Kecamatan> GetDataKecamatanByIdKabupaten(int id_kabupaten)
        {
            List<Kecamatan> dataKecamatan = new List<Kecamatan>();

            string query = @"select *
                            from kecamatan JOIN kabupaten ON kecamatan.kabupaten_id_kabupaten = kabupaten.id_kabupaten
                            where id_kabupaten = @id_kabupaten";

            DBSQLhelper db = new DBSQLhelper(this._constr);

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_kabupaten", id_kabupaten);

                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dataKecamatan.Add(new Kecamatan
                    {
                        id_kecamatan = int.Parse(reader["id_kecamatan"].ToString()),
                        nama_kecamatan = reader["nama_kecamatan"].ToString(),
                        latitude = Convert.ToDouble(reader["latitude"]),
                        longitude = Convert.ToDouble(reader["longitude"]),
                    });
                }

                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
            }

            return dataKecamatan;
        }


        public List<Kecamatan> GetDataKecamatanByKabupaten(string nama_kabupaten)
        {
            List<Kecamatan> dataKecamatan = new List<Kecamatan>();

            string query = @"select *
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
                        nama_kecamatan = reader["nama_kecamatan"].ToString(),
                        latitude = Convert.ToDouble(reader["latitude"]),
                        longitude = Convert.ToDouble(reader["longitude"]),
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

        public List<StatusPaket> GetDataStatusPaket()
        {
            List<StatusPaket> dataStatus = new List<StatusPaket>();

            string query = @"select * from status_paket";

            DBSQLhelper db = new DBSQLhelper(this._constr);

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                NpgsqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dataStatus.Add(new StatusPaket
                    {
                        id_status = int.Parse(reader["id_status"].ToString()),
                        nama_status = reader["nama_status"].ToString(),
                    });
                }

                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
            }

            return dataStatus;
        }

        public List<Kabupaten> getAllkabupaten()
        {
            List<Kabupaten> listKabupaten = new List<Kabupaten>();

            string query = @"select * from kabupaten";

            DBSQLhelper db = new DBSQLhelper(this._constr);

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                NpgsqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    listKabupaten.Add(new Kabupaten
                    {
                        id_kabupaten = int.Parse(reader["id_kabupaten"].ToString()),
                        nama_kabupaten = reader["nama_kabupaten"].ToString(),
                    });
                }

                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
            }

            return listKabupaten;

        }
    }
}
