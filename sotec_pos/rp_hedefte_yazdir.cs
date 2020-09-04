using DevExpress.XtraReports.UI;

namespace sotec_pos
{
    public partial class rp_hedefte_yazdir : DevExpress.XtraReports.UI.XtraReport
    {
        public rp_hedefte_yazdir(string urun_adi, string barkod)
        {
            InitializeComponent();

            lb_urun_adi.Text = urun_adi;
            bc_barkod.Text = barkod;
        }
    }
}
