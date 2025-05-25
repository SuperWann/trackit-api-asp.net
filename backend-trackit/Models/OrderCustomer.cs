namespace backend_trackit.Models
{
    public class OrderCustomer
    {
        public int id_customer_order {  get; set; }
        public int id_jenis_paket {  get; set; }
        public double berat {  get; set; }
        public string nama_pengirim { get; set; }
        public string telepon_pengirim { get; set; }
        public string detail_alamat_pengirim { get; set; }
        public string nama_penerima {  get; set; }
        public string telepon_penerima { get; set; }
        public string detail_alamat_penerima { get; set; }
        public bool isAccepted { get; set; }    
        public string catatan_kurir {  get; set; }
        public DateTime waktu_order {  get; set; }
    }
}
