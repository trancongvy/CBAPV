using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using CDTDatabase;
using CDTLib;
using CDTControl;
namespace Inventory
{
    
    class GiaNTXT
    {


        int Nam = int.Parse(Config.GetValue("NamLamViec").ToString());
        private Database _dbData = Database.NewDataDatabase();//"server = vuonghuynh; database = CBA11; user = sa; pwd = sa");
        private Database _dbStruct = Database.NewStructDatabase();//"server = vuonghuynh; database = CDT; user = sa; pwd = sa");
        private DataTable _dtVatTu;
        private int _tuThang;
        private int _denThang;
        private DateTime TuNgay;
        private DateTime DenNgay;
        public string MaKho=null;
        public DataTable DtVatTu
        {
            get { return _dtVatTu; }
            set { _dtVatTu = value; }
        }

        public GiaNTXT(int tuThang, int denThang)
        {
            _tuThang = tuThang;
            _denThang = denThang;
            string str = _tuThang.ToString() + "/" + "01" + "/" + Nam.ToString();
            TuNgay = Convert.ToDateTime(str);

            str = (_denThang).ToString() + "/" + "01" + "/" + Nam.ToString();
            DenNgay = Convert.ToDateTime(str).AddMonths(1).AddDays(-1);

        }
        public void TinhGia()
        {
            string sql;
            string str;
            if (MaKho == null)
            {
                str = "select makho, mavt into tmpvt from blvt where ngayct between '" + TuNgay.ToShortDateString() + "' and '" + DenNgay.ToShortDateString();
                str += "' and mavt in (select mavt from dmvt where tonkho=1) and soluong_x>0 group by makho, mavt ";
                _dbData.UpdateByNonQuery(str);
                sql = "delete blvtmapid where ngayctX>='" + TuNgay.ToString() + "' ";
                _dbData.UpdateByNonQuery(sql); 
            }
            else
            {
                str = "select makho, mavt into tmpvt from blvt where ngayct between '" + TuNgay.ToShortDateString() + "' and '" + DenNgay.ToShortDateString();
                str += "' and mavt in (select mavt from dmvt where tonkho=1) and soluong_x>0 and makho='";
                str += MaKho + "' group by makho, mavt";
                _dbData.UpdateByNonQuery(str);
                //--Xóa những dòng trong blvtmapid mà tính từ phiếu xuất trong kỳ của kho đó
                sql = "delete blvtmapid where ngayctX>='" + TuNgay.ToString() + "' and and convert(nvarchar(36),MTID_X) + convert(nvarchar(36),DTID_X) + convert(nvarchar(4),NhomDK_X) in " +
	                " (select convert(nvarchar(36),MTID) + convert(nvarchar(36),MTIDDT) + convert(nvarchar(4),NhomDK) from blvt where makho='" + MaKho + "'";
                _dbData.UpdateByNonQuery(sql);
                //Xóa tất cả những dòng tính từ phiếu xuất đã xóa
                sql = "delete blvtmapid where ngayctX>='" + TuNgay.ToString() + "' and and convert(nvarchar(36),MTID_X) + convert(nvarchar(36),DTID_X) + convert(nvarchar(4),NhomDK_X) not in " +
                    " (select convert(nvarchar(36),MTID) + convert(nvarchar(36),MTIDDT) + convert(nvarchar(4),NhomDK) from blvt ";
                _dbData.UpdateByNonQuery(sql);
            }
            
            
            sql = "select * from tmpvt";
            DataTable dmvt = _dbData.GetDataTable(sql);
            sql = " drop table tmpvt ";
            _dbData.UpdateByNonQuery(sql);

            
            foreach (DataRow dr in dmvt.Rows)
            {
                DataTable tmp = _dbData.GetDataSetByStore("TinhGiaNTXT", new string[] { "@TuNgay", "@DenNgay", "@MaVT", "@MaKho" }, new object[] { TuNgay, DenNgay, dr["MaVT"].ToString(), dr["MaKho"].ToString() });
                if (_dtVatTu == null)
                {
                    _dtVatTu = tmp.Copy();
                }
                else
                {
                    DataRow[] lst = tmp.Select();
                    foreach (DataRow drx in lst)
                    {
                        _dtVatTu.ImportRow(drx);
                    }
                }
            }
            if (_dtVatTu != null)
                this.Apgia();
        }
        public void Apgia()
        {
            string sql = "select b.mttableid,b.dttableid,a.* from sysdataconfigdt a,sysdataconfig b ";
            sql += "where b.blconfigid=a.blconfigid  ";
            sql += "and systableid=330 AND blfieldid=3484  ";    //330: mã của bảng blvt, 3485: mã trường blvt.DonGiaNT
            sql += "AND b.dttableid is not null and (a.blconfigid in(select blconfigid from sysdataconfigdt where   blfieldid=3484  AND (mtFieldID IS NOT NULL OR dtFieldID IS NOT NULL OR Formula IS NOT NULL))  or NhomDK='HTL1' )";
            DataTable dsMtdt = _dbStruct.GetDataTable(sql);
            string mt;
            string dt;
            string GiaField;
            string tygiaField;//trong blvt là 3480 - rồi tìm trong bảng nguồn tương ứng
            string GiaNTField;//trong blvt là 3485 - 
            //string mavtField;//trong blvt là 1796 - rồi tìm trong bảng nguồn tương ứng
            //string makhoField;//trong blvt là 1797 - rồi tìm trong bảng nguồn tương ứng
            string ngayctField;//trong blvt là 3467 - rồi tìm trong bảng nguồn tương ứng
            string MaCT;
            string MTIDDT;//trong blvt là 3487 - rồi tìm trong bảng nguồn tương ứng
            updateCongthuc UpdateGia;
            PostBl post;
            //bool mtdt = false;
            _dbData.BeginMultiTrans();
            try
            {
                foreach (DataRow dr in dsMtdt.Rows)//duyệt qua từ loại chứng từ
                {
                    mt = _dbStruct.GetValue("select tableName from systable where systableid=" + dr["mttableid"].ToString()).ToString();
                    dt = _dbStruct.GetValue("select tableName from systable where systableid=" + dr["dttableid"].ToString()).ToString();
                    MaCT = _dbStruct.GetValue("select MaCT from systable where systableid=" + dr["dttableid"].ToString()).ToString();
                    if (!(dr["dtfieldid"] is DBNull))
                    {//Lấy thông tin các trường cần dùng
                        GiaField = _dbStruct.GetValue("  select fieldName from sysfield where systableid=" + dr["dttableid"].ToString() + " and sysfieldid=" + dr["dtfieldid"].ToString()).ToString();
                        GiaNTField = _dbStruct.GetValue("  select dtfieldid from sysdataconfigdt where blfieldid=3485  and blconfigid=" + dr["blconfigid"].ToString()).ToString();
                        GiaNTField = _dbStruct.GetValue("  select fieldName from sysfield where sysfieldid=" + GiaNTField).ToString();
                        MTIDDT = _dbStruct.GetValue("  select dtfieldid from sysdataconfigdt where blfieldid=3487  and blconfigid=" + dr["blconfigid"].ToString()).ToString();
                        MTIDDT = _dbStruct.GetValue("  select fieldName from sysfield where sysfieldid=" + MTIDDT).ToString();
                        ngayctField = _dbStruct.GetValue("  select mtfieldid from sysdataconfigdt where blfieldid=3467  and blconfigid=" + dr["blconfigid"].ToString()).ToString();
                        ngayctField = _dbStruct.GetValue("  select fieldName from sysfield where sysfieldid=" + ngayctField).ToString();
                        tygiaField = _dbStruct.GetValue("  select mtfieldid from sysdataconfigdt where blfieldid=3480  and blconfigid=" + dr["blconfigid"].ToString()).ToString();
                        if (tygiaField != "")
                        {
                            tygiaField = _dbStruct.GetValue("  select fieldName from sysfield where sysfieldid=" + tygiaField).ToString();
                        }
                        foreach (DataRow rGia in _dtVatTu.Select("MaCT='" + MaCT + "'"))//duyệt qua bảng giá tương ứng với bảng dữ liệu
                        {
                            if (rGia["DonGia"] != DBNull.Value || rGia["DonGia"].ToString()!=string.Empty)
                            {
                                updateMTDT(mt, dt, GiaField, GiaNTField, tygiaField, rGia, MTIDDT);
                            }
                        }
                        UpdateGia = new updateCongthuc(_dbData, dr["mttableid"].ToString(), dr["dttableid"].ToString(), GiaNTField, TuNgay, DenNgay, ngayctField);
                        UpdateGia.Update();
                        post = new PostBl(_dbData, dr["mttableid"].ToString(), TuNgay, DenNgay, UpdateGia.LField);
                        post.Post();
                    }
                    if (_dbData.HasErrors)
                    {
                        _dbData.RollbackMultiTrans();
                        break;
                    }

                }
                if (!_dbData.HasErrors)
                _dbData.EndMultiTrans();
            }
            catch (SqlException ex)
            {
                if (_dbData.Connection.State != ConnectionState.Closed)
                    _dbData.Connection.Close();
                MessageBox.Show(ex.Message);

            }
        }
        public void updateMTDT(string mt, string dt, string Giafield, string GiaNTfield, string tygiaField, DataRow dr, string IDField)
        {
            string sql;
            sql = "update " + dt + " set " + Giafield + " = " + dr["DonGia"].ToString().Replace(",", ".");
            sql += " where  " + IDField + "=convert(uniqueidentifier,'" + dr["MTIDDT"].ToString()+ "')";
            _dbData.UpdateByNonQuery(sql);
            if (tygiaField != "")
            {
                sql = "update " + dt + " set " + GiaNTfield + " = a." + Giafield + "/b." + tygiaField + " from ";
                sql += dt + " a," + mt + " b  where a." + mt.Trim() + "id = b." + mt.Trim() + "id and ";
                sql += IDField  + "=convert(uniqueidentifier,'" + dr["MTIDDT"].ToString() + "')";
                _dbData.UpdateByNonQuery(sql);
            }
        }
    }
}
