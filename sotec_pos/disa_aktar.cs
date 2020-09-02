using DevExpress.XtraPrinting;
using System;
using System.Windows.Forms;

namespace sotec_pos
{
    public partial class disa_aktar : Form
    {
        public disa_aktar()
        {
            InitializeComponent();
        }

        private void disa_aktar_Load(object sender, EventArgs e)
        {
            rp_viewer.PrintingSystem.ExecCommand(PrintingSystemCommand.ZoomToWholePage);
        }
    }
}
