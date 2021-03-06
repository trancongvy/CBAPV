using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Utils;
using DevExpress.XtraGrid;
using CDTDatabase;
using CDTLib;
using Formula;
using CDTControl;
namespace DongTienCus
{
    public partial class fNhapKho : DevExpress.XtraEditors.XtraForm
    {
        public fNhapKho()
        {
            InitializeComponent();
        }
        DateTime ngayct1;
        DateTime ngayct2;
        DataSet dsMt;
        DataSet dshoadon;
        Database db = CDTDatabase.Database.NewDataDatabase();
        BindingSource bs = new BindingSource();
        bool eventActive = true;
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            ngayct1 = DateTime.Parse(dateEdit1.EditValue.ToString());
            ngayct2 = DateTime.Parse(dateEdit2.EditValue.ToString());
            dsMt = db.GetDataSetByStore1("GetDataNhapTP", new string[] { "@ngayct1", "@ngayct2" }, new object[] { ngayct1, ngayct2 });
            if (dsMt == null) return;
            designColumn(dsMt.Tables[1]);
            dsMt.Tables[0].TableName="dmvt";
            dsMt.Tables[0].PrimaryKey = new DataColumn[] { dsMt.Tables[0].Columns["mavt"] };
            DataRelation dre = new DataRelation("dre", dsMt.Tables[0].Columns["mavt"], dsMt.Tables[1].Columns["mavt"], true);
            bs.DataSource = null;
            bs.DataSource = dsMt;
            bs.DataMember = dsMt.Tables[0].TableName;
            dsMt.Relations.Add(dre);

            gridControl1.DataSource = bs;

            gridControl2.DataSource = bs;
            gridControl2.DataMember = dre.RelationName;
            gridView2.OptionsView.ShowFooter = true;
            dsMt.Tables[1].ColumnChanged += new DataColumnChangeEventHandler(fNhapKho_ColumnChanged);
        }

        void fNhapKho_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            if (!eventActive) return;
            eventActive = false;
            if (e.Column.ColumnName.Contains("N_"))
            {
                DataRow dr = e.Row;
                string mavt = dr["mavt"].ToString();
                string mabak = e.Column.ColumnName.Replace("N_", "");
                DateTime ngayct =DateTime.Parse( dr["ngayct"].ToString().ToString());
                DataRow[] ldr = dsMt.Tables[1].Select("mavt='" + mavt + "' and ngayct>='" + ngayct.ToString() + "'");
                double toncuoi = 0;
                double sln = 0;
                double slx = 0;
                DataRow[] rtc = dsMt.Tables[1].Select("mavt='" + mavt + "' and ngayct='" + ngayct.AddDays(-1).ToString() + "'");
                if (rtc.Length > 0) toncuoi = double.Parse(rtc[0]["T_" + mabak].ToString());

                foreach (DataRow drdt in ldr)
                {
                    sln = double.Parse(drdt["N_" + mabak].ToString());
                    slx = double.Parse(drdt["X_" + mabak].ToString());
                    toncuoi = sln - slx + toncuoi;
                    drdt["T_" + mabak] = toncuoi;
                }
            }
            eventActive = true;
        }

        private void designColumn(DataTable tb)
        {
            gridControl2.BeginInit();
            gridView2.BeginInit();
            while (gridView2.Columns.Count > 2)
            {
                DevExpress.XtraGrid.Columns.GridColumn gcol = gridView2.Columns[2];
                if (gcol.DisplayFormat.FormatType == FormatType.Numeric) gridView2.Columns.Remove(gcol);
            }
            foreach (DataColumn col in tb.Columns)
            {
                if (col.DataType != typeof(decimal)) continue;
                DevExpress.XtraGrid.Columns.GridColumn gcol1 = new DevExpress.XtraGrid.Columns.GridColumn();
                gcol1.FieldName = col.ColumnName;
                gcol1.DisplayFormat.FormatType = FormatType.Numeric;
                gcol1.DisplayFormat.FormatString = "### ### ### ###.##   ";

                gcol1.Caption = col.ColumnName;
                gcol1.ColumnEdit = repositoryItemCalcEdit1;
                if (col.ColumnName.Substring(0, 1) == "N")
                    gcol1.OptionsColumn.AllowEdit = true;
                else
                    gcol1.OptionsColumn.AllowEdit = false;
                gcol1.Width = 60;
                gcol1.VisibleIndex = gridView2.Columns.Count;
                gridView2.Columns.Add(gcol1);
                gcol1.SummaryItem.Assign(new GridSummaryItem(DevExpress.Data.SummaryItemType.Sum, gcol1.FieldName, "{0:### ### ###.##}"));
            }
            gridControl2.EndInit();
            gridView2.EndInit();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            eventActive = false;
            DataTable tb = dsMt.Tables[1];
            Random Rand=new Random();
            foreach (DataRow drMt in dsMt.Tables[0].Rows)
            {
                bool thucong = bool.Parse(drMt["tonthucong"].ToString());
                if (thucong) continue;
                string mavt = drMt["mavt"].ToString();
                double titrongtk = double.Parse(drMt["titrongtk"].ToString());
                DataRow[] ldr = tb.Select("mavt='" + mavt + "'");
                int c = 2;
                while (c < tb.Columns.Count)
                {
                    string Mabak = tb.Columns[c].ColumnName.Replace("N_", "");
                    double toncuoi=0;
                    double sln=0;
                    double slx=0;
                    bool begin = true;
                    foreach (DataRow dr in ldr) 
                    {
                        if (begin)
                        {
                            if (dr["T_" + Mabak] != DBNull.Value)
                                toncuoi = double.Parse(dr["T_" + Mabak].ToString());
                        }
                        else
                        {
                            slx = double.Parse(dr["X_" + Mabak].ToString());

                            double tontmp = Math.Round(Rand.NextDouble() * titrongtk * slx / 100, 0);
                            sln = slx + tontmp - toncuoi;
                            if (sln < 0)
                            {
                                sln = 0;
                                tontmp = toncuoi - slx;
                            }
                            toncuoi = tontmp;
                            dr["N_" + Mabak] = sln;
                            dr["T_" + Mabak] = toncuoi;
                        }
                        begin = false;
                    }
                    c = c + 3;
                }
            }
            eventActive = true;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            string sql = "select *   from hoadon where ngayct between '" + ngayct1.ToString("MM/dd/yyyy") + "' and '" + ngayct2.ToString("MM/dd/yyyy") + "'";
            dshoadon = db.GetDataSet(sql);
            DataTable tb = dsMt.Tables[1];
            int i = 0;
            foreach (DataRow drMt in tb.Rows)
            {
                int c = 2;
                
                i++;
                while (c < tb.Columns.Count)
                {
                    double sln = double.Parse(drMt[tb.Columns[c].ColumnName].ToString());
                    if (sln <= 0)
                    {
                        c = c + 3;
                        continue;
                    }
                    string Mabak = tb.Columns[c].ColumnName.Replace("N_", "");
                    string mavt = drMt["mavt"].ToString();
                   
                    DateTime ngayct = DateTime.Parse(drMt["ngayct"].ToString().ToString());
                    sql = "update hoadon set slNhap=" + sln + " where  stt = (select min(stt) as stt from hoadon where mavt='" + mavt + "' and ngayct='" + ngayct.ToString() + "' and makh='" + Mabak + "')";
                    int re = 0;
                    db.UpdateByNonQuery(sql,ref re);
                    if (re == 0)
                    {
                        sql = "insert hoadon (ngayct, mavt, makh,soluong, slNhap) values('" + ngayct.ToString() + "','" + mavt + "','" + Mabak + "',0," + sln + ")";
                        db.UpdateByNonQuery(sql);
                    }
                    if (db.HasErrors)
                    {
                        MessageBox.Show("Có lỗi khi tạo dữ liệu");
                        return;
                    }
                    //DataRow[] ldr = dshoadon.Tables[0].Select("mavt='" + mavt + "' and ngayct='" + ngayct.ToString() + "' and makh='" + Mabak + "'");
                    //if (ldr.Length > 0) ldr[0]["slNhap"] = sln;
                    c = c + 3;
                }
            }
            db.UpdateDatabyStore("CreateNK_DC_XK_TP", new string[] { "@ngayct1", "@ngayct2" }, new object[] {ngayct1, ngayct2  });
            MessageBox.Show("Hoàn thành");
           // db.UpdateDataSet(dshoadon);
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            db.UpdateDatabyStore("DeleteNK_DC_XK_TP", new string[] { "@ngayct1", "@ngayct2" }, new object[] { ngayct1, ngayct2 });
            MessageBox.Show("Hoàn thành");
        }
    }
}