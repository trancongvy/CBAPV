using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
namespace GiaThanh
{
    class YtDinhMuc :Ytgt
    {
        private DataTable tbDM;
        private DataTable tbNVLXuat;
        private DataTable tbTongNVLDM;
        public YtDinhMuc(DataRow dr, DataTable TPNhapkho, DateTime tungay, DateTime denngay)
        {
            dryt = dr;
            tk = dr["Tk"].ToString().Trim();
            tkdu = dr["TkDu"].ToString().Trim();
            dk = dr["Dk"].ToString().Trim();
            drTPNhapkho = TPNhapkho;
            _Tungay = tungay;
            _Denngay = denngay;
            dtkq = TPNhapkho.Copy();
            dtkq.Columns.RemoveAt(1);
            dtkq.Columns.Add(createCol());
            dtkq.PrimaryKey = new DataColumn[] { dtkq.Columns[0] };
            Name = dr["MaYT"].ToString();

        }
        public override void TinhGiatri()
        {

            tbDM = _dbData.GetDataSetByStore("GetsldmNVL", new string[] { "@tungay", "@denngay", "@manhom", "@BangDM", "@TruongSP", "@Mavt", "@Heso", "@tk", "@dk","Ngay" },
                    new object[] { _Tungay, _Denngay, Manhom, dryt["BangDM"].ToString(), dryt["TruongSP"].ToString(), dryt["Mavt"].ToString(), dryt["Heso"].ToString(), tk, dk, bool.Parse(dryt["Ngay"].ToString()) });
            //tbDM = _dbData.GetDataSetByStore("GetsldmNVL", new string[] { "@tungay", "@denngay", "@manhom", "@BangDM", "@TruongSP", "@Mavt", "@Heso", "@tk" },
            //        new object[] { _Tungay, _Denngay, Manhom, dryt["BangDM"].ToString(), dryt["TruongSP"].ToString(), dryt["Mavt"].ToString(), dryt["Heso"].ToString(), tk });
            foreach (DataRow dr in tbDM.Rows)
            {

                Tongtien += double.Parse(dr["Tien"].ToString());
            }

            foreach (DataRow dr in dtkq.Rows)
            {
                string masp = dr["masp"].ToString().Trim();
                dr[Name] = getSum(tbDM.Select("Masp='" + masp + "'"), "tien");
            }

            //}
            //catch
            //{ 
            //}

        }
        
    }
}
