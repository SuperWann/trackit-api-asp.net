namespace backend_trackit.Models
{
    public class AddListAlamat
    {
        public int id_alamat {  get; set; }
        public string nama { get; set; }
        public string no_telepon { get; set; }
        public string detail_alamat { get; set; }
        public int id_customer { get; set; }
        public int id_kecamatan { get; set;}
    }
}
