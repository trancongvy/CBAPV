using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CDTDatabase;
namespace Inventory
{
    public partial class FormHTK : DevExpress.XtraEditors.XtraForm
    {
        public FormHTK()
        {
            InitializeComponent();
        }
        public string makho;
        private void simpleButtonTinhGia_Click(object sender, EventArgs e)
        {
            int tuthang = Int32.Parse(spinEditTuThang.EditValue.ToString());
            int denthang = Int32.Parse(spinEditDenThang.EditValue.ToString());
            
            DevExpress.XtraEditors.XtraForm f = new GiaVT(tuthang,denthang, radioGroupPP.SelectedIndex,makho);
            f.MdiParent = this.MdiParent;
            f.Show();
        }

        private void FormHTK_Load(object sender, EventArgs e)
        {
            Database db = CDTDatabase.Database.NewDataDatabase();
            gridLookUpEdit1.Properties.DataSource = db.GetDataTable("select MaKho,TenKho from dmkho");
        }

        private void FormHTK_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
            else if (e.KeyCode == Keys.F12)
            {
                simpleButtonTinhGia_Click(simpleButtonTinhGia as object , new EventArgs());
            }
            else if (e.KeyCode == Keys.F11)
            {
                int i = int.Parse("fsdf");
            }
        }

        private void gridLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (gridLookUpEdit1.Properties.View.SelectedRowsCount > 0)
                makho = gridLookUpEdit1.Properties.View.GetDataRow(gridLookUpEdit1.Properties.View.GetSelectedRows()[0])["makho"].ToString();
            else
                makho = null;
        }

    }
}