using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CDTDatabase;
namespace Inventory
{
    public partial class fTiendoThanhtoan : DevExpress.XtraEditors.XtraForm
    {
        public fTiendoThanhtoan()
        {
            InitializeComponent();
        }
        Database db = Database.NewDataDatabase();
        private void gridLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if(gridLookUpEdit1.EditValue!=null)
            {
                string MaKH = gridLookUpEdit1.EditValue.ToString();
                DataTable tb = db.GetDataSetByStore("ThuchienThanhToan", new string[] { "@MaKH" }, new object[] { MaKH });
                if (tb != null)
                {
                    gridControl1.DataSource = tb;                    
                }
            }
        }

        private void fTiendoduan_Load(object sender, EventArgs e)
        {
            string sql = "select * from dmKH";
            DataTable tb = db.GetDataTable(sql);
            gridLookUpEdit1.Properties.DataSource = tb;
            gridLookUpEdit1.Properties.DisplayMember = "TenKH";
            gridLookUpEdit1.Properties.ValueMember = "MaKH";
        }
    }
}
