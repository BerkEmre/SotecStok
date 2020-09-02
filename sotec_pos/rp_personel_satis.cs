﻿using DevExpress.XtraReports.UI;
using System;
using System.Data;

namespace sotec_pos
{
    public partial class rp_personel_satis : DevExpress.XtraReports.UI.XtraReport
    {
        public rp_personel_satis(DateTime tarih1, DateTime tarih2)
        {
            InitializeComponent();

            lbl_tarih.Text = tarih1.Day + "." + tarih1.Month + "." + tarih1.Year + "-" + tarih2.Day + "." + tarih2.Month + "." + tarih2.Year;
            DataTable dt = SQL.get(" SELECT " +
                                   "     ad_soyad = k.ad + ' ' + k.soyad, " +
                                   "     tutar = ISNULL((SELECT SUM(ak.miktar * u.fiyat) FROM adisyon_kalem ak INNER JOIN urunler u ON u.urun_id = ak.urun_id WHERE ak.silindi = 0 AND ak.kaydeden_kullanici_id = k.kullanici_id AND ak.kayit_tarihi BETWEEN '" + tarih1.ToString("yyyy-MM-dd HH:mm:00.000") + "' AND DATEADD(DAY, 0, '" + tarih2.ToString("yyyy-MM-dd HH:mm:00.000") + "')), 0) " +
                                   " FROM kullanicilar k WHERE k.silindi = 0 AND 0 != ISNULL((SELECT SUM(ak.miktar * u.fiyat) FROM adisyon_kalem ak INNER JOIN urunler u ON u.urun_id = ak.urun_id WHERE ak.silindi = 0 AND ak.kaydeden_kullanici_id = k.kullanici_id AND ak.kayit_tarihi BETWEEN '" + tarih1.ToString("yyyy-MM-dd HH:mm:00.000") + "' AND DATEADD(DAY, 0, '" + tarih2.ToString("yyyy-MM-dd HH:mm:00.000") + "')), 0)");

            this.DataSource = dt;

            XRBinding binding0 = new XRBinding("Text", this.DataSource, "ad_soyad", "");
            tc_masa_grubu.DataBindings.Add(binding0);
            XRBinding binding1 = new XRBinding("Text", this.DataSource, "tutar", "{0:c2}");
            tc_tutar.DataBindings.Add(binding1);

            GroupField sortField = new GroupField("tutar");
            sortField.SortOrder = XRColumnSortOrder.Descending;
            this.Detail.SortFields.Add(sortField);
        }

    }
}
