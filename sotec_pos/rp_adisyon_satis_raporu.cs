using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace sotec_pos
{
    public partial class rp_adisyon_satis_raporu : DevExpress.XtraReports.UI.XtraReport
    {
        public rp_adisyon_satis_raporu(DateTime tarih1, DateTime tarih2)
        {
            InitializeComponent();

            lbl_tarih.Text = tarih1.Day + "." + tarih1.Month + "." + tarih1.Year + "-" + tarih2.Day + "." + tarih2.Month + "." + tarih2.Year;
            DataTable dt_kdv = SQL.get("SELECT kdv = AVG(a.kdv) FROM (SELECT kdv = (SELECT AVG(u.kdv) FROM adisyon_kalem ak INNER JOIN urunler u ON u.urun_id = ak.urun_id WHERE ak.silindi = 0 AND ak.adisyon_id = fh.referans_id) FROM finans_hareket fh WHERE fh.silindi = 0 AND fh.hareket_tipi_parametre_id IN (25, 26, 27) AND kayit_tarihi BETWEEN '" + tarih1.ToString("yyyy-MM-dd HH:mm:00.000") + "' AND DATEADD(DAY, 0, '" + tarih2.ToString("yyyy-MM-dd HH:mm:00.000") + "')) a");
            DataTable dt_nakit = SQL.get("SELECT tutar = ISNULL(SUM(miktar), 0) FROM finans_hareket WHERE silindi = 0 AND hareket_tipi_parametre_id = 25 AND kayit_tarihi BETWEEN '" + tarih1.ToString("yyyy-MM-dd HH:mm:00.000") + "' AND DATEADD(DAY, 0, '" + tarih2.ToString("yyyy-MM-dd HH:mm:00.000") + "')");
            DataTable dt_kredi = SQL.get("SELECT tutar = ISNULL(SUM(miktar), 0) FROM finans_hareket WHERE silindi = 0 AND hareket_tipi_parametre_id = 26 AND kayit_tarihi BETWEEN '" + tarih1.ToString("yyyy-MM-dd HH:mm:00.000") + "' AND DATEADD(DAY, 0, '" + tarih2.ToString("yyyy-MM-dd HH:mm:00.000") + "')");
            DataTable dt_yfisi = SQL.get("SELECT tutar = ISNULL(SUM(miktar), 0) FROM finans_hareket WHERE silindi = 0 AND hareket_tipi_parametre_id = 27 AND kayit_tarihi BETWEEN '" + tarih1.ToString("yyyy-MM-dd HH:mm:00.000") + "' AND DATEADD(DAY, 0, '" + tarih2.ToString("yyyy-MM-dd HH:mm:00.000") + "')");
            DataTable dt_gg_nakit = SQL.get("SELECT tutar = ISNULL(SUM(miktar), 0) FROM finans_hareket WHERE silindi = 0 AND hareket_tipi_parametre_id = 60 AND kayit_tarihi BETWEEN '" + tarih1.ToString("yyyy-MM-dd HH:mm:00.000") + "' AND DATEADD(DAY, 0, '" + tarih2.ToString("yyyy-MM-dd HH:mm:00.000") + "')");
            DataTable dt_gg_kredi = SQL.get("SELECT tutar = ISNULL(SUM(miktar), 0) FROM finans_hareket WHERE silindi = 0 AND hareket_tipi_parametre_id = 61 AND kayit_tarihi BETWEEN '" + tarih1.ToString("yyyy-MM-dd HH:mm:00.000") + "' AND DATEADD(DAY, 0, '" + tarih2.ToString("yyyy-MM-dd HH:mm:00.000") + "')");
            DataTable dt_ft_satin_alma = SQL.get("SELECT tutar = ISNULL(SUM(tutar) , 0) FROM (SELECT tutar = CASE f.fatura_tipi_parametre_id WHEN 29 THEN -1 WHEN 30 THEN 1 END * (SELECT SUM(fk.miktar * (((fk.birim_fiyat - (fk.birim_fiyat / 100 * fk.iskonto_1)) - ((fk.birim_fiyat - (fk.birim_fiyat / 100 * fk.iskonto_1)) / 100 * fk.iskonto_2)) + (((fk.birim_fiyat - (fk.birim_fiyat / 100 * fk.iskonto_1)) - ((fk.birim_fiyat - (fk.birim_fiyat / 100 * fk.iskonto_1)) / 100 * fk.iskonto_2)) / 100 * fk.kdv))) FROM urunler_fatura_kalem fk WHERE fk.silindi = 0 AND fk.fatura_id = f.fatura_id) FROM urunler_fatura f WHERE f.silindi = 0 AND f.fatura_tipi_parametre_id = 29 AND f.kayit_tarihi BETWEEN '" + tarih1.ToString("yyyy-MM-dd HH:mm:00.000") + "' AND DATEADD(DAY, 0, '" + tarih2.ToString("yyyy-MM-dd HH:mm:00.000") + "')) tbl");
            DataTable dt_ft_satis = SQL.get("SELECT tutar = ISNULL(SUM(tutar) , 0) FROM (SELECT tutar = CASE f.fatura_tipi_parametre_id WHEN 29 THEN -1 WHEN 30 THEN 1 END * (SELECT SUM(fk.miktar * (((fk.birim_fiyat - (fk.birim_fiyat / 100 * fk.iskonto_1)) - ((fk.birim_fiyat - (fk.birim_fiyat / 100 * fk.iskonto_1)) / 100 * fk.iskonto_2)) + (((fk.birim_fiyat - (fk.birim_fiyat / 100 * fk.iskonto_1)) - ((fk.birim_fiyat - (fk.birim_fiyat / 100 * fk.iskonto_1)) / 100 * fk.iskonto_2)) / 100 * fk.kdv))) FROM urunler_fatura_kalem fk WHERE fk.silindi = 0 AND fk.fatura_id = f.fatura_id) FROM urunler_fatura f WHERE f.silindi = 0 AND f.fatura_tipi_parametre_id = 30 AND f.kayit_tarihi BETWEEN '" + tarih1.ToString("yyyy-MM-dd HH:mm:00.000") + "' AND DATEADD(DAY, 0, '" + tarih2.ToString("yyyy-MM-dd HH:mm:00.000") + "')) tbl");
            DataTable dt_tah_nakit = SQL.get("SELECT tutar = ISNULL(SUM(tutar), 0) FROM finans_tahsilat fh WHERE fh.silindi = 0 AND fh.tahsilat_tipi_parametre_id = 35 AND fh.odeme_tipi_parametre_id = 67");
            DataTable dt_tah_kredi = SQL.get("SELECT tutar = ISNULL(SUM(tutar), 0) FROM finans_tahsilat fh WHERE fh.silindi = 0 AND fh.tahsilat_tipi_parametre_id = 35 AND fh.odeme_tipi_parametre_id = 68");
            DataTable dt_tah_cek = SQL.get("SELECT tutar = ISNULL(SUM(tutar), 0) FROM finans_tahsilat fh WHERE fh.silindi = 0 AND fh.tahsilat_tipi_parametre_id = 35 AND fh.odeme_tipi_parametre_id = 69");
            DataTable dt_tah_senet = SQL.get("SELECT tutar = ISNULL(SUM(tutar), 0) FROM finans_tahsilat fh WHERE fh.silindi = 0 AND fh.tahsilat_tipi_parametre_id = 35 AND fh.odeme_tipi_parametre_id = 70");
            DataTable dt_ted_nakit = SQL.get("SELECT tutar = ISNULL(SUM(tutar), 0) FROM finans_tahsilat fh WHERE fh.silindi = 0 AND fh.tahsilat_tipi_parametre_id = 37 AND fh.odeme_tipi_parametre_id = 67");
            DataTable dt_ted_kredi = SQL.get("SELECT tutar = ISNULL(SUM(tutar), 0) FROM finans_tahsilat fh WHERE fh.silindi = 0 AND fh.tahsilat_tipi_parametre_id = 37 AND fh.odeme_tipi_parametre_id = 68");
            DataTable dt_ted_cek = SQL.get("SELECT tutar = ISNULL(SUM(tutar), 0) FROM finans_tahsilat fh WHERE fh.silindi = 0 AND fh.tahsilat_tipi_parametre_id = 37 AND fh.odeme_tipi_parametre_id = 69");
            DataTable dt_ted_senet = SQL.get("SELECT tutar = ISNULL(SUM(tutar), 0) FROM finans_tahsilat fh WHERE fh.silindi = 0 AND fh.tahsilat_tipi_parametre_id = 37 AND fh.odeme_tipi_parametre_id = 70");

            tc_nakit.Text = Convert.ToDecimal(dt_nakit.Rows[0]["tutar"]).ToString("c2");
            tc_kredi.Text = Convert.ToDecimal(dt_kredi.Rows[0]["tutar"]).ToString("c2");
            tc_yemek_fisi.Text = Convert.ToDecimal(dt_yfisi.Rows[0]["tutar"]).ToString("c2");

            tc_genel_gider_nakit.Text = Convert.ToDecimal(dt_gg_nakit.Rows[0]["tutar"]).ToString("c2");
            tc_genel_gider_kredi.Text = Convert.ToDecimal(dt_gg_kredi.Rows[0]["tutar"]).ToString("c2");

            decimal kdv = 0;
            try { kdv = Convert.ToDecimal(dt_kdv.Rows[0]["kdv"]); }
            catch { kdv = 0; }

            tc_nakit_kdv.Text = ((Convert.ToDecimal(dt_nakit.Rows[0]["tutar"]) / 100) * kdv).ToString("c2");
            tc_kredi_kdv.Text = ((Convert.ToDecimal(dt_kredi.Rows[0]["tutar"]) / 100) * kdv).ToString("c2");
            tc_yemek_fisi_kdv.Text = ((Convert.ToDecimal(dt_yfisi.Rows[0]["tutar"]) / 100) * kdv).ToString("c2");

            tc_top.Text = (Convert.ToDecimal(dt_nakit.Rows[0]["tutar"]) + Convert.ToDecimal(dt_kredi.Rows[0]["tutar"]) + Convert.ToDecimal(dt_yfisi.Rows[0]["tutar"])).ToString("c2");
            tc_top_kdv.Text = (((Convert.ToDecimal(dt_nakit.Rows[0]["tutar"]) + Convert.ToDecimal(dt_kredi.Rows[0]["tutar"]) + Convert.ToDecimal(dt_yfisi.Rows[0]["tutar"])) / 100) * kdv).ToString("c2");

            lbl_satin_alma.Text = (Convert.ToDecimal(dt_ft_satin_alma.Rows[0]["tutar"])).ToString("c2");
            lbl_satis.Text = (Convert.ToDecimal(dt_ft_satis.Rows[0]["tutar"])).ToString("c2");

            lbl_tahsilat_nakit.Text = (Convert.ToDecimal(dt_tah_nakit.Rows[0]["tutar"])).ToString("c2");
            lbl_tahsilat_kredi.Text = (Convert.ToDecimal(dt_tah_kredi.Rows[0]["tutar"])).ToString("c2");
            lbl_tahsilat_cek.Text = (Convert.ToDecimal(dt_tah_cek.Rows[0]["tutar"])).ToString("c2");
            lbl_tahsilat_senet.Text = (Convert.ToDecimal(dt_tah_senet.Rows[0]["tutar"])).ToString("c2");

            lbl_tediye_nakit.Text = (Convert.ToDecimal(dt_ted_nakit.Rows[0]["tutar"])).ToString("c2");
            lbl_tediye_kredi.Text = (Convert.ToDecimal(dt_ted_kredi.Rows[0]["tutar"])).ToString("c2");
            lbl_tediye_cek.Text = (Convert.ToDecimal(dt_ted_cek.Rows[0]["tutar"])).ToString("c2");
            lbl_tediye_senet.Text = (Convert.ToDecimal(dt_ted_senet.Rows[0]["tutar"])).ToString("c2");

            lbl_top_ted.Text = (Convert.ToDecimal(dt_ted_nakit.Rows[0]["tutar"]) + Convert.ToDecimal(dt_ted_kredi.Rows[0]["tutar"]) + Convert.ToDecimal(dt_ted_cek.Rows[0]["tutar"]) + Convert.ToDecimal(dt_ted_senet.Rows[0]["tutar"])).ToString("c2");
            lbl_top_tah.Text = (Convert.ToDecimal(dt_tah_nakit.Rows[0]["tutar"]) + Convert.ToDecimal(dt_tah_kredi.Rows[0]["tutar"]) + Convert.ToDecimal(dt_tah_cek.Rows[0]["tutar"]) + Convert.ToDecimal(dt_tah_senet.Rows[0]["tutar"])).ToString("c2");
            lbl_top_fat.Text = (Convert.ToDecimal(dt_ft_satin_alma.Rows[0]["tutar"]) + Convert.ToDecimal(dt_ft_satis.Rows[0]["tutar"])).ToString("c2");
            lbl_top_gg.Text = (Convert.ToDecimal(dt_gg_nakit.Rows[0]["tutar"]) + Convert.ToDecimal(dt_gg_kredi.Rows[0]["tutar"])).ToString("c2");
        }

    }
}
