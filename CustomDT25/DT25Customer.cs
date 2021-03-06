using System;
using System.Collections.Generic;
using System.Text;
using Plugins;
using System.Windows.Forms;
using System.Data;
using CDTDatabase;
namespace DT25Customer
{
    public class DT25Customer : ICustomData
    {
        #region ICustomData Members
        private bool _result = true;
        private StructPara _info;
        DataRow _drMater;
        DataRowView _drDetail;
        DataRowView _drDetailOrg;
        int _action = 0;
        string mt25id;
        string dt25id;
        public bool Result
        {
            //get { throw new Exception("The method or operation is not implemented."); }
            get
            {
                return _result;
            }
        }

        public StructPara Info
        {
            //set { throw new Exception("The method or operation is not implemented."); }
            set
            {
                _info = value;
            }
        }

        public void ExecuteBefore()
        {

            if (!(_info.TableName == "MT25" || _info.TableName == "MT22" || _info.TableName == "MT23" ))
            {
                _result = true;
                return;
            }
            if ( _info.TableName == "MT22")
            {
                _result = CheckMT22();                 
                return;
                
            }
            if (_info.TableName == "MT23")
            {
                _result = CheckMT23();
                return;

            }
            //if (_info.TableName == "DMKH")
            //{
            //    //MessageBox.Show(_info.TableName);
            //    _result = CheckDMKH();
            //    return;
            //}
            try 
            {
               // MessageBox.Show(_info.TableName);
                _drMater=_info.DsData.Tables[0].Rows[_info.CurMasterIndex];                
                DataView dvDt = new DataView(_info.DsData.Tables[1]);
                DataView dvDtOrg = new DataView(_info.DsData.Tables[1]);
                if (_drMater.RowState==DataRowState.Added)
                {//
                    _action=0;
                    dvDt.RowStateFilter = DataViewRowState.Added;
                    mt25id = _drMater["MT25ID"].ToString();
                    foreach (DataRowView drview in dvDt)
                    {
                        _drDetail = drview;                        
                        if (_drDetail["MT25ID"].ToString() == mt25id)
                        {
                            dt25id=_drDetail["DT25ID"].ToString();
                            UpdateChiPhi();
                        }
                    }
                }
                else if(_drMater.RowState==DataRowState.Modified)
                {
                    mt25id = _drMater["MT25ID"].ToString();
                    
                    foreach (DataRow dr in _info.DsData.Tables[1].Rows)
                    {
                        if (dr.RowState == DataRowState.Deleted)
                        { continue; }
                        if (dr.RowState == DataRowState.Added)
                        {
                            if (dr["MT25ID"].ToString() != mt25id)
                            { continue; }
                            _action = 0;//xem như thêm mới
                            dvDt.RowStateFilter = DataViewRowState.Added;
                            dvDt.Sort = "DT25ID";
                            _drDetail = dvDt.FindRows(dr["DT25ID"])[0];
                            UpdateChiPhi();
                        }
                        else if (dr.RowState == DataRowState.Modified)
                        {
                            if (dr["MT25ID"].ToString() != mt25id)
                            { continue; }
                            _action = 1;//chỉnh sửa
                            dvDt.RowStateFilter = DataViewRowState.ModifiedCurrent;
                            dvDtOrg.RowStateFilter = DataViewRowState.ModifiedOriginal;
                            dvDt.Sort = "DT25ID";
                            dvDtOrg.Sort = "DT25ID";
                            _drDetail = dvDt.FindRows(dr["DT25ID"])[0];
                            _drDetailOrg = dvDtOrg.FindRows(dr["DT25ID"])[0];
                            UpdateChiPhi();
                        }
                        
                    }
                    _action = 2;//Xóa
                    dvDt.RowStateFilter = DataViewRowState.Deleted;
                    foreach (DataRowView drview in dvDt)
                    {
                        _drDetail = drview;
                        if (_drDetail["MT25ID"].ToString() == mt25id)
                        {
                            dt25id = _drDetail["DT25ID"].ToString();
                            UpdateChiPhi();
                        }
                    }

                }
                else if (_drMater.RowState == DataRowState.Deleted)
                {
                    _action = 2;//Xóa
                    _info.DsData.Tables[0].DefaultView.RowStateFilter = DataViewRowState.Deleted;
                    mt25id = _info.DsData.Tables[0].DefaultView[0]["MT25ID"].ToString();
                    dvDt.RowStateFilter = DataViewRowState.Deleted;
                    foreach (DataRowView drview in dvDt)
                    {
                        _drDetail = drview;
                        if (_drDetail["MT25ID"].ToString() == mt25id)
                        {
                            dt25id = _drDetail["DT25ID"].ToString();
                            UpdateChiPhi();
                        }
                    }
                }

                _result= true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                _result=false;
            }
        }
        private void UpdateChiPhi()
        {
            string sql="";
            
            if (_action == 0)
            {
                sql = "update blvt set ";
                sql += " CPNT=CPNT + " + _drDetail["PsNT"].ToString().Replace(",",".") + ",";
                sql += " CP=CP + " + _drDetail["Ps"].ToString().Replace(",", ".") + ",";
                sql += " PsNoNT=PsNoNT + " + _drDetail["PsNT"].ToString().Replace(",", ".") + ",";
                sql += " PsNo=PsNo + " + _drDetail["Ps"].ToString().Replace(",", ".");
                sql += " where MTID=cast('" + _drDetail["MT22ID"].ToString() + "' as uniqueidentifier) and MTIDDT =cast('" + _drDetail["DT22ID"].ToString() + "' as uniqueidentifier)";
                //MessageBox.Show(sql + "  action = " + _action.ToString());
                _info.DbData.UpdateByNonQuery(sql);
            }
            else if (_action == 1)
            {
                
                sql = "update blvt set ";
                sql += " CPNT=CPNT + " + _drDetail["PsNT"].ToString().Replace(",", ".") + ",";
                sql += " CP=CP + " + _drDetail["Ps"].ToString().Replace(",", ".") + ",";
                sql += " PsNoNT=PsNoNT + " + _drDetail["PsNT"].ToString().Replace(",", ".") + ",";
                sql += " PsNo=PsNo + " + _drDetail["Ps"].ToString().Replace(",", ".");
                sql += " where MTID=cast('" + _drDetail["MT22ID"].ToString() + "' as uniqueidentifier) and MTIDDT =cast('" + _drDetail["DT22ID"].ToString() + "' as uniqueidentifier)";
                
                _info.DbData.UpdateByNonQuery(sql);
                //MessageBox.Show(sql + "  action = " + _action.ToString() );
                sql = "update blvt set ";
                sql += " CPNT=CPNT  " + " - " + _drDetailOrg["PsNT"].ToString().Replace(",", ".") + ",";
                sql += " CP=CP  " + " - " + _drDetailOrg["Ps"].ToString().Replace(",", ".") + ",";
                sql += " PsNoNT=PsNoNT  " + " - " + _drDetailOrg["PsNT"].ToString().Replace(",", ".") + ",";
                sql += " PsNo=PsNo  " + " - " + _drDetailOrg["Ps"].ToString().Replace(",", ".");
                sql += " where MTID=cast('" + _drDetailOrg["MT22ID"].ToString() + "' as uniqueidentifier) and MTIDDT =cast('" + _drDetailOrg["DT22ID"].ToString() + "' as uniqueidentifier)";
               // MessageBox.Show(sql + "  action = " + _action.ToString() + "  " + Name);
                _info.DbData.UpdateByNonQuery(sql);
                //sql = "update blvt set dongiaNT=PsNoNT/Soluong, Dongia=PsNo/Soluong ";
                //sql += " where Soluong>0 and MTID=cast('" + _drDetailOrg["MT" + Name + "ID"].ToString() + "' as uniqueidentifier) and MTIDDT =cast('" + _drDetailOrg["DT" + Name + "ID"].ToString() + "' as uniqueidentifier)";
                //_info.DbData.UpdateByNonQuery(sql);
                //MessageBox.Show(sql + "  action = " + _action.ToString());
            }
            else if (_action == 2)
            {
                sql = "update blvt set ";
                sql += " CPNT=CPNT - " + _drDetail["PsNT"].ToString().Replace(",", ".") + ",";
                sql += " CP=CP - " + _drDetail["Ps"].ToString().Replace(",", ".") + ",";
                sql += " PsNoNT=PsNoNT - " + _drDetail["PsNT"].ToString().Replace(",", ".") + ",";
                sql += " PsNo=PsNo - " + _drDetail["Ps"].ToString().Replace(",", ".");
                sql += " where MTID=cast('" + _drDetail["MT22ID"].ToString() + "' as uniqueidentifier) and MTIDDT =cast('" + _drDetail["DT22ID"].ToString() + "' as uniqueidentifier)";
                _info.DbData.UpdateByNonQuery(sql);
                //MessageBox.Show(sql + "  action = " + _action.ToString());
            }
           

            sql = "update blvt set dongia=psno/soluong, dongiaNT=psnoNT/soluong ";
            sql += " where soluong >0  and MTID=cast('" + _drDetail["MT22ID"].ToString() + "' as uniqueidentifier) and MTIDDT =cast('" + _drDetail["DT22ID"].ToString() + "' as uniqueidentifier)";
            _info.DbData.UpdateByNonQuery(sql);
            
            
            
        }
        //Không cho xóa sửa Phiếu mua hàng nếu đã có phân bổ chi phí vào
        bool MT22Action = false;
        private bool CheckMT22()
        {
            string mt22id;
            _drMater = _info.DsData.Tables[0].Rows[_info.CurMasterIndex];
            if (_drMater.RowState == DataRowState.Modified)
            {
                mt22id = _drMater["MT22ID"].ToString();
            }
            else if (_drMater.RowState == DataRowState.Deleted)
            {
                _info.DsData.Tables[0].DefaultView.RowStateFilter = DataViewRowState.Deleted;
                mt22id = _info.DsData.Tables[0].DefaultView[0]["MT22ID"].ToString();
            }
            else
            {
                return true;
            }
            string sql = "select count(*) from DT25 where MT22ID=cast('" + mt22id + "' as uniqueidentifier)";
            DataTable tb= _info.DbData.GetDataTable(sql);
            if (int.Parse(tb.Rows[0][0].ToString()) > 0)
            {
                if (_drMater.RowState == DataRowState.Modified)
                {
                    if (MessageBox.Show("Phiếu đã được phân bổ chi phí!, bạn có muốn sửa không?", "Cảnh báo!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        MT22Action = true;
                        return true;
                    }
                    else { return false; }
                }
                else if (_drMater.RowState == DataRowState.Deleted)
                {
                    MessageBox.Show("Phiếu đã được phân bổ chi phí!Không được xóa!");
                    return false;
                }
                return true;
            }
            else
            {
                return true;
            }
        }

        private bool CheckMT23()
        {
            string mt23id;
            _drMater = _info.DsData.Tables[0].Rows[_info.CurMasterIndex];
            if (_drMater.RowState == DataRowState.Modified)
            {
                mt23id = _drMater["MT23ID"].ToString();
            }
            else if (_drMater.RowState == DataRowState.Deleted)
            {
                _info.DsData.Tables[0].DefaultView.RowStateFilter = DataViewRowState.Deleted;
                mt23id = _info.DsData.Tables[0].DefaultView[0]["MT23ID"].ToString();
            }
            else
            {
                return true;
            }
            string sql = "select count(*) from DT25 where MT22ID=cast('" + mt23id + "' as uniqueidentifier)";
            DataTable tb = _info.DbData.GetDataTable(sql);
            if (int.Parse(tb.Rows[0][0].ToString()) > 0)
            {
                if (_drMater.RowState == DataRowState.Modified)
                {
                    if (MessageBox.Show("Phiếu đã được phân bổ chi phí!, bạn có muốn sửa không?", "Cảnh báo!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        MT22Action = true;
                        return true;
                    }
                    else { return false; }
                }
                else if (_drMater.RowState == DataRowState.Deleted)
                {
                    MessageBox.Show("Phiếu đã được phân bổ chi phí!Không được xóa!");
                    return false;
                }
                return true;
            }
            else
            {
                return true;
            }
        }
        public void ExecuteAfter()
        {
            if (_info.TableName == "MT22"  && MT22Action)
            {
                string mt22id;
                mt22id = _drMater["MT22ID"].ToString();
                string sql = " select * from Dt25 where DT22id in (";
                sql += "select DT22ID from DT22 where MT22ID= cast('" + mt22id + "' as uniqueidentifier))";
                DataTable tb = _info.DbData.GetDataTable(sql);
                foreach (DataRow dr in tb.Rows)
                {
                    sql = "update blvt set ";
                    sql += " CPNT=CPNT + " + dr["PsNT"].ToString().Replace(",", ".") + ",";
                    sql += " CP=CP + " + dr["Ps"].ToString().Replace(",", ".") + ",";
                    sql += " PsNoNT=PsNoNT + " + dr["PsNT"].ToString().Replace(",", ".") + ",";
                    sql += " PsNo=PsNo + " + dr["Ps"].ToString().Replace(",", ".");
                    sql += " where MTID=cast('" + dr["MT22ID"].ToString() + "' as uniqueidentifier) and MTIDDT =cast('" + dr["DT22ID"].ToString() + "' as uniqueidentifier)";
                    //MessageBox.Show(sql + "  action = " + _action.ToString());
                    _info.DbData.UpdateByNonQuery(sql);
                }
                sql = "update blvt set dongia=psno/soluong, dongiaNT=psnoNT/soluong ";
                sql += " where soluong >0  and MTID=cast('" + mt22id + "' as uniqueidentifier) ";
                _info.DbData.UpdateByNonQuery(sql);

                return;
            }
            if (_info.TableName == "MT23" && MT22Action)
            {
                string mt23id;
                mt23id = _drMater["MT23ID"].ToString();
                string sql = " select * from Dt25 where DT22id in (";
                sql += "select DT23ID from DT23 where MT23ID= cast('" + mt23id + "' as uniqueidentifier))";
                DataTable tb = _info.DbData.GetDataTable(sql);
                foreach (DataRow dr in tb.Rows)
                {
                    sql = "update blvt set ";
                    sql += " CPNT=CPNT + " + dr["PsNT"].ToString().Replace(",", ".") + ",";
                    sql += " CP=CP + " + dr["Ps"].ToString().Replace(",", ".") + ",";
                    sql += " PsNoNT=PsNoNT + " + dr["PsNT"].ToString().Replace(",", ".") + ",";
                    sql += " PsNo=PsNo + " + dr["Ps"].ToString().Replace(",", ".");
                    sql += " where MTID=cast('" + dr["MT22ID"].ToString() + "' as uniqueidentifier) and MTIDDT =cast('" + dr["DT22ID"].ToString() + "' as uniqueidentifier)";
                    //MessageBox.Show(sql + "  action = " + _action.ToString());
                    _info.DbData.UpdateByNonQuery(sql);
                }
                sql = "update blvt set dongia=psno/soluong, dongiaNT=psnoNT/soluong ";
                sql += " where soluong >0  and MTID=cast('" + mt23id + "' as uniqueidentifier) ";
                _info.DbData.UpdateByNonQuery(sql);

                return;
            }
        }
        private bool CheckDMKH()
        {
            _drMater=_info.DsData.Tables[0].Rows[_info.CurMasterIndex];
            if (_drMater.RowState != DataRowState.Added) return true;
            if (_drMater["MaKH"] is DBNull && !(_drMater["nhom1"] is DBNull))
            {
                string sql = "select max(Replace(makh,'" + _drMater["nhom1"].ToString().Trim() + "','')) from dmkh where makh like '" + _drMater["nhom1"].ToString().Trim() + "%' and replace(makh,'" + _drMater["nhom1"].ToString().Trim() + "','')<'a'";
                object m = _info.DbData.GetValue(sql);
                if (m != null)
                {
                    try
                    {
                        _drMater["MaKH"] = _drMater["nhom1"].ToString().Trim() + (int.Parse(m.ToString()) + 1).ToString("000");
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                    
                }
                else
                {
                    return false;
                }
            }
            else { return true; }
        }

        #endregion
    }
}

