using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace sotec_pos
{
    public partial class rp_stok_hareket_raporu : DevExpress.XtraReports.UI.XtraReport
    {
        public rp_stok_hareket_raporu(int urun_id, DateTime ilk_tarih, DateTime son_tarih)
        {
            InitializeComponent();

            DataTable dt_urun = SQL.get("SELECT * FROM urunler WHERE urun_id = " + urun_id);

            lbl_urun_adi.Text = dt_urun.Rows[0]["urun_adi"].ToString();
            lbl_siparis_tarihi.Text = ilk_tarih.ToShortDateString() + " - " + son_tarih.ToShortDateString();

            DataTable dt_stok_hareket = SQL.get("SELECT uh.kayit_tarihi, uh.miktar, tip = p.deger, belge_no = (CASE uh.hareket_tipi_parametre_id WHEN 34 THEN (SELECT uf.fatura_no FROM urunler_fatura_kalem ufk INNER JOIN urunler_fatura uf ON uf.fatura_id = ufk.fatura_id WHERE ufk.fatura_kalem_id = uh.referans_id) WHEN 4 THEN (SELECT ui.irsaliye_no FROM urunler_irsaliye_kalem uik INNER JOIN urunler_irsaliye ui ON ui.irsaliye_id = uik.irsaliye_id WHERE uik.irsaliye_kalem_id = uh.referans_id) ELSE '-' END), cari_adi = (CASE uh.hareket_tipi_parametre_id WHEN 34 THEN (SELECT c.cari_adi FROM urunler_fatura_kalem ufk INNER JOIN urunler_fatura uf ON uf.fatura_id = ufk.fatura_id INNER JOIN cariler c ON c.cari_id = uf.cari_id WHERE ufk.fatura_kalem_id = uh.referans_id) WHEN 4 THEN (SELECT c.cari_adi FROM urunler_irsaliye_kalem uik INNER JOIN urunler_irsaliye ui ON ui.irsaliye_id = uik.irsaliye_id INNER JOIN cariler c ON c.cari_id = ui.cari_id WHERE uik.irsaliye_kalem_id = uh.referans_id) ELSE '-' END) FROM urunler_hareket uh INNER JOIN parametreler p ON p.parametre_id = uh.hareket_tipi_parametre_id LEFT OUTER JOIN adisyon a ON uh.hareket_tipi_parametre_id = 3 AND a.adisyon_id = uh.referans_id WHERE uh.silindi = 0 AND uh.urun_id = " + urun_id + " AND uh.kayit_tarihi BETWEEN '" + ilk_tarih.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' AND DATEADD(DAY, 0, '" + son_tarih.ToString("yyyy-MM-dd HH:mm:ss.fff") + "') ORDER by uh.kayit_tarihi");
            this.DataSource = dt_stok_hareket;

            XRBinding binding0 = new XRBinding("Text", this.DataSource, "kayit_tarihi", "");
            xrTableCell2.DataBindings.Add(binding0);
            XRBinding binding1 = new XRBinding("Text", this.DataSource, "tip", "");
            xrTableCell4.DataBindings.Add(binding1);
            XRBinding binding4 = new XRBinding("Text", this.DataSource, "cari_adi", "");
            xrTableCell12.DataBindings.Add(binding4);
            XRBinding binding5 = new XRBinding("Text", this.DataSource, "belge_no", "");
            xrTableCell13.DataBindings.Add(binding5);
            XRBinding binding2 = new XRBinding("Text", this.DataSource, "miktar", "{0:0.##}");
            xrTableCell5.DataBindings.Add(binding2);
            XRBinding binding3 = new XRBinding("Text", this.DataSource, "miktar", "{0:0.##}");
            xrTableCell8.DataBindings.Add(binding3);

            XRSummary sum1 = new XRSummary(SummaryRunning.Report, SummaryFunc.Sum, "{0:0.##}");
            xrTableCell8.Summary = sum1;
        }
    }
}
