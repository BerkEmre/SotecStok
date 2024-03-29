﻿using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace sotec_pos
{
    public partial class ayarlar_yazici_sec : Form
    {
        bool geri_donuslu;
        public string yazici = "";
        string dosya_yolu = "";

        public ayarlar_yazici_sec(bool geri_donuslu, string dosya_yolu)
        {
            InitializeComponent();

            this.geri_donuslu = geri_donuslu;
            this.dosya_yolu = dosya_yolu;

            DataTable dt = new DataTable();
            dt.Columns.Add("yazici", typeof(String));

            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                DataRow dr;
                dr = dt.NewRow();
                dr["yazici"] = printer;
                dt.Rows.Add(dr);
            }

            grid_masa_kategori.DataSource = dt;
        }

        private void grid_masa_kategori_DoubleClick(object sender, EventArgs e)
        {
            if (gv_masa_kategori.SelectedRowsCount <= 0)
                return;

            if (geri_donuslu)
            {
                yazici = gv_masa_kategori.GetDataRow(gv_masa_kategori.GetSelectedRows()[0])["yazici"].ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                //dosya_yolu = @"printer_info.txt";

                if (File.Exists(dosya_yolu))
                    File.Delete(dosya_yolu);

                FileStream fs = new FileStream(dosya_yolu, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(gv_masa_kategori.GetDataRow(gv_masa_kategori.GetSelectedRows()[0])["yazici"].ToString());
                sw.Flush();
                sw.Close();
                fs.Close();

                this.Close();
            }
        }

        private void ayarlar_yazici_sec_Load(object sender, EventArgs e)
        {

        }
    }
}
