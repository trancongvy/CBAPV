using System;
using System.Collections.Generic;
using System.Text;
using Plugins;
using System.Windows.Forms;
using System.Data;
using CDTDatabase;
namespace GiaThanhCustomer
{
    class GiaThanhCustomer : ICustomData
    {
        private bool _result = true;
        private StructPara _info;
        #region ICustomData Members
        DataRow _drMater;
        public void ExecuteAfter()
        {
           
        }
        private void CreateTable(string NhomGt)
        {
            string sql = "create table " + NhomGt + "(";
            sql += "[ID] [int] IDENTITY (1, 1) NOT NULL ,";
            sql += "[NgayCT] [smalldatetime] NOT NULL ,";
            sql += "[MaSP] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,";
            sql += "[Soluong] [decimal](20, 6) NULL CONSTRAINT [df_" + NhomGt + "_soluong] DEFAULT ('0'),";
            sql += "[dddk] [decimal](20, 6) NULL CONSTRAINT [df_" + NhomGt + "_dddk] DEFAULT ('0'),";
            sql += "[ddck] [decimal](20, 6) NULL CONSTRAINT [df_" + NhomGt + "_ddck] DEFAULT ('0'),";
            sql += "[Gia] [decimal](20, 6) NULL CONSTRAINT [df_" + NhomGt + "_gia] DEFAULT ('0'),";
            sql += "CONSTRAINT [PK_" + NhomGt + "] PRIMARY KEY  CLUSTERED ([ID])  ON [PRIMARY], ";
            sql += "CONSTRAINT [fk_" + NhomGt + "_dmvt] FOREIGN KEY ([MaSP]) REFERENCES [DMVT] ([MaVT]) ON UPDATE CASCADE ";
            sql += ") ON [PRIMARY]";
            _info.DbData.UpdateByNonQuery(sql);

        }
        private void Deletetable(string NhomGt)
        {
            string sql = "Drop table " + NhomGt;
            _info.DbData.UpdateByNonQuery(sql);
        }
        public void ExecuteBefore()
        {
            //Custom nhóm giá thành : Mỗi nhóm giá thành có 1 bảng lưu riêng
            if (_info.TableName.ToUpper() == "DMNHOMGT")
            {
                try
                {
                    _drMater = _info.DsData.Tables[0].Rows[_info.CurMasterIndex];
                    if (_drMater.RowState == DataRowState.Deleted)
                    {
                        _info.DsData.Tables[0].DefaultView.RowStateFilter = DataViewRowState.Deleted;
                        string Nhomgt = _info.DsData.Tables[0].DefaultView[0]["MaNhom"].ToString();
                        Deletetable("CoGia" + Nhomgt);
                        _info.DsData.Tables[0].DefaultView.RowStateFilter = DataViewRowState.None;

                        //
                    }
                    else if (_drMater.RowState == DataRowState.Added)
                    {
                        string Nhomgt = _drMater["MaNhom"].ToString();
                        CreateTable("CoGia" + Nhomgt);
                    }
                    _result = true;
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    _result = false;
                    return;
                }
            }
            if (_info.TableName.ToUpper() == "CODTGT")
            {
                try
                {
                    string sql;
                    //Thêm 1 yếu tố, tạo 1 cột
                    _info.DsData.Tables[0].DefaultView.RowStateFilter = DataViewRowState.Added;
                    foreach (DataRowView dr in _info.DsData.Tables[0].DefaultView)
                    {
                        sql = " alter table CoGia" + dr["MaNhom"].ToString() + " add " + dr["MaYT"].ToString() ;
                        sql += " [decimal](20, 6) not null  CONSTRAINT [df_CoGia" + dr["MaNhom"].ToString() + "_" +dr["MaYT"].ToString() + "] DEFAULT (0)";
                        _info.DbData.UpdateByNonQuery(sql);
                    }
                    _info.DsData.Tables[0].DefaultView.RowStateFilter = DataViewRowState.Deleted;
                    foreach (DataRowView dr in _info.DsData.Tables[0].DefaultView)
                    {
                        sql = " alter table CoGia" + dr["MaNhom"].ToString() + " drop CONSTRAINT [df_CoGia" + dr["MaNhom"].ToString() + "_" + dr["MaYT"].ToString() + "] \n";
                        sql += " alter table CoGia" + dr["MaNhom"].ToString() + " drop column " + dr["MaYT"].ToString() ;
                        _info.DbData.UpdateByNonQuery(sql);
                    }
                    _info.DsData.Tables[0].DefaultView.RowStateFilter = DataViewRowState.ModifiedOriginal;
                    foreach (DataRowView dr in _info.DsData.Tables[0].DefaultView)
                    {
                        sql = " alter table CoGia" + dr["MaNhom"].ToString() + " drop CONSTRAINT [df_CoGia" + dr["MaNhom"].ToString() + "_" + dr["MaYT"].ToString() + "] \n";
                        sql += " alter table CoGia" + dr["MaNhom"].ToString() + " drop column " + dr["MaYT"].ToString();
                        _info.DbData.UpdateByNonQuery(sql);
                    }
                    _info.DsData.Tables[0].DefaultView.RowStateFilter = DataViewRowState.ModifiedCurrent;
                    foreach (DataRowView dr in _info.DsData.Tables[0].DefaultView)
                    {
                        sql = " alter table CoGia" + dr["MaNhom"].ToString() + " add " + dr["MaYT"].ToString();
                        sql += " [decimal](20, 6) not null  CONSTRAINT [df_CoGia" + dr["MaNhom"].ToString() + "_" + dr["MaYT"].ToString() + "] DEFAULT (0)";
                        _info.DbData.UpdateByNonQuery(sql);
                    }
                    _info.DsData.Tables[0].DefaultView.RowStateFilter = DataViewRowState.None;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    _result = false;
                    return;
                }
                
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

        public bool Result
        {
            //get { throw new Exception("The method or operation is not implemented."); }
            get
            {
                return _result;
            }
        }

        #endregion
    }
}
