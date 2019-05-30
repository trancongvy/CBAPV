using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DataFactory;
using CDTDatabase;
using FormFactory;
using CDTLib;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
namespace Piriou
{
    public partial class ChkStockCutof : XtraForm
    {

        Database db = Database.NewDataDatabase();
        Database dbStr = Database.NewStructDatabase();
        DataTable PurList;
        DataTable tb;
        public ChkStockCutof()
        {
            InitializeComponent();
            
        }

        private void ChkStock_Load(object sender, EventArgs e)
        {
            string sql = "select MaKH, TenKH, Department, Leve from dmkh where SelectCode3='PRE' ";
            PurList = db.GetDataTable(sql);
            gridLookUpEdit1.Properties.DataSource = PurList;
            gridLookUpEdit1.Properties.ValueMember = "MaKH";
            gridLookUpEdit1.Properties.DisplayMember = "TenKH";
            gridLookUpEdit1.EditValue = Config.GetValue("FullName");
            
        }

        private void tbGet_Click(object sender, EventArgs e)
        {
           // if (gridLookUpEdit1.EditValue == null || gridLookUpEdit1.EditValue.ToString() == "") return;

            tb = db.GetDataSetByStore("GetCheckStockCutoff", new string[] { }, new object[] { });
            if (tb == null) return;
            tb.ColumnChanged += Tb_ColumnChanged;
            tb.ColumnChanging += tb_ColumnChanging;
            gridControl1.DataSource = tb;
        }

        void tb_ColumnChanging(object sender, DataColumnChangeEventArgs e)
        {
            if (e.Column.ColumnName == "bookstock" && e.ProposedValue != DBNull.Value)
            {
                 if (double.Parse(e.Row["soluong"].ToString()) < double.Parse(e.ProposedValue.ToString()))
                {
                    if (e.Row["bookstock"] != DBNull.Value)
                    {
                        e.ProposedValue = double.Parse(e.Row["bookstock"].ToString());
                    }
                }
            }
        }

        private void Tb_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            if(e.Column.ColumnName== "bookstock" && e.Row["bookstock"]!=DBNull.Value)
            {

                e.Row["slcan"] = double.Parse(e.Row["soluong"].ToString()) - double.Parse(e.Row["bookstock"].ToString());
                if (double.Parse(e.Row["slcan"].ToString()) < 0) e.Row["slcan"] = 0;
                e.Row.EndEdit();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gridControl1.DataSource == null) return;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel file|*.xls";
            sfd.ShowDialog();
            if (sfd.FileName == string.Empty) return;
            gridControl1.ExportToXls(sfd.FileName);
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            //string sql = "select dt29id, MaVT, Nomenclature,SoLuong,BookStock, SlCan, NgayCan from dt29 " +
            //        " where(Dacheck is null or Dacheck = 0) order by MaVT, NgayCan";
            db.BeginMultiTrans();
            bool r = true;
            try
            {
                //DataTable dsVT = db.GetDataTable(sql);
                //DataRow[] ldr;
                //double slBook = 0;
                //foreach (DataRow drCheck in tb.Rows)
                //{
                //    if (double.Parse(drCheck["bookstock"].ToString()) <= 0) continue;
                //    slBook = double.Parse(drCheck["bookstock"].ToString());
                //    ldr = dsVT.Select("MaVT='" + drCheck["MaVT"].ToString() + "' and Nomenclature='" + drCheck["Nomenclature"].ToString() + "'");
                //    foreach (DataRow drvt in ldr)
                //    {
                //        if (double.Parse(drvt["soluong"].ToString()) <= slBook)
                //        {
                //            drvt["bookstock"] = double.Parse(drvt["soluong"].ToString());
                //            drvt["slcan"] = 0;
                //            slBook = slBook - double.Parse(drvt["soluong"].ToString());
                //        }
                //        else
                //        {
                //            drvt["bookstock"] = slBook;
                //            drvt["slcan"] = double.Parse(drvt["soluong"].ToString()) - slBook;
                //            slBook = 0;
                //        }
                //        drvt.EndEdit();
                //        if (slBook == 0) break;
                //    }
                //}
                //Update
                foreach (DataRow dr in tb.Rows)
                {
                    if (dr["bookstock"] == DBNull.Value && dr["mrchange"] == DBNull.Value) continue;

                    string upsql = "";
                    if (dr["mrchange"] != DBNull.Value)
                    {
                        upsql = "update dt29 set MrChange='" + dr["mrchange"].ToString() + "'";
                        if (dr["bookstock"] != DBNull.Value) upsql += ", daCheck=1, bookstock=" + double.Parse(dr["bookstock"].ToString()).ToString("###########0.##") + ", slcan=" + double.Parse(dr["slcan"].ToString()).ToString("###########0.##");

                    }
                    else if (dr["bookstock"] != DBNull.Value)
                    {
                        upsql= "update dt29 set daCheck=1, bookstock=" + double.Parse(dr["bookstock"].ToString()).ToString("###########0.##") + ", slcan=" + double.Parse(dr["slcan"].ToString()).ToString("###########0.##");
                    }
                    if (dr["mavt1"] != DBNull.Value) upsql += ", mavt1='" + dr["mavt1"].ToString() + "'";                    
                    upsql += " where dt29id='" + dr["DT29ID"].ToString() + "'";
                    db.UpdateByNonQuery(upsql);
                    if (db.HasErrors)
                    {
                        db.RollbackMultiTrans();
                        MessageBox.Show("Can not update, error occur");
                        return;
                    }

                }
            }
            catch
            {
                MessageBox.Show("Can not update, error occur");
                r = false;
            }
            finally
            {
                if (r) db.EndMultiTrans();
                else db.RollbackMultiTrans();
            }
            tbGet_Click(sender, e);
        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {
            
        }
    }
}
