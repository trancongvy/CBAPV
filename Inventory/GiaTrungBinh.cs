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
    public class GiaTrungBinh
    {
        int Nam = int.Parse(Config.GetValue("NamLamViec").ToString());
        private Database _dbData = Database.NewDataDatabase();//"server = vuonghuynh; database = CBA11; user = sa; pwd = sa");
        private Database _dbStruct = Database.NewStructDatabase();//"server = vuonghuynh; database = CDT; user = sa; pwd = sa");
        private DataTable _dtVatTu;
        private int _tuThang;
        private int _denThang;
        DateTime TuNgay ;
        DateTime DenNgay;
        string[] Paras;
        object[] Values;
        private string makhoapgia=null;
        public DataTable DtVatTu
        {
            get { return _dtVatTu; }
            set { _dtVatTu = value; }
        }

        public GiaTrungBinh(int tuThang, int denThang,string makho)
        {
            _tuThang = tuThang;
            _denThang = denThang;
            if (makho != null)
                makhoapgia = makho;
        }
        public int solantinh()
        {
            if (makhoapgia != null) return 1;
            string str = _tuThang.ToString() + "/" + "01" + "/" + Nam.ToString();
            DateTime TuNgay = Convert.ToDateTime(str);
            str = (_denThang).ToString() + "/" + "01" + "/" + Nam.ToString();
            DateTime DenNgay = Convert.ToDateTime(str).AddMonths(1).AddDays(-1);
            string sql = "select count(makho) from blvt where soluong>0 and maCt='PDC'  and ngayct between cast('" + TuNgay.ToString() + "' as datetime) and cast('" + DenNgay.ToString() + "' as datetime) group by makho";
            DataTable solan = _dbData.GetDataTable(sql);
            return solan.Rows.Count;
        }
        public void TinhGia()
        {
            string str = _tuThang.ToString() + "/" + "01" + "/" + Nam.ToString();
            TuNgay = Convert.ToDateTime(str);
            str = (_denThang).ToString() + "/" + "01" + "/" + Nam.ToString();
            DenNgay = Convert.ToDateTime(str).AddMonths(1).AddDays(-1);
            Paras = new string[] {"@tungay","@denngay","@makho"};
            Values = new object[] {TuNgay, DenNgay,makhoapgia };

            _dtVatTu = _dbData.GetDataSetByStore("TinhgiaTB", Paras, Values);
            ApGia();
            //this.updateDFnvl();
        }
        private void updateDFnvl()
        {
            string sql;
            sql = "update dfnvl set gia= x.dongia  from \n";
            sql += "(	select avg(dongia) as dongia,mavt from blvt where makho='NL' and soluong_x>0 and datepart(m,ngayct) between @thang1 and @thang2 group by mavt \n";
            sql += " ) x,dfnvl where dfnvl.mavt=x.mavt and datepart(m,ngayct)= @thang2 \n";
            sql += " update dfnvl set Tien=Soluong*gia where datepart(m,ngayct)= @thang2 \n";
            sql = sql.Replace("@thang1", TuNgay.Month.ToString("#0"));
            sql = sql.Replace("@thang2", DenNgay.Month.ToString("#0"));
            _dbData.UpdateByNonQuery(sql);

        }
        public void ApGia()
        {
            string blvtid;

            string sql = "select systableid from systable where TableName='BLVT' and syspackageid=" + Config.GetValue("sysPackageID").ToString();
            blvtid = _dbStruct.GetValue(sql).ToString();
            sql = "select sysfieldid from sysfield where systableid=" + blvtid + " and FieldName='Dongia'";
            string dongiaID = _dbStruct.GetValue(sql).ToString();
            sql = "select sysfieldid from sysfield where systableid=" + blvtid + " and FieldName='Soluong_x'";
            string Soluong_xID=_dbStruct.GetValue(sql).ToString();
            sql = "select sysfieldid from sysfield where systableid=" + blvtid + " and FieldName='Soluong'";
            string Soluong_nID = _dbStruct.GetValue(sql).ToString();
                sql= "select b.mttableid,b.dttableid,a.* from sysdataconfigdt a,sysdataconfig b ";
            sql +="where b.blconfigid=a.blconfigid  ";
            sql += "and systableid=" + blvtid + " AND blfieldid=" + dongiaID;
            sql += " AND (a.blconfigid in(select blconfigid from sysdataconfigdt where   blfieldid=" + Soluong_xID + "  AND (mtFieldID IS NOT NULL OR dtFieldID IS NOT NULL OR Formula IS NOT NULL))    or NhomDK='HTL1' ) and NhomDK<>'PNH5'";//or NhomDK='PNH1'
            DataTable dsMtdt = _dbStruct.GetDataTable(sql);
            string mt;
            string dt;
            string GiaField = dongiaID;//
            string tygiaField = _dbStruct.GetValue("select sysfieldid from sysfield where sysTableid=" + blvtid + " and FieldName='TyGia'").ToString();//trong blvt là 1795 - rồi tìm trong bảng nguồn tương ứng
            string GiaNTField = _dbStruct.GetValue("select sysfieldid from sysfield where sysTableid=" + blvtid + " and FieldName='DonGiaNT'").ToString();//trong blvt là 1792 - 
            string mavtField = _dbStruct.GetValue("select sysfieldid from sysfield where sysTableid=" + blvtid + " and FieldName='MaVT'").ToString();//trong blvt là 1796 - rồi tìm trong bảng nguồn tương ứng
            string makhoField = _dbStruct.GetValue("select sysfieldid from sysfield where sysTableid=" + blvtid + " and FieldName='MaKho'").ToString();//trong blvt là 1797 - rồi tìm trong bảng nguồn tương ứng
            string ngayctField = _dbStruct.GetValue("select sysfieldid from sysfield where sysTableid=" + blvtid + " and FieldName='NgayCt'").ToString();//trong blvt là 1782 - rồi tìm trong bảng nguồn tương ứng
            updateCongthuc UpdateGia;
            PostBl post;
            //bool mtdt = false;
            _dbData.BeginMultiTrans();
            try
            {
                foreach (DataRow dr in dsMtdt.Rows)//duyệt qua từ loại chứng từ
                {

                    mt = _dbStruct.GetValue("select tableName from systable where systableid=" + dr["mttableid"].ToString()).ToString();
                    if (dr["dttableid"] != DBNull.Value)
                        dt = _dbStruct.GetValue("select tableName from systable where systableid=" + dr["dttableid"].ToString()).ToString();
                    else continue;
                    string t = mt + ": ";
                    TimeSpan ts;
                    DateTime d1 = System.DateTime.Now;
                    string GiaFieldName;
                    string tygiaFieldName = "";
                    string GiaNTFieldName;
                    string mavtFieldName;
                    string makhoFieldName;
                    string ngayctFieldName;
                    if (!(dr["dtfieldid"] is DBNull))
                    {//Lấy thông tin các trường cần dùng
                        GiaFieldName = _dbStruct.GetValue("  select dtfieldid from sysdataconfigdt where blfieldid=" + GiaField + "  and blconfigid=" + dr["blconfigid"].ToString()).ToString();
                        GiaFieldName = _dbStruct.GetValue("  select fieldName from sysfield where sysfieldid=" + GiaFieldName).ToString();
                        mavtFieldName = _dbStruct.GetValue("  select dtfieldid from sysdataconfigdt where blfieldid=" + mavtField + "  and blconfigid=" + dr["blconfigid"].ToString()).ToString();
                        mavtFieldName = _dbStruct.GetValue("  select fieldName from sysfield where sysfieldid=" + mavtFieldName).ToString();
                        //xét trường hợp mã kho nằm trong dt hay mt
                        bool khoinMt = false;
                        makhoFieldName = _dbStruct.GetValue("  select dtfieldid from sysdataconfigdt where blfieldid=" + makhoField + "  and blconfigid=" + dr["blconfigid"].ToString()).ToString();
                        if (makhoFieldName == null || makhoFieldName == "")
                        {
                            makhoFieldName = _dbStruct.GetValue("  select mtfieldid from sysdataconfigdt where blfieldid=" + makhoField + "  and blconfigid=" + dr["blconfigid"].ToString()).ToString();
                            khoinMt = true;
                        }
                        makhoFieldName = _dbStruct.GetValue("  select fieldName from sysfield where sysfieldid=" + makhoFieldName).ToString();
                        ngayctFieldName = _dbStruct.GetValue("  select mtfieldid from sysdataconfigdt where blfieldid=" + ngayctField + " and blconfigid=" + dr["blconfigid"].ToString()).ToString();
                        ngayctFieldName = _dbStruct.GetValue("  select fieldName from sysfield where sysfieldid=" + ngayctFieldName).ToString();
                        GiaNTFieldName = _dbStruct.GetValue("  select dtfieldid from sysdataconfigdt where blfieldid=" + GiaNTField + "  and blconfigid=" + dr["blconfigid"].ToString()).ToString();
                        GiaNTFieldName = _dbStruct.GetValue("  select fieldName from sysfield where sysfieldid=" + GiaNTFieldName).ToString();

                        tygiaFieldName = _dbStruct.GetValue("  select mtfieldid from sysdataconfigdt where blfieldid=" + tygiaField + "  and blconfigid=" + dr["blconfigid"].ToString()).ToString();
                        if (tygiaFieldName != "")
                        {
                            tygiaFieldName = _dbStruct.GetValue("  select fieldName from sysfield where sysfieldid=" + tygiaFieldName).ToString();
                        }
                        //foreach (DataRow rGia in _dtVatTu.Rows)//duyệt qua bảng giá
                        //{
                        //    updateMTDT(mt, dt, GiaFieldName, GiaNTFieldName, tygiaFieldName, rGia, mavtFieldName, makhoFieldName, ngayctFieldName);
                        //}

                        ts = DateTime.Now - d1;
                        t += ts.TotalMilliseconds.ToString("### ### ###.##") + "--khoitao;";
                        updateMTDT(mt, dt, GiaFieldName, GiaNTFieldName, tygiaFieldName, mavtFieldName, makhoFieldName, ngayctFieldName, khoinMt);
                        ts = DateTime.Now - d1;
                        t += ts.TotalMilliseconds.ToString("### ### ###.##") + "--updateMT;";
                        d1 = DateTime.Now;
                        UpdateGia = new updateCongthuc(_dbData, dr["mttableid"].ToString(), dr["dttableid"].ToString(), GiaNTFieldName, TuNgay, DenNgay, ngayctFieldName);
                        UpdateGia.Update();
                        ts = DateTime.Now - d1;
                        t += ts.TotalMilliseconds.ToString("### ### ###.##") + "--updateGia";
                        d1 = DateTime.Now;
                        post = new PostBl(_dbData, dr["mttableid"].ToString(), TuNgay, DenNgay, UpdateGia.LField);
                        if (dr["mttableid"].ToString() == "2560")
                        {
                        }
                        post.Post();
                        ts = DateTime.Now - d1;
                        t += ts.TotalMilliseconds.ToString("### ### ###.##") + "--Post";
                        // MessageBox.Show(t);
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
                MessageBox.Show(ex.Message);
                _dbData.RollbackMultiTrans();
            }
        }

        private void updateMTDT(string mt, string dt,string Giafield,string GiaNTfield, string tygiaField,string mavtField,string makhoField,string ngayctFiedl,bool khoinMt)
        {
                string sql;
            string mtPk=mt + "ID";
            string dtRefPK=mtPk;

            sql = " select Pk from systable where tableName='" + mt + "'";
            DataTable Ttmp = _dbStruct.GetDataTable(sql);
            if (Ttmp.Rows.Count > 0) mtPk = Ttmp.Rows[0]["Pk"].ToString();
            sql = "select fieldName from sysfield where  reftable='" + mt + "' and sysTableID= (select systableID from systable where tableName='" + dt + "')";
            Ttmp = _dbStruct.GetDataTable(sql);
            if (Ttmp.Rows.Count > 0) dtRefPK = Ttmp.Rows[0]["fieldName"].ToString();
                if (makhoapgia == null)
                {
                    if (khoinMt)
                    {
                        sql = "update " + dt + " set " + Giafield + " = x.dongia from " + dt + " y," + mt.Trim() + " a, banggiatb x  ";
                        sql += " where  y." + mavtField + "=x.mavt  ";
                        sql += " and y." + dtRefPK + " = a." + mtPk + "  and ";
                        sql += "a." + ngayctFiedl + " between cast('" + TuNgay.ToString() + "' as datetime) and cast('" + DenNgay.ToString() + "' as datetime)";
                        sql += " and x.ngayct=cast('" + DenNgay.ToString() + "' as datetime)";

                        if (mt == "MT33" || mt == "MT42")
                        {
                            sql += " and a.NhapTb=1 ";
                        }
                        if (mt == "MT43")
                        {
                            sql += " and a.XuatDD <>1";
                        }
                    }
                    else
                    {
                        sql = "update " + dt + " set " + Giafield + " = x.dongia from " + dt + " y, banggiatb x  ";
                        sql += " where  y." + mavtField + "=x.mavt  ";
                        sql += " and y." + dtRefPK + " in  (select " +mtPk + " from " + mt + " where ";
                        sql += mt.Trim() + "." + ngayctFiedl + " between cast('" + TuNgay.ToString() + "' as datetime) and cast('" + DenNgay.ToString() + "' as datetime)";
                        sql += " and x.ngayct=cast('" + DenNgay.ToString() + "' as datetime)";

                        if (mt == "MT33" || mt == "MT42")
                        {
                            sql += " and " + mt.Trim() + ".NhapTb=1";

                        }
                        if (mt == "MT43")
                        {
                            sql += " and " + mt.Trim() + ".XuatDD <>1";
                        }
                        sql += ")";
                    }
                }
                else
                {
                    if (khoinMt)
                    {
                        sql = "update " + dt + " set " + Giafield + " = x.dongia from " + dt + " y," + mt.Trim() + " a, banggiatb x  ";
                        sql += " where  y." + mavtField + "=x.mavt  ";
                        sql += " and y." + dtRefPK + " = a." + mtPk+ " and  a." + makhoField + "=x.makho and x.makho='" + makhoapgia + "' and ";
                        sql += "a." + ngayctFiedl + " between cast('" + TuNgay.ToString() + "' as datetime) and cast('" + DenNgay.ToString() + "' as datetime)";
                        sql += " and x.ngayct=cast('" + DenNgay.ToString() + "' as datetime) and x.makho='" + makhoapgia + "'";

                        if (mt == "MT33" || mt == "MT42")
                        {
                            sql += " and a.NhapTb=1";
                        }
                        if (mt == "MT43")
                        {
                            sql += " and a.XuatDD <>1";
                        }

                    }
                    else
                    {
                        sql = "update " + dt + " set " + Giafield + " = x.dongia from " + dt + " y, banggiatb x  ";
                        sql += " where  y." + mavtField + "=x.mavt and y." + makhoField + "=x.makho and x.makho='" + makhoapgia + "'";
                        sql += " and y." + dtRefPK + " in  (select " + mtPk + " from " + mt + " where ";
                        sql += mt.Trim() + "." + ngayctFiedl + " between cast('" + TuNgay.ToString() + "' as datetime) and cast('" + DenNgay.ToString() + "' as datetime)";
                        sql += " and x.ngayct=cast('" + DenNgay.ToString() + "' as datetime)";

                        if (mt == "MT33" || mt == "MT42")
                        {
                            sql += " and " + mt.Trim() + ".NhapTb=1";

                        }
                        if (mt == "MT43")
                        {
                            sql += " and " + mt.Trim() + ".XuatDD <>1";
                        }
                        sql += ")";
                    }
                }
                //MessageBox.Show(sql);
                _dbData.UpdateByNonQuery(sql);
                if (tygiaField != "")
                {
                    sql="update " + dt + " set " + GiaNTfield + " = a." + Giafield + "/b." + tygiaField + " from ";
                    sql += dt + " a," + mt + " b  where a." +dtRefPK + " = b." + mtPk + " and ";
                    sql += "b." + ngayctFiedl + " between cast('" + TuNgay.ToString() + "' as datetime) and cast('" + DenNgay.ToString() + "' as datetime)";
                    if (mt == "MT33" || mt == "MT42")
                    {
                        sql += " and b.NhapTb=1";
                    }
                    _dbData.UpdateByNonQuery(sql);
                }
               
        }
        private void updateMTDTCongthuc(string mt, string dt) 
        {
 
        }
        private void updateBl() 
        {
      
        }
       
    }
}
