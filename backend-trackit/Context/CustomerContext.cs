using backend_trackit.Helper;
using backend_trackit.Models;
using Npgsql;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace backend_trackit.Context
{
    public class CustomerContext
    {
        private string _constr;
        private string _errMsg;

        public CustomerContext(string conn)
        {
            _constr = conn;
        }

        public bool cariNomorTelepon(string no_telepon)
        {
            bool isExist = false;
            string query = @"SELECT COUNT(*) FROM customer WHERE no_telepon = @no_telepon";
            DBSQLhelper db = new DBSQLhelper(this._constr);

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@no_telepon", no_telepon);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                isExist = (count > 0);

                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
            }

            return isExist;
        }

        public bool registrasiAkun(RegistrasiCustomer dataRegis)
        {
            List<RegistrasiCustomer> dataRegistrasi = new List<RegistrasiCustomer>();
            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"insert into customer(nama_customer, no_telepon, pin, kecamatan_id_kecamatan, detail_alamat) values(@nama, @telepon, @pin, @id_kecamatan, @detail_alamat)";

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@nama", dataRegis.nama);
                cmd.Parameters.AddWithValue("@telepon", dataRegis.no_telepon);
                cmd.Parameters.AddWithValue("@pin", dataRegis.pin);
                cmd.Parameters.AddWithValue("@id_kecamatan", dataRegis.id_kecamatan);
                cmd.Parameters.AddWithValue("@detail_alamat", dataRegis.detail_alamat);

                int rowsAffected = cmd.ExecuteNonQuery();

                cmd.Dispose();
                db.closeConnection();

                return rowsAffected > 0;
            }
            catch(Exception ex)
            {
                _errMsg = ex.Message;
                return false;
            }
        }

        public bool checkLoginCustomer(string no_telepon, string pin)
        {
            bool isExist = false;
            string query = @"SELECT COUNT (*) FROM customer WHERE no_telepon = @no_telepon and pin = @pin";
            DBSQLhelper db = new DBSQLhelper(this._constr);

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@no_telepon", no_telepon);
                cmd.Parameters.AddWithValue("@pin", pin);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                isExist = (count > 0);

                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
            }

            return isExist;
        }

        public List<Customer> getDataCustomerLogin(string no_telepon, string pin)
        {
            List<Customer> customer = new List<Customer>();

            string query = @"SELECT id_customer, nama_customer, no_telepon, pin, nama_kecamatan, nama_kabupaten, detail_alamat FROM customer c 
                            join kecamatan k on c.kecamatan_id_kecamatan = k.id_kecamatan
                            join kabupaten kk on kk.id_kabupaten = k.kabupaten_id_kabupaten
                            WHERE no_telepon = @no_telepon and pin = @pin";

            DBSQLhelper db = new DBSQLhelper(this._constr);

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@no_telepon", no_telepon);
                cmd.Parameters.AddWithValue("@pin", pin);

                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customer.Add(new Customer()
                    {
                        id = int.Parse(reader["id_customer"].ToString()),
                        nama = reader["nama_customer"].ToString(),
                        no_telepon = reader["no_telepon"].ToString(),
                        pin = reader["pin"].ToString(),
                        kecamatan = reader["nama_kecamatan"].ToString(),
                        kabupaten = reader["nama_kabupaten"].ToString(),
                        detail_alamat = reader["detail_alamat"].ToString()
                    });
                }

                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
            }

            return customer;
        }

        public bool orderPengiriman(OrderCustomer order)
        {
            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"insert into ""Order""(customer_id_customer, berat_paket, nama_pengirim, no_telepon_pengirim, detail_alamat_pengirim, 
                            jenis_paket_id_jenis_paket, nama_penerima, no_telepon_penerima, detail_alamat_penerima, isaccepted, catatan_kurir, waktu_order,
                            id_kecamatan_pengirim, id_kecamatan_penerima)
                            values( @id_customer, @berat_paket, @nama_pengirim, @telepon_pengirim, @detail_alamat_pengirim, @id_jenis_paket, 
                            @nama_penerima, @telepon_penerima, @detail_alamat_penerima, false, @catatan, @waktu_order, @id_kecamatan_pengirim, @id_kecamatan_penerima)";
            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_customer", order.id_customer_order);
                cmd.Parameters.AddWithValue("@berat_paket", order.berat);
                cmd.Parameters.AddWithValue("@nama_pengirim", order.nama_pengirim);
                cmd.Parameters.AddWithValue("@telepon_pengirim", order.telepon_pengirim);
                cmd.Parameters.AddWithValue("@detail_alamat_pengirim", order.detail_alamat_pengirim);
                cmd.Parameters.AddWithValue("@id_jenis_paket", order.id_jenis_paket);
                cmd.Parameters.AddWithValue("@nama_penerima", order.nama_penerima);
                cmd.Parameters.AddWithValue("@telepon_penerima", order.telepon_penerima);
                cmd.Parameters.AddWithValue("@detail_alamat_penerima", order.detail_alamat_penerima);
                cmd.Parameters.AddWithValue("@catatan", order.catatan_kurir);
                cmd.Parameters.AddWithValue("@waktu_order", order.waktu_order);
                cmd.Parameters.AddWithValue("@id_kecamatan_pengirim", order.id_kecamatan_pengirim);
                cmd.Parameters.AddWithValue("@id_kecamatan_penerima", order.id_kecamatan_penerima);

                int rowsAffected = cmd.ExecuteNonQuery();

                cmd.Dispose();
                db.closeConnection();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
                return false;
            }
        }

        public List<OrderCustomer> getDataOrderNotAccepted(int id)
        {
            List<OrderCustomer> orders = new List<OrderCustomer>();

            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"select * from ""Order"" where isaccepted = false and customer_id_customer = @id_customer";

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_customer", id);

                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()){
                    orders.Add(new OrderCustomer()
                    {
                        id_order = int.Parse(reader["id_order"].ToString()),
                        id_customer_order = int.Parse(reader["customer_id_customer"].ToString()),
                        id_jenis_paket = int.Parse(reader["jenis_paket_id_jenis_paket"].ToString()),
                        berat = Convert.ToDouble(reader["berat_paket"]),
                        nama_pengirim = reader["nama_pengirim"].ToString(),
                        telepon_pengirim = reader["no_telepon_pengirim"].ToString(),
                        detail_alamat_pengirim = reader["detail_alamat_pengirim"].ToString(),
                        nama_penerima = reader["nama_penerima"].ToString(),
                        telepon_penerima = reader["no_telepon_penerima"].ToString(),
                        detail_alamat_penerima = reader["detail_alamat_penerima"].ToString(),
                        isAccepted = bool.Parse(reader["isaccepted"].ToString()),
                        waktu_order = DateTime.Parse(reader["waktu_order"].ToString()),
                        catatan_kurir = reader["catatan_kurir"].ToString(),
                        id_kecamatan_pengirim = int.Parse(reader["id_kecamatan_pengirim"].ToString()),
                        id_kecamatan_penerima = int.Parse(reader["id_kecamatan_penerima"].ToString()),
                    });
                }

                cmd.Dispose();
                db.closeConnection();
            }
            catch(Exception ex)
            {
                _errMsg = ex.Message;
            }

            return orders;
        }

        public List<OrderCustomerProcessed> getDataOrderProcessedByCustomer(int id_customer)
        {
            List<OrderCustomerProcessed> orders = new List<OrderCustomerProcessed>();

            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"select * from ""Order"" o 
                            join paket p on o.id_order = p.order_id_order 
                            JOIN pegawai pp ON p.pegawai_id_pegawai = pp.id_pegawai
                            where customer_id_customer = @id_customer";

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_customer", id_customer);

                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new OrderCustomerProcessed()
                    {
                        id_order = int.Parse(reader["id_order"].ToString()),
                        no_resi = reader["no_resi"].ToString(),
                        id_customer_order = int.Parse(reader["customer_id_customer"].ToString()),
                        id_jenis_paket = int.Parse(reader["jenis_paket_id_jenis_paket"].ToString()),
                        id_status_paket = int.Parse(reader["status_paket_id_status"].ToString()),
                        berat = Convert.ToDouble(reader["berat_paket"]),
                        nama_pengirim = reader["nama_pengirim"].ToString(),
                        telepon_pengirim = reader["no_telepon_pengirim"].ToString(),
                        detail_alamat_pengirim = reader["detail_alamat_pengirim"].ToString(),
                        nama_penerima = reader["nama_penerima"].ToString(),
                        telepon_penerima = reader["no_telepon_penerima"].ToString(),
                        detail_alamat_penerima = reader["detail_alamat_penerima"].ToString(),
                        isAccepted = bool.Parse(reader["isaccepted"].ToString()),
                        waktu_order = DateTime.Parse(reader["waktu_order"].ToString()),
                        catatan_kurir = reader["catatan_kurir"].ToString(),
                        id_kecamatan_pengirim = int.Parse(reader["id_kecamatan_pengirim"].ToString()),
                        id_kecamatan_penerima = int.Parse(reader["id_kecamatan_penerima"].ToString()),
                        nama_kurir = reader["nama_pegawai"].ToString()
                    });
                }

                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
            }

            return orders;
        }

        public List<OrderCustomerProcessed> getDataOrderProcessedByResi(string no_resi)
        {
            List<OrderCustomerProcessed> orders = new List<OrderCustomerProcessed>();

            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"select * from ""Order"" o 
                            join paket p on o.id_order = p.order_id_order 
                            JOIN pegawai pp ON p.pegawai_id_pegawai = pp.id_pegawai
                            where no_resi = @no_resi";

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@no_resi", no_resi);

                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new OrderCustomerProcessed()
                    {
                        id_order = int.Parse(reader["id_order"].ToString()),
                        no_resi = reader["no_resi"].ToString(),
                        id_customer_order = int.Parse(reader["customer_id_customer"].ToString()),
                        id_jenis_paket = int.Parse(reader["jenis_paket_id_jenis_paket"].ToString()),
                        id_status_paket = int.Parse(reader["status_paket_id_status"].ToString()),
                        berat = Convert.ToDouble(reader["berat_paket"]),
                        nama_pengirim = reader["nama_pengirim"].ToString(),
                        telepon_pengirim = reader["no_telepon_pengirim"].ToString(),
                        detail_alamat_pengirim = reader["detail_alamat_pengirim"].ToString(),
                        nama_penerima = reader["nama_penerima"].ToString(),
                        telepon_penerima = reader["no_telepon_penerima"].ToString(),
                        detail_alamat_penerima = reader["detail_alamat_penerima"].ToString(),
                        isAccepted = bool.Parse(reader["isaccepted"].ToString()),
                        waktu_order = DateTime.Parse(reader["waktu_order"].ToString()),
                        catatan_kurir = reader["catatan_kurir"].ToString(),
                        id_kecamatan_pengirim = int.Parse(reader["id_kecamatan_pengirim"].ToString()),
                        id_kecamatan_penerima = int.Parse(reader["id_kecamatan_penerima"].ToString()),
                        nama_kurir = reader["nama_pegawai"].ToString()
                    });
                }

                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
            }

            return orders;
        }

        public bool cancelOrder(int id_order)
        {
            string query = @"delete from ""Order"" where id_order = @id_order";
            DBSQLhelper db = new DBSQLhelper(this._constr);

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_order", id_order);

                int rowsAffected = cmd.ExecuteNonQuery();

                cmd.Dispose();
                db.closeConnection();

                return rowsAffected > 0;
            }catch (Exception ex)
            {
                _errMsg = ex.Message;
                return false;
            }
        }

        public List<CoordinateKecamatan> getCoordinateKecamatan(int id_kecamatan)
        {
            List<CoordinateKecamatan> coorsKecamatan = new List<CoordinateKecamatan>();

            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"select longitude, latitude from kecamatan where id_kecamatan = @id_kecamatan";

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_kecamatan", id_kecamatan);

                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    coorsKecamatan.Add(new CoordinateKecamatan()
                    {
                        longitude = Convert.ToDouble(reader["longitude"]),
                        latitude = Convert.ToDouble(reader["latitude"])
                    });
                }

                cmd.Dispose();
                db.closeConnection();
            }catch(Exception ex)
            {
                _errMsg = ex.Message;
            }

            return coorsKecamatan;
        }

        public List<TrackingHistory> getDataTrackingHistories(string no_resi)
        {
            List<TrackingHistory> trackingHistories = new List<TrackingHistory>();

            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"select * from tracking_history where paket_no_resi = @no_resi order by timestamp desc ";

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@no_resi", no_resi);

                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    trackingHistories.Add(new TrackingHistory()
                    {
                        timestamp = DateTime.Parse(reader["timestamp"].ToString()),
                        deskripsi = reader["deskripsi_lokasi"].ToString()
                    });
                }

                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
            }

            return trackingHistories;
        }

        public bool createListAlamat(AddListAlamat listAlamat)
        {
            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"insert into list_alamat(nama, no_telepon, detail_alamat, customer_id_customer, kecamatan_id_kecamatan) 
                            values (@nama, @telepon, @detail_alamat, @id_customer, @id_kecamatan)";

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@nama", listAlamat.nama);
                cmd.Parameters.AddWithValue("@telepon", listAlamat.no_telepon);
                cmd.Parameters.AddWithValue("@detail_alamat", listAlamat.detail_alamat);
                cmd.Parameters.AddWithValue("@id_customer", listAlamat.id_customer);
                cmd.Parameters.AddWithValue("@id_kecamatan", listAlamat.id_kecamatan);

                int rowsAffected = cmd.ExecuteNonQuery();

                cmd.Dispose();
                db.closeConnection();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
                return false;
            }
        }

        public List<ListAlamat> getListAlamatByCustomer(int id_customer)
        {
            DBSQLhelper db = new DBSQLhelper(this._constr);
            List<ListAlamat> listAlamat = new List<ListAlamat>();

            string query = @"select * from list_alamat la 
                            join kecamatan k on la.kecamatan_id_kecamatan = k.id_kecamatan
                            join kabupaten kk on k.kabupaten_id_kabupaten = kk.id_kabupaten
                            where customer_id_customer = @id_customer
                            order by id_alamat desc";

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue(@"id_customer", id_customer);

                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listAlamat.Add(new ListAlamat()
                    {
                        nama = reader["nama"].ToString(),
                        no_telepon = reader["no_telepon"].ToString(),
                        detail_alamat = reader["detail_alamat"].ToString(),
                        nama_kecamatan = reader["nama_kecamatan"].ToString(),
                        nama_kabupaten = reader["nama_kabupaten"].ToString(),
                    });
                }

                cmd.Dispose();
                db.closeConnection();

            }catch (Exception ex)
            {
                _errMsg = ex.Message;
            }

            return listAlamat;
        }

        public bool deleteAlamat(int id_alamat)
        {
            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"delete from list_alamat 
                            where id_alamat = @id_alamat;";

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_alamat", id_alamat);

                int rowsAffected = cmd.ExecuteNonQuery();

                cmd.Dispose();
                db.closeConnection();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
                Console.WriteLine(_errMsg);
                return false;
            }
        }

        public bool updateAlamat(AddListAlamat data)
        {
            DBSQLhelper db = new DBSQLhelper(this._constr);

            string query = @"update list_alamat 
                            set nama = @nama, 
                                    no_telepon = @telepon, 
                                    detail_alamat = @detail_alamat, 
                                    customer_id_customer = @id_customer,
                                    kecamatan_id_kecamatan = @id_kecamatan
                            where id_alamat = @id_alamat";

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_alamat", data.id_alamat);
                cmd.Parameters.AddWithValue("@nama", data.nama);
                cmd.Parameters.AddWithValue("@telepon", data.no_telepon);
                cmd.Parameters.AddWithValue("@detail_alamat", data.detail_alamat);
                cmd.Parameters.AddWithValue("@id_customer", data.id_customer);
                cmd.Parameters.AddWithValue("@id_kecamatan", data.id_kecamatan);

                int rowsAffected = cmd.ExecuteNonQuery();

                cmd.Dispose();
                db.closeConnection();

                return rowsAffected > 0;

            }catch (Exception ex)
            {
                _errMsg = ex.Message;
                return false;
            }
        }
    }
}
