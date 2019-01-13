using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.UI;
namespace QLSX
{
   public class LSXappoint : Appointment
    {
        DataRow _dr;
        public DataRow dr
        {
            get { return _dr; }
            set
            {
                _dr = value;
                MaMin = dr["MaMin"].ToString();
                GhiChu = dr["GhiChu"].ToString();
                TuNgayKH = DateTime.Parse(dr["TuNgayKH"].ToString());
                DenNgayKH = DateTime.Parse(dr["DenNgayKH"].ToString());
                TenHang = dr["TenHang"].ToString();
                SoLuong = decimal.Parse(dr["SoLuong"].ToString());
                SoCT = dr["SoCT"].ToString();
                TenKH = dr["TenKH"].ToString();
                NgayGiao = DateTime.Parse(dr["NgayGiao"].ToString());
            }
        }
        public string MaMin { get; set; }
        public string GhiChu { get; set; }
        public DateTime TuNgayKH { get; set; }
        public DateTime DenNgayKH { get; set; }
        public string TenHang { get; set; }
        public decimal SoLuong { get; set; }
        public string SoCT { get; set; }
        public string TenKH { get; set; }
        public DateTime NgayGiao { get; set; }
        public void UpdateDr()
        {
            if (dr != null)
            {
                dr["TuNgayKH"] = TuNgayKH;
                dr["DenNgayKH"] = DenNgayKH;
                dr["MaMin"] = MaMin;
                dr["SoLuong"] = SoLuong;
                dr["GhiChu"] = GhiChu;
            }
            
        }
        public void Save()
        {
            UpdateDr();
        }
    }
}
