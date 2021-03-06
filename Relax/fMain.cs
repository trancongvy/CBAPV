using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CDTLib;
using CDTDatabase;
using FormFactory;
using DevExpress.XtraGrid;
using DevExpress.XtraLayout;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using System.Collections;
using CDTControl;
using CDTLib;
namespace Relax
{
    public partial class fMain : DevExpress.XtraEditors.XtraForm
    {
        Database db = CDTDatabase.Database.NewDataDatabase();
        ReadMaKH re = new ReadMaKH();
        public fMain()
        {
            InitializeComponent();
            getRidata();
            GetBindingData();
            tSothe.LostFocus += new EventHandler(tSothe_LostFocus);
            gridView1.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(gridView1_FocusedColumnChanged);
            if (Config.GetValue("isDebug").ToString() == "1")
            {
                btPrint.Enabled = true;
                btEditForm.Enabled = true;
            }
            else
            {
                btPrint.Enabled = false;
                btEditForm.Enabled = false;
            }
        }

        void gridView1_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            if (e.PrevFocusedColumn == gridView1.Columns[2])
            {
                gridView1.FocusedColumn = gridView1.Columns[0];
                gridView1.FocusedRowHandle = gridView1.RowCount - 1;
                gridView1.FocusedRowHandle = -999998;
            }
            if (e.PrevFocusedColumn == gridView1.Columns[0])
            {
                gridView1.FocusedColumn = gridView1.Columns[2];
            }
        }

        void tSothe_LostFocus(object sender, EventArgs e)
        {
            MaKH = re.Read(tSothe.Text);
            if (MaKH != "")
            {
                drMain["MaKH"] = MaKH;
                drMain["isNo"] = 1;
            }
            else
            {
                drMain["MaKH"] = "";
                drMain["isNo"] = 0;                
            }
            drMain.EndEdit();
        }

        private void btVe_Click(object sender, EventArgs e)
        {
            fInve finve = new fInve();
            finve.ShowDialog();

        }

        private void btXu_Click(object sender, EventArgs e)
        {
            fDoixu fdoixu = new fDoixu();
            fdoixu.ShowDialog();
        }
        private void btDiem_Click(object sender, EventArgs e)
        {
            fGhiDiem fGhidiem = new fGhiDiem();
            fGhidiem.ShowDialog();
        }

        string MaKho = "";
        string MaKH = "";
        bool DT = false;
        #region Đổi quà
        //Lấy dữ liệu
        DataTable dmVT;
        DataTable dmKho;
        DataSet dsData;
        DataRow drMain;
        private void getRidata()
        {
            string sql = "select MaVT, TenVT, MaDVT, GiaBan,GiaBan1 from dmvt";
            dmVT = db.GetDataTable(sql);
            rMaVT.DataSource = dmVT;
            rMaVT.View.BestFitColumns();
            rTenVT.DataSource = dmVT;
            sql = "select MaKho, TenKho from dmKho";
            dmKho = db.GetDataTable(sql);
            gdmKho.Properties.DataSource = dmKho;
            gdmKho.Properties.View.BestFitColumns();
            if (dmKho.Rows.Count > 0) MaKho = dmKho.Rows[0]["MaKho"].ToString();
        }
        
        private void GetBindingData()
        {
            string sql = "select * from MTDQ where 1=0; select *,'' as TenVT from DTDQ where 1=0";
            dsData = db.GetDataSet(sql);
            if (dsData.Tables.Count == 2)
            {                
                dsData.Tables[1].TableNewRow += new DataTableNewRowEventHandler(fMain1_TableNewRow);
                dsData.Tables[1].ColumnChanged += new DataColumnChangeEventHandler(fMain1_ColumnChanged);
                dsData.Tables[0].ColumnChanged+=new DataColumnChangeEventHandler(fMain0_ColumnChanged);
                dsData.Tables[0].TableNewRow +=new DataTableNewRowEventHandler(fMain0_TableNewRow);
                
                dsData.Tables[1].RowDeleted += new DataRowChangeEventHandler(fMain_RowDeleted);
                dsData.Tables[1].Columns["Soluong"].DefaultValue = 0;
                drMain = dsData.Tables[0].NewRow();
                dsData.Tables[0].Rows.Add(drMain);
                gridControl1.DataSource = dsData.Tables[1];
                gdmKho.DataBindings.Add(new Binding("EditValue", dsData.Tables[0], "MaKho", true, DataSourceUpdateMode.OnValidation));
                rgDT.DataBindings.Add(new Binding("EditValue", dsData.Tables[0], "DT", true, DataSourceUpdateMode.OnValidation));
                cisNo.DataBindings.Add(new Binding("Checked", dsData.Tables[0], "isNo", true, DataSourceUpdateMode.OnValidation));
                tMaKH.DataBindings.Add(new Binding("Text", dsData.Tables[0], "MaKh", true, DataSourceUpdateMode.OnValidation));
                tTien.DataBindings.Add(new Binding("Text", dsData.Tables[0], "TTien", true, DataSourceUpdateMode.OnValidation));
                tDiem.DataBindings.Add(new Binding("Text", dsData.Tables[0], "TDiem", true, DataSourceUpdateMode.OnValidation));
                gridControl1.KeyDown += new KeyEventHandler(gridControl1_KeyDown);
            }

        }

        void fMain_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            for (int i = 0; i < lstRow.Count; i++)
            {
                DataRow dr = lstRow[i];
                if (dr.RowState == DataRowState.Deleted || dr.RowState == DataRowState.Detached)
                {
                    lstRow.Remove(dr);
                    i--;
                }
            }
            TinhTongTien();
        }

        void gridControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                gridView1.DeleteSelectedRows();
            }
        }

        
        DataRow getMaVTInfo(string MaVT)
        {
            if (dmVT == null) return null;
            DataRow[] ldr = dmVT.Select("MaVT='" + MaVT + "'");
            if (ldr.Length == 0) return null;
            return ldr[0];
        }


        void fMain0_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            switch (e.Column.ColumnName)
            {
                case "DT":
                    foreach (DataRow dr in dsData.Tables[1].Rows)
                    {
                        dr["DT"] = drMain["DT"];
                    }
                    break;
                case "isNo":
                    break;
            }
            e.Row.EndEdit();
        }
        void fMain0_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["MTDQID"] = Guid.NewGuid();
            e.Row["DT"] = false;
            e.Row["isNo"] = false;
            e.Row["Ngayct"] = DateTime.Parse(Config.GetValue("NgayHethong").ToString());
            e.Row["MaKho"] = MaKho;
        }
        void TinhTien( DataRow e)
        {
            if (e["MaVT"] != DBNull.Value && e["MaVT"].ToString() != "" && e["Soluong"] != DBNull.Value)
            {
                DataRow drVt = getMaVTInfo(e["MaVT"].ToString());
                if (drVt == null) return;

                double GiaTien = double.Parse(drVt["Giaban"].ToString());
                double GiaDiem = double.Parse(drVt["Giaban1"].ToString());
                string TenVT = drVt["TenVT"].ToString();
                e["TenVT"] = TenVT;
                if (e["DT"].ToString() == "True")
                {
                    e["Diem"] = GiaDiem * double.Parse(e["Soluong"].ToString());
                    e["Tien"] = 0;                    
                }
                else
                {
                    e["Diem"] = 0;
                    e["Tien"] = GiaTien * double.Parse(e["Soluong"].ToString());
                }

            }
        }
        void TinhTongTien()
        {
            //if (dsData.Tables[1].Rows.Count == 0) return;
            double TDiem = 0;
            double TTien = 0;
            foreach (DataRow dr in lstRow)
            {
                try
                {
                    TTien += double.Parse(dr["Tien"].ToString());
                    TDiem += double.Parse(dr["Diem"].ToString());
                }
                catch { }
            }
            drMain["TTien"] = TTien;
            drMain["TDiem"] = TDiem;
            drMain.EndEdit();
        }
        List<DataRow> lstRow = new List<DataRow>();
        void fMain1_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            try
            {
                e.Row.EndEdit();
                
                    switch (e.Column.ColumnName)
                {
                    case "DT":
                        TinhTien( e.Row);
                        TinhTongTien();
                        break;
                    case "Soluong":
                        try
                        {
                            //e.Row.AcceptChanges();
                           // if (e.Row.RowState != DataRowState.Detached)
                           // {
                                TinhTien(e.Row);
                                TinhTongTien();
                                //dsData.Tables[1].Rows.Add(e.Row);
                           // }
                        }
                        catch (Exception ex) { }
                       
                        break;
                    case "MaVT":
                        TinhTien( e.Row);
                        TinhTongTien();
                        break;
                }


            }
            catch
            {
            }
           

        }

        void fMain1_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["DT"] = drMain["DT"];
            e.Row["MTDQID"] = drMain["MTDQID"];            
            e.Row.EndEdit();
            lstRow.Add(e.Row);
          //  throw new Exception("The method or operation is not implemented.");
        }
        #endregion

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            
        }


        private void rgDT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rgDT.SelectedIndex == 0) rgDT.EditValue = true;
            if (rgDT.SelectedIndex == 1) rgDT.EditValue = false;
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (dsData.Tables[1].Rows.Count == 0) return;

            for (int i = 0; i < dsData.Tables[1].Rows.Count; i++)
            {
                DataRow dr = dsData.Tables[1].Rows[i];
                if (dr["MaVT"]==DBNull.Value || int.Parse( dr["Soluong"].ToString())==0)
                {
                    dsData.Tables[1].Rows.RemoveAt(i);
                    i--;
                }
            }


            bool kq = true;
            CDTControl.Printing pr = new Printing(dsData.Tables[1], "Re_inDoiQua");
            if (Config.GetValue("isDebug").ToString() == "1")
                pr.Preview();
            else
                kq = kq && pr.Print();
            if (!kq) return;
            try
            {
                db.BeginMultiTrans();
                string sql = "insert into MTDQ(MTDQID,MaKho,NgayCT,MaKH, isNo,DT, TTien, TDiem,ws ) values(@MTDQID,@MaKho,@NgayCT,@MaKH, @isNo,@DT, @TTien, @TDiem, @ws)";
                List<string> paraNames = new List<string>();
                List<object> paraValues = new List<object>();
                List<SqlDbType> paraTypes = new List<SqlDbType>();
                paraNames.AddRange(new string[] { "MTDQID", "MaKho", "NgayCT", "MaKH", "isNo", "DT", "TTien", "TDiem", "ws" });
                paraValues.AddRange(new object[] { drMain["MTDQID"], drMain["MaKho"], drMain["Ngayct"], drMain["MaKH"], drMain["isNo"], drMain["DT"], drMain["TTien"], drMain["TDiem"], Config.GetValue("sysUserID").ToString() });
                paraTypes.AddRange(new SqlDbType[] { SqlDbType.UniqueIdentifier, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.Decimal, SqlDbType.Decimal, SqlDbType.NVarChar });
                kq = db.UpdateData(sql, paraNames.ToArray(), paraValues.ToArray(), paraTypes.ToArray());

                sql = "insert into DTDQ(MTDQID,DTDQID,MaVT,MaKho, Soluong, Tien, Diem, DT) values(@MTDQID,@DTDQID,@MaVT,@MaKho, @Soluong, @Tien, @Diem, @DT)";
                paraNames.Clear();
                paraNames.AddRange(new string[] { "MTDQID", "DTDQID", "MaVT", "MaKho", "Soluong", "Tien", "Diem", "DT" });
                paraTypes.Clear();
                paraTypes.AddRange(new SqlDbType[] { SqlDbType.UniqueIdentifier, SqlDbType.UniqueIdentifier, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Decimal, SqlDbType.Decimal, SqlDbType.Decimal, SqlDbType.Bit });

                foreach (DataRow dr in dsData.Tables[1].Rows)
                {
                    paraValues.Clear();
                    Guid dtid = Guid.NewGuid();
                    paraValues.AddRange(new object[] { dr["MTDQID"], dtid, dr["MaVT"], drMain["MaKho"], dr["soluong"], dr["Tien"], dr["Diem"], dr["DT"] });
                    kq = kq && db.UpdateData(sql, paraNames.ToArray(), paraValues.ToArray(), paraTypes.ToArray());
                    string sql1 = "Insert into blvt(MTID,MTIDDT, MaCT, soct, mavt, makho, soluong_x, psco) values (";
                    sql1 += "'" + dr["MTDQID"].ToString() + "','" + dtid.ToString() + "','PDQ','PDQ','" + dr["MaVT"].ToString() + "','" + drMain["MaKho"].ToString() + "'," + double.Parse(dr["Soluong"].ToString()).ToString("############0.#####") + ",0)";
                    kq = kq && db.UpdateByNonQuery(sql1);
                }
                if (kq)
                {
                    db.EndMultiTrans();

                    while (dsData.Tables[1].Rows.Count > 0)
                    {
                        dsData.Tables[1].Rows.RemoveAt(0);
                    }
                    lstRow.Clear();
                    dsData.Tables[0].Rows.RemoveAt(0);

                    drMain = dsData.Tables[0].NewRow();
                    dsData.Tables[0].Rows.Add(drMain);
                    tSothe.Text = "";
                }
                else
                    db.RollbackMultiTrans();
                

            }
            catch (Exception ex)
            {
                db.RollbackMultiTrans();
                MessageBox.Show("Lỗi " + ex.Message);

            }
        }

        private void btPrint_Click(object sender, EventArgs e)
        {
            CDTControl.Printing pr = new Printing(dsData.Tables[1], "Re_inDoiQua");
            if (Config.GetValue("isDebug").ToString() == "1")
                pr.Preview();
            else
                pr.Print();

        }

        private void btEditForm_Click(object sender, EventArgs e)
        {
            CDTControl.Printing pr = new Printing(dsData.Tables[1], "Re_inDoiQua");
            pr.EditForm();
        }

        private void gdmKho_EditValueChanged(object sender, EventArgs e)
        {
            if (gdmKho.EditValue == DBNull.Value) return;
            MaKho = gdmKho.EditValue.ToString();
        }



        

    }
}