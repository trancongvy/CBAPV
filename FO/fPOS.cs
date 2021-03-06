using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CDTDatabase;
using FormFactory;
using DataFactory;
using DevExpress.XtraGrid;
using DevControls;
using DevExpress.XtraLayout;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using CDTLib;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
namespace FO
{
    public partial class fPOS : DevExpress.XtraEditors.XtraForm
    {
        private Database _dbData = Database.NewDataDatabase();
        private Database _dbStruct = Database.NewStructDatabase();
        private DataSet dmBan;
        private DataTable dmBan1;
        private BindingSource bsBan = new BindingSource();
        private BindingSource bsCt = new BindingSource();
        private BindingSource bsLoaiGia = new BindingSource();
        private DataTable dmMon;
        private DataTable dmLoaigia;
        private List<DataRow> lstDr = new List<DataRow>();
        private double tile = 0;
        private double TTienH = 0;
        private double PtCk = 0;
        private double CK = 0;
        private double TTien = 0;
        private DataRow curMaster;

        GridHitInfo hitInfo = null;
        private DataTable SodoBan;


        public fPOS()
        {
            InitializeComponent();
            getDataForBep();
            GetData();
            repositoryItemCalcEdit1.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(repositoryItemCalcEdit1_ButtonClick);
            bsBan.CurrentChanged += new EventHandler(bsBan_CurrentChanged);
            bsLoaiGia.CurrentChanged += new EventHandler(bsLoaiGia_CurrentChanged);
            this.KeyDown += new KeyEventHandler(POS_KeyDown);
            gridControl4.MouseDown += new MouseEventHandler(gridControl4_MouseDown);
            gridControl4.MouseMove += new MouseEventHandler(gridControl4_MouseMove);
            gridControl3.DragEnter += new DragEventHandler(gridControl3_DragEnter);
            gridControl3.DragDrop += new DragEventHandler(gridControl3_DragDrop);
        }
        #region Drag món vào bếp
        void gridControl3_DragDrop(object sender, DragEventArgs e)
        {
            DataRow drMon = (e.Data.GetData(typeof(DataRow)) as DataRow);
            if (drMon == null) return;
            //foreach (DataRow drBep in tbBep.Rows)
            //{
            //    if (drBep["MaMon"].ToString() == drMon["MaMon"].ToString()&& drBep["MaBan"].ToString()==tbBep.Columns["MaBan"].DefaultValue.ToString())
            //    {
            //        drBep["SoLuong"] = int.Parse(drBep["SoLuong"].ToString()) + 1;
            //        return;
            //    }
            //}
            DataRow drAdd = tbBep.NewRow();
            drAdd["MaMon"] = drMon["MaMon"].ToString();
            drAdd["TenMon"] = drMon["TenMon"].ToString();
            drAdd["SoLuong"] = 1;
            tbBep.Rows.Add(drAdd);
        }

        void gridControl3_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        //Drag Drop
        
        
            
        void gridControl4_MouseDown(object sender, MouseEventArgs e)
        {
            hitInfo = gridView5.CalcHitInfo(new Point(e.X, e.Y));
            
        }
        void gridControl4_MouseMove(object sender, MouseEventArgs e)
        {
            if (hitInfo == null) return;
            if (e.Button != MouseButtons.Left) return;
            Rectangle dragRect = new Rectangle(new Point(
                hitInfo.HitPoint.X - SystemInformation.DragSize.Width / 2,
                hitInfo.HitPoint.Y - SystemInformation.DragSize.Height / 2), SystemInformation.DragSize);
            if (!dragRect.Contains(new Point(e.X, e.Y)))
            {
                if (hitInfo.InRowCell)
                    gridControl4.DoDragDrop(gridView5.GetDataRow(hitInfo.RowHandle), DragDropEffects.Copy);
                if (hitInfo.HitTest == GridHitTest.RowIndicator)
                {
                    DataRow data = gridView5.GetDataRow(hitInfo.RowHandle);
                    gridControl4.DoDragDrop(data, DragDropEffects.Copy);
                }
            }
        }
        #endregion

        #region Thao tác trên form
        void POS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                simpleButton1_Click(btChuyen, new EventArgs());
        }


        private void POS_Load(object sender, EventArgs e)
        {
            gridView1.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(gridView1_RowCellStyle);
        }

        void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            DataRow dr=gridView1.GetDataRow(e.RowHandle);
            if (dr == null) return;
            if (dr[e.Column.FieldName] == DBNull.Value) return;
            string MaBan=   dr[e.Column.FieldName].ToString()   ;
            int i = bsBan.Find("MaBan", MaBan);
            if (i < 0) return;
            if (double.Parse(dmBan.Tables[0].Rows[i]["TTienH"].ToString()) > 0)
                e.Appearance.BackColor = Color.Red;
        }
        void gridControl2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F8)
            {
                gridView2.DeleteSelectedRows();
            }
        }
        void gridView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0) TinhTongTien();
        }
        #endregion

        #region Thao tác trên data
        private void RefreshData()
        {
            string sql = "select MTPOSID,Ngayct, MaPhong,MaBan, Ghichu, MaPOSArea,BanSo,MaPOSLoaiGia,Tile =case when Tile is null then 1.0 else Tile end,TTienH =case when TTienH is null then 0.0 else TTienH end,PtCk =case when PtCk is null then 0.0 else PtCk end,CK =case when CK is null then 0.0 else CK end,TTien =case when TTien is null then 0.0 else TTien end from " +
                    "(select a.*,b.Ngayct,b.maposloaigia,b.tile,b.MaPhong,TTienH, PtCk,Ck, TTien,MTPOSID from dmban a, mtpos b where a.Maban*=b.MaBan and b.Datt=0) x" +
                    "; select MTPOSID,MaBan, MaMon, SoLuong,DonGia,ThanhTien from ctPOS where MTPOSID in (select MTPOSID from MTPOS where daTT=0)";
            dmBan = _dbData.GetDataSet(sql);
            dmBan.Tables[0].PrimaryKey = new DataColumn[] { dmBan.Tables[0].Columns["MaBan"] };

            DataRelation rela = new DataRelation("Main", dmBan.Tables[0].Columns["MaBan"], dmBan.Tables[1].Columns["MaBan"], true);
            dmBan.Relations.Add(rela);
            dmBan.Tables[0].ColumnChanged += new DataColumnChangeEventHandler(Table0_ColumnChanged);
            dmBan.Tables[1].ColumnChanged += new DataColumnChangeEventHandler(Table1_ColumnChanged);

            dmBan.Tables[1].TableNewRow += new DataTableNewRowEventHandler(Table1_TableNewRow);
            dmBan.Tables[1].RowDeleting += new DataRowChangeEventHandler(POS_RowDeleting);
            dmBan.Tables[1].RowDeleted += new DataRowChangeEventHandler(POS_RowDeleted);
            bsBan.DataSource = dmBan;

            bsBan.DataMember = dmBan.Tables[0].TableName;
            //gridControl1.DataSource = bsBan;
            //gridView1.ExpandAllGroups();
            bsBan_CurrentChanged(bsBan, new EventArgs());
            gridControl2.DataSource = bsBan;
            gridControl2.DataMember = "Main";
            gridView2.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridView2_FocusedRowChanged);
            gridControl2.KeyDown += new KeyEventHandler(gridControl2_KeyDown);


            if (txtTile.DataBindings.Count == 0)
            {
                txtTile.DataBindings.Add(new Binding("Text", bsBan, "Tile"));
                grLoaiGia.DataBindings.Add(new Binding("EditValue", bsBan, "MaPOSLoaiGia"));
                calcEdit1.DataBindings.Add(new Binding("Value", bsBan, "TTienH"));
                calcEdit2.DataBindings.Add(new Binding("Value", bsBan, "PtCk"));
                calcEdit4.DataBindings.Add(new Binding("Value", bsBan, "CK"));
                calcEdit3.DataBindings.Add(new Binding("Value", bsBan, "TTien"));
                gridLookUpEdit2.DataBindings.Add(new Binding("EditValue", bsBan, "MaPhong"));
            }
            if (bsLoaiGia.Current != null) bsLoaiGia_CurrentChanged(bsLoaiGia, new EventArgs());
            dxErrorProviderMain.DataSource = dmBan.Tables[1];
        }
        private void GetData()
        {
            string sql = "select * from dmMon";
            dmMon = _dbData.GetDataTable(sql);
            dmMon.PrimaryKey = new DataColumn[] { dmMon.Columns["MaMon"] };
            repositoryItemGridLookUpEdit1.DataSource = dmMon;
            sql = "select * from dmPOSLoaiGia";
            dmLoaigia = _dbData.GetDataTable(sql);
            dmLoaigia.PrimaryKey = new DataColumn[] { dmLoaigia.Columns["MaPOSLoaiGia"] };
            bsLoaiGia.DataSource = dmLoaigia;
            grLoaiGia.Properties.DataSource = bsLoaiGia;

            sql = "select * from dmBan";
            dmBan1 = _dbData.GetDataTable(sql);
            dmBan1.PrimaryKey = new DataColumn[] { dmBan1.Columns["MaBan"] };
            gridLookUpEdit1.Properties.DataSource = dmBan1;


            sql = "select MaPhong,TenPhong from dmPhong where MaTT='IN'";
            DataTable dmPhong = _dbData.GetDataTable(sql);
            dmPhong.PrimaryKey = new DataColumn[] { dmPhong.Columns["MaPhong"] };
            gridLookUpEdit2.Properties.DataSource = dmPhong;
            SodoBan = _dbData.GetDataSetByStore("getDMBan", new string[] { }, new object[] { });
            if (SodoBan != null)
            {
                gridControl1.DataSource = SodoBan;
                gridView1.SelectionChanged += new DevExpress.Data.SelectionChangedEventHandler(gridView1_SelectionChanged);
            }
            RefreshData();
        }

        void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            DevExpress.XtraGrid.Views.Base.GridCell[] Cell = gridView1.GetSelectedCells();
            if (Cell.Length == 0) return;
            DataRow dr = SodoBan.Rows[Cell[0].RowHandle];
            string MaBantmp = dr[Cell[0].Column.FieldName].ToString();
            int h = bsBan.Find("MaBan", MaBantmp);
            if (h < 0) return;
            bsBan.Position = h;

        }

        void Table0_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            try
            {
                if (e.Column.ColumnName == "TTienH")
                {
                    if (curMaster["TTienH"].ToString() == "" || curMaster["PtCk"].ToString() == "") return;
                    curMaster["CK"] = double.Parse(curMaster["TTienH"].ToString()) * double.Parse(curMaster["PtCk"].ToString()) / 100;
                    curMaster.EndEdit();
                }
                if (e.Column.ColumnName == "PtCk")
                {
                    if (curMaster["PtCk"].ToString() == "") return;
                    curMaster["CK"] = double.Parse(curMaster["TTienH"].ToString()) * double.Parse(curMaster["PtCk"].ToString()) / 100;
                    curMaster.EndEdit();
                }
                if (e.Column.ColumnName == "CK")
                {
                    curMaster["TTien"] = double.Parse(curMaster["TTienH"].ToString()) - double.Parse(curMaster["CK"].ToString());
                    curMaster.EndEdit();
                }

            }
            catch (Exception ex) { }
        }

        void Table1_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            if (e.Row.RowState == DataRowState.Detached)
                e.Row["MaBan"] = curMaster["MaBan"].ToString();
            lstDr.Clear();
            DataRow[] lstDr1 = dmBan.Tables[1].Select("MaBan='" + curMaster["MaBan"].ToString() + "'");
            foreach (DataRow dr in lstDr1)
                lstDr.Add(dr);
            lstDr.Add(e.Row);

        }
        void POS_RowDeleting(object sender, DataRowChangeEventArgs e)
        {
            lstDr.Remove(e.Row);
        }
        void POS_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            TinhTongTien();
        }

        void bsLoaiGia_CurrentChanged(object sender, EventArgs e)
        {
            if (bsLoaiGia.Current == null) return;

            tile = double.Parse((bsLoaiGia.Current as DataRowView).Row["Tile"].ToString());
            curMaster["MaPOSLoaiGia"] = (bsLoaiGia.Current as DataRowView).Row["MaPOSLoaiGia"];
            curMaster["Tile"] = tile;
            SetTile();
            TinhTongTien();
        }

        private void SetTile()
        {
            DataRow[] lstdr = dmBan.Tables[1].Select("MaBan='" + curMaster["MaBan"].ToString() + "'");

            foreach (DataRow r in lstdr)
            {
                if (r["SoLuong"].ToString() == "") continue;
                if (r["MaMon"].ToString() == "") continue;
                DataRow dr = dmMon.Rows.Find(r["MaMon"].ToString());
                if (dr == null) continue;
                r["DonGia"] = double.Parse(dr["GiaBan"].ToString()) * tile;
                r["ThanhTien"] = double.Parse(r["DonGia"].ToString()) * double.Parse(r["SoLuong"].ToString());

            }
        }

        void Table1_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {

            if (e.Column.ColumnName == "MaMon")
            {
                if (e.Row[e.Column.ColumnName].ToString() != "")
                {
                    DataRow dr = dmMon.Rows.Find(e.Row[e.Column.ColumnName].ToString());
                    if (dr == null) return;
                    e.Row["DonGia"] = double.Parse(dr["GiaBan"].ToString()) * tile;
                    if (e.Row["SoLuong"].ToString() == "") return;
                    e.Row["ThanhTien"] = double.Parse(e.Row["DonGia"].ToString()) * double.Parse(e.Row["SoLuong"].ToString());
                }
            }
            if (e.Column.ColumnName == "SoLuong")
            {
                if (e.Row[e.Column.ColumnName].ToString() != "" && e.Row["DonGia"].ToString() != "")
                {
                    e.Row["ThanhTien"] = double.Parse(e.Row["DonGia"].ToString()) * double.Parse(e.Row["SoLuong"].ToString());
                }
            }
            if (e.Column.ColumnName == "ThanhTien")
            {
                TinhTongTien();
                if (e.Row.RowState == DataRowState.Detached) dmBan.Tables[1].Rows.Add(e.Row);
                curMaster.EndEdit();
            }
        }
        void repositoryItemCalcEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

        }
        void bsBan_CurrentChanged(object sender, EventArgs e)
        {

            curMaster = (bsBan.Current as DataRowView).Row;
            if (curMaster["MaPOSLoaiGia"].ToString() == "" && bsLoaiGia != null)
            {
                curMaster["MaPOSLoaiGia"] = (bsLoaiGia.Current as DataRowView).Row["MaPOSLoaiGia"].ToString();
            }
            lstDr.Clear();
            DataRow[] lstDr1 = dmBan.Tables[1].Select("MaBan='" + curMaster["MaBan"].ToString() + "'");
            foreach (DataRow dr in lstDr1)
                lstDr.Add(dr);
            if (tbBep != null)
            {
                tbBep.Columns["MaBep"].DefaultValue = curMaster["MaPOSArea"].ToString();
                tbBep.Columns["MaBan"].DefaultValue = curMaster["MaBan"].ToString();
            }
        }

        private void TinhTongTien()
        {

            TTienH = 0;

            foreach (DataRow dr in lstDr)
            {
                if (dr["ThanhTien"].ToString() == "") continue;
                TTienH += double.Parse(dr["ThanhTien"].ToString());
            }
            CK = TTienH * (PtCk / 100);
            TTien = TTienH - CK;
            if (curMaster == null) return;
            curMaster["TTienH"] = TTienH;

            curMaster.EndEdit();

        }
        #endregion

        #region Các sự kiện click
        private void CheckRule()
        {
            foreach (DataRow dr in dmBan.Tables[1].Rows)
            {
                if (dr.RowState == DataRowState.Detached || dr.RowState == DataRowState.Deleted) return;
                if (dr["MaMon"] == null || dr["MaMon"].ToString() == string.Empty) dr.SetColumnError("MaMon", "Phải nhập");
                if (dr["SoLuong"] == null || dr["SoLuong"].ToString() == string.Empty) dr.SetColumnError("SoLuong", "Phải nhập");
            }
        }
        private void tbSave_Click(object sender, EventArgs e)
        {
            CheckRule();
            btLuu_Click(sender, new EventArgs());
            if (dxErrorProviderMain.HasErrors || dmBan.Tables[1].HasErrors) return;
            string sql;

            try
            {
                _dbData.BeginMultiTrans();
                foreach (DataRow dr in dmBan.Tables[0].Rows)
                {
                    DataRow[] drCt = dmBan.Tables[1].Select("MaBan='" + dr["MaBan"].ToString() + "'");
                    if (drCt.Length == 0)
                    {
                        if (dr["MTPOSID"].ToString() != string.Empty)//Lần trước đã lưu, nhưng sau khi load lại thì chuyển qua bàn khác hoặc xóa đi rồi
                        {
                            //Delete
                            sql = "delete Ctpos where MTPOSID='" + dr["MTPOSID"].ToString().Trim() + "'";
                            _dbData.UpdateByNonQuery(sql);
                            sql = "delete Mtpos where MTPOSID='" + dr["MTPOSID"].ToString().Trim() + "'";
                            _dbData.UpdateByNonQuery(sql);
                        }
                    }
                    else
                    {
                        if (dr["MTPOSID"].ToString() == string.Empty)//Thêm mới
                        {
                            //Insert
                            Guid ID = Guid.NewGuid();
                            sql = "insert into MTPOS(MTPOSID, NgayCT,MaPhong,MaPOSLoaiGia,Tile,TTienH, PtCk, CK, TTien, MaBan, DaTT) values('" +
                                ID.ToString() + "',getdate()," + (dr["MaPhong"] == DBNull.Value ? "null,'" : ("'" + dr["MaPhong"].ToString() + "','")) + dr["MaPOSLoaiGia"].ToString() + "'," + dr["TiLe"].ToString() + "," + dr["TTienH"].ToString() +
                                "," + dr["PtCk"].ToString() + "," + dr["CK"].ToString() + "," + dr["TTien"].ToString() + ",'" + dr["MaBan"].ToString() + "',0)";
                            _dbData.UpdateByNonQuery(sql);
                            foreach (DataRow drct1 in drCt)
                            {
                                sql = "insert into CTPOS(MTPOSID, NgayCT,MaBan, MaMon,SoLuong,DonGia,ThanhTien, DaTT) values('" +
                                ID.ToString() + "',getdate(),'" + drct1["MaBan"].ToString() + "','" + drct1["MaMon"].ToString() +
                                "'," + drct1["SoLuong"].ToString() + "," + drct1["DonGia"].ToString() + "," + drct1["ThanhTien"].ToString() + ",0)";
                                _dbData.UpdateByNonQuery(sql);
                                if (_dbData.HasErrors)
                                {
                                    _dbData.RollbackMultiTrans();
                                    return;
                                }
                            }
                            if (_dbData.HasErrors)
                            {
                                _dbData.RollbackMultiTrans();
                                return;
                            }
                        }
                        else
                        {
                            //Update
                            sql = "Update MTPOS set MaPOSLoaiGia='" + dr["MaPOSLoaiGia"].ToString() + "',Tile=" + dr["TiLe"].ToString() + ",TTienH=" +
                                 dr["TTienH"].ToString() + ", MaPhong=" + ((dr["MaPhong"] == DBNull.Value )? "null" : ("'" + dr["MaPhong"].ToString() + "'")) + ",PtCk=" + dr["PtCk"].ToString() + ",CK=" + dr["CK"].ToString() + ",TTien=" + dr["TTien"].ToString() +
                                 " where MTPOSID='" + dr["MTPOSID"].ToString() + "'";
                            _dbData.UpdateByNonQuery(sql);
                            sql = "delete CTPOS where MTPOSID='" + dr["MTPOSID"].ToString() + "'";

                            _dbData.UpdateByNonQuery(sql);
                            foreach (DataRow drct1 in drCt)
                            {
                                if (drct1.RowState != DataRowState.Deleted && drct1.RowState != DataRowState.Detached)
                                {
                                    sql = "insert into CTPOS(MTPOSID, NgayCT,MaBan, MaMon,SoLuong,DonGia,ThanhTien, DaTT) values('" +
                                    dr["MTPOSID"].ToString() + "',getdate(),'" + drct1["MaBan"].ToString() + "','" + drct1["MaMon"].ToString() +
                                    "'," + drct1["SoLuong"].ToString() + "," + drct1["DonGia"].ToString() + "," + drct1["ThanhTien"].ToString() + ",0)";
                                    _dbData.UpdateByNonQuery(sql);
                                    if (_dbData.HasErrors)
                                    {
                                        _dbData.RollbackMultiTrans();
                                        return;
                                    }
                                }
                            }
                            if (_dbData.HasErrors)
                            {
                                _dbData.RollbackMultiTrans();
                                return;
                            }
                        }
                    }
                }
                _dbData.EndMultiTrans();
                RefreshData();
                //MessageBox.Show("Đã lưu xong!");
            }
            catch (Exception ex)
            {
                if (_dbData.HasErrors)
                {
                    MessageBox.Show(ex.Message);
                    _dbData.RollbackMultiTrans();
                    return;
                }
            }
        }

        private void btChuyen_Click(object sender, EventArgs e)
        {
            if (gridLookUpEdit1.EditValue == null) return;
            if (bsBan.Current == null) return;
            string MaBanDi = curMaster["MaBan"].ToString();
            string MaBanDen = gridLookUpEdit1.EditValue.ToString();
            if (MaBanDen == MaBanDi) return;
            try
            {
                DataRow[] lstDr1 = dmBan.Tables[1].Select("MaBan='" + MaBanDi + "'");
                foreach (DataRow dr in lstDr1)
                    dr["MaBan"] = MaBanDen;
                lstDr.Clear();
                TinhTongTien();
                int cur = bsBan.Find("MaBan", MaBanDen);
                if (cur >= 0) bsBan.Position = cur;
                TinhTongTien();
            }
            catch
            {
            }
        }

        private void btChuyenvePhong_Click(object sender, EventArgs e)
        {
            CheckRule();
            if (dxErrorProviderMain.HasErrors || dmBan.Tables[1].HasErrors) return;

            if (gridLookUpEdit2.EditValue == null) return;
            if (bsBan.Current == null) return;
            string sql;
            try
            {
                _dbData.BeginMultiTrans();
                foreach (DataRow dr in dmBan.Tables[0].Rows)
                {
                    DataRow[] drCt = dmBan.Tables[1].Select("MaBan='" + dr["MaBan"].ToString() + "'");
                    if (drCt.Length == 0)
                    {
                        if (dr["MTPOSID"].ToString() != string.Empty)//Lần trước đã lưu, nhưng sau khi load lại thì chuyển qua bàn khác hoặc xóa đi rồi
                        {
                            //Delete
                            sql = "delete Ctpos where MTPOSID='" + dr["MTPOSID"].ToString().Trim() + "'";
                            _dbData.UpdateByNonQuery(sql);
                            sql = "delete Mtpos where MTPOSID='" + dr["MTPOSID"].ToString().Trim() + "'";
                            _dbData.UpdateByNonQuery(sql);
                        }
                    }
                    else
                    {
                        if (dr["MTPOSID"].ToString() == string.Empty)//Thêm mới
                        {
                            //Insert
                            Guid ID = Guid.NewGuid();
                            dr["MTPOSID"] = ID;
                            sql = "insert into MTPOS(MTPOSID, NgayCT,MaPhong,MaPOSLoaiGia,Tile,TTienH, PtCk, CK, TTien, MaBan, DaTT) values('" +
                                ID.ToString() + "',getdate(),'"  + (dr["MaPhong"]==DBNull.Value? "null":("'" + dr["MaPhong"].ToString() + "'")) + dr["MaPOSLoaiGia"].ToString() + "'," + dr["TiLe"].ToString() + "," + dr["TTienH"].ToString() +
                                "," + dr["PtCk"].ToString() + "," + dr["CK"].ToString() + "," + dr["TTien"].ToString() + ",'" + dr["MaBan"].ToString() + "',0)";
                            _dbData.UpdateByNonQuery(sql);
                            foreach (DataRow drct1 in drCt)
                            {
                                sql = "insert into CTPOS(MTPOSID, NgayCT,MaBan, MaMon,SoLuong,DonGia,ThanhTien, DaTT) values('" +
                                ID.ToString() + "',getdate(),'"  + drct1["MaBan"].ToString() + "','" + drct1["MaMon"].ToString() +
                                "'," + drct1["SoLuong"].ToString() + "," + drct1["DonGia"].ToString() + "," + drct1["ThanhTien"].ToString() + ",0)";
                                _dbData.UpdateByNonQuery(sql);
                                if (_dbData.HasErrors)
                                {
                                    _dbData.RollbackMultiTrans();
                                    return;
                                }
                            }
                            if (_dbData.HasErrors)
                            {
                                _dbData.RollbackMultiTrans();
                                return;
                            }
                        }
                        else
                        {
                            //Update
                            sql = "Update MTPOS set ngayct=getdate(), MaPOSLoaiGia='" + dr["MaPOSLoaiGia"].ToString() + "',Tile=" + dr["TiLe"].ToString() + ",TTienH=" +
                                 dr["TTienH"].ToString() + ", MaPhong=" + (dr["MaPhong"] == DBNull.Value ? "null" : ("'" + dr["MaPhong"].ToString() + "'")) + ",PtCk=" + dr["PtCk"].ToString() + ",CK=" + dr["CK"].ToString() + ",TTien=" + dr["TTien"].ToString() +
                                 " where MTPOSID='" + dr["MTPOSID"].ToString() + "'";
                            _dbData.UpdateByNonQuery(sql);
                            sql = "delete CTPOS where MTPOSID='" + dr["MTPOSID"].ToString() + "'";

                            _dbData.UpdateByNonQuery(sql);
                            foreach (DataRow drct1 in drCt)
                            {
                                if (drct1.RowState != DataRowState.Deleted && drct1.RowState != DataRowState.Detached)
                                {
                                    sql = "insert into CTPOS(MTPOSID, NgayCT,MaBan, MaMon,SoLuong,DonGia,ThanhTien, DaTT) values('" +
                                    dr["MTPOSID"].ToString() + "',getdate(),'" + drct1["MaBan"].ToString() + "','" + drct1["MaMon"].ToString() +
                                    "'," + drct1["SoLuong"].ToString() + "," + drct1["DonGia"].ToString() + "," + drct1["ThanhTien"].ToString() + ",0)";
                                    _dbData.UpdateByNonQuery(sql);
                                    if (_dbData.HasErrors)
                                    {
                                        _dbData.RollbackMultiTrans();
                                        return;
                                    }
                                }
                            }
                            if (_dbData.HasErrors)
                            {
                                _dbData.RollbackMultiTrans();
                                return;
                            }
                        }
                    }
                }
                //btPrint_Click(sender, new EventArgs());
                //update phòng vào 
                sql = "update MTPOS set Maphong='" + gridLookUpEdit2.EditValue + "', DaTT=1 where MTPOSID='" + curMaster["MTPOSID"].ToString() + "'";
                _dbData.UpdateByNonQuery(sql);
                string MaBan = curMaster["MaBan"].ToString();
                _dbData.EndMultiTrans();
                RefreshData();

                int cur = bsBan.Find("MaBan", MaBan);
                if (cur >= 0) bsBan.Position = cur;

            }
            catch { }

        }
        private void btThanhtoan_Click(object sender, EventArgs e)
        {
            CheckRule();
            if (dxErrorProviderMain.HasErrors || dmBan.Tables[1].HasErrors) return;
            string sql;
            try
            {
                foreach (DataRow dr in dmBan.Tables[0].Rows)
                {
                    DataRow[] drCt = dmBan.Tables[1].Select("MaBan='" + dr["MaBan"].ToString() + "'");
                    if (drCt.Length == 0)
                    {
                        if (dr["MTPOSID"].ToString() != string.Empty)//Lần trước đã lưu, nhưng sau khi load lại thì chuyển qua bàn khác hoặc xóa đi rồi
                        {
                            //Delete
                            sql = "delete Ctpos where MTPOSID='" + dr["MTPOSID"].ToString().Trim() + "'";
                            _dbData.UpdateByNonQuery(sql);
                            sql = "delete Mtpos where MTPOSID='" + dr["MTPOSID"].ToString().Trim() + "'";
                            _dbData.UpdateByNonQuery(sql);
                        }
                    }
                    else
                    {
                        if (dr["MTPOSID"].ToString() == string.Empty)//Thêm mới
                        {
                            //Insert
                            Guid ID = Guid.NewGuid();
                            sql = "insert into MTPOS(MTPOSID, NgayCT,MaPhong, MaPOSLoaiGia, Tile, TTienH, PtCk, CK, TTien, MaBan, DaTT,ws) values('" +
                                ID.ToString() + "',getdate()," + (dr["MaPhong"]==DBNull.Value? "null":("'" + dr["MaPhong"].ToString() + "'")) + ",'" + dr["MaPOSLoaiGia"].ToString() + "'," + dr["TiLe"].ToString() + "," + dr["TTienH"].ToString() +
                                "," + dr["PtCk"].ToString() + "," + dr["CK"].ToString() + "," + dr["TTien"].ToString() + ",'" + dr["MaBan"].ToString() + "',0,'" + Config.GetValue("sysUserID").ToString() + "')";
                            _dbData.UpdateByNonQuery(sql);
                            foreach (DataRow drct1 in drCt)
                            {
                                sql = "insert into CTPOS(MTPOSID, NgayCT,MaBan, MaMon,SoLuong,DonGia,ThanhTien, DaTT,ws) values('" +
                                ID.ToString() + "',getdate(),'" + drct1["MaBan"].ToString() + "','" + drct1["MaMon"].ToString() +
                                "'," + drct1["SoLuong"].ToString() + "," + drct1["DonGia"].ToString() + "," + drct1["ThanhTien"].ToString() + ",0,'" + Config.GetValue("sysUserID").ToString() + "')";
                                _dbData.UpdateByNonQuery(sql);
                                if (_dbData.HasErrors)
                                {
                                    _dbData.RollbackMultiTrans();
                                    return;
                                }
                            }
                            if (_dbData.HasErrors)
                            {
                                _dbData.RollbackMultiTrans();
                                return;
                            }
                        }
                        else
                        {
                            //Update
                            sql = "Update MTPOS set  ngayct=getdate(), MaPOSLoaiGia='" + dr["MaPOSLoaiGia"].ToString() + "',Tile=" + dr["TiLe"].ToString() + ",TTienH=" +
                                 dr["TTienH"].ToString() + ", MaPhong=" + (dr["MaPhong"] == DBNull.Value ? "null" : ("'" + dr["MaPhong"].ToString() + "'")) + ",PtCk=" + dr["PtCk"].ToString() + ",CK=" + dr["CK"].ToString() + ",TTien=" + dr["TTien"].ToString() +
                                 ",ws='" + Config.GetValue("sysUserID").ToString() + "' where MTPOSID='" + dr["MTPOSID"].ToString() + "'";
                            _dbData.UpdateByNonQuery(sql);
                            sql = "delete CTPOS where MTPOSID='" + dr["MTPOSID"].ToString() + "'";

                            _dbData.UpdateByNonQuery(sql);
                            foreach (DataRow drct1 in drCt)
                            {
                                sql = "insert into CTPOS(MTPOSID, NgayCT,MaBan, MaMon,SoLuong,DonGia,ThanhTien, DaTT,ws) values('" +
                                dr["MTPOSID"].ToString() + "',getdate(),'" + drct1["MaBan"].ToString() + "','" + drct1["MaMon"].ToString() +
                                "'," + drct1["SoLuong"].ToString() + "," + drct1["DonGia"].ToString() + "," + drct1["ThanhTien"].ToString() + ",0,'" + Config.GetValue("sysUserID").ToString() + "')";
                                _dbData.UpdateByNonQuery(sql);
                                if (_dbData.HasErrors)
                                {
                                    _dbData.RollbackMultiTrans();
                                    return;
                                }
                            }
                            if (_dbData.HasErrors)
                            {
                                _dbData.RollbackMultiTrans();
                                return;
                            }
                        }
                    }
                }
                sql = "update MTPOS set DaTT=1, DaTTPhong=1 where MTPOSID='" + curMaster["MTPOSID"].ToString() + "'";
                _dbData.UpdateByNonQuery(sql);
                string MaBan = curMaster["MaBan"].ToString();
                RefreshData();
            }
            catch (Exception ex)
            {
                if (_dbData.HasErrors)
                {
                    MessageBox.Show(ex.Message);
                    _dbData.RollbackMultiTrans();
                    return;
                }
            }
        }
        #endregion

        #region In ấn


        private void btPrint_Click(object sender, EventArgs e)
        {
            tbSave_Click(btChuyen, new EventArgs());
            //RefreshData();
            DataSet dataPrint = GetDataForPrint();
            DevExpress.XtraReports.UI.XtraReport rptTmp = null;
            string path;
            if (Config.GetValue("DuongDanBaoCao") != null)
                path = Config.GetValue("DuongDanBaoCao").ToString() + "\\" + Config.GetValue("Package").ToString() + "\\PosThanhtoan.repx";
            else
                path = Application.StartupPath + "\\Reports\\" + Config.GetValue("Package").ToString() + "\\PosThanhtoan.repx";
            string pathTmp;
            if (Config.GetValue("DuongDanBaoCao") != null)
                pathTmp = Config.GetValue("DuongDanBaoCao").ToString() + "\\" + Config.GetValue("Package").ToString() + "\\PosThanhtoan.repx";
            else
                pathTmp = Application.StartupPath + "\\" + Config.GetValue("Package").ToString() + "\\Reports\\template.repx";
            if (System.IO.File.Exists(path))
                rptTmp = DevExpress.XtraReports.UI.XtraReport.FromFile(path, true);
            else if (System.IO.File.Exists(pathTmp))
                rptTmp = DevExpress.XtraReports.UI.XtraReport.FromFile(pathTmp, true);
            else
                rptTmp = new DevExpress.XtraReports.UI.XtraReport();
            SetVariables(rptTmp);
            rptTmp.ScriptReferences = new string[] { Application.StartupPath + "\\CDTLib.dll" };
            rptTmp.DataSource = dataPrint;
            
            rptTmp.ShowPreview();

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
                    if (value.Contains("/"))
                        xrc.Text = DateTime.Parse(value).ToShortDateString();
                    else
                        xrc.Text = value;
                    xrc = null;
                }
            }
        }
        private DataSet GetDataForPrint()
        {
            string sql = "select Sophieu,Ngayct, MaPhong,MTPOSID,MaBan, Ghichu, MaPOSArea,BanSo,MaPOSLoaiGia,Tile =case when Tile is null then 1.0 else Tile end,TTienH =case when TTienH is null then 0.0 else TTienH end,PtCk =case when PtCk is null then 0.0 else PtCk end,CK =case when CK is null then 0.0 else CK end,TTien =case when TTien is null then 0.0 else TTien end from " +
         "(select a.*,b.Ngayct,b.sophieu,b.maposloaigia,b.tile,b.MaPhong,TTienH, PtCk,Ck, TTien,MTPOSID from dmban a, mtpos b where a.Maban*=b.MaBan and b.Datt=0  and a.Maban='" + curMaster["MaBan"].ToString() + "') x" +
         "; select MTPOSID,MaBan, ctpos.MaMon, TenMon, SoLuong,DonGia,ThanhTien from ctPOS inner join dmMon on ctPOS.MaMon=dmMON.MaMon  where MTPOSID in (select MTPOSID from MTPOS where daTT=0 and MaBan='" + curMaster["MaBan"].ToString() + "')  ";
            DataSet ds = _dbData.GetDataSet(sql);
            return ds;
        }

        private void btSuamau_Click(object sender, EventArgs e)
        {
            tbSave_Click(btChuyen, new EventArgs());
            DataSet dataPrint = GetDataForPrint();
            //RefreshData();
            DevExpress.XtraReports.UI.XtraReport rptTmp = null;
            string path;
            if (Config.GetValue("DuongDanBaoCao") != null)
                path = Config.GetValue("DuongDanBaoCao").ToString() + "\\" + Config.GetValue("Package").ToString() + "\\PosThanhtoan.repx";
            else
                path = Application.StartupPath + "\\Reports\\" + Config.GetValue("Package").ToString() + "\\PosThanhtoan.repx";
            string pathTmp;
            if (Config.GetValue("DuongDanBaoCao") != null)
                pathTmp = Config.GetValue("DuongDanBaoCao").ToString() + "\\" + Config.GetValue("Package").ToString() + "\\PosThanhtoan.repx";
            else
                pathTmp = Application.StartupPath + "\\" + Config.GetValue("Package").ToString() + "\\Reports\\template.repx";
            if (System.IO.File.Exists(path))
                rptTmp = DevExpress.XtraReports.UI.XtraReport.FromFile(path, true);
            else if (System.IO.File.Exists(pathTmp))
                rptTmp = DevExpress.XtraReports.UI.XtraReport.FromFile(pathTmp, true);
            else
                rptTmp = new DevExpress.XtraReports.UI.XtraReport();
            if (rptTmp != null)
            {
                rptTmp.DataSource = dataPrint;
                XRDesignFormEx designForm = new XRDesignFormEx();
                designForm.OpenReport(rptTmp);
                if (System.IO.File.Exists(path))
                    designForm.FileName = path;
                designForm.KeyPreview = true;
                designForm.KeyDown += new KeyEventHandler(designForm_KeyDown);
                designForm.Show();
            }
        }
        void designForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.X)
                (sender as XRDesignFormEx).Close();
        }
        #endregion

        #region  Làm bếp
        private DataTable tbBep = new DataTable();
        private BindingSource bsBep = new BindingSource();
        private BindingSource dmMon1 = new BindingSource();
        private void getDataForBep()
        {
            string sql = "select a.*,b.TenMon from posbep a inner join dmMon b on a.MaMon=b.MaMon where dadu = 0";
            tbBep = _dbData.GetDataTable(sql);

            tbBep.Columns["DaYC"].DefaultValue = 0;
            tbBep.Columns["DaDU"].DefaultValue = 0;
            tbBep.ColumnChanged += new DataColumnChangeEventHandler(tbBep_ColumnChanged);

            bsBep.DataSource = tbBep;

            gridControl3.DataSource = bsBep;

            sql = "select * from dmban";
            repositoryItemGridLookUpEdit3.DataSource = _dbData.GetDataTable(sql);
            sql = "select * from dmMon";
            repositoryItemGridLookUpEdit2.DataSource = _dbData.GetDataTable(sql);
            dmMon1.DataSource=_dbData.GetDataTable(sql);
            gridControl4.DataSource = dmMon1;
            sql = "select * from dmBep";
            repositoryItemGridLookUpEdit4.DataSource = _dbData.GetDataTable(sql);

            repositoryItemGridLookUpEdit2.EditValueChanged += new EventHandler(repositoryItemGridLookUpEdit2_EditValueChanged);
        }

        void repositoryItemGridLookUpEdit2_EditValueChanged(object sender, EventArgs e)
        {
            GridLookUpEdit tmp =(sender as GridLookUpEdit);
            if (tmp.EditValue==null) return;
            int index = tmp.Properties.GetIndexByKeyValue(tmp.EditValue);
            if (index < 0) return;
            DataRow drtmp = (repositoryItemGridLookUpEdit2.DataSource as DataTable).Rows[index];
            (bsBep.Current as DataRowView).Row["TenMon"] = drtmp["TenMon"].ToString();
        }

        void tbBep_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            //Khi đã đáp ứng thì tự tăng số lượng món bên bảng giá
            //Xong rồi thì lưu, remove ra khoi tbBep

            if (e.Column.ColumnName == "DaDU")
            {
                if (e.Row["Soluong"].ToString() == string.Empty || double.Parse(e.Row["Soluong"].ToString()) == 0)
                {
                    return;
                }
                if (e.Row["MaBan"].ToString() != string.Empty)
                {
                    int i = bsBan.Find("MaBan", e.Row["MaBan"].ToString());
                    if (i < 0) return;
                    bsBan.Position = i;
                }
                else
                {
                    return;
                }

                if (e.Row["DaDU"].ToString() == "True")
                {
                    bool Daco = false;
                    foreach (DataRow dr in lstDr)
                    {
                        if (dr["MaMon"].ToString() == e.Row["MaMon"].ToString())
                        {
                            Daco = true;
                            dr["Soluong"] = double.Parse(dr["Soluong"].ToString()) + double.Parse(e.Row["Soluong"].ToString());
                            break;
                        }
                    }
                    if (Daco == false)
                    {
                        DataRow drTable1 = dmBan.Tables[1].NewRow();
                        drTable1["MaBan"] = e.Row["MaBan"].ToString();
                        drTable1["MaMon"] = e.Row["MaMon"].ToString();
                        drTable1["Soluong"] = double.Parse(e.Row["Soluong"].ToString());
                    }

                }
                else
                {
                    foreach (DataRow dr in lstDr)
                    {
                        if (dr["MaMon"].ToString() == e.Row["MaMon"].ToString())
                        {
                            dr["Soluong"] = double.Parse(dr["Soluong"].ToString()) - double.Parse(e.Row["Soluong"].ToString());
                            break;
                        }
                    }
                }
            }
        }

        private DataTable getDataForBepPrint()
        {
            DataTable tbBepPrint = tbBep.Clone();
            DataRow[] lstDrChuaYC = tbBep.Select("DaYC=0 and DaDU=0");
            foreach (DataRow dr in lstDrChuaYC)
            {
                DataRow drTmp = tbBepPrint.NewRow();
                drTmp.ItemArray = dr.ItemArray;
                tbBepPrint.Rows.Add(drTmp);
            }
            return tbBepPrint;
        }
        private void btPrintBep_Click(object sender, EventArgs e)
        {
            DataTable tbBepPrint = getDataForBepPrint();
            DevExpress.XtraReports.UI.XtraReport rptTmp = null;
            string path;
            if (Config.GetValue("DuongDanBaoCao") != null)
                path = Config.GetValue("DuongDanBaoCao").ToString() + "\\" + Config.GetValue("Package").ToString() + "\\POSYeuCau.repx";
            else
                path = Application.StartupPath + "\\Reports\\" + Config.GetValue("Package").ToString() + "\\POSYeuCau.repx";
            string pathTmp;
            if (Config.GetValue("DuongDanBaoCao") != null)
                pathTmp = Config.GetValue("DuongDanBaoCao").ToString() + "\\" + Config.GetValue("Package").ToString() + "\\POSYeuCau.repx";
            else
                pathTmp = Application.StartupPath + "\\" + Config.GetValue("Package").ToString() + "\\Reports\\template.repx";
            if (System.IO.File.Exists(path))
                rptTmp = DevExpress.XtraReports.UI.XtraReport.FromFile(path, true);
            else if (System.IO.File.Exists(pathTmp))
                rptTmp = DevExpress.XtraReports.UI.XtraReport.FromFile(pathTmp, true);
            else
                rptTmp = new DevExpress.XtraReports.UI.XtraReport();
            SetVariables(rptTmp);
            rptTmp.ScriptReferences = new string[] { Application.StartupPath + "\\CDTLib.dll" };
            rptTmp.DataSource = tbBepPrint;
            rptTmp.ShowPreview();
            //Update DaYC
            DataRow[] lstDrChuaYC = tbBep.Select("DaYC=0");
            foreach (DataRow dr in lstDrChuaYC)
            {
                dr["DaYC"] = 1;
            }
        }
        private void btSuaMauYC_Click(object sender, EventArgs e)
        {
            DataTable tbBepPrint = getDataForBepPrint();
            DevExpress.XtraReports.UI.XtraReport rptTmp = null;
            string path;
            if (Config.GetValue("DuongDanBaoCao") != null)
                path = Config.GetValue("DuongDanBaoCao").ToString() + "\\" + Config.GetValue("Package").ToString() + "\\POSYeuCau.repx";
            else
                path = Application.StartupPath + "\\Reports\\" + Config.GetValue("Package").ToString() + "\\POSYeuCau.repx";
            string pathTmp;
            if (Config.GetValue("DuongDanBaoCao") != null)
                pathTmp = Config.GetValue("DuongDanBaoCao").ToString() + "\\" + Config.GetValue("Package").ToString() + "\\POSYeuCau.repx";
            else
                pathTmp = Application.StartupPath + "\\" + Config.GetValue("Package").ToString() + "\\Reports\\template.repx";
            if (System.IO.File.Exists(path))
                rptTmp = DevExpress.XtraReports.UI.XtraReport.FromFile(path, true);
            else if (System.IO.File.Exists(pathTmp))
                rptTmp = DevExpress.XtraReports.UI.XtraReport.FromFile(pathTmp, true);
            else
                rptTmp = new DevExpress.XtraReports.UI.XtraReport();
            if (rptTmp != null)
            {
                rptTmp.DataSource = tbBepPrint;
                XRDesignFormEx designForm = new XRDesignFormEx();
                designForm.OpenReport(rptTmp);
                if (System.IO.File.Exists(path))
                    designForm.FileName = path;
                designForm.KeyPreview = true;
                designForm.KeyDown += new KeyEventHandler(designForm_KeyDown);
                designForm.Show();
            }
        }

        private void btLuu_Click(object sender, EventArgs e)
        {
            DataRow[] lstDrBep = tbBep.Select();
            DataRow[] lstDrBepDaDU = tbBep.Select("DaDU=1");
            _dbData.BeginMultiTrans();
            foreach (DataRow dr in lstDrBep)
            {

                string sql;
                if (dr["Stt"].ToString() == string.Empty)
                {
                    sql = "insert into POSBep (MaBan,MaMon,MaBep,Soluong, DaYC, DaDU) values('" +
                        dr["MaBan"].ToString() + "','" + dr["MaMon"].ToString() + "','" + dr["MaBep"].ToString() + "'," +
                        dr["Soluong"].ToString() + ",";
                    sql += dr["DaYC"].ToString() == "True" ? "1," : "0,";
                    sql += dr["DaDU"].ToString() == "True" ? "1" : "0";
                    sql += ")";
                    _dbData.UpdateByNonQuery(sql);
                    object o = _dbData.GetValue("select @@identity");
                    if (o != null || o.ToString() == string.Empty) dr["Stt"] = int.Parse(o.ToString());

                }
                else
                {
                    sql = "update POSBep set MaBan='" + dr["MaBan"].ToString() + "',MaMon='" + dr["MaMon"].ToString() + "',MaBep='" +
                        dr["MaBep"].ToString() + "',Soluong=" + dr["Soluong"].ToString() + ",DaYC=";
                    sql += dr["DaYC"].ToString() == "True" ? "1,DaDU=" : "0,DaDU=";
                    sql += dr["DaDU"].ToString() == "True" ? "1" : "0";
                    sql += " where Stt=" + dr["Stt"].ToString();
                    _dbData.UpdateByNonQuery(sql);
                }

                if (_dbData.HasErrors)
                {
                    _dbData.RollbackMultiTrans();
                    return;
                }

            }
            _dbData.EndMultiTrans();
            foreach (DataRow dr in lstDrBepDaDU)
            {
                tbBep.Rows.Remove(dr);
            }
        }
        #endregion

        //Thoát
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //int i =int.Parse("sdfsdf");
            tbSave_Click(btChuyen, new EventArgs());
            this.Dispose();
        }

        private void gridControl3_Click(object sender, EventArgs e)
        {

        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }        
    }
}