using System;
using System.Windows.Forms;

namespace sotec_pos
{
    public partial class onay_iste : Form
    {
        public onay_iste()
        {
            InitializeComponent();
        }

        private void onay_iste_Load(object sender, EventArgs e)
        {
            label3.Text = Program.GetMacAddress();
        }

        private void btn_log_out_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
