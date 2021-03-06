using System;
using System.Collections.Generic;
using System.Text;
using CDTDatabase;
using System.Data;
using DataFactory;
using System.Windows.Forms;
namespace FO
{
    
    class DataMT62
    {
        public DataSet ds;
        public DataRow mt;
        public DataTable dt;
        public DataTable ct;
        public DataTable tGia;
        public DataTable dmnt;
        public DataTable dmThueSuat;
        public DataTable tLoaigia;
        public DataTable tPhong;
        public DataTable tDSPhong=new DataTable();
        public DataTable tkhach;
        public DataTable tLoaithe;
        public DataTable tLoaiphong;
        public DataTable tGiayto;
        public DataTable tQuocGia;
        public DataTable tTinh;
        
        public DataTable tLoaiPhong1;
        public DataTable tMt61;
        public DataTable t_hang;
        public DataTable tpttt;
        public DataTable tbPhongTrong;
        public DataTable tbPhongtrong_tmp;
        public  Database _dbData = Database.NewDataDatabase();
        private Database _dbStruct = Database.NewStructDatabase();
        public bool _isEdit;
        public event DataColumnChangeEventHandler MT61_changed;
        public event DataColumnChangeEventHandler MaLoaiGia_changed;
        public event EventHandler Ngay_Changed;
        public event DataTableClearEventHandler dt_deleted;
        private List<DataRow> _lstDtRow=new List<DataRow>();
        private List<DataRow> _lstCtRow = new List<DataRow>();
        public bool isUpdating=false;
        private object isThuePhong = 1;
        private object isUsingTimeOut = 0;
        private object TimeIn;
        private object TimeOut;
        private object AddDayOutver = 0;
        private object Mathue = 0;
        public object ngayht;
        
        public DataMT62()
        {
            _isEdit = false;
            getData();
            GetData4Lookup();
           
            
        }
        public DataMT62(string MT62ID)
        {
            _isEdit = true;
             getData(MT62ID);
            GetData4Lookup();
           
        }


        private void GetData4Lookup()
        {
            string sql = "select a.*, b.* from mt61 a inner join dmkhachdl b on a.makhach=b.makhach where isVisible=1";
            if (_isEdit)
            {
                sql = "select a.*, b.* from mt61 a inner join dmkhachdl b on a.makhach=b.makhach where MT61ID in (select MT61ID from MT62 where MT62ID='" + mt["MT62ID"].ToString() + "')";
            }

            tMt61 = _dbData.GetDataTable(sql);
            tMt61.PrimaryKey = new DataColumn[] { tMt61.Columns["MT61ID"] };

            isThuePhong = _dbData.GetValue("select giatri from foConfig where TenTS='GIAPHONG'");
            isUsingTimeOut = _dbData.GetValue("select giatri from foConfig where TenTS='USETIMEOUT'");
            TimeIn = _dbData.GetValue("select giatri from foConfig where TenTS='TIMEIN'");
            TimeOut = _dbData.GetValue("select giatri from foConfig where TenTS='TIMEOUT'");
            AddDayOutver = _dbData.GetValue("select giatri from foConfig where TenTS='ADDOVERTIME'");
            



            sql = "select * from dmKhachDL";
            tkhach = _dbData.GetDataTable(sql);
            tkhach.PrimaryKey = new DataColumn[] { tkhach.Columns["MaKhach"] };
            sql = "select * from dmloaithe";
            tLoaithe = _dbData.GetDataTable(sql);
            tLoaithe.PrimaryKey = new DataColumn[] { tLoaithe.Columns["Maloaithe"] };

            sql = "select * from dmPTTT";
            tpttt = _dbData.GetDataTable(sql);
            tpttt.PrimaryKey = new DataColumn[] { tpttt.Columns["MaPTTT"] };


            sql = "select * from dmHangDL";
            t_hang = _dbData.GetDataTable(sql);
            t_hang.PrimaryKey = new DataColumn[] { t_hang.Columns["MaHang"] };

            sql = "select MaGiayto,TenGiayto from dmGiayto";
            tGiayto = _dbData.GetDataTable(sql);
            tGiayto.PrimaryKey = new DataColumn[] { tGiayto.Columns["MaLoaiPhong"] };

            sql = "select MaLoaiPhong,TenLoaiPhong,SoNguoi from dmLoaiphong";
            tLoaiPhong1 = _dbData.GetDataTable(sql);
            tLoaiPhong1.PrimaryKey = new DataColumn[] { tLoaiPhong1.Columns["MaLoaiPhong"] };

            sql = "select MaLoaiPhong, TenLoaiPhong,SoNguoi from dmloaiphong";
            tLoaiphong = _dbData.GetDataTable(sql);
            tLoaiphong.PrimaryKey = new DataColumn[] { tLoaiphong.Columns["MaLoaiPhong"] };

            sql = "select MaQuocgia, TenQuocgia,TenVN,Khuvuc from dmquocgia";
            tQuocGia = _dbData.GetDataTable(sql);
            tQuocGia.PrimaryKey = new DataColumn[] { tLoaiphong.Columns["MaQuocgia"] };

            sql = "select MaTinh, TenTinh, MaQuocgia,ZipCode, DialCode, MaCuoc from dmTinh";
            tTinh = _dbData.GetDataTable(sql);
            tTinh.PrimaryKey = new DataColumn[] { tLoaiphong.Columns["MaTinh"] };


            sql = "select MaLoaiGia, TenLoaiGia,NgayTinh,Muctinh from dmloaigia order by Ngaytinh";
            tLoaigia = _dbData.GetDataTable(sql);
            tLoaigia.PrimaryKey = new DataColumn[] { tLoaigia.Columns["MaLoaigia"] };


            sql = "select MaPhong,TenPhong,MaLoaiPhong,SoGiuong,GhiChu,MaArea,0 as isSelected from dmphong ";
            tPhong = _dbData.GetDataTable(sql);
            tPhong.PrimaryKey = new DataColumn[] { tPhong.Columns["MaPhong"] };





            sql = "select MaLoaiPhong,TenLoaiPhong,SoNguoi from dmLoaiphong";
            tLoaiPhong1 = _dbData.GetDataTable(sql);
            tLoaiPhong1.PrimaryKey = new DataColumn[] { tLoaiPhong1.Columns["MaLoaiPhong"] };


            sql = "select MaGiayto,TenGiayto from dmGiayto";
            tGiayto = _dbData.GetDataTable(sql);
            tGiayto.PrimaryKey = new DataColumn[] { tGiayto.Columns["MaLoaiPhong"] };


            sql = "select MaGia,MaLoaiPhong,MaLoaiGia,Gia,SoNgay from dmgia";
            tGia = _dbData.GetDataTable(sql);
            tGia.PrimaryKey = new DataColumn[] { tGia.Columns["MaGia"] };


            sql = "select * from dmNT";
            dmnt = _dbData.GetDataTable(sql);
            dmnt.PrimaryKey = new DataColumn[] { dmnt.Columns["MaNT"] };


            sql = "select * from dmThueSuat";
            dmThueSuat = _dbData.GetDataTable(sql);
            dmThueSuat.PrimaryKey = new DataColumn[] { dmThueSuat.Columns["MaThue"] };
            if (mt != null && mt["NgayDen"] != DBNull.Value && mt["NgayDi"] != DBNull.Value)
            {

                tbPhongTrong = _dbData.GetDataSetByStore("sp_GetPhongTrong", new string[] { "@NgayDen", "@NgayDi", "MT62ID" }, new object[] { mt["NgayDen"], mt["NgayDi"], mt["MT62ID"] });
                tbPhongTrong.PrimaryKey = new DataColumn[] { tbPhongTrong.Columns["MaPhong"] };
                tbPhongtrong_tmp = tbPhongTrong.Copy();
                removeCurrentRoom();
            }

        }

        private void removeCurrentRoom()
        {
            foreach (DataRow drcheck in dt.Rows)
            {
                TimeSpan ps1 = DateTime.Parse(drcheck["NgayDen"].ToString()) - DateTime.Parse(mt["NgayDi"].ToString());
                TimeSpan ps2 = DateTime.Parse(drcheck["NgayDi"].ToString()) - DateTime.Parse(mt["NgayDen"].ToString());
               // MessageBox.Show(drcheck["MaPhong"].ToString() + "   " + ps1.TotalMinutes.ToString() + "    " + ps2.TotalMinutes.ToString());  
                if (ps1.TotalMinutes * ps2.TotalMinutes / 10000 < 0)
                {
                    DataRow drFine = tbPhongtrong_tmp.Rows.Find(drcheck["MaPhong"].ToString());
                    if (drFine != null)
                    {
                        tbPhongtrong_tmp.Rows.Remove(drFine);
                    }
                    DataRow drFine1 = tbPhongTrong.Rows.Find(drcheck["MaPhong"].ToString());
                    if (drFine1 != null)
                    {
                        tbPhongTrong.Rows.Remove(drFine1);
                    }
                }
                
            }
        }
        private DataTable GetSoPhongdat(string MT61ID)
        {
            string sql = "select MaLoaiPhong,Soluong,GiaPhong,0 as Dacheck, Soluong as Conlai,NgayDen1,NgayDi1 from dt61 where MT61ID='" + MT61ID + "'";
            return _dbData.GetDataTable(sql);
        }
        private void getData(string MT62ID)
        {
            Mathue = _dbData.GetValue("select giatri from foConfig where TenTS='THUE'");
            string sql = "select getdate()";
             ngayht = _dbData.GetValue(sql);
            sql = "select * from mt62 where MT62ID='" + MT62ID.ToString() + "';select * from dt62 where MT62ID='" + MT62ID.ToString() + "'; select * from ct62 where DT62ID in (select DT62ID from dt62 where MT62ID='" + MT62ID.ToString() + "')";
            ds = _dbData.GetDataSet(sql);
           

            ds.Tables[0].Columns["SoNgay"].DefaultValue = 0;
            ds.Tables[0].Columns["MaThue"].DefaultValue = Mathue.ToString();
            ds.Tables[0].Columns["Thuesuat"].DefaultValue = double.Parse(Mathue.ToString());
            ds.Tables[0].Columns["MaNT"].DefaultValue = "VND";
            ds.Tables[0].Columns["TyGia"].DefaultValue = 1;
            
            ds.Tables[0].Columns["Datcoc"].DefaultValue = 0;
            ds.Tables[0].Columns["Conlai"].DefaultValue = 0;
            ds.Tables[0].Columns["TTienH"].DefaultValue = 0;
            ds.Tables[0].Columns["TThue"].DefaultValue = 0;
            ds.Tables[0].Columns["TTien"].DefaultValue = 0;
            ds.Tables[0].Columns["TongTien"].DefaultValue = 0;
            ds.Tables[0].Columns["notice"].DefaultValue = "";
            ds.Tables[0].Columns["CK"].DefaultValue = 0;
            ds.Tables[0].Columns["PtCk"].DefaultValue = 0;
            ds.Tables[0].Columns["SoNguoi"].DefaultValue = 1;
            ds.Tables[0].Columns["NgayDen"].DefaultValue = DateTime.Parse(ngayht.ToString());
            ds.Tables[1].Columns["NgayDen"].DefaultValue = DateTime.Parse(ngayht.ToString());
            ds.Tables[1].Columns["DaChuyen"].DefaultValue = false;

            mt = ds.Tables[0].Rows[0];
            mt["MT62ID"] = MT62ID;
            if (mt["MT61ID"] !=DBNull.Value)
            {
                sql = "select MaLoaiPhong,Soluong,0 as Dacheck, Soluong as Conlai from dt61 where MT61ID='" + mt["MT61ID"].ToString() + "'";
                tDSPhong = GetSoPhongdat(mt["MT61ID"].ToString());
            }

            dt = ds.Tables[1];
            ds.Tables[0].ColumnChanged += new DataColumnChangeEventHandler(mt_ColumnChanged);
            dt.ColumnChanged += new DataColumnChangeEventHandler(dt_ColumnChanged);
            dt.ColumnChanging += new DataColumnChangeEventHandler(dt_ColumnChanging);
            dt.RowDeleted += new DataRowChangeEventHandler(dt_RowDeleted);
            dt.RowDeleting += new DataRowChangeEventHandler(dt_RowDeleting);
            dt.TableNewRow += new DataTableNewRowEventHandler(fCheckin_TableNewRow);

            ct = ds.Tables[2];
            ct.TableNewRow += new DataTableNewRowEventHandler(ct_TableNewRow);
            ct.ColumnChanged += new DataColumnChangeEventHandler(ct_ColumnChanged);
            ct.RowDeleted += new DataRowChangeEventHandler(ct_RowDeleted);
            _lstDtRow.Clear();
            _lstCtRow.Clear();
            //bool tmp = false;
            //foreach (DataRow dr in dt.Rows)
            //{
            //    _lstDtRow.Add(dr);
            //    if (dr["isCheckIn"].ToString() == "True")
            //    {
            //        tmp = true;
            //    }
            //}
            //if (!tmp)
            //{
                //mt["NgayDen"] = DateTime.Parse(ngayht.ToString()).AddMinutes(5);
                //mt["NgayDi"] = DateTime.Now.AddDays(1);
            //}
            //else
            //{
                //foreach (DataRow dr in dt.Rows)
                //{
                //    if (dr["isCheckIn"].ToString() == "False")
                //    {
                //        dr["NgayDen"] = DateTime.Parse(ngayht.ToString());
                //        dr["NgayDi"] = mt["NgayDi"];
                //    }
                //}
            //}
            foreach (DataRow dr in ct.Rows)
                _lstCtRow.Add(dr);
            TinhSLPhongDaCheck();
            
            mt_ColumnChanged(mt.Table, new DataColumnChangeEventArgs(mt, mt.Table.Columns["NgayDen"], mt["NgayDen"]));
            mt_ColumnChanged(mt.Table,new DataColumnChangeEventArgs(mt,mt.Table.Columns["NgayDi"],mt["NgayDi"]));
            mt_ColumnChanged(mt.Table, new DataColumnChangeEventArgs(mt, mt.Table.Columns["SoNgay"], mt["SoNgay"]));
            mt_ColumnChanged(mt.Table, new DataColumnChangeEventArgs(mt, mt.Table.Columns["MaLoaiGia"], mt["MaLoaiGia"]));
           
        }

        void ct_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            if (e.Column.ColumnName == "MaPhong")
            {
                DataRow[] dr = dt.Select("MaPhong='" + e.Row["MaPhong"].ToString() + "'");
                if (dr.Length > 0) e.Row["DT62ID"] = dr[0]["DT62ID"];
                //else e.Row["MaPhong"] = DBNull.Value;
            }
        }


        

                 
        

        
        public void getData()
        {

            string sql = "select getdate()";
            ngayht = _dbData.GetValue(sql);
            Mathue = _dbData.GetValue("select giatri from foConfig where TenTS='THUE'");
            sql = "select * from mt62 where 1=0;select * from dt62 where 1=0; select * from ct62 where 1=0";
            ds = _dbData.GetDataSet(sql);

            ds.Tables[0].Columns["SoNgay"].DefaultValue = 0;
            ds.Tables[0].Columns["MaThue"].DefaultValue = Mathue.ToString();
            ds.Tables[0].Columns["Thuesuat"].DefaultValue = double.Parse(Mathue.ToString());
            ds.Tables[0].Columns["MaNT"].DefaultValue = "VND";
            ds.Tables[0].Columns["TyGia"].DefaultValue = "1";
            ds.Tables[0].Columns["Datcoc"].DefaultValue = 0;
            ds.Tables[0].Columns["Conlai"].DefaultValue = 0;
            ds.Tables[0].Columns["SoNguoi"].DefaultValue = 1;
            ds.Tables[0].Columns["TTienH"].DefaultValue = 0;
            ds.Tables[0].Columns["TThue"].DefaultValue = 0;
            ds.Tables[0].Columns["TTien"].DefaultValue = 0;
            ds.Tables[0].Columns["TongTien"].DefaultValue = 0;
            ds.Tables[0].Columns["TTienMNB"].DefaultValue = 0;
            ds.Tables[0].Columns["TThueMNB"].DefaultValue = 0;
            ds.Tables[0].Columns["TTienDV"].DefaultValue = 0;
            ds.Tables[0].Columns["TThueDV"].DefaultValue = 0;
            ds.Tables[0].Columns["notice"].DefaultValue = "";
            ds.Tables[0].Columns["CK"].DefaultValue = 0;
            ds.Tables[0].Columns["MaPTTT"].DefaultValue = "CASH";
            ds.Tables[0].Columns["PtCk"].DefaultValue = 0;
            ds.Tables[0].Columns["CKinRoom"].DefaultValue = 1;
            ds.Tables[0].Columns["NgayDen"].DefaultValue = DateTime.Parse(ngayht.ToString());
            ds.Tables[1].Columns["NgayDen"].DefaultValue = DateTime.Parse(ngayht.ToString());
            mt = ds.Tables[0].NewRow();
            mt["MT62ID"] = Guid.NewGuid();
            ds.Tables[0].Rows.Add(mt);
            dt=ds.Tables[1];
            ds.Tables[0].ColumnChanged += new DataColumnChangeEventHandler(mt_ColumnChanged);
            dt.ColumnChanged += new DataColumnChangeEventHandler(dt_ColumnChanged);
            dt.ColumnChanging += new DataColumnChangeEventHandler(dt_ColumnChanging);
            dt.RowDeleted += new DataRowChangeEventHandler(dt_RowDeleted);
            dt.RowDeleting += new DataRowChangeEventHandler(dt_RowDeleting);
            dt.TableNewRow += new DataTableNewRowEventHandler(fCheckin_TableNewRow);

            ct = ds.Tables[2];
            ct.TableNewRow += new DataTableNewRowEventHandler(ct_TableNewRow);
            ct.ColumnChanged   +=new DataColumnChangeEventHandler(ct_ColumnChanged);
            ct.RowDeleted += new DataRowChangeEventHandler(ct_RowDeleted);
            DateTime ngayden = DateTime.Parse((DateTime.Parse(ngayht.ToString())).ToShortDateString() + " 14:00");
            if (ngayden < DateTime.Parse(ngayht.ToString()).AddMinutes(10)) ngayden=DateTime.Parse(ngayht.ToString()).AddMinutes(10);
            mt["NgayDen"] = ngayden;
            mt.EndEdit();
           

        }

 #region Table_event
       
        void mt_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            try
            {
                if (e.Column.ColumnName == "MT61ID")
                {
                    DataRow dr = tMt61.Rows.Find(e.Row["MT61ID"]);
                    if (dr == null) return;
                    mt["MaKhach"] = dr["MaKhach"];
                    mt["NgayDen"] = dr["NgayDen"];

                    mt["NgayDi"] = dr["NgayDi"];
                    if (DateTime.Parse(mt["NgayDi"].ToString()) <= DateTime.Parse(mt["NgayDen"].ToString()))
                    { mt["NgayDi"] = mt["NgayDen"]; }
                    mt["OngBa"] = dr["Ongba"];
                    mt["SoNguoi"] = dr["SoNguoi"];
                    mt["MaPTTT"] = dr["MaPTTT"];
                    mt["MaHang"] = dr["MaHang"];
                    mt["notice"] = dr["notice"];
                    //mt["MaLoaiGia"] = dr["MaLoaiGia"];
                    mt.EndEdit();
                    //cNgaydat.EditValue = DateTime.Parse(dr["NgayCT"].ToString());
                    string sql = "select MaLoaiPhong,Soluong,0 as Dacheck, Soluong as Conlai from dt61 where MT61ID='" + e.Row["MT61ID"].ToString() + "'";
                    tDSPhong = GetSoPhongdat(e.Row["MT61ID"].ToString());

                    dt.Clear();
                    this.MT61_changed(sender, e);


                }
                else if (e.Column.ColumnName == "MaKhach")
                {
                    if (mt["MaKhach"] != null)
                    {
                        DataRow dr = tkhach.Rows.Find(mt["MaKhach"].ToString());
                        if (dr != null)
                        {
                            mt["Ongba"] = dr["Ongba"];
                            ct.Columns["TenKhach"].DefaultValue = dr["Ongba"];
                            ct.Columns["DiaChi"].DefaultValue = dr["Diachi"];
                            ct.Columns["SDT"].DefaultValue = dr["SDT"];
                            ct.Columns["Email"].DefaultValue = dr["Email"];
                            ct.Columns["MaGiayto"].DefaultValue = dr["MaGiayto"];
                            ct.Columns["SOID"].DefaultValue = dr["SOID"];
                            ct.Columns["MaQuocGia"].DefaultValue = dr["MaQuocGia"];
                            ct.Columns["MaTinh"].DefaultValue = dr["MaTinh"];
                            if (dr["SoThe"] == DBNull.Value) mt["PtCk"] = 0;
                            else 
                            {
                                DataRow[] ldr = tLoaithe.Select("MaLoaithe='" + dr["Maloaithe"].ToString() + "'");
                                if (ldr.Length == 1) mt["PtCk"] = double.Parse(ldr[0]["PtCk"].ToString());
                            }
                            foreach (DataRow drct in ct.Rows)
                            {
                                if (drct["TenKhach"] == DBNull.Value || drct["TenKhach"].ToString() == string.Empty)
                                {
                                    drct["TenKhach"] = dr["Ongba"];
                                    drct["DiaChi"] = dr["Diachi"];
                                    drct["SDT"] = dr["SDT"];
                                    drct["Email"] = dr["Email"];
                                    drct["MaGiayto"] = dr["MaGiayto"];
                                    drct["SOID"] = dr["SOID"];
                                    drct["MaQuocGia"] = dr["MaQuocGia"];
                                    drct.EndEdit();
                                }
                            }
                        }
                        
                    }
                }
                else if (e.Column.ColumnName == "SoNgay")
                {
                    if (mt["SoNgay"] != null)
                    {
                       dt.Columns["SoNgay"].DefaultValue = mt["SoNgay"];
                       
                        //setAll("SoNgay",mt["SoNgay"]);
                       foreach (DataRow dr in dt.Rows)
                       {
                           if ((!bool.Parse(dr["isCheckIn"].ToString()) && !bool.Parse(dr["isCheckOut"].ToString())) && mt["MT61ID"] ==DBNull.Value)
                               dr["NgayDi"] = mt["NgayDi"];
                       }
                    }
                }
                else if (e.Column.ColumnName == "NgayDen")
                {
                    dt.Columns["NgayDen"].DefaultValue = mt["NgayDen"];
                    dt.Columns["NgayTT"].DefaultValue = mt["NgayDen"];
                    //setAll("NgayDen",mt["NgayDen"]);
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["isCheckIn"] != DBNull.Value && !bool.Parse(dr["isCheckin"].ToString()) && mt["MT61ID"] ==DBNull.Value)
                            dr["NgayDen"] = mt["NgayDen"];
                    }
                    setLoaiGiaAll();
                }
                else if (e.Column.ColumnName == "NgayDi")
                {
                    dt.Columns["NgayDi"].DefaultValue = mt["NgayDi"];
                    
                   // setAll("NgayDi", mt["NgayDi"]);
                    foreach (DataRow dr in dt.Rows)
                    {
                        if ((((dr["isCheckOut"].ToString()==string.Empty || !bool.Parse(dr["isCheckOut"].ToString())  ) && ( dr["isCheckin"].ToString()!=string.Empty && !bool.Parse(dr["isCheckIn"].ToString())) )  || dr["ngaydi"]==DBNull.Value)  && mt["MT61ID"] ==DBNull.Value)
                            dr["NgayDi"] = mt["NgayDi"];
                    }
                    setLoaiGiaAll();
                }
                else if (e.Column.ColumnName == "MaLoaigia")
                {
                    if (mt["MaLoaiGia"] != null)
                    {
                        
                        setMaGiaAll(mt["MaLoaiGia"].ToString());

                        MaLoaiGia_changed(sender, e);
                    }
                }
                else if (e.Column.ColumnName == "MaNT")
                {
                    if (mt["MaNT"] != null)
                    {
                        DataRow dr = dmnt.Rows.Find(mt["MaNT"].ToString());
                        if (dr != null)
                        {
                            mt["TyGia"] = dr["TyGia"];
                        }
                    }
                }
                else if (e.Column.ColumnName == "MaThue")
                {
                    if (mt["MaThue"] != null)
                    {
                        DataRow dr = dmThueSuat.Rows.Find(mt["MaThue"].ToString());
                        if (dr != null)
                        {
                            mt["Thuesuat"] = dr["Thuesuat"];
                        }
                    }
                }
                else if (e.Column.ColumnName == "CKinRoom")
                {
                    if (bool.Parse(mt["CkInRoom"].ToString()))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {

                            dr["GiaPhong"] = Math.Round(double.Parse(dr["GiaPhong1"].ToString()) * (1 - double.Parse(mt["PtCK"].ToString()) / 100));

                        }
                    }
                    else
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr["GiaPhong"] = dr["GiaPhong1"];
                        }
                    }
                }
                else if (e.Column.ColumnName == "PtCK")
                {
                    if (bool.Parse(mt["CkInRoom"].ToString()))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {

                            dr["GiaPhong"] = Math.Round(double.Parse(dr["GiaPhong1"].ToString()) * (1 - double.Parse(mt["PtCK"].ToString()) / 100));

                        }
                    }
                    if (!bool.Parse(mt["CKInRoom"].ToString()))
                    {
                        mt["CK"] = double.Parse(mt["TTienH"].ToString()) * double.Parse(mt["PtCk"].ToString()) / 100;
                    }
                    else
                    {
                        mt["CK"] = 0;
                    }
                    if (mt["Thuesuat"] != null)
                    {
                        mt["TThue"] = double.Parse(mt["Thuesuat"].ToString()) * (double.Parse(mt["TTienH"].ToString()) - double.Parse(mt["CK"].ToString())) / 100;
                    }

                    mt["TTien"] = double.Parse(mt["TThue"].ToString()) + double.Parse(mt["TTienH"].ToString()) - double.Parse(mt["CK"].ToString());

                }
                else if (e.Column.ColumnName == "TTienH")
                {
                    if (!bool.Parse(mt["CKInRoom"].ToString()))
                    {
                        mt["CK"] = double.Parse(mt["TTienH"].ToString()) * double.Parse(mt["PtCk"].ToString()) / 100;
                    }
                    else
                    {
                        mt["CK"] = 0;
                    }
                    if (mt["Thuesuat"] != null)
                    {
                        mt["TThue"] = double.Parse(mt["Thuesuat"].ToString()) * (double.Parse(mt["TTienH"].ToString()) - double.Parse(mt["CK"].ToString())) / 100;
                    }

                    mt["TTien"] = double.Parse(mt["TThue"].ToString()) + double.Parse(mt["TTienH"].ToString()) - double.Parse(mt["CK"].ToString());
                    
                }

                else if (e.Column.ColumnName == "TTien")
                {
                    mt["TongTien"] = double.Parse(mt["TTien"].ToString()) + double.Parse(mt["TTienMNB"].ToString()) + double.Parse(mt["TThueMNB"].ToString()) + double.Parse(mt["TTienDV"].ToString()) + double.Parse(mt["TThueDV"].ToString());
                    mt["Conlai"] = double.Parse(mt["TongTien"].ToString()) - double.Parse(mt["Datcoc"].ToString());
                }
                else if (e.Column.ColumnName == "Datcoc")
                {
                    mt["Conlai"] = double.Parse(mt["TongTien"].ToString()) - double.Parse(mt["Datcoc"].ToString());
                }
                else if (e.Column.ColumnName == "Thuesuat")
                {
                    if (mt["Thuesuat"] != null)
                    {
                        mt["TThue"] = double.Parse(mt["Thuesuat"].ToString()) * double.Parse(mt["TTienH"].ToString()) / 100;
                    }
                }
                else if (e.Column.ColumnName == "TThue")
                {
                    mt["TTien"] = double.Parse(mt["TThue"].ToString()) + double.Parse(mt["TTienH"].ToString());
                }
                else if (e.Column.ColumnName == "isCheckIn")
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["isCheckIn"] = mt["isCheckIn"];
                    }
                }
                mt.EndEdit();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        void dt_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            try
            {
                if (e.Column.ColumnName == "MaPhong")
                {
                    if (e.Row["MaPhong"] != null)
                    {
                        DataRow dr = tPhong.Rows.Find(e.Row["MaPhong"].ToString());
                        if (dr != null)
                        {
                            e.Row["MaLoaiPhong"] = dr["MaLoaiPhong"];
                        }
                        SelectPhong(e.Row["MaPhong"].ToString(), 1);
                        updateCt(e.Row);
                    }
                }
                if (e.Column.ColumnName == "MaLoaiPhong")
                {
                    if (mt["MaLoaiGia"] != null && mt["MaLoaiGia"].ToString() != "")
                    {

                        DataRow[] drGia = tGia.Select("MaLoaiPhong='" + e.Row["MaLoaiPhong"].ToString() + "' and MaLoaiGia='" + mt["MaLoaiGia"].ToString() + "'");
                        if (drGia.Length > 0)
                        {
                            e.Row["MaGia"] = drGia[0]["MaGia"];
                            if (double.Parse(mt["PtCk"].ToString()) > 0 && bool.Parse(mt["CKInRoom"].ToString()) == true)
                            {
                                e.Row["GiaPhong"] =Math.Round(double.Parse(drGia[0]["Gia"].ToString()) * (1- double.Parse(mt["PtCk"].ToString()) /100)) ;
                            }
                            else
                            {
                                e.Row["GiaPhong"] = drGia[0]["Gia"];
                            }
                            e.Row["GiaPhong1"] = drGia[0]["Gia"];
                            e.Row["SoNT"] = drGia[0]["SoNgay"];
                        }
                        
                    }
                    GenChonkhach(e.Row);
                }
                if (e.Column.ColumnName == "MaGia")
                {
                    DataRow[] drGia = tGia.Select("MaGia=" + e.Row["MaGia"]);
                    if (drGia.Length > 0)
                    {
                       // e.Row["MaGia"] = drGia[0]["MaGia"];
                        if (double.Parse(mt["PtCk"].ToString()) > 0 && mt["CKinRoom"]!=DBNull.Value && bool.Parse(mt["CKInRoom"].ToString()) == true)
                        {
                            e.Row["GiaPhong"] = Math.Round(double.Parse(drGia[0]["Gia"].ToString()) * (1- double.Parse(mt["PtCk"].ToString()) / 100));
                        }
                        else
                        {
                            e.Row["GiaPhong"] = drGia[0]["Gia"];
                        }
                        e.Row["SoNT"] = drGia[0]["SoNgay"];
                    }
                }
                if (e.Column.ColumnName == "NgayDen" || e.Column.ColumnName == "NgayDi")
                {
                    e.Row["NgayTT"] = e.Row["NgayDen"];
                    setLoaiGia(e.Row);
                }
                if (e.Column.ColumnName == "SoNT")
                {
                    TinhTienPhong(e.Row);
                }
                if (e.Column.ColumnName == "SoNgay")
                {
                    TinhTienPhong(e.Row);
                }
                if (e.Column.ColumnName == "GiaPhong")
                {
                    TinhTienPhong(e.Row);
                }
                if (e.Column.ColumnName == "ps")
                {
                    TinhTongTien();
                }
            }
            catch (Exception ex)
            {
            }
        }


        void dt_ColumnChanging(object sender, DataColumnChangeEventArgs e)
        {
            
        }
        void dt_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            TinhTongTien();
            if (!_lstDtRow.Contains(e.Row)) _lstDtRow.Add(e.Row);
            dt_deleted(sender,new DataTableClearEventArgs(dt));
        }
        void dt_RowDeleting(object sender, DataRowChangeEventArgs e)
        {
            DelChonkhach(e.Row);
        }
        void fCheckin_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["DT62ID"] = Guid.NewGuid();
            _lstDtRow.Add(e.Row);
        }
        void ct_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            if (!_lstCtRow.Contains(e.Row)) _lstCtRow.Add(e.Row);
        }

        void ct_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            _lstCtRow.Add(e.Row);
            
            e.Row["CT62ID"] = Guid.NewGuid();
        }
        private void DelChonkhach(DataRow drdt)
        {
            for (int i = ct.Rows.Count - 1; i >= 0; i--)
            {
                if (ct.Rows[i]["DT62ID"].ToString() == drdt["DT62ID"].ToString()) ct.Rows.RemoveAt(i);
            }
        }
        private void updateCt(DataRow drdt)
        {
            foreach (DataRow drct in ct.Rows)
            {
                if (drct["DT62ID"].ToString() == drdt["DT62ID"].ToString()) drct["MaPhong"] = drdt["MaPhong"].ToString();
            }
        }
        private void GenChonkhach(DataRow drdt)
        {

            DataRow drLP = tLoaiphong.Rows.Find(drdt["MaLoaiPhong"]);
            if (drLP == null)
            {
                ct.Clear();
                return;
            }
            if (drLP["SoNguoi"].ToString() == "")
            {
                ct.Clear();
                return;
            }
            DelChonkhach(drdt);
            //for (int i = 0; i < int.Parse(drLP["SoNguoi"].ToString()); i++)
            //{
                DataRow drct = ct.NewRow();
                drct["DT62ID"] = drdt["DT62ID"];
                drct["CT62ID"] = Guid.NewGuid();
                ds.Tables[2].Rows.Add(drct);
           // }

        }
        private  void setLoaiGiaAll()
        {
            
            if (mt["NgayDi"] == null || mt["NgayDen"] == null) return;
            tbPhongTrong = _dbData.GetDataSetByStore("sp_GetPhongTrong", new string[] { "@NgayDen", "@NgayDi", "MT62ID" }, new object[] { mt["NgayDen"], mt["NgayDi"], mt["MT62ID"] });
            tbPhongTrong.PrimaryKey = new DataColumn[] { tbPhongTrong.Columns["MaPhong"] };
            tbPhongtrong_tmp = tbPhongTrong.Copy();
            removeCurrentRoom();
            this.Ngay_Changed(this, new EventArgs());
            TimeSpan tsp = DateTime.Parse(mt["NgayDi"].ToString()) - DateTime.Parse(mt["NgayDen"].ToString());

            if (tLoaigia.Rows.Count < 2) return;
            if (double.Parse(tLoaigia.Rows[1]["Muctinh"].ToString()) < tsp.TotalDays)
            {
                if (isUsingTimeOut.ToString() == "0")
                {
                    mt["SoNgay"] = Math.Ceiling(tsp.TotalDays);      //Cái này tính phức tạp dựa vào giờ vào và ra.
                }
                else
                {
                    mt["SoNgay"] = TinhSoNgay(mt["NgayDen"], mt["NgayDi"]);
                }
            }
            else
            {
                if (tsp.TotalHours < 1)
                {
                    mt["SoNgay"] = 1.0 / 24;
                }
                else
                {
                    mt["SoNgay"] = Math.Ceiling(tsp.TotalDays * 24) / 24.0;
                }
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
            if (mt["MaLoaiGia"].ToString() != findDr["MaLoaiGia"].ToString())
                mt["MaLoaiGia"] = findDr["MaLoaiGia"];
        }
        private void setLoaiGia(DataRow dr)
        {
            if (dr["NgayDi"] == null || dr["NgayDen"] == null) return;
            TimeSpan tsp = DateTime.Parse(dr["NgayDi"].ToString()) - DateTime.Parse(dr["NgayDen"].ToString());

            if (tLoaigia.Rows.Count < 2) return;
            if (double.Parse(tLoaigia.Rows[1]["Muctinh"].ToString()) < tsp.TotalDays)
            {
                if (isUsingTimeOut.ToString() == "0")
                {
                    dr["SoNgay"] = Math.Ceiling(tsp.TotalDays);        //Cái này tính phức tạp dựa vào giờ vào và ra.
                }
                else
                {
                    dr["Songay"] = TinhSoNgay(dr["NgayDen"], dr["NgayDi"]);
                }
            }
            else
            {
                if (tsp.TotalHours < 1)
                {
                    dr["SoNgay"] = 1.0 / 24;
                }
                else
                {
                    dr["SoNgay"] = Math.Ceiling(tsp.TotalDays*24)/24.0;
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
            setMaGia(dr, findDr["MaLoaiGia"].ToString());           
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
#endregion
        public bool TinhSLPhongDaCheck()
        {
            if (tDSPhong == null) return false;
            if (tDSPhong.Rows.Count == 0) return false;
            bool re = true;
            foreach (DataRow Mdr in tDSPhong.Rows)
            {
                Mdr["DaCheck"] = 0;
                dt.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
                foreach (DataRowView dr in dt.DefaultView)
                {
                    if (dr["MaLoaiPhong"].ToString() == Mdr["MaLoaiPhong"].ToString() && dr["NgayDen"].ToString() == Mdr["NgayDen1"].ToString() && dr["NgayDi"].ToString() == Mdr["NgayDi1"].ToString())
                    {
                        Mdr["DaCheck"] = int.Parse(Mdr["DaCheck"].ToString()) + 1;
                        Mdr["Conlai"] = int.Parse(Mdr["Soluong"].ToString()) - int.Parse(Mdr["DaCheck"].ToString());
                    }
                }
                if (int.Parse(Mdr["Conlai"].ToString()) <= 0)
                    re = re && true;
                else
                    re = false;
            }
            return re;
        }
        public void setAll(string fi,object val)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr[fi] = val;
                }
            }
            catch
            {
            }
        }
        public void setMaGia(DataRow dr, string val)
        {
            try
            {                
                    DataRow[] drGia = tGia.Select("MaLoaiPhong='" + dr["MaLoaiPhong"].ToString() + "' and MaLoaiGia='" + val + "'");
                    if (drGia.Length > 0 &&  dr["MaGia"].ToString() != drGia[0]["MaGia"].ToString())
                    {
                        dr["MaGia"] = drGia[0]["MaGia"];
                        dr["GiaPhong"] = drGia[0]["Gia"];
                        dr["GiaPhong1"] = drGia[0]["Gia"];
                        dr["SoNT"] = drGia[0]["SoNgay"];
                    }
            }
            catch
            {
            }
        }
        public void setMaGiaAll(string val)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow[] drGia = tGia.Select("MaLoaiPhong='" + dr["MaLoaiPhong"].ToString() + "' and MaLoaiGia='" + val + "'");
                    if (drGia.Length > 0)
                    {
                        dr["MaGia"] = drGia[0]["MaGia"];
                        dr["GiaPhong1"] = drGia[0]["Gia"];
                        if(bool.Parse(mt["CKinRoom"].ToString()))
                        {
                            dr["GiaPhong"] = Math.Round(double.Parse(dr["GiaPhong1"].ToString()) * (1 - double.Parse(mt["PtCk"].ToString()) / 100));
                        }
                        else
                            dr["GiaPhong"] = drGia[0]["Gia"];

                        dr["SoNT"] = drGia[0]["SoNgay"];
                    }
                }
            }
            catch
            {
            }
        }
        private void TinhTienPhong(DataRow dt)
        {
            //trường hợp tính giá theo giờ
            double thuesuat=double.Parse(mt["Thuesuat"].ToString());
            if (double.Parse(dt["SoNT"].ToString()) < 1)
            {
                double sogio = double.Parse(dt["SoNgay"].ToString()) * 24;
                if (isThuePhong.ToString() == "0")
                {
                    dt["ps"] =Math.Round( double.Parse(dt["GiaPhong"].ToString())/(1+thuesuat/100),0);
                }
                else
                {
                    dt["ps"] =  double.Parse(dt["GiaPhong"].ToString());
                }
            }
            else
            {
                if (isThuePhong.ToString() == "0")
                {
                    dt["ps"] = Math.Round(double.Parse(dt["SoNgay"].ToString()) * double.Parse(dt["GiaPhong"].ToString()) / ((1 + thuesuat / 100) * double.Parse(dt["SoNT"].ToString())));
                }
                else
                {
                    dt["ps"] = Math.Round(double.Parse(dt["SoNgay"].ToString()) * double.Parse(dt["GiaPhong"].ToString()) /  double.Parse(dt["SoNT"].ToString()));
                }
            }
        }
        public void TinhTongTien()
        {
            double TTienH = 0;
            dt.DefaultView.RowStateFilter = DataViewRowState.Added | DataViewRowState.Unchanged | DataViewRowState.ModifiedCurrent;
            foreach (DataRowView dr in dt.DefaultView)
            {
                TTienH += double.Parse(dr["Ps"].ToString());
            }
            mt["TTienH"] = TTienH;
      
           
        }
        private void SelectPhong(string p, int p_2)
        {
            foreach (DataRow dr in tPhong.Rows)
            {
                if (dr["MaPhong"].ToString() == p)
                {
                    dr["isSelected"] = p_2;
                    break;
                }
            }
        }
        public string ChonPhongTudong()
        {
            string re = "";
            //if (mt["NgayDen"] == null || mt["NgayDi"] == null||tbPhongTrong==null)
            //{
            //    return "Chưa chọn ngày đến, ngày đi";
            //}
            
            
            dt.Clear();
            ct.Clear();
            foreach (DataRow drLoaiPhongDat in tDSPhong.Rows)
            {
                mt["NgayDen"] = drLoaiPhongDat["NgayDen1"];
                mt["NgayDi"] = drLoaiPhongDat["NgayDi1"];
                if (tbPhongTrong == null)
                {
                    return "Lấy số liệu bị lỗi";
                }
                //xóa những phòng đã chọn

                    //foreach (DataRow drcheck in dt.Rows)
                    //{
                    //    TimeSpan ps1 = DateTime.Parse(drcheck["NgayDen"].ToString()) - DateTime.Parse(mt["NgayDi"].ToString());
                    //    TimeSpan ps2 = DateTime.Parse(drcheck["NgayDi"].ToString()) - DateTime.Parse(mt["NgayDen"].ToString());
                    //    if (ps1.TotalMinutes * ps2.TotalMinutes / 10000 < 0)
                    //    {
                    //        DataRow drFine = tbPhongTrong.Rows.Find(drcheck["MaPhong"].ToString());
                    //        if (drFine != null)
                    //            tbPhongTrong.Rows.Remove(drFine);
                    //    }
                    //}

                string MaLoaiPhong = drLoaiPhongDat["MaLoaiPhong"].ToString();
                DataRow[] lstPhong = tbPhongTrong.Select("MaLoaiPhong='" + MaLoaiPhong + "'");
                if (lstPhong.Length < int.Parse(drLoaiPhongDat["soluong"].ToString()))
                {
                    re += " Loại phòng " + MaLoaiPhong + " không đủ số lượng;/n";
                    for (int i = 0;i< lstPhong.Length; i++)
                    {
                        DataRow drdt = dt.NewRow();
                        dt.Rows.Add(drdt);
                        drdt["MaPhong"] = lstPhong[i]["MaPhong"].ToString();
                        drdt["GiaPhong"] = drLoaiPhongDat["GiaPhong"];
                        DataRow drFine = tbPhongtrong_tmp.Rows.Find(drdt["MaPhong"].ToString());
                        if (drFine != null)
                            tbPhongtrong_tmp.Rows.Remove(drFine);
                    }
                }
                else
                {
                    for (int i = 0; i < int.Parse(drLoaiPhongDat["soluong"].ToString()); i++)
                    {
                        DataRow drdt = dt.NewRow();
                        dt.Rows.Add(drdt);
                        drdt["MaPhong"] = lstPhong[i]["MaPhong"].ToString();
                        drdt["GiaPhong"] = drLoaiPhongDat["GiaPhong"];
                        
                        DataRow drFine = tbPhongtrong_tmp.Rows.Find(drdt["MaPhong"].ToString());
                        if (drFine != null)
                            tbPhongtrong_tmp.Rows.Remove(drFine);
                    }
                }
            }
            return re;
        }
        public bool UpdateData()
        {
            bool re = true;
            isUpdating = true;
            if (!_isEdit)
                re = re && InsertToDatabase();
            else
                re = re && UpdateToDatabase();


            return re;
        }
        private bool InsertToDatabase()
        {
            string sql;
            _dbData.BeginMultiTrans();
            //Guid Gu = Guid.NewGuid();

            string soct = GetSoCT();
            try
            {
                if (mt["MT61ID"] == null || mt["MT61ID"].ToString() == "")
                {
                    sql = "insert into mt62 (Mt62ID,SoCT, MaKhach, NgayDen, NgayDi,OngBa, SoNguoi,PtCk,CKinRoom,";
                    sql += (mt["MaHang"].ToString() == "") ? "" : "MaHang,";
                    sql += " MaPTTT, MaNT,TyGia," + "MaThue, ThueSuat, Songay, TTienH,TThue, CK, TTien,TongTien,Datcoc, Conlai, MaLoaiGia,notice, isCheckIn)values('" +
                              mt["MT62ID"].ToString() + "','" + soct + "','" + mt["MaKhach"].ToString() + "','" + mt["NgayDen"].ToString() + "','" +
                              mt["NgayDi"].ToString() + "',N'" + mt["Ongba"].ToString() + "'," + mt["SoNguoi"].ToString() +
                              "," + mt["PtCK"].ToString() + "," + (bool.Parse(mt["CKInRoom"].ToString()) ? 1 : 0).ToString() + ",'";
                                
                    sql += (mt["MaHang"].ToString() == "") ? "" : mt["MaHang"].ToString() + "','";
                    sql += mt["MaPTTT"].ToString() + "','" + mt["MaNT"].ToString() + "'," +
                              mt["TyGia"].ToString() + ",'" + mt["MaThue"].ToString() + "'," + mt["ThueSuat"].ToString() + "," +
                              mt["SoNgay"].ToString() + "," + mt["TTienH"].ToString() + "," + mt["TThue"].ToString() + "," + mt["CK"].ToString() + "," +
                              mt["TTien"].ToString() + "," + mt["TongTien"].ToString() + "," + mt["Datcoc"].ToString() + "," +
                              mt["ConLai"].ToString() + ",'" + mt["MaLoaiGia"].ToString() + "',N'" + mt["notice"].ToString() + "',0)";

                }
                else
                {
                    sql = "insert into mt62 (Mt62ID,Mt61ID,SoCT, MaKhach, NgayDen, NgayDi,OngBa, SoNguoi,Ptck,CKinRoom,";
                    sql += (mt["MaHang"].ToString() == "") ? "" : "MaHang,";
                    sql += " MaPTTT, MaNT,TyGia," + "MaThue, ThueSuat, Songay, TTienH,TThue, CK, TTien,TongTien,Datcoc, Conlai, MaLoaiGia,notice, isCheckIn)values('" +
                               mt["MT62ID"].ToString() + "','" + mt["MT61ID"].ToString() + "','" + soct + "','" + mt["MaKhach"].ToString() + "','" + mt["NgayDen"].ToString() + "','" +
                              mt["NgayDi"].ToString() + "',N'" + mt["Ongba"].ToString() + "'," + mt["SoNguoi"].ToString() +
                               "," + mt["PtCK"].ToString() + "," + (bool.Parse(mt["CKInRoom"].ToString()) ? 1 : 0).ToString() + ",'";
                    sql += (mt["MaHang"].ToString() == "") ? "" : mt["MaHang"].ToString() + "','";
                    sql += mt["MaPTTT"].ToString() + "','" + mt["MaNT"].ToString() + "'," +
                              mt["TyGia"].ToString() + ",'" + mt["MaThue"].ToString() + "'," + mt["ThueSuat"].ToString() + "," +
                              mt["SoNgay"].ToString() + "," + mt["TTienH"].ToString() + "," + mt["TThue"].ToString() + "," + mt["CK"].ToString() + "," + 
                              mt["TTien"].ToString() + "," + mt["TongTien"].ToString() + ","+ mt["Datcoc"].ToString() + "," +
                              mt["ConLai"].ToString() + ",'" + mt["MaLoaiGia"].ToString() + "',N'" + mt["notice"].ToString() + "',0)";

                }
                _dbData.UpdateByNonQuery(sql);
                if (_dbData.HasErrors)
                {
                    _dbData.RollbackMultiTrans();
                    return false;
                }
                                 
                 foreach (DataRow rdt in dt.Rows)
                 {
                     //Kiểm tra phòng
                     tbPhongTrong = _dbData.GetDataSetByStore("sp_GetPhongTrong", new string[] { "@NgayDen", "@NgayDi", "MT62ID" }, new object[] { rdt["NgayDen"], rdt["NgayDi"], mt["MT62ID"] });
                     if (tbPhongTrong.Select("MaPhong='" + rdt["MaPhong"].ToString() + "'").Length == 0)
                     {
                         _dbData.RollbackMultiTrans();
                         return false;
                     }

                     sql = "insert into dt62 (MT62ID, DT62ID,MaPhong, GiaPhong,GiaPhong1,Ps,SoNT, NgayDen, NgayDi,MaLoaiPhong, MaGia,NgayTT,DaChuyen,SoNgay) values('" +
                          mt["MT62ID"].ToString() + "','" + rdt["DT62ID"].ToString() + "','" + rdt["MaPhong"].ToString() + "'," + rdt["GiaPhong"].ToString() + "," + rdt["GiaPhong1"].ToString() + "," +
                         rdt["Ps"].ToString() + "," + rdt["SoNT"].ToString() + ",'" + rdt["NgayDen"].ToString() + "','" + rdt["NgayDi"].ToString() + "','" +
                         rdt["MaLoaiPhong"].ToString() + "'," + rdt["MaGia"].ToString() + ",'" + rdt["NgayTT"].ToString() + "',0," + rdt["SoNgay"].ToString() + ")";
                     _dbData.UpdateByNonQuery(sql);
                     
                     if (_dbData.HasErrors)
                     {
                         _dbData.RollbackMultiTrans();
                         return false;
                     }

                 }

                foreach (DataRow dr in ct.Rows)
                {
                    if (dr["TenKhach"].ToString() == "") dr["TenKhach"]="Chưa có tên";
                    if (dr["MaGiayTo"].ToString() == "") dr["MaGiayTo"] = "CMND";

                        sql = "insert into ct62 (DT62ID, CT62ID, MaPhong, TenKhach, DiaChi, SDT, Email,MaGiayto, SoID) values('" +
                            dr["DT62ID"].ToString() + "','" + dr["CT62ID"].ToString() + "','" + dr["MaPhong"].ToString() + "',N'" +
                            dr["TenKhach"].ToString() + "',N'" + dr["DiaChi"].ToString() + "',N'" + dr["SDT"].ToString() + "',N'" + dr["Email"].ToString() + "','" +
                            dr["MaGiayto"].ToString() + "',N'" + dr["SoID"].ToString() + "')";
                    _dbData.UpdateByNonQuery(sql);
                    if (_dbData.HasErrors)
                    {
                        _dbData.RollbackMultiTrans();
                        return false;
                    }
                }
                if (mt["MT61ID"] != null && mt["MT61ID"].ToString() != "")
                {
                    sql = "update Mt61 set isVisible=0 where Mt61ID='" + mt["MT61ID"].ToString() + "'";
                    _dbData.UpdateByNonQuery(sql);
                    if (_dbData.HasErrors)
                    {
                        _dbData.RollbackMultiTrans();
                        return false;
                    }
                }
                mt.Table.AcceptChanges();
                dt.AcceptChanges();
                ct.AcceptChanges();
                this._isEdit = true;
                _dbData.EndMultiTrans();
                return true;
            }
            catch 
            {
               
                if (_dbData.HasErrors)
                    _dbData.RollbackMultiTrans();
                return false;
            }
        }
        private bool UpdateToDatabase()
        {
             string sql;
            _dbData.BeginMultiTrans();
            try
            {
                if (mt["MT61ID"] == null || mt["MT61ID"].ToString() == "")
                {
                    sql = "UPDATE MT62 SET  MaKhach='" + mt["MaKhach"].ToString() + "', NgayCT='" + mt["NgayCT"].ToString() + "', NgayDen='" + mt["NgayDen"].ToString() + "', NgayDi='" + mt["NgayDi"].ToString() + "', OngBa=N'" + mt["OngBa"].ToString() + "', SoNguoi=" + mt["SoNguoi"].ToString() + "," ;
                    sql+=( mt["MaHang"].ToString()=="")? "":(" MaHang ='" + mt["MaHang"].ToString() + "',");
                    sql += "PtCk=" + mt["PtCK"].ToString() + ", CK=" + mt["CK"].ToString() + ",";
                    sql += " CKinRoom = " + (bool.Parse(mt["CKInRoom"].ToString()) ? 1 : 0).ToString() + ",";
                    sql += " notice=N'" + mt["notice"].ToString() + "',";
                    sql += " MaPTTT='" + mt["MaPTTT"].ToString() + "', MaNT='" + mt["MaNT"].ToString() + "', MaThue='" + mt["MaThue"].ToString() + "', Thuesuat=" + mt["ThueSuat"].ToString() + ", SoNgay=" + mt["SoNgay"].ToString() + ", TTienH=" + mt["TTienH"].ToString() + ", TThue=" + mt["TThue"].ToString() + ", TTien=" + mt["TTien"].ToString() + ", TongTien=" + mt["TongTien"].ToString() + ",";
                    sql += " Datcoc=" + mt["DatCoc"].ToString() + ", Conlai=" + mt["ConLai"].ToString() + ",  MaLoaigia='" + mt["MaLoaiGia"].ToString() + "', TyGia=" + mt["TyGia"].ToString() + ", isCheckOut=0";
                    sql += " WHERE MT62ID='" + mt["MT62ID"].ToString() + "'";
                }
                else
                {
                    sql = "UPDATE MT62 SET MT61ID='" + mt["MT61ID"].ToString() + "', MaKhach='" + mt["MaKhach"].ToString() + "', NgayCT='" + mt["NgayCT"].ToString() + "', NgayDen='" + mt["NgayDen"].ToString() + "', NgayDi='" + mt["NgayDi"].ToString() + "', OngBa=N'" + mt["OngBa"].ToString() + "', SoNguoi=" + mt["SoNguoi"].ToString() + ",";
                    sql += (mt["MaHang"].ToString() == "") ? "" : (" MaHang ='" + mt["MaHang"].ToString() + "',");
                    sql += "PtCk=" + mt["PtCK"].ToString() + ", CK=" + mt["CK"].ToString() + ",";
                    sql += " CKinRoom = " + (bool.Parse(mt["CKInRoom"].ToString()) ? 1 : 0).ToString() + ",";
                    sql += " notice=N'" + mt["notice"].ToString() + "',";
                    sql += "MaPTTT='" + mt["MaPTTT"].ToString() + "', MaNT='" + mt["MaNT"].ToString() + "', MaThue='" + mt["MaThue"].ToString() + "', Thuesuat=" + mt["ThueSuat"].ToString() + ", SoNgay=" + mt["SoNgay"].ToString() + ", TTienH=" + mt["TTienH"].ToString() + ", TThue=" + mt["TThue"].ToString() + ", TTien=" + mt["TTien"].ToString() + ", TongTien=" + mt["TongTien"].ToString() + ",";
                    sql += " Datcoc=" + mt["DatCoc"].ToString() + ", Conlai=" + mt["ConLai"].ToString() + ",  MaLoaigia='" + mt["MaLoaiGia"].ToString() + "', TyGia=" + mt["TyGia"].ToString() + ", isCheckOut=0";
                    sql += " WHERE MT62ID='" + mt["MT62ID"].ToString() + "'";
                }
                _dbData.UpdateByNonQuery(sql);
                if (_dbData.HasErrors)
                {
                    _dbData.RollbackMultiTrans();
                    return false;
                }
                //Update Dt
                
                dt.DefaultView.RowStateFilter = DataViewRowState.ModifiedCurrent;
                foreach (DataRowView rdt in dt.DefaultView)
                {
                    tbPhongTrong = _dbData.GetDataSetByStore("sp_GetPhongTrong", new string[] { "@NgayDen", "@NgayDi", "MT62ID" }, new object[] { rdt["NgayDen"], rdt["NgayDi"], mt["MT62ID"] });
                    if (tbPhongTrong.Select("MaPhong='" + rdt["MaPhong"].ToString() + "'").Length == 0)
                    {
                        _dbData.RollbackMultiTrans();
                        return false;
                    }

                    sql = "Update  dt62 set MaPhong='" + rdt["MaPhong"].ToString() + "',GiaPhong=" +
                        rdt["GiaPhong"].ToString() + ",Giaphong1=" + rdt["GiaPhong1"].ToString() + ", Ps=" + rdt["Ps"].ToString() + ",SoNT=" + rdt["SoNT"].ToString() + ",NgayDen='" +
                        rdt["NgayDen"].ToString() + "',NgayDi='" + rdt["NgayDi"].ToString() + "',NgayTT='" + rdt["NgayTT"].ToString() + "',MaLoaiPhong='" +
                        rdt["MaLoaiPhong"].ToString() + "',MaGia=" + rdt["MaGia"].ToString() + ",SoNgay=" + rdt["SoNgay"].ToString() + " where DT62ID='" + rdt["DT62ID"].ToString() + "'";
                    _dbData.UpdateByNonQuery(sql);

                    if (_dbData.HasErrors)
                    {
                        _dbData.RollbackMultiTrans();
                        return false;
                    }
                }
                dt.DefaultView.RowStateFilter = DataViewRowState.Added;
                foreach (DataRowView rdt in dt.DefaultView)
                {
                    if (tbPhongTrong.Select("MaPhong='" + rdt["MaPhong"].ToString() + "'").Length == 0)
                    {
                        _dbData.RollbackMultiTrans();
                        return false;
                    }
                    
                    sql = "insert into dt62 (MT62ID, DT62ID,MaPhong, GiaPhong,GiaPhong1,Ps,SoNT, NgayDen, NgayDi,MaLoaiPhong, MaGia,NgayTT,DaChuyen,SoNgay) values('" +
                                             mt["MT62ID"].ToString() + "','" + rdt["DT62ID"].ToString() + "','" + rdt["MaPhong"].ToString() + "'," + rdt["GiaPhong"].ToString() + "," + rdt["GiaPhong1"].ToString() + "," +
                                            rdt["Ps"].ToString() + "," + rdt["SoNT"].ToString() + ",'" + rdt["NgayDen"].ToString() + "','" + rdt["NgayDi"].ToString() + "','" +
                                            rdt["MaLoaiPhong"].ToString() + "'," + rdt["MaGia"].ToString() + ",'" + rdt["NgayTT"].ToString() + "',0," + rdt["SoNgay"].ToString() + ")";
                    _dbData.UpdateByNonQuery(sql);

                    if (_dbData.HasErrors)
                    {
                        _dbData.RollbackMultiTrans();
                        return false;
                    }
                }
                dt.DefaultView.RowStateFilter = DataViewRowState.Deleted;
                foreach (DataRowView rdt in dt.DefaultView)
                {
                    sql = "delete dt62 where DT62ID='" + rdt["DT62ID"].ToString() + "'";
                    _dbData.UpdateByNonQuery(sql);
                }
                dt.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
                //Update Ct
                ct.DefaultView.RowStateFilter = DataViewRowState.ModifiedCurrent;
                foreach (DataRowView dr in ct.DefaultView)
                {
                    if (dr["TenKhach"].ToString() == "") dr["TenKhach"] = "Chưa có tên";
                    if (dr["MaGiayTo"].ToString() == "") dr["MaGiayTo"] = "CMND";

                    sql = "Update ct62  set MaPhong='" + dr["MaPhong"].ToString() + "',TenKhach=N'" +
                        dr["TenKhach"].ToString() + "',DiaChi=N'" + dr["DiaChi"].ToString() + "',SDT=N'" +
                        dr["SDT"].ToString() + "',Email=N'" + dr["Email"].ToString() + "',MaGiayto='" +
                        dr["MaGiayto"].ToString() + "',SoID=N'" + dr["SoID"].ToString() + "' where CT62ID='" + dr["CT62ID"].ToString() + "'";

                    _dbData.UpdateByNonQuery(sql);

                    if (_dbData.HasErrors)
                    {
                        _dbData.RollbackMultiTrans();
                        return false;
                    }
                }

                ct.DefaultView.RowStateFilter = DataViewRowState.Added;
                foreach (DataRowView dr in ct.DefaultView)
                {
                    if (dr["TenKhach"].ToString() == "") dr["TenKhach"] = "Chưa có tên";
                    if (dr["MaGiayTo"].ToString() == "") dr["MaGiayTo"] = "CMND";
                    sql = "insert into ct62 (DT62ID, CT62ID, MaPhong, TenKhach, DiaChi, SDT, Email,MaGiayto, SoID) values('" +
                        dr["DT62ID"].ToString() + "','" + dr["CT62ID"].ToString() + "','" + dr["MaPhong"].ToString() + "',N'" +
                        dr["TenKhach"].ToString() + "',N'" + dr["DiaChi"].ToString() + "',N'" + dr["SDT"].ToString() + "',N'" + dr["Email"].ToString() + "','" +
                        dr["MaGiayto"].ToString() + "',N'" + dr["SoID"].ToString() + "')"; 
                    _dbData.UpdateByNonQuery(sql);


                    if (_dbData.HasErrors)
                    {
                        _dbData.RollbackMultiTrans();
                        return false;
                    }
                }
                ct.DefaultView.RowStateFilter = DataViewRowState.Deleted;
                foreach (DataRowView rdt in ct.DefaultView)
                {
                    sql = "delete ct62 where CT62ID='" + rdt["CT62ID"].ToString() + "'";
                    _dbData.UpdateByNonQuery(sql);
                }
                ct.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
                _dbData.EndMultiTrans();
                mt.Table.AcceptChanges();
                mt.AcceptChanges();
                dt.AcceptChanges();
                ct.AcceptChanges();
                return true;
            }
            catch(Exception ex)
            {
                if (_dbData.strMain != null) _dbData.RollbackMultiTrans();
                return false;
            }
            return true;
            
        }
        public bool CheckIn(bool All)
        {
            _dbData.BeginMultiTrans();
            try
            {
                string sql;                
                foreach (DataRow dr in dt.Rows)
                {
                   // if (tbPhongTrong.Select("MaPhong='" + dr["MaPhong"].ToString() + "'").Length == 0)
                    //{
                    //    _dbData.RollbackMultiTrans();
                    //    return false;
                    //}
                    if (bool.Parse(dr["isCheckIn"].ToString()) && (dr["isCheckout"]==DBNull.Value || !bool.Parse(dr["isCheckout"].ToString())))
                    {
                        sql = "update DT62 set isCheckIn =1 where DT62ID ='" + dr["DT62ID"].ToString() + "'; update dmPhong set MaTT='IN' where MaPhong='" + dr["MaPhong"].ToString() + "'";
                        _dbData.UpdateByNonQuery(sql);
                        if (_dbData.HasErrors)
                        {
                            _dbData.RollbackMultiTrans();
                            return false;
                        }
                    }
                }
                if (All)
                {
                    sql = "Update MT62 set isCheckIn=1 where MT62ID='" + mt["MT62ID"].ToString() + "'";
                    _dbData.UpdateByNonQuery(sql);
                }
                if (_dbData.HasErrors)
                {
                    _dbData.RollbackMultiTrans();
                    return false;
                }
                _dbData.EndMultiTrans();
            }
            catch (Exception ex)
            {
                if (_dbData.strMain != null)
                {
                    _dbData.RollbackMultiTrans();
                    return false;
                }
            }
            return true;
        }
        private string GetSoCT()
        {
            string sql = "select top 1 Soct from mt62 order by soct desc";
            DataTable tmp = _dbData.GetDataTable(sql);
            try
            {
                if (tmp.Rows.Count > 0)
                {
                    int i = int.Parse(tmp.Rows[0][0].ToString()) + 1;
                    return i.ToString("000000000000");
                }
            }
            catch
            {
                return "000000000000";
            }

            return "000000000000";
        }
        
    }
}
