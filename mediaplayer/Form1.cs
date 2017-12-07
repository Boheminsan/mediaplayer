using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace mediaplayer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string conString = "Server=.;Database=tugce_db;Integrated Security=True";     
        //Bu veritabanına bağlanmak için gerekli olan bağlantı cümlemiz.Bu satıra dikkat edelim.
        //Sql Servera bağlanırken kullandığımız bilgileri ve veritabanı ismini yazıyoruz.


        private void Form1_Load(object sender, EventArgs e)
        {

            Form1 frm1 = new Form1();
            listele();

        }

        private void listele()
        {
            SqlConnection baglanti = new SqlConnection(conString);
            SqlCommand komut = new SqlCommand("SELECT * from tbl_sarkilar", baglanti);
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
                //bağlantı açıldı
            }

            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                listBox1.Items.Add(dr["sarkiAdi"]);
                //şarkılar listeleniyor
            }
            baglanti.Close();
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
                //bağlantı açıldı
            }
            SqlCommand komut2 = new SqlCommand("SELECT * FROM tbl_sarkilar", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut2);
            //SqlDataAdapter sınıfı verilerin databaseden aktarılması işlemini gerçekleştirir.
            DataTable dt = new DataTable();
            da.Fill(dt);
            //Bir DataTable oluşturarak DataAdapter ile getirilen verileri tablo içerisine dolduruyoruz.
            dataGridView1.DataSource = dt;
            baglanti.Close();
        }
        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SqlConnection baglanti = new SqlConnection(conString);
            string tiklanan = listBox1.SelectedItem.ToString();
            //listboxtaki seçili elemanın linki değişkene kopyalandı.
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            SqlCommand komut = new SqlCommand("SELECT link from tbl_sarkilar WHERE sarkiAdi='" + tiklanan + "'", baglanti);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                wmp1.URL = dr["link"].ToString();
                //şarkı/video çalınıyor.
            }
            baglanti.Close();

        }
       
  

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int id= Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            SqlConnection baglanti = new SqlConnection(conString);
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            SqlCommand komut = new SqlCommand("UPDATE tbl_sarkilar SET sarkiAdi=@sarkiAdi,link=@link where id=@id", baglanti);
            //güncelleme komutu oluşturuldu
            komut.Parameters.AddWithValue("@id", id);
            komut.Parameters.AddWithValue("@sarkiAdi", textBox1.Text);
            komut.Parameters.AddWithValue("@link", textBox2.Text);
            //parametreler eklendi
            komut.ExecuteNonQuery();
            //komut çalıştırıldı
            listele();
            baglanti.Close();
        }

        private void Btn_sil_Click(object sender, EventArgs e)
        {

            SqlConnection baglanti = new SqlConnection(conString);
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            SqlCommand komut = new SqlCommand("DELETE FROM tbl_Sarkilar WHERE id=@id", baglanti);
            //silme komutu oluşturuldu
            komut.Parameters.AddWithValue("@id", dataGridView1.CurrentRow.Cells[0].Value);
            //parametreler eklendi
            komut.ExecuteNonQuery();
            //komut çalıştırıldı
            listele();
        }


        private void Btn_gozat_Click(object sender, EventArgs e)
        {
            OpenFileDialog sarki = new OpenFileDialog();
            //dosya seçim penceresi açıldı
            sarki.Filter = "Genel medya dosyalar |*.mp3| |*.mp4| |*.wmv| |*.wma| |*.avi";
            //dosya formatları
            sarki.ShowDialog();
            //medya seçildi
            FileInfo dosyabilgisi = new FileInfo(sarki.FileName);
            //özellikleri almak için nesne tanımlandı
            adi.Text = dosyabilgisi.Name.ToString();
            link.Text = dosyabilgisi.FullName.ToString();
            //dosya özellikleri labellere alındı
        }

        private void Btn_Ekle_Click(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection(conString);
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            //burada bağlantı açık mı kontrol etmeliyiz. Açık olan bağlantıyı açmaya çalışmamak için.
            SqlCommand komut = new SqlCommand("INSERT into tbl_sarkilar(sarkiAdi, link) values(@sarki, @link)", baglanti);
            //ekleme komutu gönderildi
            komut.Parameters.AddWithValue("@sarki", adi.Text);
            komut.Parameters.AddWithValue("@link", link.Text);
            //parametreler eklendi
            komut.ExecuteNonQuery();
            //komut çalıştırıldı
            listBox1.Items.Clear();
            dataGridView1.Dispose();
            listele();
            baglanti.Close();
            
        }

        private void Btn_duzenle_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex != 0)
            {
                textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                //Düzenlemek için tablodan verileri aldık.
            }
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            gizlilabel1.Text = "Tuğçe Cambazoğlu";
            gizlilabel1.Visible = true;
            //mouse resmin üstüne geldiğinde yazı görünür olacak.
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            gizlilabel1.Visible = false;
            //mouse gittiğinde kaybolacak.
        }
    }
}