using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StokTakipSistemiFinal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Stok_Takip;Integrated Security=True");

        private void Form1_Load(object sender, EventArgs e)
        {
            musteri_grid_guncelle();
            urun_grid_guncelle();
            siparis_grid_guncelle();
        }

        private void btn_MusteriEkle_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txt_Tc.Text) && !string.IsNullOrEmpty(txt_Name.Text) && !string.IsNullOrEmpty(txt_No.Text))
            {
                if(txt_Tc.Text.Length==11)
                {
                    try
                    {
                        string ad = txt_Name.Text;
                        string tc = txt_Tc.Text;
                        string no = txt_No.Text;

                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = baglanti;
                        cmd.CommandText = "INSERT INTO musteri (tc, adsoyad, telefon) VALUES ('" + tc + "', '" + ad + "', '" + no + "')";
                        baglanti.Open();
                        cmd.ExecuteNonQuery();
                        baglanti.Close();

                        txt_Name.Clear();
                        txt_Tc.Clear();
                        txt_No.Clear();

                        musteri_grid_guncelle();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Veritabanına veri girilirken hata! \n " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Tc kimlik numarası 11 haneden daha az olamaz", "HATA",MessageBoxButtons.OK);
                }
                
            }
            else
            {
                MessageBox.Show("Lütfen boş bırakmayınız");
                return;
            }
            



        }
        public void musteri_grid_guncelle()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = baglanti;
            cmd.CommandText = "SELECT * FROM musteri";

            baglanti.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            dataGridView1.Rows.Clear();
            cmbx_musteri.Items.Clear();
            cmbx_guncelle_musteri.Items.Clear();
            while (reader.Read())
            {
                dataGridView1.Rows.Add(reader["tc"], reader["adsoyad"], reader["telefon"]);
                cmbx_musteri.Items.Add(reader["adsoyad"]);
                cmbx_guncelle_musteri.Items.Add(reader["adsoyad"]);
            }
            baglanti.Close();
            lbl_ToplamMusteri.Text = dataGridView1.RowCount.ToString();
        }

        public void urun_grid_guncelle()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = baglanti;
            cmd.CommandText = "SELECT * FROM urun";
            baglanti.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            dataGridView2.Rows.Clear();
            cmbx_urunadi.Items.Clear();
            cmbx_guncelle_urunadi.Items.Clear();
            while (reader.Read())
            {
                dataGridView2.Rows.Add(reader["barkod_no"], reader["urun_adi"], reader["miktari"], reader["satis_fiyati"], reader["toplam_fiyat"]);
                cmbx_urunadi.Items.Add(reader["barkod_no"]);
                cmbx_guncelle_urunadi.Items.Add(reader["barkod_no"]);
            }
            baglanti.Close();
            lbltoplamürün.Text = dataGridView2.RowCount.ToString();


        }

        public void siparis_grid_guncelle()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = baglanti;
            cmd.CommandText = "SELECT * FROM siparis";
            baglanti.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            dataGridView3.Rows.Clear();
            while(reader.Read())
            {
                dataGridView3.Rows.Add(reader["musteri"], reader["urun_adi"], reader["miktari"]);
            }
            baglanti.Close();
            lbl_toplam_siparis.Text = dataGridView3.RowCount.ToString();
        }

        private void btn_MusteriSil_Click(object sender, EventArgs e)
        {
           
            if (dataGridView1.SelectedRows.Count >= 1)
            {
                string tc = dataGridView1.CurrentRow.Cells["tc"].Value.ToString();
                string adsoyad = dataGridView1.CurrentRow.Cells["ad_soyad"].Value.ToString();
                string telefon = dataGridView1.CurrentRow.Cells["telefon"].Value.ToString();
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "DELETE FROM musteri WHERE tc = '" + tc + "' AND adsoyad = '" + adsoyad + "' AND telefon ='" + telefon + "'";
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
                musteri_grid_guncelle();    
            }
            else
            {
                MessageBox.Show("Lütfen silinecek satırı seçin", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_gnclle_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count >= 1)
            {
                string ad_soyad = txt_guncelle_adsoyad.Text;
                string tc = txt_guncelle_tc.Text;
                string telefon = txt_guncelle_telefon.Text;

                string ad_soyad_guncelle = dataGridView1.CurrentRow.Cells["ad_soyad"].Value.ToString();
                string tc_guncelle = dataGridView1.CurrentRow.Cells["tc"].Value.ToString();
                string no_guncelle = dataGridView1.CurrentRow.Cells["telefon"].Value.ToString();
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "UPDATE musteri SET tc = '" + tc + "', adsoyad = '" + ad_soyad + "',telefon ='" + telefon + "' WHERE tc = '" + tc_guncelle + "' AND adsoyad = '" + ad_soyad_guncelle + "' AND telefon ='" + no_guncelle + "'";
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
                musteri_grid_guncelle();
                txt_guncelle_adsoyad.Clear();
                txt_guncelle_tc.Clear();
                txt_guncelle_telefon.Clear();
            }
            else
            {
                MessageBox.Show("Lütfen güncellenecek satırı seçin", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_urun_ekle_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_barkod.Text) && !string.IsNullOrEmpty(txt_urunadi.Text) && !string.IsNullOrEmpty(txt_miktari.Text) && !string.IsNullOrEmpty(txt_satisfiyati.Text) && !string.IsNullOrEmpty(txt_topfiyat.Text))
            {
                try
                {
                    string barkod = txt_barkod.Text;
                    string urun_adi = txt_urunadi.Text;
                    string miktari = txt_miktari.Text;
                    string satis_fiyati = txt_satisfiyati.Text;
                    string toplam_fiyat = txt_topfiyat.Text;

                    SqlCommand komut = new SqlCommand();
                    komut.Connection = baglanti;
                    komut.CommandText = "INSERT INTO urun (barkod_no, urun_adi, miktari, satis_fiyati, toplam_fiyat) VALUES ('" + barkod + "', '" + urun_adi + "', '" + miktari + "', '" + satis_fiyati + "', '" + toplam_fiyat + "')";
                    baglanti.Open();
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    urun_grid_guncelle();

                    txt_barkod.Clear();
                    txt_urunadi.Clear();
                    txt_miktari.Clear();
                    txt_satisfiyati.Clear();
                    txt_topfiyat.Clear();
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Veritabanına veri girilirken hata! \n " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Lütfen boş bırakmayınız");
                return;
            }
            

        }


        private void btn_urunu_sil_Click(object sender, EventArgs e)
        {
            if(dataGridView2.SelectedRows.Count >= 1)
            {
                string barkod_no = dataGridView2.CurrentRow.Cells["barkod_no"].Value.ToString();
                string urun_adi = dataGridView2.CurrentRow.Cells["urun_adi"].Value.ToString();
                string miktari = dataGridView2.CurrentRow.Cells["miktari"].Value.ToString();
                string satis_fiyati = dataGridView2.CurrentRow.Cells["satis_fiyati"].Value.ToString();
                string top_fiyat = dataGridView2.CurrentRow.Cells["top_fiyat"].Value.ToString();
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "DELETE FROM urun WHERE barkod_no='" + barkod_no + "'AND urun_adi='" + urun_adi + "'AND miktari='" + miktari + "'AND satis_fiyati='" + satis_fiyati + "'AND toplam_fiyat='" + top_fiyat + "'";

                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
                urun_grid_guncelle();
            }
            else
            {
                MessageBox.Show("Lütfen silinecek satırı seçin", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }
        private void btn_siparisekle_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(cmbx_musteri.Text) && !string.IsNullOrEmpty(cmbx_urunadi.Text) && !string.IsNullOrEmpty(txt_miktarii.Text))
            {
                string musteri = cmbx_musteri.Text;
                string urun = cmbx_urunadi.Text;
                string miktari = txt_miktarii.Text;

                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "INSERT INTO siparis (musteri, urun_adi, miktari) VALUES ('" + musteri + "', '" + urun + "', '" + miktari + "')";
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
                siparis_grid_guncelle();
            }
            else
            {
                MessageBox.Show("Lütfen boş bırakmayınız");
                return;
            }
        }

        private void btn_siparisi_sil_Click(object sender, EventArgs e)
        {
            if(dataGridView3.SelectedRows.Count >=1 )
            {
                string musteri = dataGridView3.CurrentRow.Cells["musteri"].Value.ToString();
                string urun = dataGridView3.CurrentRow.Cells["urunadi"].Value.ToString();
                string miktar = dataGridView3.CurrentRow.Cells["miktarii"].Value.ToString();
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "DELETE FROM siparis WHERE musteri= '" + musteri + "' AND urun_adi='" + urun + "' AND miktari= '" + miktar + "'";
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();

                siparis_grid_guncelle();
            }
            else
            {
                MessageBox.Show("Lütfen silinecek satırı seçin", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void btn_gncelle_siparis_Click(object sender, EventArgs e)
        {
            if(dataGridView3.SelectedRows.Count >= 1)
            {
                string musteri = cmbx_guncelle_musteri.Text;
                string urun = cmbx_guncelle_urunadi.Text;
                string miktar = txt_guncelle_miktar.Text;

                string musteri_guncelle = dataGridView3.CurrentRow.Cells["musteri"].Value.ToString();
                string urun_guncelle = dataGridView3.CurrentRow.Cells["urunadi"].Value.ToString();
                string miktar_guncelle = dataGridView3.CurrentRow.Cells["miktarii"].Value.ToString();

                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "UPDATE siparis SET musteri= '" + musteri + "',urun_adi= '" + urun + "',miktari= '" + miktar + "' WHERE musteri='" + musteri_guncelle + "' AND urun_adi= '" + urun_guncelle + "' AND miktari= '" + miktar_guncelle + "'";
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
                siparis_grid_guncelle();

            }
            else
            {
                MessageBox.Show("Lütfen düzenlenecek satırı seçin", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_guncelle_urun_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count >= 1)
            {
                string barkod = txt_guncelle_barkod.Text;
                string urun_adi = txt_guncelle_urunadi.Text;
                string miktari = txt_guncelle_miktari.Text;
                string satis_fiyati = txt_guncelle_satisfiyati.Text;
                string topfiyat = txt_guncelle_topfiyat.Text;

                string barkod_guncelle = dataGridView2.CurrentRow.Cells["barkod_no"].Value.ToString();
                string urunadi_guncelle = dataGridView2.CurrentRow.Cells["urun_adi"].Value.ToString();
                string miktari_guncelle = dataGridView2.CurrentRow.Cells["miktari"].Value.ToString();
                string satisfiyati_guncelle = dataGridView2.CurrentRow.Cells["satis_fiyati"].Value.ToString();
                string topfiyat_guncelle = dataGridView2.CurrentRow.Cells["top_fiyat"].Value.ToString();

                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "UPDATE urun SET barkod_no = '" + barkod + "',urun_adi='" + urun_adi + "',miktari = '" + miktari + "',satis_fiyati = '" + satis_fiyati + "',toplam_fiyat = '" + topfiyat + "' WHERE barkod_no= '" + barkod_guncelle + "' AND urun_adi= '" + urunadi_guncelle + "' AND miktari='" + miktari_guncelle + "' AND satis_fiyati= '" + satisfiyati_guncelle + "' AND toplam_fiyat= '" + topfiyat_guncelle + "'";
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
                urun_grid_guncelle();

                txt_guncelle_barkod.Clear();
                txt_guncelle_urunadi.Clear();
                txt_guncelle_miktari.Clear();
                txt_guncelle_satisfiyati.Clear();
                txt_guncelle_topfiyat.Clear();
            }
            else
            {
                MessageBox.Show("Lütfen silinecek satırı seçin", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    
}
