using DevExpress.XtraReports.UI;
using System.Data;

namespace sotec_pos
{
    public partial class rp_fatura : DevExpress.XtraReports.UI.XtraReport
    {
        public rp_fatura(int fatura_id)
        {
            InitializeComponent();

            DataTable dt_siparis = SQL.get("SELECT s.fatura_id, s.fatura_no, s.fatura_tarihi, c.cari_adi, p.deger, c.adres, c.vergi_dairesi, c.vergi_no FROM urunler_fatura s INNER JOIN cariler c ON c.cari_id = s.cari_id INNER JOIN parametreler p ON p.parametre_id = s.fatura_tipi_parametre_id WHERE s.fatura_id = " + fatura_id);

            lbl_cari_adi.Text = dt_siparis.Rows[0]["cari_adi"].ToString();
            lbl_siparis_tarihi.Text = dt_siparis.Rows[0]["fatura_tarihi"].ToString();
            lbl_vergi_no.Text = dt_siparis.Rows[0]["deger"].ToString();
            lbl_siparis_no.Text = dt_siparis.Rows[0]["fatura_no"].ToString();
            lbl_adres.Text = dt_siparis.Rows[0]["adres"].ToString();
            lbl_vergi_dairesi.Text = dt_siparis.Rows[0]["vergi_dairesi"].ToString();
            lbl_vergi_no.Text = dt_siparis.Rows[0]["vergi_no"].ToString();

            DataTable dt_siparis_kalem = SQL.get(
                "SELECT " +
                "   s.siparis_id, " +
                "   i.irsaliye_no, " +
                "   fk.fatura_kalem_id, " +
                "   fk.referans_irsaliye_kalem_id, " +
                "   fk.urun_id, " +
                "   u.urun_adi, " +
                "   olcu_birim = p.deger, " +
                "   fk.miktar, " +
                "   fk.birim_fiyat, " +
                "   fk.iskonto_1, " +
                "   fk.iskonto_2, " +
                "   fk.kdv, " +
                "   toplam_tutar = fk.miktar * fk.birim_fiyat, " +
                "   toplam_iskonto = (fk.miktar * fk.birim_fiyat) - (fk.miktar * ((fk.birim_fiyat - (fk.birim_fiyat / 100 * fk.iskonto_1)) - ((fk.birim_fiyat - (fk.birim_fiyat / 100 * fk.iskonto_1)) / 100 * fk.iskonto_2))), " +
                "   toplam_net = (fk.miktar * ((fk.birim_fiyat - (fk.birim_fiyat / 100 * fk.iskonto_1)) - ((fk.birim_fiyat - (fk.birim_fiyat / 100 * fk.iskonto_1)) / 100 * fk.iskonto_2))), " +
                "   toplam_kdv = (fk.miktar * ((fk.birim_fiyat - (fk.birim_fiyat / 100 * fk.iskonto_1)) - ((fk.birim_fiyat - (fk.birim_fiyat / 100 * fk.iskonto_1)) / 100 * fk.iskonto_2))) / 100 * fk.kdv, " +
                "   toplam = fk.miktar * (((fk.birim_fiyat - (fk.birim_fiyat / 100 * fk.iskonto_1)) - ((fk.birim_fiyat - (fk.birim_fiyat / 100 * fk.iskonto_1)) / 100 * fk.iskonto_2)) + (((fk.birim_fiyat - (fk.birim_fiyat / 100 * fk.iskonto_1)) - ((fk.birim_fiyat - (fk.birim_fiyat / 100 * fk.iskonto_1)) / 100 * fk.iskonto_2)) / 100 * fk.kdv)), " +
                "   net_birim_fiyat = (((fk.birim_fiyat - (fk.birim_fiyat / 100 * fk.iskonto_1)) - ((fk.birim_fiyat - (fk.birim_fiyat / 100 * fk.iskonto_1)) / 100 * fk.iskonto_2)) + (((fk.birim_fiyat - (fk.birim_fiyat / 100 * fk.iskonto_1)) - ((fk.birim_fiyat - (fk.birim_fiyat / 100 * fk.iskonto_1)) / 100 * fk.iskonto_2)) / 100 * fk.kdv)) " +
                "FROM " +
                "    urunler_fatura_kalem fk " +
                "    INNER JOIN urunler u ON u.urun_id = fk.urun_id " +
                "    INNER JOIN parametreler p ON p.parametre_id = u.olcu_birimi_parametre_id " +
                "    LEFT OUTER JOIN urunler_irsaliye_kalem ik ON ik.irsaliye_kalem_id = fk.referans_irsaliye_kalem_id " +
                "    LEFT OUTER JOIN urunler_irsaliye i ON i.irsaliye_id = ik.irsaliye_id " +
                "    LEFT OUTER JOIN urunler_siparis_kalem sk ON sk.siparis_kalem_id = ik.referans_siparis_kalem_id " +
                "    LEFT OUTER JOIN urunler_siparis s ON s.siparis_id = sk.siparis_id " +
                " WHERE fk.silindi = 0 AND fk.fatura_id = " + fatura_id);
            this.DataSource = dt_siparis_kalem;

            XRBinding binding0 = new XRBinding("Text", this.DataSource, "urun_adi", "");
            lbl_urun_adi.DataBindings.Add(binding0);
            XRBinding binding1 = new XRBinding("Text", this.DataSource, "olcu_birim", "");
            lbl_olcu_birimi.DataBindings.Add(binding1);
            XRBinding binding2 = new XRBinding("Text", this.DataSource, "miktar", "{0:n0}");
            lbl_miktar.DataBindings.Add(binding2);
            XRBinding binding3 = new XRBinding("Text", this.DataSource, "birim_fiyat", "{0:n2}");
            lbl_birim_fiyat.DataBindings.Add(binding3);
            XRBinding binding4 = new XRBinding("Text", this.DataSource, "iskonto_1", "- %{0:n0}");
            lbl_iskonto_1.DataBindings.Add(binding4);
            XRBinding binding5 = new XRBinding("Text", this.DataSource, "iskonto_2", "- %{0:n0}");
            lbl_iskonto_2.DataBindings.Add(binding5);
            XRBinding binding6 = new XRBinding("Text", this.DataSource, "kdv", "+ %{0:n0}");
            lbl_kdv.DataBindings.Add(binding6);
            XRBinding binding7 = new XRBinding("Text", this.DataSource, "net_birim_fiyat", "{0:n2}");
            lbl_net_birim_fiyat.DataBindings.Add(binding7);
            XRBinding binding8 = new XRBinding("Text", this.DataSource, "toplam", "{0:n2}");
            lbl_net_toplam.DataBindings.Add(binding8);

            XRBinding binding10 = new XRBinding("Text", this.DataSource, "toplam_tutar", "{0:n2}");
            lbl_toplam_tutar.DataBindings.Add(binding10);
            XRSummary sum1 = new XRSummary(SummaryRunning.Report, SummaryFunc.Sum, "{0:n2}");
            lbl_toplam_tutar.Summary = sum1;
            XRBinding binding11 = new XRBinding("Text", this.DataSource, "toplam_iskonto", "{0:n2}");
            lbl_toplam_iskonto.DataBindings.Add(binding11);
            XRSummary sum2 = new XRSummary(SummaryRunning.Report, SummaryFunc.Sum, "{0:n2}");
            lbl_toplam_iskonto.Summary = sum2;
            XRBinding binding12 = new XRBinding("Text", this.DataSource, "toplam_net", "{0:n2}");
            lbl_net_tutar_toplam.DataBindings.Add(binding12);
            XRSummary sum3 = new XRSummary(SummaryRunning.Report, SummaryFunc.Sum, "{0:n2}");
            lbl_net_tutar_toplam.Summary = sum3;
            XRBinding binding13 = new XRBinding("Text", this.DataSource, "toplam_kdv", "{0:n2}");
            lbl_kdv_toplam.DataBindings.Add(binding13);
            XRSummary sum4 = new XRSummary(SummaryRunning.Report, SummaryFunc.Sum, "{0:n2}");
            lbl_kdv_toplam.Summary = sum4;
            XRBinding binding14 = new XRBinding("Text", this.DataSource, "toplam", "{0:n2}");
            lbl_genel_toplam.DataBindings.Add(binding14);
            XRSummary sum5 = new XRSummary(SummaryRunning.Report, SummaryFunc.Sum, "{0:n2}");
            lbl_genel_toplam.Summary = sum5;
        }

    }
}
