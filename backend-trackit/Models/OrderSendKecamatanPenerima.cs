namespace backend_trackit.Models
{
    public class OrderSendKecamatanPenerima
    {
        public string no_resi { get; set; }
        public int id_kurir {  get; set; }
        public DateTime waktu { get; set; }
        public string deskripsi { get; set; }
    }
}
