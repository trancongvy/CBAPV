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
namespace FO
{
    class DataCheckOut
    {
        private Database _dbData = Database.NewDataDatabase();
        private Database _dbStruct = Database.NewStructDatabase();
        public DataRow mt;
        public DataTable dt;
        public DataSet ds;
        public List<DataCheckOut1Room> lChkOut = new List<DataCheckOut1Room>();
        public DataTable dmVT;
        public DataTable dmDV;
        public DataTable dmnt;
        public DataTable tLoaigia;
        private object NgayDiOld;
        //For Print
        private DataTable dmThueSuat;
        object ngayht;
        public DataCheckOut(string MT62ID)
        {
            GetData4Rep();
            string sql = "select getdate()";
            ngayht = _dbData.GetValue(sql);
             sql = "select * from mt62 where mt62id='" + MT62ID + "' ; select * from dt62 where mt62id='" + MT62ID + "' and isCheckIn=1";
            ds = _dbData.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 0) return;
            ds.Tables[0].ColumnChanged += new DataColumnChangeEventHandler(DataCheckOut_ColumnChanged);
            mt = ds.Tables[0].Rows[0];
            NgayDiOld = mt["NgayDi"];
           // mt["NgayDi"] = DateTime.Now;
            dt = ds.Tables[1];
            dt.TableNewRow += new DataTableNewRowEventHandler(dt_TableNewRow);

            dt.PrimaryKey = new DataColumn[] { dt.Columns["DT62ID"] };
            foreach (DataRow dr in dt.Rows)
            {
                DataCheckOut1Room dtChkOut = new DataCheckOut1Room(dr,_dbData);
                dtChkOut.ThueSuat = double.Parse(mt["Thuesuat"].ToString());
                dtChkOut.setMinibarData();
               // dtChkOut.dt["NgayDi"] = DateTime.Now;
                dr["NgayDi"] = dtChkOut.dt["NgayDi"];
                dr["SoNgay"] = dtChkOut.dt["SoNgay"];
                dr["MaGia"] = dtChkOut.dt["MaGia"];
                dr["GiaPhong"] = dtChkOut.dt["GiaPhong"];
                dr["SoNT"] = dtChkOut.dt["SoNT"];
                dr["Ps"] = dtChkOut.dt["Ps"];
                lChkOut.Add(dtChkOut);
                dtChkOut.TTienChanged += new EventHandler(dtChkOut_TTienChanged);
            }
            
            TinhTongTien();

        }



        void DataCheckOut_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            try
            {
                
               

                if (e.Column.ColumnName == "Thuesuat")
                {
                    foreach (DataCheckOut1Room dtchkOut in lChkOut)
                    {
                        dtchkOut.ThueSuat = double.Parse(mt["Thuesuat"].ToString());
                        dtchkOut.minibar.Columns["MaThue"].DefaultValue = mt["MaThue"].ToString();
                        dtchkOut.minibar.Columns["ThueSuat"].DefaultValue = double.Parse(mt["Thuesuat"].ToString());
                        foreach (DataRow dr in dtchkOut.minibar.Rows)
                        {
                            dr["Thuesuat"] = double.Parse(mt["Thuesuat"].ToString());
                            dr["MaThue"] = mt["MaThue"].ToString();
                        }
                        dtchkOut.TinhTienPhong();
                    }
                    TinhTongTien();
                }
                else if (e.Column.ColumnName == "PtCk")
                {
                     mt["CK"] = Math.Round(double.Parse(mt["TTienH"].ToString()) * double.Parse(mt["PtCk"].ToString()) / 100, 0);
                }
                else if (e.Column.ColumnName == "CK")
                {
                    mt["PtCk"] = 0;
                    mt["TThue"] = Math.Round(((double.Parse(mt["TTienH"].ToString()) - double.Parse(mt["CK"].ToString())) * double.Parse(mt["ThueSuat"].ToString()) / 100), 0);
                    mt["TTien"] = double.Parse(mt["TTienH"].ToString()) + double.Parse(mt["TThue"].ToString()) - double.Parse(mt["CK"].ToString());
                    mt["TongTien"] = double.Parse(mt["TTienMNB"].ToString()) + double.Parse(mt["TTienDV"].ToString()) + double.Parse(mt["TThueMNB"].ToString()) + double.Parse(mt["TThueDV"].ToString()) + double.Parse(mt["TTien"].ToString());
                    mt["Conlai"] = double.Parse(mt["TongTien"].ToString()) - double.Parse(mt["DatCoc"].ToString());

                }
                else if (e.Column.ColumnName == "MaThue")
                {
                    DataRow[] ldr = dmThueSuat.Select("MaThue='" + e.Row[e.Column.ColumnName].ToString() + "'");
                    if (ldr.Length == 1) e.Row["Thuesuat"] = ldr[0]["Thuesuat"];
                }
                else if (e.Column.ColumnName == "NgayDi")
                {
                    setLoaiGiaAll();
                    foreach (DataCheckOut1Room dtchkOut in lChkOut)
                    {
                        dtchkOut.dt["NgayDi"] = e.Row["NgayDi"];
                    }
                }
                else if (e.Column.ColumnName == "MaNT")
                {
                    if (e.Row["MaNT"] != null)
                    {
                        DataRow dr = dmnt.Rows.Find(e.Row["MaNT"].ToString());
                        if (dr != null)
                        {
                            e.Row["TyGia"] = dr["TyGia"];
                        }
                    }
                }
                else if (e.Column.ColumnName == "Datcoc")
                {
                    mt["Conlai"] = double.Parse(mt["TongTien"].ToString()) - double.Parse(mt["Datcoc"].ToString());
                }
                e.Row.EndEdit();
            }
            catch { }
        }

        void dtChkOut_TTienChanged(object sender, EventArgs e)
        {
            TinhTongTien();
        }
        private void setLoaiGiaAll()
        {

            if (mt["NgayDi"] == null || mt["NgayDen"] == null) return;
            TimeSpan tsp = DateTime.Parse(mt["NgayDi"].ToString()) - DateTime.Parse(mt["NgayDen"].ToString());

            if (tLoaigia.Rows.Count < 2) return;
            if (double.Parse(tLoaigia.Rows[1]["Muctinh"].ToString()) < tsp.TotalDays)
            {
                mt["SoNgay"] = Math.Ceiling(tsp.TotalDays); //Cái này tính phức tạp dựa vào giờ vào và ra.
            }
            else
            {
                mt["SoNgay"] = Math.Ceiling(tsp.TotalDays * 24) / 24.0;
            }
            DataRow findDr = tLoaigia.Rows[0];
            foreach (DataRow dr in tLoaigia.Rows)
            {
                if (tsp.TotalDays > double.Parse(dr["MucTinh"].ToString()))
                {
                    findDr = dr;
                }
                else
                {
                    break;
                }
            }
            if (findDr == null) return;
            mt["MaLoaiGia"] = findDr["MaLoaiGia"];
        }
        private void TinhTongTien()
        {
            double TTienH = 0;//Tiền phòng
            double TThue = 0;//Thuế tiền phòng
            double CK = 0;
            double PtCk = double.Parse(mt["PtCk"].ToString());
            double TTien = 0;//Tiền phòng
            bool CkInRoom = mt["CKinRoom"]==DBNull.Value? false : bool.Parse(mt["CKinRoom"].ToString());
            double TTienDV = 0;
            double TTienMNB = 0;
            double TThueDV = 0;
            double TThueMNB = 0;
            double TongTien = 0;
            double Conlai = 0;
            
            try
            {
                foreach (DataCheckOut1Room dtChkOut in lChkOut)
                {
                    TTienMNB += double.Parse(dtChkOut.dt["TTienMNB"].ToString());
                    TThueMNB += double.Parse(dtChkOut.dt["TThueMNB"].ToString());
                    TTienDV += double.Parse(dtChkOut.dt["TTienDV"].ToString());
                    TThueDV += double.Parse(dtChkOut.dt["TThueDV"].ToString());
                    TTienH += double.Parse(dtChkOut.dt["ps"].ToString());
                }
                if (PtCk != 0 && !CkInRoom)
                    CK = Math.Round(TTienH * PtCk / 100, 0);
                else
                    CK = double.Parse(mt["CK"].ToString());
                TThue =  Math.Round(((TTienH-CK) * double.Parse(mt["ThueSuat"].ToString()) / 100),0);
                TTien = TTienH + TThue-CK;
                TongTien = TTienMNB + TTienDV + TThueMNB + TThueDV + TTien ;
                Conlai = TongTien - double.Parse(mt["DatCoc"].ToString());
            }
            catch { }
            mt["TTienH"] = TTienH;
            mt["CK"]=CK;
            mt["TTien"] = TTien;
            mt["TThue"] = TThue;
            mt["TTienMNB"] = TTienMNB;
            mt["TThueMNB"] = TThueMNB;
            mt["TTienDV"] = TTienDV;
            mt["TThueDV"] = TThueDV;
            mt["TongTien"] = TongTien;
            mt["ConLai"] = Conlai;

        }

        public void GetData4Rep()
        {
            string sql = "select MaVT, TenVT, MaDVT, GiaBan from dmvt where isMinibar=1";
            dmVT = _dbData.GetDataTable(sql);
             sql = "select MaDV, TenDV from dmdichvu ";
            dmDV = _dbData.GetDataTable(sql);
            sql = "select * from dmNT";
            dmnt = _dbData.GetDataTable(sql);
            dmnt.PrimaryKey = new DataColumn[] { dmnt.Columns["MaNT"] };
            sql = "select * from dmThueSuat";
            dmThueSuat = _dbData.GetDataTable(sql);
            dmThueSuat.PrimaryKey = new DataColumn[] { dmThueSuat.Columns["MaThue"] };
            sql = "select MaLoaiGia, TenLoaiGia,NgayTinh,Muctinh from dmloaigia order by Ngaytinh";
            tLoaigia = _dbData.GetDataTable(sql);
            tLoaigia.PrimaryKey = new DataColumn[] { tLoaigia.Columns["MaLoaigia"] };
        }       

        public DataCheckOut1Room FindRoom(string dt62id)
        {
            foreach (DataCheckOut1Room d in lChkOut)
            {
                if (d.Dt62ID == dt62id) return d;
            }
            return null;

        }

        public bool Save(bool isMultiTrans)
        {
            TinhTongTien();
            string sql;
          if(!isMultiTrans)  _dbData.BeginMultiTrans();
            try
            {
                sql = "update mt62 set ngaydi='" + mt["NgayDi"].ToString() + "', MaPTTT='" + mt["MaPTTT"].ToString() + "',";
                sql += " SoNgay=" + mt["SoNgay"].ToString() + ", MaLoaiGia='" + mt["MaLoaiGia"].ToString() + "',";
                sql += " Ptck=" + mt["PtCk"].ToString() + ", CK=" + mt["CK"].ToString() + ","; 
                sql += " TTienH=" + mt["TTienH"].ToString() + ", TTien=" + mt["TTien"].ToString() + ",TThue=" + mt["TThue"].ToString() + ",";
                sql += " TongTien=" + mt["TongTien"].ToString() + ", Conlai=" + mt["Conlai"].ToString() + ",";
                sql += " TTienMNB=" + mt["TTienMNB"].ToString() + ", TTienDV=" + mt["TTienDV"].ToString() + ",TThueMNB=" + mt["TThueMNB"].ToString() + ",  TThueDV=" + mt["TThueDV"].ToString();
                sql += ", Notice=N'" + mt["Notice"].ToString() + "'";
                sql += " where mt62id='" + mt["mt62ID"].ToString() + "'";
                _dbData.UpdateByNonQuery(sql);
                foreach (DataCheckOut1Room dt1Room in lChkOut)
                {
                    dt1Room.Save(isMultiTrans);
                    if (_dbData.HasErrors)
                    {
                        if (!isMultiTrans) _dbData.RollbackMultiTrans();
                        return false;
                    }
                }
                foreach (DataCheckOut1Room dt1Room in lChkOut)
                {
                    dt1Room.minibar.AcceptChanges();
                    dt1Room.dichvu.AcceptChanges();
                }
                if (!isMultiTrans) _dbData.EndMultiTrans();
                return true;
            }
            catch
            {
                return false;
            }
        }
       

        public bool CheckOut1Room(DataCheckOut1Room dt1R)
        {
            dt1R.CheckOut(false);
            Save(false);
            return true;
        }
        public bool CheckOut1RoomWithoutTrans(DataCheckOut1Room dt1R)
        {
            dt1R.CheckOut(true);
            Save(true);
            return true;
        }
        public bool CheckOut()
        {
            Save(false);
             string sql;
             foreach (DataCheckOut1Room dt1R in this.lChkOut)
             {
                 if (dt1R.dt["isCheckOut"].ToString() == "False") return false;
             }
            _dbData.BeginMultiTrans();
            try
            {
                mt["Ngaydi"] = DateTime.Parse(ngayht.ToString());
                sql = "update mt62 set isCheckOut=1,ngaydi='" + mt["ngaydi"].ToString() + "', ws='" + Config.GetValue("sysUserID").ToString() + "' where mt62ID='" + mt["MT62ID"].ToString() + "'";
                _dbData.UpdateByNonQuery(sql);
                //foreach (DataCheckOut1Room dt1Room in lChkOut)
                //{
                //    dt1Room.CheckOut();
                //}
                if (double.Parse(mt["Conlai"].ToString()) > 0)
                {
                    sql = "insert into TT62(MT62ID,SoTien,MaNT,MaPTTT,ws";
                    sql += ") values('" + mt["MT62ID"].ToString() + "'," + mt["Conlai"].ToString() + ",'" + mt["MaNT"].ToString() + "','" + mt["MaPTTT"].ToString() + "','" + Config.GetValue("sysUserID") + "')";
                    _dbData.UpdateByNonQuery(sql);
                    sql = "update mt62 set Datcoc=b.sotien from mt62 a inner join (select mt62id,sum(sotien) as sotien from tt62 where mt62id='" + mt["MT62ID"].ToString() + "' group by mt62id) b on a.mt62id=b.mt62id";
                    _dbData.UpdateByNonQuery(sql);
                    sql = "update mt62 set conlai=TongTien-Datcoc where mt62id='" + mt["MT62ID"].ToString() + "'";
                    _dbData.UpdateByNonQuery(sql);
                }
                _dbData.EndMultiTrans();
                return true;
            }
            catch
            {
                _dbData.RollbackMultiTrans();
                return false;
            }
        }
        void dt_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            DataRow dr = e.Row;
            dr["MT62ID"] = mt["MT62ID"];
            
        }
        public bool ChuyenPhong(DataCheckOut1Room crRoom, string MaPhongDen)
        {

            DataRow rdt = dt.NewRow();
            rdt["DT62ID"] = Guid.NewGuid();
            rdt["MaPhong"] = MaPhongDen;
            rdt["MaGia"] = crRoom.dt["MaGia"];
            rdt["DaChuyen"] = 0;            
            rdt["MaLoaiPhong"] = crRoom.dt["MaLoaiPhong"];
            rdt["GiaPhong"] = crRoom.dt["GiaPhong"];
            rdt["GiaPhong1"] = crRoom.dt["GiaPhong1"];
            rdt["NgayTT"] = crRoom.dt["NgayTT"];
            rdt["NgayDen"] = DateTime.Parse(ngayht.ToString());
            rdt["SoNT"] = crRoom.dt["SoNT"];
            //rdt["NgayDi"] = mt["NgayDi"];
            DataCheckOut1Room dtChkOut = new DataCheckOut1Room(rdt, _dbData);
            dtChkOut.ThueSuat = double.Parse(mt["Thuesuat"].ToString());
            dtChkOut.setMinibarData();
            // dtChkOut.dt["NgayDi"] = DateTime.Now;
            rdt["NgayDi"] = mt["NgayDi"];
            rdt["isCheckOut"] = false;
            rdt["isCheckIn"] = true;
            rdt["SoNgay"] = mt["SoNgay"];
            _dbData.BeginMultiTrans();

            try
            {
                string sql = "insert into dt62 (MT62ID, DT62ID,MaPhong, GiaPhong,GiaPhong1,Ps,SoNT,isCheckIn,isCheckOut, NgayDen, NgayDi,MaLoaiPhong, MaGia,NgayTT,DaChuyen,SoNgay) values('" +
                                                  mt["MT62ID"].ToString() + "','" + rdt["DT62ID"].ToString() + "','" + rdt["MaPhong"].ToString() + "'," + rdt["GiaPhong"].ToString() + "," + rdt["GiaPhong1"].ToString() + "," +
                                                 rdt["Ps"].ToString() + "," + rdt["SoNT"].ToString() + ",1,0,'" + rdt["NgayDen"].ToString() + "','" + rdt["NgayDi"].ToString() + "','" +
                                                 rdt["MaLoaiPhong"].ToString() + "'," + rdt["MaGia"].ToString() + ",'" + rdt["NgayTT"].ToString() + "',0," + rdt["SoNgay"].ToString() + ")";
                _dbData.UpdateByNonQuery(sql);
                sql = " update dmPhong set MaTT='IN' where MaPhong='" + rdt["MaPhong"].ToString() + "'";
                _dbData.UpdateByNonQuery(sql);
                crRoom.dt["DaChuyen"] = true;
               // crRoom.dt["isCheckOut"] = false;
                if (crRoom != null && crRoom.dt["isCheckOut"].ToString() == "False")
                {
                    crRoom.dt["NgayDi"] = DateTime.Parse(ngayht.ToString());
                    if (this.CheckOut1RoomWithoutTrans(crRoom))
                    {
                        crRoom.dt["isCheckOut"] = true;
                    }
                }
                if (!_dbData.HasErrors)
                {
                    lChkOut.Add(dtChkOut);
                    dtChkOut.TTienChanged += new EventHandler(dtChkOut_TTienChanged); 
                    dt.Rows.Add(rdt);
                    rdt["NgayDi"] = mt["NgayDi"];
                    dt.AcceptChanges();
                    _dbData.EndMultiTrans();
                    return true;
                }
                else
                {
                    dt.RejectChanges();
                    _dbData.RollbackMultiTrans();
                    return false;
                }
            }
            catch
            {
                dt.RejectChanges();
                _dbData.RollbackMultiTrans();
                return false;
            }
            
        }
    }
}
