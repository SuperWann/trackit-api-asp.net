namespace backend_trackit.Models
{
    public class ListPengirimanKurir
    {
        public int id_order { get; set; }
        public string no_resi { get; set; }
        public int id_customer_order { get; set; }
        public int id_jenis_paket { get; set; }
        public int id_status_paket { get; set; }
        public double berat { get; set; }
        public string nama_pengirim { get; set; }
        public string telepon_pengirim { get; set; }
        public string detail_alamat_pengirim { get; set; }
        public int id_kecamatan_pengirim { get; set; }
        public string nama_penerima { get; set; }
        public string telepon_penerima { get; set; }
        public string detail_alamat_penerima { get; set; }
        public int id_kecamatan_penerima { get; set; }
        public string catatan_kurir { get; set; }
    }
}
