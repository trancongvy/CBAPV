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
        public string[] makho;
        private void simpleButtonTinhGia_Click(object sender, EventArgs e)
        {
            
            int tuthang = Int32.Parse(spinEditTuThang.EditValue.ToString());
            int denthang = Int32.Parse(spinEditDenThang.EditValue.ToString());
            if (checkedListBox1.SelectedItems.Count == 0) makho =new string[] {};
            else
            {
                //makho = new string[checkedListBox1.CheckedItems.Count] ;
                List<string> lKho=new List<string>();
                foreach (int indexChecked in checkedListBox1.CheckedIndices)
                {
                    lKho.Add(((DataRowView)checkedListBox1.Items[indexChecked])["MaKho"].ToString());
                }
                makho = lKho.ToArray();
            }
            DevExpress.XtraEditors.XtraForm f = new GiaVT(tuthang,denthang, radioGroupPP.SelectedIndex,makho);
            f.MdiParent = this.MdiParent;
            f.Show();
        }

        private void FormHTK_Load(object sender, EventArgs e)
        {
            Database db = CDTDatabase.Database.NewDataDatabase();

          ((ListBox) checkedListBox1).DataSource= db.GetDataTable("select MaKho,TenKho,TTTinhgia from dmkho order by TTTinhgia");
            ((ListBox) checkedListBox1).MultiColumn=true;
            ((ListBox) checkedListBox1).ValueMember="MaKho";
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

        

    }
}