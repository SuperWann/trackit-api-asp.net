using backend_trackit.Helper;
using Npgsql;

namespace backend_trackit.Context
{
    public partial class PaketContext
    {
        private string _constr;
        private string _errMsg;

        public PaketContext(string conn)
        {
            _constr = conn;
        }

        public async Task<bool> UpdateFotoBuktiPenyelesaianAsync(string noResi, string fotoUrl)
        {
            try
            {
                using var connection = new NpgsqlConnection(this._constr);
                await connection.OpenAsync();

                var query = @"
                    UPDATE paket 
                    SET foto_bukti_penyelesaian = @fotoUrl,
                    status_paket_id_status = 5
                    WHERE no_resi = @noResi";

                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@fotoUrl", fotoUrl);
                command.Parameters.AddWithValue("@noResi", noResi);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating foto bukti: {ex.Message}");
            }
        }

        public async Task<bool> IsPaketExistsAndValidForCompletion(string noResi)
        {
            DBSQLhelper db = new DBSQLhelper(this._constr);
            string query = @"
                    SELECT COUNT(*) 
                    FROM paket p
                    WHERE p.no_resi = @noResi";

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@noResi", noResi);

                var count = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                cmd.Dispose();
                db.closeConnection();

                return count > 0;
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
                return false;
            }
        }

        // Method untuk get current foto bukti (untuk delete old image)
        public async Task<string?> GetCurrentFotoBuktiAsync(string noResi)
        {
            try
            {
                using var connection = new NpgsqlConnection(this._constr);
                await connection.OpenAsync();

                var query = "SELECT foto_bukti_penyelesaian FROM paket WHERE no_resi = @noResi";

                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@noResi", noResi);

                var result = await command.ExecuteScalarAsync();
                return result?.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting current foto bukti: {ex.Message}");
            }
        }
    }
}