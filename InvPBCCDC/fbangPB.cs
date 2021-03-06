using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace InvPBCCDC
{
    public partial class fbangPB : DevExpress.XtraEditors.XtraForm
    {
        DataTable _bangGia;
        CalPB BangGia;
        public fbangPB(CalPB banggia)
        {
            InitializeComponent();
            BangGia = banggia;
            _bangGia = banggia.dtCCDC;
            this.KeyUp += FbangPB_KeyUp;
        }

        private void FbangPB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Dispose();
        }

        private void fbangPB_Load(object sender, EventArgs e)
        {
            this.gridControl1.DataSource = _bangGia.DefaultView;

        }

        private void gridControl1_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        private void gridControl1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            BangGia._dbData.BeginMultiTrans();
            try
            {
                BangGia.deleteBt();
                foreach (DataRow dr in _bangGia.Rows)
                {
                    BangGia.createBt(dr);
                }
                BangGia._dbData.EndMultiTrans();
                MessageBox.Show("Hoàn thành");
            }
            catch
            {
                BangGia._dbData.RollbackMultiTrans();
                MessageBox.Show("Lỗi phát sinh");
            }
        }
    }
}