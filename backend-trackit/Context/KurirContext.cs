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

        public List<Kurir> getDataKurir()
        {
            List<Kurir> kurir = new List<Kurir>();

            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"select * from pegawai p 
                            join peran_pegawai pp on pp.id_peran = p.peran_pegawai_id_peran
	                        where id_peran = 2  ";

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                NpgsqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    kurir.Add(new Kurir
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
    }
}
