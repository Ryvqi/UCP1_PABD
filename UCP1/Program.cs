﻿using System;
using System.Data;
using System.Data.SqlClient;

namespace Delete_data
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Program pr = new Program();
            while (true)
            {
                try
                {
                    Console.Write("\nKetik E untuk terhubung ke database atau Q untuk keluar dari aplikasi: ");
                    char chr = Convert.ToChar(Console.ReadLine().ToUpper());
                    switch (chr)
                    {
                        case 'E':
                            {
                                Console.Clear();
                                Console.WriteLine("Masukkan database yang dituju kemudian klik enter:");
                                string db = Console.ReadLine();
                                string strkoneksi = "Data Source=METEORITE\\SQL2019;Initial Catalog=PK;User ID=sa;Password=meteorite";
                                using (SqlConnection conn = new SqlConnection(strkoneksi))
                                {
                                    conn.Open();
                                    Console.Clear();
                                    while (true)
                                    {
                                        try
                                        {
                                            Console.WriteLine("\nMenu");
                                            Console.WriteLine("1. Kelola Data Almarhumm");
                                            Console.WriteLine("2. Laporkan Kematian");
                                            Console.WriteLine("3. Berikan Kesaksian");
                                            Console.WriteLine("4. Keluar");
                                            Console.WriteLine("\nEnter your choice (1-4): ");
                                            char ch = Convert.ToChar(Console.ReadLine());
                                            switch (ch)
                                            {
                                                case '1':
                                                    pr.ManageDeath(conn);
                                                    break;
                                                case '2':
                                                    pr.ReportDeath(conn);
                                                    break;
                                                case '3':
                                                    pr.ProvideTestimony(conn);
                                                    break;
                                                case '4':
                                                    return;
                                                default:
                                                    Console.Clear();
                                                    Console.WriteLine("\nInvalid option");
                                                    break;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.Clear();
                                            Console.WriteLine("\nError: " + ex.Message);
                                        }
                                    }
                                }
                            }
                        case 'Q':
                            return;
                        default:
                            Console.WriteLine("\nInvalid option");
                            break;
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Cannot access the specified database.\n");
                    Console.ResetColor();
                }
            }
        }

        // Method untuk menu Kelola Data Almarhumm
        public void ManageDeath(SqlConnection conn)
        {
            while (true)
            {
                Console.WriteLine("\nMenu Almarhumm");
                Console.WriteLine("1. Lihat Data Almarhumm");
                Console.WriteLine("2. Update Data Almarhumm");
                Console.WriteLine("3. Tambah Data Almarhumm");
                Console.WriteLine("4. Hapus Data Almarhumm");
                Console.WriteLine("5. Kembali ke Menu Utama");
                Console.WriteLine("\nEnter your choice (1-5): ");

                char choice = Convert.ToChar(Console.ReadLine());

                switch (choice)
                {
                    case '1':
                        ViewDeath(conn);
                        break;
                    case '2':
                        UpdateDeath(conn);
                        break;
                    case '3':
                        AddDeath(conn);
                        break;
                    case '4':
                        DeleteDeath(conn);
                        break;
                    case '5':
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine("\nInvalid option");
                        break;
                }
            }
        }

        // Method untuk melihat data almarhumm
        public void ViewDeath(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Almarhumm", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("Id_Almarhumm: " + reader["Id_Almarhumm"].ToString());
                Console.WriteLine("Nama: " + reader["Nama"].ToString());
                Console.WriteLine("Tgl_Lahir: " + GetDateTimeSafe(reader, "Tgl_Lahir").ToString("yyyy-MM-dd"));
                Console.WriteLine("Tgl_Kematian: " + GetDateTimeSafe(reader, "Tgl_Kematian").ToString("yyyy-MM-dd"));
                Console.WriteLine("Penyebab_Kematian: " + reader["Penyebab_Kematian"].ToString());
                Console.WriteLine();
            }
            reader.Close();
        }

        // Method untuk mendapatkan nilai DateTime dari SqlDataReader dengan penanganan nilai null
        private DateTime GetDateTimeSafe(SqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? DateTime.MinValue : reader.GetDateTime(ordinal);
        }

        // Method untuk menambah data almarhumm
        public void AddDeath(SqlConnection conn)
        {
            Console.WriteLine("Masukkan Id Almarhumm: ");
            string Id_Almarhumm = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(Id_Almarhumm))
            {
                Console.WriteLine("ID tidak boleh kosong.");
                return;
            }
            if (!int.TryParse(Id_Almarhumm, out _))
            {
                Console.WriteLine("ID harus berupa angka.");
                return;
            }

            string queryCheck = "SELECT COUNT(*) FROM Almarhumm WHERE Id_Almarhumm = @Id_Almarhumm";
            SqlCommand cmdCheck = new SqlCommand(queryCheck, conn);
            cmdCheck.Parameters.AddWithValue("@Id_Almarhumm", Id_Almarhumm);
            int existingRecords = (int)cmdCheck.ExecuteScalar();
            if (existingRecords > 0)
            {
                Console.WriteLine("Data dengan ID yang sama sudah ada.");
                return;
            }

            Console.WriteLine("Masukkan Nama Almarhumm: ");
            string Nama = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(Nama))
            {
                Console.WriteLine("Nama tidak boleh kosong.");
                return;
            }
            if (int.TryParse(Nama, out _))
            {
                Console.WriteLine("Nama tidak boleh berupa angka.");
                return;
            }

            queryCheck = "SELECT COUNT(*) FROM Almarhumm WHERE Nama = @Nama";
            cmdCheck = new SqlCommand(queryCheck, conn);
            cmdCheck.Parameters.AddWithValue("@Nama", Nama);
            existingRecords = (int)cmdCheck.ExecuteScalar();
            if (existingRecords > 0)
            {
                Console.WriteLine("Data dengan Nama yang sama sudah ada.");
                return;
            }

            Console.WriteLine("Masukkan Tanggal Lahir (yyyy-MM-dd): ");
            string Tgl_Lahir_Input = Console.ReadLine();
            if (!DateTime.TryParse(Tgl_Lahir_Input, out DateTime Tgl_Lahir))
            {
                Console.WriteLine("Format tanggal tidak valid.");
                return;
            }

            Console.WriteLine("Masukkan Tanggal Kematian (yyyy-MM-dd): ");
            string Tgl_Kematian_Input = Console.ReadLine();
            if (!DateTime.TryParse(Tgl_Kematian_Input, out DateTime Tgl_Kematian))
            {
                Console.WriteLine("Format tanggal tidak valid.");
                return;
            }

            Console.WriteLine("Masukkan Penyebab Kematian: ");
            string Penyebab_Kematian = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(Penyebab_Kematian))
            {
                Console.WriteLine("Penyebab kematian tidak boleh kosong.");
                return;
            }
            if (int.TryParse(Penyebab_Kematian, out _))
            {
                Console.WriteLine("Penyebab kematian tidak boleh berupa angka.");
                return;
            }

            string query = "INSERT INTO Almarhumm (Id_Almarhumm, Nama, Tgl_Lahir, Tgl_Kematian, Penyebab_Kematian)" +
                "VALUES (@Id_Almarhumm, @Nama, @Tgl_Lahir, @Tgl_Kematian, @Penyebab_Kematian)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id_Almarhumm", Id_Almarhumm);
            cmd.Parameters.AddWithValue("@Nama", Nama);
            cmd.Parameters.AddWithValue("@Tgl_Lahir", Tgl_Lahir);
            cmd.Parameters.AddWithValue("@Tgl_Kematian", Tgl_Kematian);
            cmd.Parameters.AddWithValue("@Penyebab_Kematian", Penyebab_Kematian);
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine("Data Almarhumm berhasil ditambahkan.");
            }
            else
            {
                Console.WriteLine("Gagal menambahkan data Almarhumm.");
            }
        }

        // Method untuk menghapus data almarhumm
        public void DeleteDeath(SqlConnection conn)
        {
            Console.WriteLine("Masukkan ID Almarhumm yang akan dihapus: ");
            string Id_Almarhumm = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(Id_Almarhumm))
            {
                Console.WriteLine("ID tidak boleh kosong.");
                return;
            }
            if (!int.TryParse(Id_Almarhumm, out _))
            {
                Console.WriteLine("ID harus berupa angka.");
                return;
            }

            string queryCheck = "SELECT COUNT(*) FROM Almarhumm WHERE Id_Almarhumm = @Id_Almarhumm";
            SqlCommand cmdCheck = new SqlCommand(queryCheck, conn);
            cmdCheck.Parameters.AddWithValue("@Id_Almarhumm", Id_Almarhumm);
            int existingRecords = (int)cmdCheck.ExecuteScalar();
            if (existingRecords == 0)
            {
                Console.WriteLine("Data dengan ID yang dimasukkan tidak ditemukan.");
                return;
            }

            Console.WriteLine("Apakah Anda yakin ingin menghapus data ini? (Y/N): ");
            char confirmation = Convert.ToChar(Console.ReadLine().ToUpper());
            if (confirmation == 'Y')
            {
                string query = "DELETE FROM Almarhumm WHERE Id_Almarhumm = @Id_Almarhumm";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id_Almarhumm", Id_Almarhumm);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Data Almarhumm berhasil dihapus.");
                }
                else
                {
                    Console.WriteLine("Gagal menghapus data almarhumm.");
                }
            }
            else if (confirmation == 'N')
            {
                Console.WriteLine("Penghapusan data dibatalkan.");
            }
            else
            {
                Console.WriteLine("Input tidak valid.");
            }
        }

        // Method untuk memperbarui data almarhumm
        public void UpdateDeath(SqlConnection conn)
        {
            Console.WriteLine("Masukkan ID Almarhumm yang akan diperbarui: ");
            string Id_Almarhumm = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(Id_Almarhumm))
            {
                Console.WriteLine("ID tidak boleh kosong.");
                return;
            }
            if (!int.TryParse(Id_Almarhumm, out _))
            {
                Console.WriteLine("ID harus berupa angka.");
                return;
            }

            string queryCheck = "SELECT COUNT(*) FROM Almarhumm WHERE Id_Almarhumm = @Id_Almarhumm";
            SqlCommand cmdCheck = new SqlCommand(queryCheck, conn);
            cmdCheck.Parameters.AddWithValue("@Id_Almarhumm", Id_Almarhumm);
            int existingRecords = (int)cmdCheck.ExecuteScalar();
            if (existingRecords == 0)
            {
                Console.WriteLine("Data dengan ID yang dimasukkan tidak ditemukan.");
                return;
            }

            Console.WriteLine("Masukkan Nama Almarhumm: ");
            string newNama = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newNama))
            {
                Console.WriteLine("Nama tidak boleh kosong.");
                return;
            }
            if (int.TryParse(newNama, out _))
            {
                Console.WriteLine("Nama tidak boleh berupa angka.");
                return;
            }

            Console.WriteLine("Masukkan Tanggal Lahir (yyyy-MM-dd): ");
            string newTglLahirInput = Console.ReadLine();
            if (!DateTime.TryParse(newTglLahirInput, out DateTime newTglLahir))
            {
                Console.WriteLine("Format tanggal tidak valid.");
                return;
            }

            Console.WriteLine("Masukkan Tanggal Kematian (yyyy-MM-dd): ");
            string newTglKematianInput = Console.ReadLine();
            if (!DateTime.TryParse(newTglKematianInput, out DateTime newTglKematian))
            {
                Console.WriteLine("Format tanggal tidak valid.");
                return;
            }

            Console.WriteLine("Masukkan Penyebab Kematian: ");
            string newPenyebabKematian = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newPenyebabKematian))
            {
                Console.WriteLine("Penyebab kematian tidak boleh kosong.");
                return;
            }
            if (int.TryParse(newPenyebabKematian, out _))
            {
                Console.WriteLine("Penyebab kematian tidak boleh berupa angka.");
                return;
            }

            try
            {
                string query = "UPDATE Almarhumm SET Nama = @NewNama, Tgl_Lahir = @NewTglLahir, Tgl_Kematian = @NewTglKematian, Penyebab_Kematian = @NewPenyebabKematian WHERE Id_Almarhumm = @Id_Almarhumm";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NewNama", newNama);
                    cmd.Parameters.AddWithValue("@NewTglLahir", newTglLahir);
                    cmd.Parameters.AddWithValue("@NewTglKematian", newTglKematian);
                    cmd.Parameters.AddWithValue("@NewPenyebabKematian", newPenyebabKematian);
                    cmd.Parameters.AddWithValue("@Id_Almarhumm", Id_Almarhumm);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Data Almarhumm berhasil diperbarui.");
                    }
                    else
                    {
                        Console.WriteLine("Gagal memperbarui data Almarhumm. Pastikan ID Almarhumm yang dimasukkan benar.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Method untuk melaporkan kematian
        public void ReportDeath(SqlConnection conn)
        {
            Console.WriteLine("Masukkan Id_Almarhumm: ");
            string Id_Almarhumm = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(Id_Almarhumm))
            {
                Console.WriteLine("ID tidak boleh kosong.");
                return;
            }
            if (!int.TryParse(Id_Almarhumm, out _))
            {
                Console.WriteLine("ID harus berupa angka.");
                return;
            }

            Console.WriteLine("\nMasukkan detail tentang kematian:");
            Console.WriteLine("Masukkan Tanggal Kematian (yyyy-MM-dd): ");
            DateTime tglKematian;
            if (!DateTime.TryParse(Console.ReadLine(), out tglKematian))
            {
                Console.WriteLine("Format tanggal tidak valid.");
                return;
            }

            Console.WriteLine("Masukkan Penyebab Kematian: ");
            string penyebabKematian = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(penyebabKematian))
            {
                Console.WriteLine("Penyebab kematian tidak boleh kosong.");
                return;
            }
            if (int.TryParse(penyebabKematian, out _))
            {
                Console.WriteLine("Penyebab kematian tidak boleh berupa angka.");
                return;
            }

            try
            {
                string query = "INSERT INTO Almarhumm (Id_Almarhumm, Tgl_Kematian, Penyebab_Kematian) VALUES (@Id_Almarhumm, @Tgl_Kematian, @Penyebab_Kematian)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id_Almarhumm", Id_Almarhumm);
                    cmd.Parameters.AddWithValue("@Tgl_Kematian", tglKematian);
                    cmd.Parameters.AddWithValue("@Penyebab_Kematian", penyebabKematian);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Kematian berhasil dilaporkan.");
                    }
                    else
                    {
                        Console.WriteLine("Gagal melaporkan kematian.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public void ProvideTestimony(SqlConnection conn)
        {
            Console.WriteLine("Berikan kesaksian atas kematian");

            Console.WriteLine("Masukkan Informasi tentang orang yang meninggal (ID Almarhumm): ");
            string Id_Almarhumm = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(Id_Almarhumm))
            {
                Console.WriteLine("ID Almarhumm tidak boleh kosong.");
                return;
            }
            if (!int.TryParse(Id_Almarhumm, out _))
            {
                Console.WriteLine("ID Almarhumm harus berupa angka.");
                return;
            }

            string queryCheckId = "SELECT COUNT(*) FROM Almarhumm WHERE Id_Almarhumm = @Id_Almarhumm";
            SqlCommand cmdCheckId = new SqlCommand(queryCheckId, conn);
            cmdCheckId.Parameters.AddWithValue("@Id_Almarhumm", Id_Almarhumm);
            int existingRecords = (int)cmdCheckId.ExecuteScalar();
            if (existingRecords == 0)
            {
                Console.WriteLine("ID Almarhumm yang dimasukkan tidak ditemukan dalam database.");
                return;
            }

            Console.WriteLine("Masukkan kesaksian Anda: ");
            string Detail = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(Detail))
            {
                Console.WriteLine("Detail tidak boleh kosong.");
                return;
            }
            if (int.TryParse(Detail, out _))
            {
                Console.WriteLine("Detail tidak boleh berupa angka.");
                return;
            }

            try
            {
                string query = "INSERT INTO Catatan_kematian (Id_Records, Detail) VALUES (@Id_Almarhumm, @Detail)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id_Almarhumm", Id_Almarhumm);
                    cmd.Parameters.AddWithValue("@Detail", Detail);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Kesaksian berhasil diberikan.");
                    }
                    else
                    {
                        Console.WriteLine("Gagal memberikan kesaksian.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
