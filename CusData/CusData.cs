using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using CusCDTData;

using DataFactory;
using System.Windows.Forms;
using DevExpress;
using DevExpress.XtraGrid;
using DevExpress.XtraLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevControls;
using CDTLib;
using CDTControl;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CusData
{
    public partial class publicKH
    {
        public string MST { get; set; }
        public string TenCty { get; set; }
        public string Diachi { get; set; }
        public string Nguoidaidien { get; set; }
        public string Linhvuc { get; set; }
    }
    public class CusData : CusCDTData.ICDTData
    {

        public event EventHandler Refresh;
        CDTData _data;
        DevExpress.XtraGrid.GridControl _gc;
        DevExpress.XtraGrid.Views.Grid.GridView _gv;

        List<DevExpress.XtraLayout.LayoutControlItem> _lo;
        List<DevExpress.XtraEditors.BaseEdit> _be;
        List<CDTGridLookUpEdit> _glist;
        List<CDTRepGridLookup> _rlist;
        List<DevExpress.XtraGrid.GridControl> _gridList;
        bool isAddedEvent = false;      
        string _Name;
        DataTable Mt28Selected;
        void DataTable_i_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {//Tự động nhảy liên trong chi tiết lệnh in
            if (e.Row.Table.TableName == "KTLSX")
            {
                for (int i = 2; i < _data.DsData.Tables.Count; i++)
                {
                    if(_data.DsData.Tables[i].TableName== e.Row.Table.TableName)
                    {
                       int x= _data._lstCurRowDetail.FindAll(m => m.TableName == "KTLSX").Count;
                        e.Row["lien"] = x.ToString();
                    }
                }
            }
        }
        void CusData_ColumnChanged(object sender, System.Data.DataColumnChangeEventArgs e)
        {
            //Kiem tra theo ten truong
            switch (e.Column.ColumnName.ToLower())
            {
                case "mant":
                    MantChanged(e);
                    break;
                case "dalayhd":
                   // DalayHDChange(e);
                    break;
                case "hanhtrinh":
                   // DalayHDChange(e);
                    break;
                
            }
            switch (e.Column.Table.TableName.ToLower())
            {
                case "dmkh":
                    if (e.Column.ColumnName.ToLower() == "makh" && _data.DrCurrentMaster!=null && _data.DrCurrentMaster.RowState==DataRowState.Added)
                    {
                       if( _data.DrCurrentMaster["MaKH"].ToString().Trim().Length==10 || _data.DrCurrentMaster["MaKH"].ToString().Trim().Length == 13 || _data.DrCurrentMaster["MaKH"].ToString().Trim().Length == 14)
                        {
                            publicKH kh = getKH(_data.DrCurrentMaster["MaKH"].ToString().Trim());
                            if (kh != null)
                            {
                                _data.DrCurrentMaster["TenKH"] = kh.TenCty; _data.DrCurrentMaster["Diachi"] = kh.Diachi; _data.DrCurrentMaster["MST"] = kh.MST;
                                _data.DrCurrentMaster.EndEdit();
                            }
                        }
                    }
                    if(e.Column.ColumnName.ToLower()=="mst" && _data.DrCurrentMaster != null)
                    {
                        if (e.Row["MST"] != DBNull.Value && !isMST(e.Row["MST"].ToString()))
                            e.Row.SetColumnError("MST", "Mã số thuế không đúnng");
                    }
                        break;
                case "mt35"://Khải Hoàng
                    if (_data.DrCurrentMaster !=null && e.Column.ColumnName.ToLower() == "layhd")
                    {
                        AplaiGia();
                    }

                    break;
                case "mt38"://Khải Hoàng
                    if (e.Column.ColumnName.ToLower() == "maxe")
                    {
                        UpdateDT38(e);
                    }
                    if (e.Column.ColumnName.ToLower() == "chondon")
                    {
                       List<DataRow> ldr= _data.LstDrCurrentDetails.FindAll(x => x.RowState == DataRowState.Added);
                        if (bool.Parse(e.Row["ChonDon"].ToString()) && ldr.Count==0)
                            InsertAll_Donhang(e);
                    }
                    break;
                case "mt29"://Piriou
                    if (e.Column.ColumnName.ToLower() == "po_no")
                    {
                        Updatedt29PoNo(e);
                    }
                    if (e.Column.ColumnName.ToLower() == "etadate")
                    {
                        Updatedt29ETA(e);
                    }
                     if (e.Column.ColumnName.ToLower() == "puric")
                    {
                        Updatedt29PurIC(e);
                    }
                    break;
                case "mt2a"://Piriou
                    if (e.Column.ColumnName.ToLower() == "ngaycan")
                    {
                        Updatedt2ANgayCan(e);
                    }
                    break;
                //case "mt28"://Piriou
                //    if (e.Column.ColumnName.ToLower() == "ngayct" && e.Row.RowState==DataRowState.Added)
                //    {
                //        e.Row["NgayCT"] =DateTime.Parse(Config.GetValue("NgayHethong").ToString());
                //        e.Row.EndEdit();
                //    }
                //    if (e.Column.ColumnName.ToLower() == "revbom" && e.Row.RowState == DataRowState.Added)
                //    {
                //        e.Row["RevBOM"] ="0";
                //        e.Row.EndEdit();
                //    }
                   
                //    break;

            }
            //ValidMtID(e);
            
        }

        private void CheckTonkho(DataColumnChangeEventArgs e)
        {
            try
            {
                if (e.Column.Table.TableName.ToLower() == "dt32" || e.Column.Table.TableName.ToLower() == "dt43")
                {
                    if (e.Row["MaKho"] != DBNull.Value && e.Row["MaVT"] != DBNull.Value)
                    {
                        double tonkho = _data.DbData.GetValueByStore("Get1TonkhoVT", new string[] { "@MaVT", "MaKho", "@Tonkho" }, new object[] { e.Row["MaVT"].ToString(), e.Row["MaKho"].ToString(), 0 }, new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output }, 2);
                        if (e.Row.Table.Columns.Contains("SlHienTai"))
                        {
                            e.Row["SlHienTai"] = tonkho;
                            e.Row.EndEdit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public publicKH getKH(string makh)
        {
            string url = @"https://www.phanmemsgd.com/api/publicKHs/" + makh;

            string sContentType = "application/json";

            string ob = JsonConvert.SerializeObject(url);
            HttpContent s = new StringContent(ob, Encoding.UTF8, sContentType);

            HttpClient oHttpClient = new HttpClient();
            try
            {
                var oTaskPostAsync = oHttpClient.GetAsync(url);
                if (oTaskPostAsync.Result.StatusCode == HttpStatusCode.BadRequest)
                    return null;
                else
                if (oTaskPostAsync.Result.StatusCode == HttpStatusCode.OK)
                {
                    string s1 = oTaskPostAsync.Result.Content.ReadAsStringAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                    publicKH kh = JsonConvert.DeserializeObject<publicKH>(s1);
                    if (kh != null) return kh;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
        private void ApCongno()
        {
            if (_data.DrCurrentMaster == null || _data.DrCurrentMaster.RowState == DataRowState.Detached || _data.DrCurrentMaster.RowState == DataRowState.Deleted) return;
            object o = _data.DbData.GetValueByStore("Get1CongnoKH", new string[] { "@MaKH", "@congno" }, new object[] { _data.DrCurrentMaster["MaKH"].ToString(),0.0 }, new ParameterDirection[]{ParameterDirection.Input, ParameterDirection.Output},1);
            try
            {
                if (o != null) _data.DrCurrentMaster["Congno"] = double.Parse(o.ToString());
            }
            catch { }

        }

        private void AplaiGia()
        {
            string col="Gia";
            if (_data.DrCurrentMaster["LayHD"] != DBNull.Value && bool.Parse(_data.DrCurrentMaster["LayHD"].ToString()))
                _data.DrCurrentMaster["MaThue"] = "10";
            else
                _data.DrCurrentMaster["MaThue"] = "00";
            try
            {
                _data.DrCurrentMaster.EndEdit();
            }
            catch (Exception e)
            {
               // MessageBox.Show(e.Message);
            }
            foreach (DataRow dr in _data.LstDrCurrentDetails)
            {
                if (_data.DrCurrentMaster["LayHD"] != DBNull.Value && bool.Parse(_data.DrCurrentMaster["LayHD"].ToString()))
                {
                    dr["MaThueCt"] = "10";
                    dr["Thuesuat"] = "10";
                }
                else
                {
                    dr["MaThueCt"] = "00";
                    dr["Thuesuat"] = "00";
                }
                UpdateGiaBan(dr);
            }
        }

        private void InsertAll_Donhang(DataColumnChangeEventArgs e)// Khải hoàn
        {
            string MaCN = "";
            if (_data.DrCurrentMaster["NgayCT"] == DBNull.Value) return;
            string sql="select * from wDonHangChuaGiao where NgayGDK<='" + DateTime.Parse(_data.DrCurrentMaster["NgayCT"].ToString()).ToShortDateString() + "'";
            
            if (_data.DrCurrentMaster["MaCN"] != DBNull.Value)
            {
                MaCN = _data.DrCurrentMaster["MaCN"].ToString();
                sql += " and MaCN='" + MaCN + "'";
            }
            sql += " order by NgayGDK, SoCT";
            DataTable dtTable = _data.DbData.GetDataTable(sql);
            DevExpress.XtraGrid.Views.Grid.GridView gvMain = gc.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
            for (int i = 0; i < dtTable.Rows.Count; i++)
            {
                DataRow drdt = dtTable.Rows[i];
                    gvMain.AddNewRow();

                   
                int j = gvMain.DataRowCount;
                foreach (DataColumn col in dtTable.Columns)
                {
                    if (_data.DsData.Tables[1].Columns.Contains(col.ColumnName))
                    {
                        _data.LstDrCurrentDetails[_data.LstDrCurrentDetails.Count - 1][col.ColumnName] = drdt[col.ColumnName];
                        if (gvMain.Columns[col.ColumnName] == null) continue;
                        {
                            GridColumn g = gvMain.Columns[col.ColumnName];
                            CDTRepGridLookup r = g.ColumnEdit as CDTRepGridLookup;
                            if (r == null) continue;
                            BindingSource bs = r.DataSource as BindingSource;
                            if (!r.Data.FullData)
                            {
                                r.Data.GetData();
                                bs.DataSource = r.Data.DsData.Tables[0];
                                r.DataSource=bs;
                            }
                            
                            DataTable tbRep = bs.DataSource as DataTable;
                            int index = r.GetIndexByKeyValue(drdt[g.FieldName]);
                            
                            DataRow RowSelected =null;
                            if(index>=0) RowSelected= tbRep.Rows[index];
                            if (RowSelected != null)
                            {
                                _data.SetValuesFromListDt(_data.LstDrCurrentDetails[j], g.FieldName, drdt[g.FieldName].ToString(), RowSelected);
                            }
                        }
                    }
                }
                _data.DsData.Tables[1].Rows.Add(_data.LstDrCurrentDetails[_data.LstDrCurrentDetails.Count - 1]);
            }
        }

        private void UpdateDT38(DataColumnChangeEventArgs e)//Chọn Xe giao hàng, Khải hoàn
        {
           if (e.Column.Table.TableName == "MT38")
           {
               DevExpress.XtraGrid.Views.Grid.GridView gvMain = gc.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
               int[] id = gvMain.GetSelectedRows();
               foreach (int i in id)
               {
                   DataRow dr = gvMain.GetDataRow(i);
                   dr["MaXe"] = e.Row["MaXe"];
                   dr.EndEdit();
               }
           }
        }

        private void Updatedt29PoNo(DataColumnChangeEventArgs e)//Piriou
        {
            if (e.Column.Table.TableName == "MT29")
            {
                DevExpress.XtraGrid.Views.Grid.GridView gvMain = gc.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
                int[] id = gvMain.GetSelectedRows();
                foreach (int i in id)
                {
                    DataRow dr = gvMain.GetDataRow(i);
                    dr["PONo"] = e.Row["PO_No"];
                    dr.EndEdit();
                }
            }
        }
        private void Updatedt29ETA(DataColumnChangeEventArgs e)//Piriou
        {
            if (e.Column.Table.TableName == "MT29")
            {
                DevExpress.XtraGrid.Views.Grid.GridView gvMain = gc.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
                int[] id = gvMain.GetSelectedRows();
                foreach (int i in id)
                {
                    DataRow dr = gvMain.GetDataRow(i);
                    dr["ETA"] = e.Row["ETADate"];
                    dr.EndEdit();
                }
            }
        }
        private void Updatedt29PurIC(DataColumnChangeEventArgs e)//Piriou
        {
            if (e.Column.Table.TableName == "MT29")
            {
                DevExpress.XtraGrid.Views.Grid.GridView gvMain = gc.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
                int[] id = gvMain.GetSelectedRows();
                foreach (int i in id)
                {
                    DataRow dr = gvMain.GetDataRow(i);
                    if (dr["PurIC"].ToString() != e.Row["PurIC"].ToString())
                    {
                        dr["PurIC"] = e.Row["PurIC"];
                        dr.EndEdit();
                    }
                }
            }
        }
        private void Updatedt2ANgayCan(DataColumnChangeEventArgs e)//Piriou
        {
            if (e.Column.Table.TableName == "MT2A")
            {
                DevExpress.XtraGrid.Views.Grid.GridView gvMain = gc.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
                int[] id = gvMain.GetSelectedRows();
                foreach (int i in id)
                {
                    DataRow dr = gvMain.GetDataRow(i);
                    if (dr["NgayCan"].ToString() != e.Row["NgayCan"].ToString())
                    {
                        dr["NgayCan"] = e.Row["NgayCan"];
                        dr.EndEdit();
                    }
                }
            }
        }

        void CusData_ColumnChanging(object sender, DataColumnChangeEventArgs e)//All
        {
            if (e.Row.RowState == DataRowState.Deleted || e.Row.RowState == DataRowState.Detached) return;
            switch (e.Column.Table.TableName.ToLower())
            {
                case "mt35"://Khải Hoàng
                    if (e.Column.ColumnName.ToLower() == "dadpxe")
                    {
                        string sql = "select count(*) from MT3A where MT35ID='" + e.Row["MT35ID"].ToString() + "' and Approved<>-1";
                        object o = _data.DbData.GetValue(sql);
                        if (o != null && int.Parse(o.ToString()) > 0)
                        {
                            e.ProposedValue = e.Row[e.Column];
                            e.Row.EndEdit();
                            return;
                        }
                    }

                    break;
            }
          DataRow[] ldr=  _data.DsStruct.Tables[0].Select("QueryInsertDt is not null");
            if(ldr.Length==0) return;
            foreach (DataRow dr in ldr)
            {
                if (e.Column.ColumnName == dr["FieldName"].ToString())
                {
                    if (e.ProposedValue.ToString() != e.Row[e.Column.ColumnName].ToString())
                    ValidMtID(e.Column, e.ProposedValue.ToString());
                }
            }
               // if (e.ProposedValue.ToString() != e.Row[e.Column.ColumnName].ToString())
                  //  ValidMtID(e.Column, e.ProposedValue.ToString());
            
        }
        

        
        private bool isRefreshed = false;
        private void ValidMtID(DataColumn colMT, string value)
        {
            string fieldName = colMT.ColumnName;

            foreach (CDTGridLookUpEdit tmp in _glist)
                if (tmp.Properties.Buttons[0].Tag != null)
                {
                    if ((tmp.Properties.Buttons[0].Tag as DataRow)["QueryInsertDt"].ToString() == string.Empty) continue;
                    if (!this.isRefreshed)
                    {
                        this.Refresh(this, new EventArgs());
                        this.isRefreshed = true;
                    }
                    if (fieldName.ToLower() == (tmp.Properties.Buttons[0].Tag as DataRow)["FieldName"].ToString().ToLower())
                    {
                        try
                        {
                            string query = (tmp.Properties.Buttons[0].Tag as DataRow)["QueryInsertDt"].ToString();
                            //xoa cac dong da nhap
                            DevExpress.XtraGrid.Views.Grid.GridView gvMain = gc.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
                            if (colMT.Table.TableName == "MT29" && fieldName == "MT2AID") //Piriou 
                            {
                                gvMain.SelectAll();
                                gvMain.DeleteSelectedRows();
                                //gvMain.ClearSelection();
                                _data.LstDrCurrentDetails.Clear();
                            }
                            
                            if (value == string.Empty) continue;
                            DataTable dtTable;
                            query = query.Replace("@" + fieldName, value);
                            dtTable = _data.DbData.GetDataTable(query);
                            //Xác định khóa ngoại
                            DataRow[] RelaRow = _data.DsStruct.Tables[1].Select("refTable='" + _data.DrTableMaster["TableName"].ToString() + "'");
                            string RelaCol = _data.DrTableMaster["Pk"].ToString();
                            if (RelaRow.Length > 0) RelaCol = RelaRow[0]["FieldName"].ToString();
                            for (   int i = 0; i < dtTable.Rows.Count; i++)
                            {
                                DataRow drdt = dtTable.Rows[i];

                                DataRow drNew = _data.DsData.Tables[1].NewRow();                                
                                drNew[RelaCol] = _data.DrCurrentMaster[_data.DrTableMaster["Pk"].ToString()];
                                
                                gvMain.RefreshData();
                                //int j = gvMain.DataRowCount;
                                foreach (DataColumn col in dtTable.Columns)
                                {
                                    if (_data.DsData.Tables[1].Columns.Contains(col.ColumnName))
                                    {
                                        drNew[col.ColumnName] = drdt[col.ColumnName];
                                        if (gvMain.Columns[col.ColumnName] == null) continue;
                                        {
                                            GridColumn g = gvMain.Columns[col.ColumnName];
                                            CDTRepGridLookup r = g.ColumnEdit as CDTRepGridLookup;
                                            if (r == null) continue;
                                            BindingSource bs = r.DataSource as BindingSource;
                                            if (!r.Data.FullData)
                                            {
                                                r.Data.GetData();
                                                bs.DataSource = r.Data.DsData.Tables[0];
                                                r.DataSource = bs;
                                            }


                                            //DataTable tbRep = bs.DataSource as DataTable;
                                            //int index = r.GetIndexByKeyValue(drdt[g.FieldName]);
                                            //if (index == -1) continue;
                                            //DataRow RowSelected = tbRep.Rows[index];
                                            //if (RowSelected != null)
                                            //{
                                            //    //_data.SetValuesFromListDt(drNew, g.FieldName, drdt[g.FieldName].ToString(), RowSelected);
                                            //}
                                        }
                                    }
                                }
                                _data.DsData.Tables[1].Rows.Add(drNew);
                                //_data.DsData.Tables[1].Rows.Add(_data.LstDrCurrentDetails[_data.LstDrCurrentDetails.Count - 1]);
                            }
                            // gvMain.AddNewRow();
                            //if (colMT.Table.TableName == "MTLSX" && fieldName == "MT35ID") //Bến Thành
                            //{
                            //    foreach (DataRow m in _data.LstDrCurrentDetails)
                            //    {
                            //        if (m["MT35ID"].ToString() != value)
                            //        {
                            //            m["MT35ID"] = DBNull.Value;
                            //            m["DT35ID"] = DBNull.Value;
                            //            m.EndEdit();
                            //        }
                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }
                }
        }


        private void refreshLookup()
        {
            for (int i = 0; i < _gv.Columns.Count; i++)
            {
                GridColumn g = _gv.Columns[i];
                CDTRepGridLookup r = (g.ColumnEdit as CDTRepGridLookup);
                if (r != null)
                    if (r.Tag != null)
                        if (r.Tag.ToString() != string.Empty)
                        {
                            BindingSource bstmp = r.DataSource as BindingSource;
                            CDTData rdatatmp = r.Buttons[1].Tag as CDTData;
                            if (!rdatatmp.FullData)
                            {
                                rdatatmp.GetData();
                                bstmp.DataSource = rdatatmp.DsData.Tables[0];
                                r.DataSource = bstmp;
                                rdatatmp.FullData = true;

                                for (int j = i + 1; j < _gv.Columns.Count; j++)
                                {
                                    if (_gv.Columns[j].FieldName.ToLower() == g.FieldName.ToLower())
                                    {
                                        CDTRepGridLookup rj = (_gv.Columns[j].ColumnEdit as CDTRepGridLookup);
                                        BindingSource bsj = rj.DataSource as BindingSource;
                                        bsj.DataSource = rdatatmp.DsData.Tables[0];
                                    }
                                }
                            }
                        }

            }
        }


        private void MantChanged(System.Data.DataColumnChangeEventArgs e)
        {
            foreach (DevExpress.XtraGrid.Columns.GridColumn gcol in _gv.Columns)
            {
                if (gcol.Caption.Contains("HT") && e.Row[e.Column.ColumnName].ToString().Trim() == "VND")
                {
                    gcol.Visible = false;

                }
                if (gcol.Caption.Contains("HT") && e.Row[e.Column.ColumnName].ToString().Trim() != "VND")
                {
                    gcol.Visible = true;
                }
            }
            foreach (LayoutControlItem l in lo)
            {
                if (l.Text.Contains("HT") && e.Row[e.Column.ColumnName].ToString().Trim() == "VND")
                    l.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                if (l.Text.Contains("HT") && e.Row[e.Column.ColumnName].ToString().Trim() != "VND")
                    l.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            _gc.Refresh();

        }
       private bool isMST(string MST)
        {
            if (MST.Length != 10 & MST.Length != 13 && MST.Length != 14)
                return false;
            string strTaxCode = MST.Substring(0, 10);
            if (!IsNumeric(strTaxCode))
                return false;
            if (MST.Length == 13 || MST.Length == 14)
            {
                string subTaxt = MST.Substring(MST.Length - 3, 3);
                if (!IsNumeric(subTaxt))
                    return false;
            }
            int[] ArrCheck = { 31, 29, 23, 19, 17, 13, 7, 5, 3 };
            int CheckNumber = 0;
            try
            {
                for (int i = 0; i < strTaxCode.Length - 1; i++)
                    CheckNumber += Convert.ToInt32(strTaxCode.Substring(i, 1)) * ArrCheck[i];
                if (Convert.ToDouble(strTaxCode.Substring(9, 1)) == (10 - CheckNumber % 11))
                    return true;
                else
                    return false;
            }
            catch { return false; }
        }
        private bool IsNumeric(string pCheck)
        {
            try
            {
                double Check;
                return double.TryParse(pCheck, out Check);
            }
            catch (Exception)
            {
                return false;
            }
        }
        #region ICDTData Members

        public void AddEvent()
        {
            //if (isAddedEvent) return;
            _data.DsData.Tables[0].ColumnChanged-=new System.Data.DataColumnChangeEventHandler(CusData_ColumnChanged);
            _data.DsData.Tables[0].ColumnChanged += new System.Data.DataColumnChangeEventHandler(CusData_ColumnChanged);
            _data.DsData.Tables[0].ColumnChanging -= new DataColumnChangeEventHandler(CusData_ColumnChanging);
            _data.DsData.Tables[0].ColumnChanging += new DataColumnChangeEventHandler(CusData_ColumnChanging);
            
            if (_data.DsData.Tables.Count > 1)
            {
                _data.DsData.Tables[1].ColumnChanged -= new DataColumnChangeEventHandler(CusData1_ColumnChanged);
                _data.DsData.Tables[1].ColumnChanging -= new DataColumnChangeEventHandler(CusData1_ColumnChanging);
                _data.DsData.Tables[1].ColumnChanged += new DataColumnChangeEventHandler(CusData1_ColumnChanged);
                _data.DsData.Tables[1].ColumnChanging += new DataColumnChangeEventHandler(CusData1_ColumnChanging);
            }
            for (int i = 2; i < _data.DsData.Tables.Count; i++)
            {
                _data.DsData.Tables[i].TableNewRow += new DataTableNewRowEventHandler(this.DataTable_i_TableNewRow);
            }
                foreach (CDTGridLookUpEdit gl in _glist)
                gl.Validated += new EventHandler( gl_Validated);
            if (_data.dataType == DataType.MasterDetail)
            {
                
                (_data as DataMasterDetail).JustDoaction += new EventHandler( CusData_JustDoaction);
            }
            //isAddedEvent = true;
        }

        private void CusData_JustDoaction(object sender, EventArgs e)
        {
            DataRow drAction = sender as DataRow;
            //if (drAction != null)
            //    MessageBox.Show(drAction["CommandName"].ToString());
            //Chèn code vào đây 
        }

        void gl_Validated(object sender, EventArgs e)
        {
            if (_data.DrTableMaster !=null && _data.DrTableMaster["TableName"].ToString().ToLower() == "mt35")
            {
                CDTGridLookUpEdit gl = sender as CDTGridLookUpEdit;
                if (gl == null && gl.fieldName!="MaKH") return;
                ApCongno();
                
            }
        }
        
        private void CusData1_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            //if (e.Column.Table.TableName == "DT32" && e.Column.ColumnName == "MaVT")//Lấy đơn giá theo báo giá tháng
            //{try
            //    {
            //    string mavt = e.Row["MaVT"].ToString();
            //    string makh = _data.DrCurrentMaster["MaKH"].ToString();
            //        DateTime ngayct = DateTime.Parse(_data.DrCurrentMaster["Ngayct"].ToString());
            //        //Đơn giá lây theo bản CBABANHANG 
            //        // string sql = "select top(1) DonGia from MT38 a inner join Dt38 b on a.MT38ID=b.MT38ID where b.mavt='" + mavt + "' and a.makh='" + makh + "' and month(a.NgayCT) = " + ngayct.Month.ToString() + " and YEAR(ngayct) = " + ngayct.Year.ToString();
            //        //Đơn giá lấy theo bản  CBABPM - CBA BTP
            //        string sql = "select top(1) GiaBan as DonGia from MT39 a inner join Dt39 b on a.MT39ID=b.MT39ID where b.mavt='" + mavt + "' and '" + ngayct.ToString() + "' between a.ngayct and a.ngayEx";
            //        object value = _data.DbData.GetValue(sql);

            //        if (value != null)
            //        {
            //            //Đơn giá lây theo bản CBABANHANG 
            //            // e.Row["Dongia"] = double.Parse(value.ToString());
            //            //Đơn giá lấy theo bản  CBABPM - CBA BTP
            //            e.Row["GiaBan"] = double.Parse(value.ToString());
            //            e.Row.EndEdit();

            //        }
            //        else
            //        {
            //           // e.Row["Dongia"] = 0;
            //            e.Row["GiaNT"] = 0;
            //            e.Row.EndEdit();
            //        }
            //    }
            //    catch { }
            //}
            if (e.Column.ColumnName.ToLower() == "mavt")
            {
                CheckTonkho(e);
            }
            
            if (e.Column.Table.TableName == "DT47")//Piriou //Update so luong, ngay phai tra trong phan muon cong cu 
            {

                if (e.Column.ColumnName == "SLTra" || e.Column.ColumnName == "SlSua")
                {
                    e.Row["NgayTra"] = DateTime.Today;
                }
                if (e.Column.ColumnName == "Hanmuon" || e.Column.ColumnName == "NgayNhan")
                {
                    try
                    {
                        DateTime ngaynhan = DateTime.Parse(e.Row["NgayNhan"].ToString());
                        e.Row["NgayPhaiTra"] = ngaynhan.AddDays(double.Parse(e.Row["Hanmuon"].ToString()));
                    }
                    catch { }
                }
            }
            if (e.Column.Table.TableName == "DT35")//Update gia ban  Khai Hoan
            {

                if (e.Column.ColumnName == "MaVT" && e.Row["MaVT"] != DBNull.Value)
                {
                    UpdateGiaBan(e.Row);
                    UpdateTonkho(e.Row);
                }
                if (e.Column.ColumnName == "KM")
                {
                    if (bool.Parse(e.Row["KM"].ToString()))
                        e.Row["GiaBan"] = 0;
                }
            }
           

        }

        private void UpdateTonkho(DataRow dataRow)
        {
            if (_data.DrCurrentMaster == null || _data.DrCurrentMaster.RowState == DataRowState.Detached || _data.DrCurrentMaster.RowState == DataRowState.Deleted) return;
            object o = _data.DbData.GetValueByStore("Get1TonkhoVT", new string[] { "@Mavt", "@MaKho", "@Tonkho" }, new object[] { dataRow["MaVT"].ToString(), _data.DrCurrentMaster["Kho"].ToString(), 0 }, new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output }, 2);
            if (o != null)dataRow["Tonkho"] = double.Parse(o.ToString());
        }

        private void UpdateGiaBan(DataRow dr)//Khải hoàng
        {
            string sql = "Select * from wGiaBan where (NgayEX is null or NgayEX>'" + _data.DrCurrentMaster["NgayCT"].ToString() + "') and MaVT='" + dr["MaVT"].ToString() + "' and (MaKH='" + _data.DrCurrentMaster["MaKH"].ToString() + "' or MaKH is null) order by MaKH desc, NgayEx desc";

            DataTable dbgia = _data.DbData.GetDataTable(sql);
            decimal gia = 0;
            if (bool.Parse(dr["KM"].ToString()))
            {
                return;
            }
            if (dbgia.Rows.Count > 0)
            {
                try
                {
                    foreach (DataRow drl in dbgia.Rows)
                    {
                        if (DateTime.Parse(_data.DrCurrentMaster["NgayCT"].ToString()) > DateTime.Parse(drl["NgayCT"].ToString()))
                            if (drl["NgayEX"] == DBNull.Value || (drl["NgayEX"] != DBNull.Value && DateTime.Parse(_data.DrCurrentMaster["NgayCT"].ToString()) < DateTime.Parse(drl["NgayEX"].ToString())))
                            {
                                string col = "Gia";
                                if (_data.DrCurrentMaster["LayHD"] != DBNull.Value && bool.Parse(_data.DrCurrentMaster["LayHD"].ToString()))
                                    col += "LayHD";
                                if (drl[col] != DBNull.Value)
                                {
                                    gia = decimal.Parse(drl[col].ToString());
                                    dr["GiaBan"] = gia;
                                    dr["GiaAD"] = gia;
                                    break;
                                }
                            }
                    }
                }
                catch { }
            }
        }

        private void CusData1_ColumnChanging(object sender, DataColumnChangeEventArgs e)
        {
            if (e.Column.Table.TableName == "DTLSX" && e.Column.ColumnName == "DTLSXDM")
            {
                if (e.Row["DTLSXID"] == DBNull.Value) e.Row.EndEdit();
                if (e.Row["DTLSXID"] == DBNull.Value) return;
                if (e.Row["DTLSXDM"] == DBNull.Value) return;
                string DTLSXID = e.Row["DTLSXDM"].ToString();
                string sql = "select * from CTLSX where DTID='" + DTLSXID + "'";
                DataTable tmpCt = _data.DbData.GetDataTable(sql);
                if (tmpCt == null) return;

                DevExpress.XtraGrid.Views.Grid.GridView gv1;//= (gc.ViewCollection[1] as DevExpress.XtraGrid.Views.Grid.GridView);
                //gv1 = gc.LevelTree.Nodes[0].LevelTemplate as DevExpress.XtraGrid.Views.Grid.GridView;
                DevExpress.XtraGrid.Views.Grid.GridView gvMain = gc.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
                gvMain.SetMasterRowExpanded(gvMain.FocusedRowHandle, "KTLSX1", true);
                gvMain.SetMasterRowExpanded(gvMain.FocusedRowHandle, "CTLSX1", true);
                gv1 = gvMain.GetDetailView(gvMain.FocusedRowHandle, 0) as DevExpress.XtraGrid.Views.Grid.GridView;
                if (gv1 != null)
                {
                    List<CDTControl.CurrentRowDt> lRowDT = _data._lstCurRowDetail.FindAll(m => m.TableName == "CTLSX" && m.RowDetail["DTID"] == e.Row["DTLSXID"]);
                    gv1.SelectAll(); gv1.DeleteSelectedRows(); 

                    
                    foreach(CDTControl.CurrentRowDt RowDT in lRowDT)
                    {
                        _data._lstCurRowDetail.Remove(RowDT);
                    }

                    //_data._lstCurRowDetail.Clear();
                    foreach (DataRow drCT in tmpCt.Rows)
                    {

                        gv1.AddNewRow();
                        CDTControl.CurrentRowDt lstRow = _data._lstCurRowDetail.FindLast(m => m.TableName == "CTLSX");
                        DataRow drcur = lstRow.RowDetail;
                        //drcur["MTID"] = e.Row["MTLSXID"]; drcur["DTID"] = e.Row["DTLSXID"];
                        foreach (DataColumn col in tmpCt.Columns)
                        {
                            if (col.ColumnName == "MTID" || col.ColumnName == "DTID" || col.ColumnName == "CTLSXID") continue;
                            drcur[col.ColumnName] = drCT[col.ColumnName];
                        }
                        drcur.EndEdit();
                    }
                }
                sql = "select * from KTLSX where DTID='" + DTLSXID + "'";
                DataTable tmpKt = _data.DbData.GetDataTable(sql);
                if (tmpKt == null) return;
                DevExpress.XtraGrid.Views.Grid.GridView gv2;// = (gc.ViewCollection[2] as DevExpress.XtraGrid.Views.Grid.GridView);
                gv2 = gvMain.GetDetailView(gvMain.FocusedRowHandle, 1) as DevExpress.XtraGrid.Views.Grid.GridView;
                if (gv2 != null)
                {
                    
                    List<CDTControl.CurrentRowDt> lRowDT = _data._lstCurRowDetail.FindAll(m => m.TableName == "KTLSX" && m.RowDetail["DTID"] == e.Row["DTLSXID"]);

                    gv2.SelectAll(); gv2.DeleteSelectedRows();
                    foreach (CDTControl.CurrentRowDt RowDT in lRowDT)
                    {
                        _data._lstCurRowDetail.Remove(RowDT);
                    }

                    foreach (DataRow drCT in tmpKt.Rows)
                    {
                        //gc.ViewCollection
                        gv2.AddNewRow();
                        CDTControl.CurrentRowDt lstRow = _data._lstCurRowDetail.FindLast(m => m.TableName == "KTLSX");
                        DataRow drcur = lstRow.RowDetail;
                        // drcur["MTID"] = e.Row["MTLSXID"]; drcur["DTID"] = e.Row["DTLSXID"];

                        foreach (DataColumn col in tmpKt.Columns)
                        {
                            if (col.ColumnName == "MTID" || col.ColumnName == "DTID" || col.ColumnName == "KTLSXID" || col.ColumnName == "Lien") continue;
                            drcur[col.ColumnName] = drCT[col.ColumnName];
                        }
                        drcur.EndEdit();
                    }
                }
            }
        }



        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }


        public List<BaseEdit> be
        {
            get
            {
                return _be;
            }
            set
            {
                _be = value;
            }
        }

        public CDTData data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        public GridControl gc
        {
            get
            {
                return _gc;
            }
            set
            {
                _gc = value;
            }
        }
        public List<GridControl> gridList
        {
            get
            {
                return _gridList;
            }
            set
            {
                _gridList = value;
            }
        }
        public List<CDTGridLookUpEdit> glist
        {
            get
            {
                return _glist;
            }
            set
            {
                _glist = value;
            }
        }

        public DevExpress.XtraGrid.Views.Grid.GridView gv
        {
            get
            {
                return _gv;
            }
            set
            {
                _gv = value;
            }
        }

        public List<LayoutControlItem> lo
        {
            get
            {
                return _lo;
            }
            set
            {
                _lo = value;
            }
        }

        public List<CDTRepGridLookup> rlist
        {
            get
            {
                return _rlist;
            }
            set
            {
                _rlist = value;
            }
        }

        #endregion
    }
}

