using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace GiaThanh
{
    class YtHeso :Ytgt
    {
        public YtHeso(DataRow dr, DataTable TPNhapkho, DateTime tungay, DateTime denngay)
        {
            dryt = dr;
            drTPNhapkho = TPNhapkho;
            _Tungay = tungay;
            _Denngay = denngay;
            dtkq = TPNhapkho.Copy();
            dtkq.Columns.Add(createCol());
            Name = dr["MaYT"].ToString();


        }
        public override void TinhGiatri()
        {
            string sql;
            //Lấy Tổng giá trị cần phân bổ
            Tongtien = _dbData.GetValueByStore("sopsKetChuyen", new string[] { "@tk", "@ngayct", "@ngayct1", "@psno" }, new object[] { tk, _Tungay, _Denngay, 0 }, new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output }, 3);
            //Lấy hệ số phân bổ
            sql = "select " + dryt["TruongSP"].ToString() + " as Masp," + dryt["Heso"].ToString() + " as Heso from " + dryt["BangDM"].ToString();
            if (bool.Parse(dryt["Ngay"].ToString()))
                sql += " where ngayct between cast('" + _Tungay.ToShortDateString() + "'as datetime) and cast('" + _Denngay.ToShortDateString() + "'as datetime) ";
            DataTable tbHeso = _dbData.GetDataTable(sql);
            tbHeso.PrimaryKey = new DataColumn[] { tbHeso.Columns["MaSP"]};
            //Lấy tổng hệ số phân bổ
            double TongHeso=0;
            DataColumn col = new DataColumn("Heso", typeof(double));
            col.DefaultValue = 0;
            dtkq.Columns.Add(col);
            foreach (DataRow dr in dtkq.Rows)
            {
                string Masp = dr["MaSP"].ToString();

                DataRow drHeso ;
                if (!tbHeso.Rows.Contains(Masp))
                    continue;
                    drHeso= tbHeso.Rows.Find(Masp);
                    double heso=double.Parse(dr["soluong"].ToString()) * double.Parse(drHeso["Heso"].ToString());
                    dr["Heso"] = heso;
                    TongHeso += heso;
            }
            //Phân bổ
            if (TongHeso==0) return;
            foreach (DataRow dr in dtkq.Rows)
            {
                dr[Name] = double.Parse(dr["Heso"].ToString()) * Tongtien / TongHeso;
            }
            dtkq.Columns.Remove("Heso");
            dtkq.Columns.Remove("Soluong");
        }
    }
}
