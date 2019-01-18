using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.UI;
using CDTDatabase;
namespace QLSX
{
    public class LSXappoint : Appointment
    {
        DataRow _dr;
        internal LSXappoint(SchedulerControl schedu, DataRow __dr):base(AppointmentType.Normal)
        {
           // this = schedu.Storage.CreateAppointment(AppointmentType.Normal);
            dr = __dr;
            // this.CustomFields["ctID"] = ctLichSXID.ToString();
           
        }
        public bool isNew { get; set; }
        public DataRow dr
        {
            get { return _dr; }
            set
            {
                _dr = value;
                if (Duration == null || Duration.Ticks == 0) Duration = new TimeSpan(2, 0, 0);
                MaMIn = dr["MaMIn"].ToString();
                MaKH = dr["MaKH"].ToString();
                MaVT = dr["MaVT"].ToString();
                GhiChu = dr["GhiChu"].ToString();
                if (dr["ctLichSXID"] == DBNull.Value) {
                    isNew = true;
                    dr["ctLichSXID"] = Guid.NewGuid();
                    ctLichSXID = Guid.Parse(dr["ctLichSXID"].ToString());
                } 
                else
                {
                    isNew = false;
                    ctLichSXID = Guid.Parse(dr["ctLichSXID"].ToString());
                }
                MTLSXID = Guid.Parse(dr["MTLSXID"].ToString());
               DTLSXID = Guid.Parse(dr["DTLSXID"].ToString());
                CTLSXID = Guid.Parse(dr["CTLSXID"].ToString());
                TuNgayKH = DateTime.Parse(dr["TuNgayKH"].ToString());
                DenNgayKH = DateTime.Parse(dr["DenNgayKH"].ToString());
                TenHang = dr["TenHang"].ToString();
                SoLuong = decimal.Parse(dr["SoLuong"].ToString());
                SoCT = dr["SoCT"].ToString();
                TenKH = dr["TenKH"].ToString();
                NgayGiao = DateTime.Parse(dr["NgayGiao"].ToString());
                Description1 = dr["Description1"].ToString();
                TrangThai = int.Parse(dr["TrangThai"].ToString());
            }
        }
        DateTime _tungayKH;
        DateTime _denngayKH;
        DateTime _tungay;
        DateTime _denngay;
        public Guid ctLichSXID { get; set; }
        public Guid MTLSXID { get; set; }
        public Guid DTLSXID { get; set; }
        public Guid CTLSXID { get; set; }
        public string MaMIn
        {
            get
            {
                return this.ResourceId.ToString();
            }
            set
            {
                ResourceId = value;
            }
        }
        public string MaKH { get; set; }
        public string MaVT { get; set; }
        public string GhiChu { get; set; }
        public string Description1 { get; set; }
        public DateTime TuNgayKH
        {
            get { return _tungayKH; }
            set
            {
                _tungayKH = value;
                if (_durationKH != null) _denngayKH = _tungayKH + _durationKH;
            }
        }
        public DateTime DenNgayKH
        {
            get { return _denngayKH; }
            set
            {
                _denngayKH = value;
                if (_durationKH != _denngayKH - _tungayKH)
                {
                    DurationKH = _denngayKH - _tungayKH;                    
                }
            }
        }
        TimeSpan _durationKH;
        
        public TimeSpan DurationKH
        {
            get
            {
                return _durationKH;
            }
            set
            {
                if (_durationKH != value)
                {
                    _durationKH = value;
                    
                    SoGioKH = _durationKH.Hours;
                    SoPhutKH = _durationKH.Minutes;
                    DenNgayKH= _tungayKH + _durationKH;
                }
            }
        }
        public int SoGioKH
        {
            get { return _sogioKH; }
            set
            {
                if (_sogioKH != value)
                {
                    _sogioKH = value;
                    DurationKH = new TimeSpan(_sogioKH, _sophutKH, 0);
                }
            }
        }
        public int SoPhutKH
        {
            get { return _sophutKH; }
            set
            {
                if (_sophutKH != value)
                {
                    _sophutKH = value;
                    DurationKH = new TimeSpan(_sogioKH, _sophutKH, 0);
                }
            }
        }
        public DateTime TuNgay
        {
            get { return _tungay; }
            set
            {
                if (value < DateTime.Now) TuNgay = _tungay;
                _tungay = value;
                if (Duration != null) _denngay = _tungay + Duration;
            }
        }
        public DateTime DenNgay
        {
            get { return _denngay; }
            set
            {
                _denngayKH = value;
                if (Duration != DenNgay - TuNgay)
                {
                    Duration = _denngay - _tungay;
                    SoGio = Duration.Hours;
                    SoPhut = Duration.Minutes;
                }
            }
        }
        int _sogioKH = 0;
        int _sophutKH = 0;
        int _sogio = 0;
        int _sophut = 0;
        
        public int SoGio
        {
            get { return _sogio; }
            set
            {
                _sogio = value;
                Duration = new TimeSpan(_sogio, _sophut, 0);
            }
        }
        public int SoPhut
        {
            get { return _sophut; }
            set
            {
                _sophut = value;
                Duration = new TimeSpan(_sogio, _sophut, 0);
            }
        }
        public string TenHang { get; set; }
        public decimal SoLuong { get; set; }
        public string SoCT { get; set; }
        public string TenKH { get; set; }
        public DateTime NgayGiao { get; set; }
        public int TrangThai { get; set; }
        public void UpdateDr()
        {
            if (dr != null)
            {
                dr["TuNgayKH"] = TuNgayKH;
                dr["DenNgayKH"] = DenNgayKH;
                dr["MaMin"] = MaMIn;
                dr["SoLuong"] = SoLuong;
                dr["GhiChu"] = GhiChu;
            }

        }
        Database db = Database.NewDataDatabase();
        public bool Save()
        {
            if (isNew)
            {
                string sql = "insert into ctLichSX(CtLichSXID, MTLSXID, DTLSXID, CTLSXID,TungayKH,DenNgayKH, SoLuong, GhiChu,MaMIn, MaVT, TenHang, TrangThai) " +
                    " values(@CtLichSXID, @MTLSXID, @DTLSXID,@CTLSXID,@TungayKH,@DenNgayKH, @SoLuong, @GhiChu,@MaMIn, @MaVT, @TenHang, @TrangThai)";
                string[] fields = new string[] { "@CtLichSXID", "@MTLSXID", "@DTLSXID","@CTLSXID","@TungayKH","@DenNgayKH","@SoLuong","@GhiChu", "@MaMIn", "@MaVT","@TenHang","@TrangThai" };
                object[] paras = new object[] { ctLichSXID, MTLSXID, DTLSXID, CTLSXID, TuNgayKH, DenNgayKH, SoLuong, GhiChu,MaMIn, MaVT, TenHang, TrangThai };
                db.UpdateDatabyPara(sql, fields, paras);
                if (!db.HasErrors)
                {
                    isNew = false;
                    dr.AcceptChanges();
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (dr.RowState == DataRowState.Modified)
                {
                    string sql = " update  ctLichSX set  MTLSXID =@MTLSXID, DTLSXID=@DTLSXID, CTLSXID=@CTLSXID,TungayKH=@TungayKH,DenNgayKH=@DenNgayKH, SoLuong=@SoLuong, GhiChu=@GhiChu,MaMIn=@MaMIn, MaVT=@MaVT, TenHang=@TenHang, TrangThai=@TrangThai " +
                        " where CtlSXID=@CTLSXID";
                    string[] fields = new string[] { "@CtLichSXID", "@MTLSXID", "@DTLSXID","@CTLSXID", "@TungayKH", "@DenNgayKH", "@SoLuong", "@GhiChu","@MaMIn", "@MaVT", "@TenHang", "@TrangThai" };
                    object[] paras = new object[] { ctLichSXID, MTLSXID, DTLSXID, CTLSXID, TuNgayKH, DenNgayKH, SoLuong, GhiChu,MaMIn, MaVT, TenHang, TrangThai };
                    db.UpdateDatabyPara(sql, fields, paras);
                    if (!db.HasErrors)
                    {
                        dr.AcceptChanges();
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return true;
            }
        }
       
        
    }
}
