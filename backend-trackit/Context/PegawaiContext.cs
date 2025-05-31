using backend_trackit.Helper;
using backend_trackit.Models;
using Npgsql;

namespace backend_trackit.Context
{
    public class PegawaiContext
    {
        private string _constr;
        private string _errMsg;

        public PegawaiContext(string conn)
        {
            _constr = conn;
        }

        public Pegawai getPegawaiLogin(string email, string password)
        {
            Pegawai pegawai = null;
            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"select * from pegawai p 
                            join peran_pegawai pp on pp.id_peran = p.peran_pegawai_id_peran
	                        join kecamatan k on k.id_kecamatan = p.kecamatan_id_kecamatan 
	                        join kabupaten kk on kk.id_kabupaten = k.kabupaten_id_kabupaten
                            where email = @email and password = @password";

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", password);

                NpgsqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    pegawai = new Pegawai() 
                    {
                        id = int.Parse(reader["id_pegawai"].ToString()),
                        nama = reader["nama_pegawai"].ToString(),
                        email = reader["email"].ToString(),
                        password = reader["password"].ToString(),
                        role = reader["nama_peran"].ToString(),
                        id_kecamatan = int.Parse(reader["kecamatan_id_kecamatan"].ToString()),
                        kabupaten = reader["nama_kabupaten"].ToString(),
                        kecamatan = reader["nama_kecamatan"].ToString()
                    };
                }

                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
            }

            return pegawai;

        }
    }
}
