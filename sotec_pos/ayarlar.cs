using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace sotec_pos
{
    public partial class ayarlar : Form
    {
        string tasinacakDosya = "", tasinacakDosyaIsmi = "", dosyaninTasinacagiKlasor = "";
        public ayarlar()
        {
            InitializeComponent();
        }

        private void btn_log_out_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ayarlar_Load(object sender, EventArgs e)
        {
            string text = "";
            try { text = System.IO.File.ReadAllText(@"printer_info.txt"); } catch { text = ""; }
            tb_yazici.Text = text;

            try { text = System.IO.File.ReadAllText(@"firma_bilgi.txt"); } catch { text = ""; }
            tb_isletme.Text = text;

            try { text = System.IO.File.ReadAllText(@"printer_info_etiket.txt"); } catch { text = ""; }
            textBox1.Text = text;

            try { pb_logo.ImageLocation = "firma_logo.png"; } catch { }

            DataTable dt_hedefler = SQL.get("SELECT * FROM hedef WHERE silindi = 0");
            grid_hedef.DataSource = dt_hedefler;

            DataTable dt_donusumler = SQL.get("SELECT kaynak_birim_id = p1.parametre_id, kaynak_birim = p1.deger, hedef_birim_id = p2.parametre_id, hedef_birim = p2.deger, kd.katsayi, kd.donusum_id FROM katsayi_donusum kd INNER JOIN parametreler p1 ON p1.parametre_id = kd.parametre_1_id INNER JOIN parametreler p2 ON p2.parametre_id = kd.parametre_2_id WHERE kd.silindi = 0");
            grid_donusumler.DataSource = dt_donusumler;

            DataTable dt_masa_kapat = SQL.get("SELECT * FROM parametreler WHERE silindi = 0 AND tip = 'masa_kapat'");
            gridMasaKapat.DataSource = dt_masa_kapat;

            DataTable dt_ikram = SQL.get("SELECT * FROM parametreler WHERE silindi = 0 AND tip = 'ikram_sebep'");
            gridIkram.DataSource = dt_ikram;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ayarlar_hareket_ekle_duzenle a = new ayarlar_hareket_ekle_duzenle(0);
            a.FormClosing += A_FormClosing1;
            a.ShowDialog();
        }

        private void grid_hedef_DoubleClick(object sender, EventArgs e)
        {
            if (gv_hedef.SelectedRowsCount <= 0)
                return;

            ayarlar_hareket_ekle_duzenle a = new ayarlar_hareket_ekle_duzenle(Convert.ToInt32(gv_hedef.GetDataRow(gv_hedef.GetSelectedRows()[0])["hedef_id"]));
            a.FormClosing += A_FormClosing1;
            a.ShowDialog();
        }

        private void A_FormClosing1(object sender, FormClosingEventArgs e)
        {
            DataTable dt_hedefler = SQL.get("SELECT * FROM hedef WHERE silindi = 0");
            grid_hedef.DataSource = dt_hedefler;

            DataTable dt_donusumler = SQL.get("SELECT kaynak_birim_id = p1.parametre_id, kaynak_birim = p1.deger, hedef_birim_id = p2.parametre_id, hedef_birim = p2.deger, kd.katsayi, kd.donusum_id FROM katsayi_donusum kd INNER JOIN parametreler p1 ON p1.parametre_id = kd.parametre_1_id INNER JOIN parametreler p2 ON p2.parametre_id = kd.parametre_2_id WHERE kd.silindi = 0");
            grid_donusumler.DataSource = dt_donusumler;

            DataTable dt_masa_kapat = SQL.get("SELECT * FROM parametreler WHERE silindi = 0 AND tip = 'masa_kapat'");
            gridMasaKapat.DataSource = dt_masa_kapat;

            DataTable dt_ikram = SQL.get("SELECT * FROM parametreler WHERE silindi = 0 AND tip = 'ikram_sebep'");
            gridIkram.DataSource = dt_ikram;
        }

        private void grid_hedef_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (gv_hedef.SelectedRowsCount <= 0)
                return;

            if (e.KeyCode == Keys.Delete)
            {
                DialogResult dialogResult = MessageBox.Show("Silmek istediğinizden emin misiniz?", "Dikkat", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    DataTable dtu = SQL.get("SELECT * FROM urunler WHERE silindi = 0 AND hedef_id = " + gv_hedef.GetDataRow(gv_hedef.GetSelectedRows()[0])["hedef_id"].ToString());
                    if (dtu.Rows.Count > 0)
                    {
                        new mesaj("Bu hedefe gidecek ürünler var hedef silinemez!").ShowDialog();
                        return;
                    }

                    SQL.set("UPDATE hedef SET silindi = 1 WHERE hedef_id = " + gv_hedef.GetDataRow(gv_hedef.GetSelectedRows()[0])["hedef_id"].ToString());
                    DataTable dt_hedefler = SQL.get("SELECT * FROM hedef WHERE silindi = 0");
                    grid_hedef.DataSource = dt_hedefler;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ayarlar_yazici_sec a = new ayarlar_yazici_sec(false, "printer_info.txt");
            a.FormClosing += A_FormClosing2;
            a.ShowDialog();
        }

        private void A_FormClosing2(object sender, FormClosingEventArgs e)
        {
            string text = "";
            try { text = System.IO.File.ReadAllText(@"printer_info.txt"); } catch { text = ""; }
            tb_yazici.Text = text;
            text = "";
            try { text = System.IO.File.ReadAllText(@"printer_info_etiket.txt"); } catch { text = ""; }
            textBox1.Text = text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (tasinacakDosya.Length > 0)
            {
                if (File.Exists("firma_logo.png"))
                    File.Delete("firma_logo.png");
                File.Copy(tasinacakDosya, "firma_logo.png");
            }

            string dosya_yolu = @"firma_bilgi.txt";

            if (File.Exists(dosya_yolu))
                File.Delete(dosya_yolu);

            FileStream fs = new FileStream(dosya_yolu, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(tb_isletme.Text);
            sw.Flush();
            sw.Close();
            fs.Close();

            new mesaj("Firma bilgileri kaydedildi!").ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ayarlar_birim_donusumleri a = new ayarlar_birim_donusumleri(0);
            a.FormClosing += A_FormClosing1;
            a.ShowDialog();
        }

        private void grid_donusumler_DoubleClick(object sender, EventArgs e)
        {
            if (gv_hedef.SelectedRowsCount <= 0)
                return;

            ayarlar_birim_donusumleri a = new ayarlar_birim_donusumleri(Convert.ToInt32(gv_donusumler.GetDataRow(gv_donusumler.GetSelectedRows()[0])["donusum_id"]));
            a.FormClosing += A_FormClosing1;
            a.ShowDialog();
        }

        private void grid_donusumler_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DialogResult dialogResult = MessageBox.Show("Silmek istediğinizden emin misiniz?", "Dikkat", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    if (gv_donusumler.SelectedRowsCount <= 0)
                        return;

                    int donusum_id = Convert.ToInt32(gv_donusumler.GetDataRow(gv_donusumler.GetSelectedRows()[0])["donusum_id"]);

                    SQL.set("UPDATE katsayi_donusum SET silindi = 1 WHERE donusum_id = " + donusum_id);

                    DataTable dt_donusumler = SQL.get("SELECT kaynak_birim_id = p1.parametre_id, kaynak_birim = p1.deger, hedef_birim_id = p2.parametre_id, hedef_birim = p2.deger, kd.katsayi, kd.donusum_id FROM katsayi_donusum kd INNER JOIN parametreler p1 ON p1.parametre_id = kd.parametre_1_id INNER JOIN parametreler p2 ON p2.parametre_id = kd.parametre_2_id WHERE kd.silindi = 0");
                    grid_donusumler.DataSource = dt_donusumler;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ayarlar_deger a = new ayarlar_deger(0, "masa_kapat");
            a.FormClosing += A_FormClosing1;
            a.ShowDialog();
        }

        private void gridMasaKapat_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DialogResult dialogResult = MessageBox.Show("Silmek istediğinizden emin misiniz?", "Dikkat", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    if (gvMasaKapat.SelectedRowsCount <= 0)
                        return;

                    int donusum_id = Convert.ToInt32(gvMasaKapat.GetDataRow(gvMasaKapat.GetSelectedRows()[0])["parametre_id"]);

                    SQL.set("UPDATE parametreler SET silindi = 1 WHERE parametre_id = " + donusum_id);

                    DataTable dt_masa_kapat = SQL.get("SELECT * FROM parametreler WHERE silindi = 0 AND tip = 'masa_kapat'");
                    gridMasaKapat.DataSource = dt_masa_kapat;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ayarlar_yazici_sec a = new ayarlar_yazici_sec(false, "printer_info_etiket.txt");
            a.FormClosing += A_FormClosing2;
            a.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ayarlar_deger a = new ayarlar_deger(0, "ikram_sebep");
            a.FormClosing += A_FormClosing1;
            a.ShowDialog();
        }

        private void btn_resim_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Resim Dosyası (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
            if (of.ShowDialog() == DialogResult.OK)
            {
                pb_logo.ImageLocation = of.FileName;

                tasinacakDosyaIsmi = of.SafeFileName.ToString();
                tasinacakDosya = of.FileName.ToString();
            }
        }
    }
}
