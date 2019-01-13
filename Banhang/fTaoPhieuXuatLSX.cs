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

namespace Banhang
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
        DataTable tb1;
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
            
            ds = db.GetDataSetByStore1("GetMTLSX", new string[] { }, new object[] { });
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
            ds.Relations.Add(r1);
            ds.Relations.Add(r2);
            _bindingSource.DataSource = ds;
            _bindingSource.DataMember = tb.TableName;
            GridLevelNode node1 = gridControl1.LevelTree.Nodes[0];
            gridControl1.DataSource = _bindingSource;
            node1.RelationName = r1.RelationName;
            gridControl4.DataSource = _bindingSource;
            gridControl4.DataMember = r2.RelationName;//"KT1";
            tb1 = tb.Clone();
            gridControl2.DataSource = tb1;
            tb4 = tb2.Clone();
            tb5 = tb3.Clone();
            gridControl3.DataSource = tb5;
            gridControl1.MouseDown += gridControl1_MouseDown;
            gridControl1.MouseMove += gridControl1_MouseMove;
            gridControl2.DragEnter += gridControl2_DragEnter;
            gridControl2.DragDrop += gridControl2_DragDrop;

            gridControl2.MouseDown += gridControl2_MouseDown;
            gridControl2.MouseMove += gridControl2_MouseMove;
            gridControl1.DragEnter += gridControl1_DragEnter;
            gridControl1.DragDrop += gridControl1_DragDrop;
            gridView2.RowCountChanged += gridView2_RowCountChanged;
            //HideDetailView(gridView1);
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

        void gridControl2_DragDrop(object sender, DragEventArgs e)
        {
            
            if (hitInfo == null) return;
            List<DataRow> ldr = (e.Data.GetData(typeof(List<DataRow>)) as List<DataRow>);
            foreach (DataRow dr in ldr)
            {
                if (dr == null) continue;
                DataRow dr1 = tb1.NewRow();
                dr1.ItemArray = dr.ItemArray;
                tb1.Rows.Add(dr1);
                DataRow[] ldrCt = tb2.Select("DTID='" + dr["DTLSXID"].ToString() + "'");
                foreach(DataRow drCt in ldrCt)
                {
                    DataRow drCt1 = tb4.NewRow();
                    drCt1.ItemArray = drCt.ItemArray;
                    tb4.Rows.Add(drCt1);
                    tb2.Rows.Remove(drCt); 
                }
                DataRow[] ldrKt = tb3.Select("DTID='" + dr["DTLSXID"].ToString() + "'");
                foreach (DataRow drKt in ldrKt)
                {
                    DataRow drKt1 = tb5.NewRow();
                    drKt1.ItemArray = drKt.ItemArray;
                    tb5.Rows.Add(drKt1);
                    tb3.Rows.Remove(drKt);
                }
                tb.Rows.Remove(dr);
            }
            tb.AcceptChanges();
            tb1.AcceptChanges();
            tb2.AcceptChanges();
            tb3.AcceptChanges();
            tb4.AcceptChanges();
            tb5.AcceptChanges();
        }

        void gridControl2_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        void gridControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (hitInfo == null) return;
            if (e.Button != MouseButtons.Left) return;
            Rectangle dragRect = new Rectangle(new Point(
                hitInfo.HitPoint.X - SystemInformation.DragSize.Width / 2,
                hitInfo.HitPoint.Y - SystemInformation.DragSize.Height / 2), SystemInformation.DragSize);
            if (!dragRect.Contains(new Point(e.X, e.Y)))
            {
                List<DataRow> data = new List<DataRow>();
                int[] i = gridView1.GetSelectedRows();
                foreach (int j in i)
                    data.Add(gridView1.GetDataRow(j));
                gridControl1.DoDragDrop(data, DragDropEffects.Copy);
            }
        }

        void gridControl1_MouseDown(object sender, MouseEventArgs e)
        {
            hitInfo = gridView1.CalcHitInfo(new Point(e.X, e.Y));
            hitInfo1 = null;
        }
        //
        void gridControl1_DragDrop(object sender, DragEventArgs e)
        {
            if (hitInfo1 == null) return;
            List<DataRow> ldr = (e.Data.GetData(typeof(List<DataRow>)) as List<DataRow>);
            foreach (DataRow dr in ldr)
            {
                if (dr == null) continue;
                DataRow dr1 = tb.NewRow();
                dr1.ItemArray = dr.ItemArray;
                tb.Rows.Add(dr1);
                DataRow[] ldrCt = tb4.Select("MTID='" + dr["MTLSXID"].ToString() + "'");
                foreach (DataRow drCt in ldrCt)
                {
                    DataRow drCt1 = tb2.NewRow();
                    drCt1.ItemArray = drCt.ItemArray;
                    tb2.Rows.Add(drCt1);
                    tb4.Rows.Remove(drCt);
                }
                DataRow[] ldrKt = tb5.Select("MTID='" + dr["MTLSXID"].ToString() + "'");
                foreach (DataRow drKt in ldrKt)
                {
                    DataRow drKt1 = tb3.NewRow();
                    drKt1.ItemArray = drKt.ItemArray;
                    tb3.Rows.Add(drKt1);
                    tb5.Rows.Remove(drKt);
                }
                tb1.Rows.Remove(dr);
            }
            tb.AcceptChanges();
            tb1.AcceptChanges();
            tb2.AcceptChanges();
            tb3.AcceptChanges();
            tb4.AcceptChanges();
            tb5.AcceptChanges();
        }

        void gridControl1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        void gridControl2_MouseMove(object sender, MouseEventArgs e)
        {
            if (hitInfo1 == null) return;
            if (e.Button != MouseButtons.Left) return;
            Rectangle dragRect = new Rectangle(new Point(
                hitInfo1.HitPoint.X - SystemInformation.DragSize.Width / 2,
                hitInfo1.HitPoint.Y - SystemInformation.DragSize.Height / 2), SystemInformation.DragSize);
            if (!dragRect.Contains(new Point(e.X, e.Y)))
            {
                List<DataRow> data = new List<DataRow>();
                int[] i = gridView2.GetSelectedRows();
                foreach (int j in i)
                    data.Add(gridView2.GetDataRow(j));
                gridControl2.DoDragDrop(data, DragDropEffects.Copy);

            }
        }

        void gridControl2_MouseDown(object sender, MouseEventArgs e)
        {
            hitInfo1 = gridView2.CalcHitInfo(new Point(e.X, e.Y));
            hitInfo = null;
        }

        private void getdata()
        {
            gridControl3.DataSource = null;
            if (tb1 == null) return;

            if (tb1.Rows.Count == 0) return;
            string con = "";
            foreach (DataRow dr in tb1.Rows)
            {
                if (dr.RowState == DataRowState.Deleted || dr.RowState == DataRowState.Detached) continue;
                con += "'" + dr["MTLSXID"].ToString() + "',";
            }
            if (con == string.Empty) return;
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
            if (tb1 == null) return;
            if (tb1.Rows.Count == 0) return;
            string con = "";
            foreach (DataRow dr in tb1.Rows)
            {
                con += dr["DTLSXID"].ToString() + ",";
            }
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
