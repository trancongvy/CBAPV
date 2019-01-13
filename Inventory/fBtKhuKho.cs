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
    public partial class fBtKhuKho : Form
    {
        public fBtKhuKho()
        {
            InitializeComponent();
        }
        Database db = Database.NewDataDatabase();
        DataTable dmkho;
        private void fBtKhuKho_Load(object sender, EventArgs e)
        {
            string sql = "declare @z bit set @z=0 select @z as Chon, MaKho, TenKho, DiaChi from dmkho";
            dmkho = db.GetDataTable(sql);

            DevExpress.XtraEditors.PopupContainerControl popupControl = new DevExpress.XtraEditors.PopupContainerControl();
            Control cKho = new dmKhoControl(ref dmkho);
            popupControl.Size = cKho.Size;// new Size(100, 200);
            popupControl.Controls.Add(cKho);
            cKho.Dock = DockStyle.Fill;
            popupContainerEdit1.Properties.PopupControl = popupControl;
            dmkho.ColumnChanged += Dmkho_ColumnChanged;
            popupContainerEdit1.Closed += PopupContainerEdit1_Closed;
        }

        private void PopupContainerEdit1_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            
        }

        private void Dmkho_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            if (e.Column.ColumnName == "Chon")
            {
                e.Row.EndEdit();
                if (e.Row["MaKho"] != DBNull.Value)
                {
                    if (bool.Parse(e.Row["Chon"].ToString()))
                        popupContainerEdit1.Text += e.Row["MaKho"].ToString() + ",";
                    else
                    {
                        popupContainerEdit1.Text = popupContainerEdit1.Text.Replace(e.Row["MaKho"].ToString() + ",", "");
                    }
                }
            }
        }

        private void gridLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            

        }

        private void popupContainerEdit1_EditValueChanged(object sender, EventArgs e)
        {
            //string value = "";
            //foreach (DataRow dr in dmkho.Rows)
            //{
            //    dr.EndEdit();
            //    if (dr["Chon"].ToString() == "True") value = dr["MaKho"].ToString() + ",";
            //}
            //if (value.Length > 0)
            //    value = value.Substring(0, value.Length - 1);
            //popupContainerEdit1.Text = value;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        { try
            {
                DataTable kq = db.GetDataSetByStore("TaoButtoanKTK", new string[] { "@makho", "@ngayct2" }, new object[] { popupContainerEdit1.Text, DateTime.Parse(dateEdit1.EditValue.ToString()) });
                if (kq != null) gridControl1.DataSource = kq; 
            }
            catch { }

        }
    }
}
