using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CDTDatabase;
namespace FO
{
    class DataCheckOut1Room
    {
        public Database _dbData ;
        private Database _dbStruct = Database.NewStructDatabase();

        public DataRow dt;
        public DataTable dichvu;
        public DataTable minibar;
        private DataTable tLoaigia;
        private DataTable tGia;
        public double ThueSuat = 10;
        public bool isCheckOut = false;
        public string Mathue = "10";
        public string MaPhong;
        public string Dt62ID;
        private double TTienDV = 0;
        private double TTienMNB = 0;
        private double TThueDV = 0;
        private double TThueMNB = 0;
        private object isThueMinibar = 1;
        private object isThuePhong = 1;
        private object isUsingTimeOut = 0;
        private object TimeIn;
        public object TimeOut;
        private object AddDayOutver = 0;
        object ngayht;
        public bool Giusongay_eralycheckout = true;
        //for print

        public DataCheckOut1Room(DataRow _dt,Database db)
        {
            _dbData = db;
            string sql = "select MaLoaiGia, TenLoaiGia,NgayTinh,Muctinh from dmloaigia order by Ngaytinh";

            tLoaigia = _dbData.GetDataTable(sql);
            tLoaigia.PrimaryKey = new DataColumn[] { tLoaigia.Columns["MaLoaigia"] };
            sql = "select MaGia,MaLoaiPhong,MaLoaiGia,Gia,SoNgay from dmgia";
            tGia = _dbData.GetDataTable(sql);
            tGia.PrimaryKey = new DataColumn[] { tGia.Columns["MaGia"] };
             sql = "select getdate()";
            ngayht = _dbData.GetValue(sql);
            Dt62ID =_dt["dt62ID"].ToString();
           // sql = "select top 1 * from dt62  where Dt62ID='" + Dt62ID + "' ";
            dt = _dt;
            DataTable tb = dt.Table;
            isThueMinibar = _dbData.GetValue("select giatri from foConfig where TenTS='GIAMINIBAR'");
            isThuePhong = _dbData.GetValue("select giatri from foConfig where TenTS='GIAPHONG'");
            isUsingTimeOut = _dbData.GetValue("select giatri from foConfig where TenTS='USETIMEOUT'");
            TimeIn = _dbData.GetValue("select giatri from foConfig where TenTS='TIMEIN'");
            TimeOut = _dbData.GetValue("select giatri from foConfig where TenTS='TIMEOUT'");
            AddDayOutver = _dbData.GetValue("select giatri from foConfig where TenTS='ADDOVERTIME'");


            //if (tb.Rows.Count == 0) return;
            
            MaPhong = dt["MaPhong"].ToString();
            if (dt["isCheckOut"].ToString() == "True") isCheckOut = true;
            tb.ColumnChanged += new DataColumnChangeEventHandler(tb_ColumnChanged);
            
            UpdateCtDVKhac(MaPhong,DateTime.Parse( dt["NgayDen"].ToString()),DateTime.Parse(ngayht.ToString()).AddSeconds(-1));
            
            //TinhTienDV();
            
        }
        public void setMinibarData()
        {
           string sql = "select * from ctminibar where dt62id='" + dt["DT62ID"].ToString() + "'";
            minibar = _dbData.GetDataTable(sql);
            minibar.ColumnChanged += new DataColumnChangeEventHandler(minibar_ColumnChanged);
            minibar.Columns["DT62ID"].DefaultValue = dt["DT62ID"];
            minibar.Columns["NgayCt"].DefaultValue = DateTime.Parse(ngayht.ToString());//dt["NgayDi"];
            minibar.Columns["Soluong"].DefaultValue = 0;
            minibar.Columns["DonGia"].DefaultValue = 0;
            minibar.Columns["ThanhTien"].DefaultValue = 0;
            minibar.Columns["MaThue"].DefaultValue = Mathue.ToString();
            minibar.Columns["ThueSuat"].DefaultValue = ThueSuat;
            minibar.Columns["TienThue"].DefaultValue = 0;
            minibar.Columns["TTien"].DefaultValue = 0;
            minibar.TableNewRow += new DataTableNewRowEventHandler(minibar_TableNewRow);
            sql = "select *,cast('" + Dt62ID + "' as uniqueidentifier) as DT62ID from ctDvKhac where dt62id='" + Dt62ID.ToString() + "' or (maphong='" + MaPhong + "' and ngayct between '" + dt["NgayDen"].ToString() + "' and '" + dt["NgayDi"].ToString() + "')";
            dichvu = _dbData.GetDataTable(sql);
            dichvu.ColumnChanged += new DataColumnChangeEventHandler(dichvu_ColumnChanged);
            dichvu.Columns["DT62id"].DefaultValue = dt["DT62ID"];
            dichvu.Columns["NgayCt"].DefaultValue = dt["NgayDi"];
            dichvu.TableNewRow += new DataTableNewRowEventHandler(dichvu_TableNewRow);
            dichvu.Columns["Soluong"].DefaultValue = 0;
            dichvu.Columns["DonGia"].DefaultValue = 0;
            dichvu.Columns["TTien"].DefaultValue = 0;
            dichvu.Columns["TThue"].DefaultValue = 0;
            dichvu.Columns["TTienDV"].DefaultValue = 0;
            TinhTienMNB();
            TinhTienDV();
        }

        void dichvu_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            
        }
        public DataRow MinibarCr;
        void minibar_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["CTMNBID"] = Guid.NewGuid();
            MinibarCr = e.Row;
        }
        public event EventHandler TTienChanged;
        private void UpdateCtDVKhac(string MaPhong, DateTime tungay, DateTime denngay)
        {
            string sql = "TinhTienDienThoai1phong";
            _dbData.UpdateDatabyStore(sql, new string[] { "@MaPhong", "@NgayCt1", "@NgayCt2" }, new object[] { MaPhong, tungay, denngay });
            sql = "TinhTienRes1phong";
            _dbData.UpdateDatabyStore(sql, new string[] { "@MaPhong", "@NgayCt1", "@NgayCt2" }, new object[] { MaPhong, tungay, denngay });
        }  
        void tb_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            if (e.Column.ColumnName == "NgayDi" && !isCheckOut  )
            {
                if (!Giusongay_eralycheckout)
                {
                    setLoaiGia();
                    TinhTienPhong();
                }
            }
            
            try
            {
                if (e.Column.ColumnName == "ps")
                {
                   //     dt["ThuePhong"] = double.Parse(dt["ps"].ToString()) * ThueSuat / 100;                    
                }
                if (e.Column.ColumnName == "GiaPhong")
                {
                    TinhTienPhong();
                }
                if (e.Column.ColumnName == "SoNgay")
                {
                    TinhTienPhong();
                }
                if (e.Column.ColumnName == "ThuePhong")
                {
                    dt["TongCong"] = double.Parse(dt["ps"].ToString()) + double.Parse(dt["ThuePhong"].ToString()) + double.Parse(dt["TTienMNB"].ToString()) + double.Parse(dt["TTienDV"].ToString()) + double.Parse(dt["TThueMNB"].ToString()) + double.Parse(dt["TThueDV"].ToString());
                }
                if (e.Column.ColumnName == "TTienMNB")
                {
                    dt["TongCong"] = double.Parse(dt["ps"].ToString()) + double.Parse(dt["ThuePhong"].ToString()) + double.Parse(dt["TTienMNB"].ToString()) + double.Parse(dt["TTienDV"].ToString()) + double.Parse(dt["TThueMNB"].ToString()) + double.Parse(dt["TThueDV"].ToString());
                }
                if (e.Column.ColumnName == "TTienDV")
                {
                    dt["TongCong"] = double.Parse(dt["ps"].ToString()) + double.Parse(dt["ThuePhong"].ToString()) + double.Parse(dt["TTienMNB"].ToString()) + double.Parse(dt["TTienDV"].ToString()) + double.Parse(dt["TThueMNB"].ToString()) + double.Parse(dt["TThueDV"].ToString());
                }
                if (e.Column.ColumnName == "TThueMNB")
                {
                    dt["TongCong"] = double.Parse(dt["ps"].ToString()) + double.Parse(dt["ThuePhong"].ToString()) + double.Parse(dt["TTienMNB"].ToString()) + double.Parse(dt["TTienDV"].ToString()) + double.Parse(dt["TThueMNB"].ToString()) + double.Parse(dt["TThueDV"].ToString());
                }
                if (e.Column.ColumnName == "TThueDV")
                {
                    dt["TongCong"] = double.Parse(dt["ps"].ToString()) + double.Parse(dt["ThuePhong"].ToString()) + double.Parse(dt["TTienMNB"].ToString()) + double.Parse(dt["TTienDV"].ToString()) + double.Parse(dt["TThueMNB"].ToString()) + double.Parse(dt["TThueDV"].ToString());
                }
                e.Row.EndEdit();
            }
            catch { }
        }
        private void setLoaiGia()
        {
            if (dt["NgayDi"] == null || dt["NgayTT"] == null) return;
            TimeSpan tsp = DateTime.Parse(dt["NgayDi"].ToString()) - DateTime.Parse(dt["NgayTT"].ToString());

            if (tLoaigia.Rows.Count < 2) return;
            if (double.Parse(tLoaigia.Rows[1]["Muctinh"].ToString()) < tsp.TotalDays)
            {
                if (isUsingTimeOut.ToString() == "0")
                {
                    dt["SoNgay"] = Math.Ceiling(tsp.TotalDays); //Cái này tính phức tạp dựa vào giờ vào và ra.
                }
                else
                {
                    dt["SoNgay"] = TinhSoNgay(dt["NgayTT"], dt["NgayDi"]);
                }
            }   
            else
            {
                if (tsp.TotalHours < 1)
                {
                    dt["SoNgay"] = 1.0 / 24;
                }
                else
                {
                    dt["SoNgay"] = Math.Ceiling(tsp.TotalDays * 24) / 24.0;
                }

            }
            DataRow findDr = tLoaigia.Rows[0];
            foreach (DataRow drtmp in tLoaigia.Rows)
            {
                if (tsp.TotalDays > double.Parse(drtmp["MucTinh"].ToString()))
                {
                    findDr = drtmp;
                }
                else
                {
                    break;
                }
            }
            if (findDr == null) return;
            DataRow[] drGia = tGia.Select("MaLoaiPhong='" + dt["MaLoaiPhong"].ToString() + "' and MaLoaiGia='" + findDr["MaLoaiGia"].ToString() + "'");
            
            if (drGia.Length > 0 &&dt["MaGia"].ToString()!= drGia[0]["MaGia"].ToString() ) 
            {
                dt["MaGia"] = drGia[0]["MaGia"];
                //dt["GiaPhong"] = drGia[0]["Gia"];
                dt["SoNT"] = drGia[0]["SoNgay"];
            }
  
        }

        private double TinhSoNgay(object ngayden, object ngaydi)
        {
            try
            {
                double extraDate = 0;
                DateTime GioDen;
                DateTime GioDi;
                GioDen = DateTime.Parse(DateTime.Parse(ngayden.ToString()).ToShortDateString() + " " + TimeIn.ToString());
                GioDi = DateTime.Parse(DateTime.Parse(ngaydi.ToString()).ToShortDateString() + " " + TimeOut.ToString());
                if (DateTime.Parse(ngayden.ToString()) < GioDen) extraDate = double.Parse(AddDayOutver.ToString());
                if (DateTime.Parse(ngaydi.ToString()) > GioDi) extraDate += double.Parse(AddDayOutver.ToString());
                GioDen = DateTime.Parse(GioDen.ToShortDateString());
                GioDi = DateTime.Parse(GioDi.ToShortDateString());
                TimeSpan tsp = DateTime.Parse(GioDi.ToString()) - DateTime.Parse(GioDen.ToString());
                TimeSpan tsp1 = DateTime.Parse(ngaydi.ToString()) - DateTime.Parse(ngayden.ToString());
                if (tsp.TotalDays == 0)
                {
                    if (tsp1.TotalHours > 6) return 1;
                }
                return Math.Ceiling(tsp.TotalDays) + extraDate;
            }
            catch
            {
                TimeSpan tsp = DateTime.Parse(ngayden.ToString()) - DateTime.Parse(ngaydi.ToString());
                return Math.Ceiling(tsp.TotalDays);
            }
        }
        public void TinhTienPhong()
        {
            //trường hợp tính giá theo giờ
            if (bool.Parse(dt["DaChuyen"].ToString()))
            {
                dt["ps"] = 0;
                dt["ThuePhong"] = 0;
                TinhTienMNB();
                TinhTienDV();
                return;
            }
            double TienEarly = 0;
            if (dt["TienEarly"] != DBNull.Value) TienEarly = double.Parse(dt["TienEarly"].ToString());
            if (double.Parse(dt["SoNT"].ToString()) < 1)
            {
                double sogio = double.Parse(dt["SoNgay"].ToString()) * 24;
                if (isThuePhong.ToString() == "1")
                {
                    dt["ps"] = double.Parse(dt["GiaPhong"].ToString()) + TienEarly;
                    dt["ThuePhong"] = double.Parse(dt["ps"].ToString()) * ThueSuat / 100;
                }
                else
                {
                    dt["ps"] = Math.Round(( double.Parse(dt["GiaPhong"].ToString())+ TienEarly) / (1 + ThueSuat / 100), 0);
                    dt["ThuePhong"] =  double.Parse(dt["GiaPhong"].ToString()) - double.Parse(dt["ps"].ToString());
                }
            }
            else
            {
                if (isThuePhong.ToString() == "1")
                {
                    dt["ps"] = Math.Round(double.Parse(dt["SoNgay"].ToString()) * double.Parse(dt["GiaPhong"].ToString()) / double.Parse(dt["SoNT"].ToString())) + TienEarly;
                    dt["ThuePhong"] = double.Parse(dt["ps"].ToString()) * ThueSuat / 100;
                }
                else
                {
                    dt["ps"] = Math.Round(double.Parse(dt["SoNgay"].ToString()) * double.Parse(dt["GiaPhong"].ToString()) / ((1 + ThueSuat / 100) * double.Parse(dt["SoNT"].ToString())))+TienEarly;
                    dt["ThuePhong"] = double.Parse(dt["SoNgay"].ToString()) * double.Parse(dt["GiaPhong"].ToString()) / double.Parse(dt["SoNT"].ToString()) + TienEarly - double.Parse(dt["ps"].ToString());
                }
            }
            dt.EndEdit();
        }
        private void TinhTienMNB()
        {
            TTienMNB = 0;
            TThueMNB = 0;
            minibar.DefaultView.RowStateFilter = DataViewRowState.Added | DataViewRowState.Unchanged | DataViewRowState.ModifiedCurrent;
            foreach (DataRowView dr in minibar.DefaultView)
            {
                TTienMNB += double.Parse(dr["ThanhTien"].ToString());
                TThueMNB += double.Parse(dr["TienThue"].ToString());
            }
            dt["TTienMNB"] = TTienMNB;
            dt["TThueMNB"] = TThueMNB;
            if (this.TTienChanged != null)
                this.TTienChanged(this, new EventArgs());
        }
        private void TinhTienDV()
        {
            TTienDV = 0;
            TThueDV = 0;
            dichvu.DefaultView.RowStateFilter = DataViewRowState.Added | DataViewRowState.Unchanged | DataViewRowState.ModifiedCurrent;
            foreach (DataRowView dr in dichvu.DefaultView)
            {
                TTienDV += double.Parse(dr["TTienDV"].ToString());
                TThueDV += double.Parse(dr["TThue"].ToString());
            }
            dt["TTienDV"] = TTienDV;
            dt["TThueDV"] = TThueDV;

            if (this.TTienChanged != null)
            this.TTienChanged(this, new EventArgs());
        }
        void dichvu_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            try
            {
                if (e.Column.ColumnName == "TThue")
                {
                    e.Row["TTien"] = double.Parse(e.Row["TThue"].ToString()) + double.Parse(e.Row["TTienDV"].ToString());
                    e.Row.EndEdit();
                   
                }
                if (e.Column.ColumnName == "TTienDV")
                {
                    e.Row["TTien"] = double.Parse(e.Row["TThue"].ToString()) + double.Parse(e.Row["TTienDV"].ToString());
                    e.Row.EndEdit();
                    TinhTienDV();
                }
                //if (e.Column.ColumnName == "TTien")
                //{
                //    e.Row["TTienDV"] =   double.Parse(e.Row["TTien"].ToString())- double.Parse(e.Row["TThue"].ToString());
                //    e.Row.EndEdit();
                //    TinhTienDV();
                //}
            }
            catch
            {
            }
        }
        void minibar_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            try
            {
                if (isThueMinibar.ToString() == "0")
                {
                    if (e.Column.ColumnName == "Soluong")
                    {
                        e.Row["ThanhTien"] = Math.Round(double.Parse(e.Row["Soluong"].ToString()) * double.Parse(e.Row["DonGia"].ToString()) / (1+ThueSuat/100), 0);
                    }
                    else if (e.Column.ColumnName == "DonGia")
                    {
                        e.Row["ThanhTien"] = Math.Round(double.Parse(e.Row["Soluong"].ToString()) * double.Parse(e.Row["DonGia"].ToString()) / (1 + ThueSuat / 100), 0);
                    }
                    else if (e.Column.ColumnName == "ThanhTien")
                    {
                        e.Row["TTien"] = double.Parse(e.Row["Soluong"].ToString()) * double.Parse(e.Row["DonGia"].ToString());
                        e.Row["TienThue"] = double.Parse(e.Row["TTien"].ToString()) - double.Parse(e.Row["ThanhTien"].ToString());
                        TinhTienMNB();
                    }
                    else if (e.Column.ColumnName == "ThueSuat")
                    {
                        e.Row["ThanhTien"] = Math.Round(double.Parse(e.Row["Soluong"].ToString()) * double.Parse(e.Row["DonGia"].ToString()) / (1 + ThueSuat / 100), 0);
                        e.Row["TienThue"] = Math.Round(double.Parse(e.Row["ThueSuat"].ToString()) * double.Parse(e.Row["ThanhTien"].ToString()) / 100, 0);
                    }
                    else if (e.Column.ColumnName == "TienThue")
                    {
                        e.Row["TTien"] = double.Parse(e.Row["TienThue"].ToString()) + double.Parse(e.Row["ThanhTien"].ToString());
                        TinhTienMNB();
                    }
                }
                else
                {
                    if (e.Column.ColumnName == "Soluong")
                    {
                        e.Row["ThanhTien"] = double.Parse(e.Row["Soluong"].ToString()) * double.Parse(e.Row["DonGia"].ToString());
                    }
                    else if (e.Column.ColumnName == "DonGia")
                    {
                        e.Row["ThanhTien"] = double.Parse(e.Row["Soluong"].ToString()) * double.Parse(e.Row["DonGia"].ToString());
                    }
                    else if (e.Column.ColumnName == "ThanhTien")
                    {
                        e.Row["TienThue"] = Math.Round(double.Parse(e.Row["ThanhTien"].ToString()) * double.Parse(e.Row["ThueSuat"].ToString()) / 100, 0);
                        e.Row["TTien"] =    double.Parse(e.Row["ThanhTien"].ToString()) + double.Parse(e.Row["TienThue"].ToString()); 
                        TinhTienMNB();
                    }
                    else if (e.Column.ColumnName == "ThueSuat")
                    {
                        e.Row["ThanhTien"] = double.Parse(e.Row["Soluong"].ToString()) * double.Parse(e.Row["DonGia"].ToString());
                        e.Row["TienThue"] = Math.Round(double.Parse(e.Row["ThueSuat"].ToString()) * double.Parse(e.Row["ThanhTien"].ToString()) / 100, 0);
                    }
                    else if (e.Column.ColumnName == "TienThue")
                    {
                        e.Row["TTien"] = double.Parse(e.Row["TienThue"].ToString()) + double.Parse(e.Row["ThanhTien"].ToString());
                        TinhTienMNB();
                    }
 
                }
            }
            catch
            {
            }
        }
        public void Save(bool isMulTiTrans)
        {
            UpdateMinibar(isMulTiTrans);
            UpdateDVKhac(isMulTiTrans);
            string sql;
            sql = "update dt62 set ngaydi='" + dt["NgayDi"].ToString() + "', SoNT='" + dt["soNT"].ToString() + "',";
            sql += " SoNgay=" + dt["SoNgay"].ToString() + ", GiaPhong=" + dt["Giaphong"].ToString() + ",ThuePhong=" + dt["ThuePhong"].ToString() + ",";
            sql += " ps=" + dt["ps"].ToString() + ", MaGia='" + dt["MaGia"].ToString() + "',TongCong=" + dt["TongCong"].ToString() + ",";
            sql += " TTienMNB=" + dt["TTienMNB"].ToString() + ", TTienDV=" + dt["TTienDV"].ToString() + ",TThueMNB=" + dt["TThueMNB"].ToString() + ",  TThueDV=" + dt["TThueDV"].ToString();
            sql += " where dt62id='" + dt["dt62ID"].ToString() + "'";
            _dbData.UpdateByNonQuery(sql);
            
        }

        private void UpdateDVKhac(bool isMultiTrans)
        {
            string sql;
            if (!isMultiTrans) _dbData.BeginMultiTrans();
            try
            {
                dichvu.DefaultView.RowStateFilter = DataViewRowState.ModifiedCurrent;
                foreach (DataRowView dr in dichvu.DefaultView)
                {
                    sql = "Update CtDvKhac set madv='" + dr["MaDV"].ToString() + "', Dt62ID='" + dt["DT62ID"].ToString() + "',soluong=" + dr["Soluong"].ToString() + ",TTienDV=" + dr["TTienDV"].ToString() + ", TTien=" + dr["TTien"].ToString() + " where stt=" + dr["Stt"].ToString();
                    _dbData.UpdateByNonQuery(sql);
                    if (_dbData.HasErrors)
                    {
                        dichvu.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
                        return;
                    }
                }
                dichvu.DefaultView.RowStateFilter = DataViewRowState.Added;
                foreach (DataRowView dr in dichvu.DefaultView)
                {
                    sql = "insert into ctdvKhac (MaDV, Soluong,TTienDV, TTien, MaPhong,NGAYCT, dt62id) values('" + dr["MaDV"].ToString();
                    sql += "'," + dr["Soluong"].ToString() + "," + dr["TTIenDV"].ToString() + "," + dr["TTIen"].ToString() + ",'" + dt["MaPhong"].ToString() + "','" + ngayht.ToString() + "','" + dt["dt62id"].ToString() + "')";
                    _dbData.UpdateByNonQuery(sql);
                    object o = this._dbData.GetValue("select @@identity");
                    if (o != null)
                    {
                        dr["stt"] = o;
                    }
                    if (_dbData.HasErrors)
                    {
                        dichvu.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
                        return;
                    }
                    dr.EndEdit();
                }
                dichvu.DefaultView.RowStateFilter = DataViewRowState.Deleted;
                foreach (DataRowView dr in dichvu.DefaultView)
                {
                    sql = "delete ctdvkhac where stt=" + dr["Stt"].ToString();
                    _dbData.UpdateByNonQuery(sql);
                    if (_dbData.HasErrors)
                    {
                        dichvu.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
                        return;
                    }
                }
                dichvu.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
                if (_dbData.HasErrors)
                {
                    if (!isMultiTrans) _dbData.RollbackMultiTrans();
                }
                else
                {
                    if (!isMultiTrans) _dbData.EndMultiTrans();
                    dichvu.AcceptChanges();
                }
            }
            catch
            {
                dichvu.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
                if (!isMultiTrans) _dbData.RollbackMultiTrans();
            }
        }
        public void CheckOut(bool isMulTiTrans)
        {
            if (!isCheckOut)
            {
                UpdateMinibar(isMulTiTrans);
                UpdateDVKhac(isMulTiTrans);
                string sql = "update dt62 set isCheckOut=1, GiaEarly=" + dt["GiaEarly"].ToString() + ", TienEarly=" + dt["TienEarly"].ToString() + ", DaChuyen=" + (dt["DaChuyen"].ToString() == "True" ? " 1" : "0") + " where dt62ID='" + Dt62ID + "' and  isCheckin=1";

                _dbData.UpdateByNonQuery(sql);
                sql = "update dmPhong set MaTT='DURTY' where maphong='" + MaPhong + "'";
                _dbData.UpdateByNonQuery(sql);
                isCheckOut = true;
            }
        }
        
        private void UpdateMinibar(bool isMultiTrans) 
        {
            string sql;

            if (!isMultiTrans) _dbData.BeginMultiTrans();
            try
            {
                //Những dòng update
                minibar.DefaultView.RowStateFilter = DataViewRowState.ModifiedCurrent;
                foreach (DataRowView dr in minibar.DefaultView)
                {
                    sql = " update ctminibar set ngayct='" + dr["NgayCt"].ToString() + "', mavt='" + dr["MaVT"].ToString() + "',";
                    sql += " Soluong=" + dr["Soluong"].ToString() + ",Dongia=" + dr["DonGia"].ToString() + ", ThanhTien=" + dr["ThanhTien"].ToString() + ",";
                    sql += " MaThue='" + dr["MaThue"].ToString() + "', ThueSuat=" + dr["ThueSuat"].ToString() + ",TienThue=" + dr["TienThue"].ToString() + ", TTien=" + dr["TTien"].ToString();
                    sql += " where CTMNBID='" + dr["CTMNBID"].ToString() + "'";
                    _dbData.UpdateByNonQuery(sql);
                    if (_dbData.HasErrors)
                    {
                        minibar.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
                        return;
                    }
                }
                minibar.DefaultView.RowStateFilter = DataViewRowState.Added;
                foreach (DataRowView dr in minibar.DefaultView)
                {
                    sql = " insert into ctminibar (CTMNBID,dt62id, ngayct, mavt, soluong, dongia, thanhtien, mathue, thuesuat,tienthue, TTien) values(newid(),'";
                    sql += dr["DT62ID"].ToString() + "','" + dr["NgayCt"].ToString() + "','" + dr["MaVT"].ToString() + "'," + dr["Soluong"].ToString();
                    sql += "," + dr["Dongia"].ToString() + "," + dr["ThanhTien"].ToString() + ",'" + dr["MaThue"].ToString() + "'," + dr["ThueSuat"].ToString() + ",";
                    sql += dr["TienThue"].ToString() + "," + dr["TTien"].ToString() + ")";
                    _dbData.UpdateByNonQuery(sql);
                    if (_dbData.HasErrors)
                    {
                        minibar.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
                        return;
                    }
                }
                minibar.DefaultView.RowStateFilter = DataViewRowState.Deleted;
                foreach (DataRowView dr in minibar.DefaultView)
                {
                    sql = " delete ctminibar where CTMNBID='" + dr["CTMNBID"].ToString() + "'";
                    _dbData.UpdateByNonQuery(sql);
                    if (_dbData.HasErrors)
                    {
                        minibar.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
                        return;
                    }
                }
                minibar.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
                if (_dbData.HasErrors)
                {
                    if (!isMultiTrans) _dbData.RollbackMultiTrans();

                }
                else
                {
                    if (!isMultiTrans) _dbData.EndMultiTrans();
                    minibar.AcceptChanges();
                }
            }
            catch
            {
                if (!isMultiTrans)  _dbData.RollbackMultiTrans();
                minibar.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
            }
        }
    }
}
