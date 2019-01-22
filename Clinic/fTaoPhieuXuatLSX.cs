using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CDTDatabase;
using CDTLib;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using DevExpress.XtraPrinting;
using System.IO;
using System.Collections;
using DevExpress.XtraGrid.Views;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;

namespace QLSX
{
    public partial class fTaoPhieuXuatLSX : DevExpress.XtraEditors.XtraForm
    {
        public fTaoPhieuXuatLSX()
        {
            InitializeComponent();
        }
        Database db = Database.NewDataDatabase();
        Database dbStruct = Database.NewStructDatabase();
        DataTable tb;

        DataTable tb2;
        DataTable tb3;
        DataTable tb4;
        DataTable tb5;
        DataRelation r1;
        DataRelation r2;
        GridHitInfo hitInfo = null;
        GridHitInfo hitInfo1 = null;
        DataTable tbResult;
        BindingSource _bindingSource = new BindingSource();
        DataSet ds = new DataSet();
        private void fViewSumRFM_Load(object sender, EventArgs e)
        {

            ds = db.GetDataSetByStore1("GetMTLSX4NVL", new string[] { }, new object[] { });
            if (ds == null) return;
            if (ds.Tables.Count != 3) return;
            tb = ds.Tables[0];
            tb2 = ds.Tables[1];
            tb3 = ds.Tables[2];
            tb.PrimaryKey = new DataColumn[] { tb.Columns["DTLSXID"] };
            tb2.PrimaryKey = new DataColumn[] { tb2.Columns["CTLSXID"] };
            tb3.PrimaryKey = new DataColumn[] { tb3.Columns["KTLSXID"] };
            r1 = new DataRelation("CT1", tb.Columns["DTLSXID"], tb2.Columns["DTID"]);
            r2 = new DataRelation("KT1", tb.Columns["DTLSXID"], tb3.Columns["DTID"]);
            tb.ColumnChanged += Tb_ColumnChanged;
            ds.Relations.Add(r1);
            ds.Relations.Add(r2);
            _bindingSource.DataSource = ds;
            _bindingSource.DataMember = tb.TableName;
            GridLevelNode node1 = gridControl1.LevelTree.Nodes[0];
            gridControl1.DataSource = _bindingSource;
            node1.RelationName = r1.RelationName;
            gridControl4.DataSource = _bindingSource;
            gridControl4.DataMember = r2.RelationName;//"KT1";

            repositoryItemCheckEdit1.EditValueChanging += RepositoryItemCheckEdit1_EditValueChanging;
            tb4 = tb2.Clone();




            //HideDetailView(gridView1);
        }

        private void RepositoryItemCheckEdit1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {

            if (bool.Parse(e.NewValue.ToString()))
             {
                DataRow dr = gridView1.GetDataRow(gridView1.GetSelectedRows()[0]);
                dr["Chon"] = true;
                dr.EndEdit();
                getdata();
            }
            else
            {
                DataRow dr = gridView1.GetDataRow(gridView1.GetSelectedRows()[0]);
                dr["Chon"] = false;
                dr.EndEdit();
                getdata();
            }


        } 

        private void Tb_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {

        }

        private void HideDetailView(GridView view)
        {
            GridLevelNode node = view.GridControl.LevelTree.Find(view);

            if (node.RelationName == "CT1")
            {
                node.OwnerCollection.Remove(node);
            }
        }
        void gridView2_RowCountChanged(object sender, EventArgs e)
        {
           // getdata();
        }


        private void getdata()
        {
            gridControl3.DataSource = null;
            if (tb == null) return;
            //tb.AcceptChanges();
            if (tb.Rows.Count == 0) return;
            string con = "";
            DataRow[] ldr = tb.Select("Chon=1");
            
            foreach (DataRow dr in ldr)
            {
                if (dr.RowState == DataRowState.Deleted || dr.RowState == DataRowState.Detached) continue;
                con += "'" + dr["DTLSXID"].ToString() + "',";
            }
            if (con == string.Empty) con = "";
            else
                con = con.Substring(0, con.Length - 1);
            tbResult = db.GetDataSetByStore("GetKTLSX", new string[] { "@con" }, new object[] { con });
            gridControl3.DataSource = tbResult;
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            fViewSumRFM_Load(this, new EventArgs());
        }
        //Tạo Request
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (tb == null) return;

            if (tb.Rows.Count == 0) return;
            string con = "";
            DataRow[] ldr = tb.Select("Chon=1");

            foreach (DataRow dr in ldr)
            {
                if (dr.RowState == DataRowState.Deleted || dr.RowState == DataRowState.Detached) continue;
                con += "'" + dr["DTLSXID"].ToString() + "',";
            }
            if (con == string.Empty) con = "";
            else
                con = con.Substring(0, con.Length - 1);
            if (db.UpdateDatabyStore("CreatePhieuXuatfromLSX", new string[] { "@con" }, new object[] { con}))
            {
                MessageBox.Show("Tạo Yêu cầu xuất kho đã hoàn thành!");
               // tb = db.GetDataSetByStore("GetMTLSX", new string[] { }, new object[] { });
                fViewSumRFM_Load(this, new EventArgs());
            }
        }
        private void SetVariables(DevExpress.XtraReports.UI.XtraReport rptTmp)
        {
            foreach (DictionaryEntry de in Config.Variables)
            {
                string key = de.Key.ToString();
                if (key.Contains("@"))
                    key = key.Remove(0, 1);
                XRControl xrc = rptTmp.FindControl(key, true);
                if (xrc != null)
                {
                    string value = de.Value.ToString();
                    DateTime r;
                    if (DateTime.TryParse(value, out r))
                        value = DateTime.Parse(value).ToString("dd/MM/yyyy");//.ToShortDateString();
                    xrc.Text = value;
                    xrc = null;
                }
            }

        }

        private void gridControl4_Click(object sender, EventArgs e)
        {

        }

        private void gridControl3_Click(object sender, EventArgs e)
        {

        }
    }
}
