using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CDTDatabase;
namespace GLKC
{
    public partial class GLChooseTrans : DevExpress.XtraEditors.XtraForm
    {   
        public List<DataRow> TransList;
        private DataTable dmglck;
        public Database _dbData = Database.NewDataDatabase();
        public GLChooseTrans()
        {
            InitializeComponent();
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GLChooseTrans_Keyup);
        }

        private void GLChooseTrans_Load(object sender, EventArgs e)
        {
            dmglck = _dbData.GetDataTable("select * from dmKetchuyen");
            this.gridControl1.DataSource = dmglck.DefaultView;
        }
        private void GLChooseTrans_Keyup(object sender, KeyEventArgs e)
        {
           // MessageBox.Show("vừa nhấn");
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
            else if (e.KeyCode == Keys.F12)
            {
                simpleButton1_Click(simpleButton1, new EventArgs());
            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            TransList=new List<DataRow>();
            TransList.Clear();
            foreach (int i in gridView1.GetSelectedRows())
            {
                TransList.Add(dmglck.Rows[i]);
            }
            Filter f = new Filter(TransList);
            f.Text = this.Text;
            f.ShowDialog();

        }
    }
}