namespace backend_trackit.Models
{
    public class Pegawai
    {
        public int id {  get; set; }
        public string nama { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string kecamatan {  get; set; }
        public string kabupaten {  get; set; }
    }
}
