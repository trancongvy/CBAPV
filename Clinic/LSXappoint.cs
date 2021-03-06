﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.UI;
using CDTDatabase;
using CDTLib;
namespace QLSX
{
    public class LSXappoint : Appointment
    {
        DataRow _dr;
        public  DataTable ctTangCa;
        internal LSXappoint(SchedulerControl schedu, DataRow __dr, ref DataTable _ctTangca) : base(AppointmentType.Normal)
        {
            // this = schedu.Storage.CreateAppointment(AppointmentType.Normal);
            ctTangCa = _ctTangca;
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
                if (dr["ctLichSXID"] == DBNull.Value)
                {
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
                TongSoGioKH = double.Parse(dr["TongSoGioKH"].ToString());
                //DenNgayKH = DateTime.Parse(dr["DenNgayKH"].ToString());
                TenHang = dr["TenHang"].ToString();
                SoLuong = decimal.Parse(dr["SoLuong"].ToString());
                SLDaNhap = decimal.Parse(dr["SLDaNhap"].ToString());
                SoCT = dr["SoCT"].ToString();
                TenKH = dr["TenKH"].ToString();
                NgayGiao = DateTime.Parse(dr["NgayGiao"].ToString());
                Description1 = dr["Description1"].ToString();
                if (dr["TuNgay"] != DBNull.Value) TuNgay = DateTime.Parse(dr["TuNgay"].ToString());
                if (dr["DenNgay"] == DBNull.Value) DenNgay = TuNgay;
                else
                    DenNgay = DateTime.Parse(dr["DenNgay"].ToString());
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
                if (_durationKH != null)
                    CalculateDenNgay(ref _tungayKH, ref _denngayKH, _durationKH);
                TrangThai = _trangthai;
            }
        }
        public DateTime DenNgayKH
        {
            get { return _denngayKH; }
            set
            {
                _denngayKH = value;

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
                    //Viết lại
                    _sogioKH = _durationKH.Hours + _durationKH.Days * 24;
                    _sophutKH = _durationKH.Minutes;

                }
            }
        }

        public void CalculateDenNgay(ref DateTime _bDate, ref DateTime eDate, TimeSpan dura)
        {

            DateTime bDate = _bDate;
            TimeSpan _dura = dura;
            while (dura.TotalMinutes > 0)
            {
                DateTime eMorning = DateTime.Parse(bDate.ToShortDateString());

                DateTime bWork = DateTime.Parse(bDate.ToShortDateString() + " " + Config.GetValue("TimeStart").ToString() + ":00");
                DateTime bLunch = DateTime.Parse(bDate.ToShortDateString() + " " + Config.GetValue("TimeStartLunch").ToString() + ":00");
                DateTime eLunch = DateTime.Parse(bDate.ToShortDateString() + " " + Config.GetValue("TimeEndLunch").ToString() + ":00");
                DateTime eWork = DateTime.Parse(bDate.ToShortDateString() + " " + Config.GetValue("TimeEnd").ToString() + ":00");
                if (bDate < bWork)
                {
                    bWork = DateTime.Parse(bDate.AddDays(-1).ToShortDateString() + " " + Config.GetValue("TimeStart").ToString() + ":00");
                    bLunch = DateTime.Parse(bDate.AddDays(-1).ToShortDateString() + " " + Config.GetValue("TimeStartLunch").ToString() + ":00");
                    eLunch = DateTime.Parse(bDate.AddDays(-1).ToShortDateString() + " " + Config.GetValue("TimeEndLunch").ToString() + ":00");
                    eWork = DateTime.Parse(bDate.AddDays(-1).ToShortDateString() + " " + Config.GetValue("TimeEnd").ToString() + ":00");
                }

                if (bWork.DayOfWeek == DayOfWeek.Saturday)// thứ 7
                {
                    if (Config.GetValue("SatudayWork").ToString() == "0")//Nghỉ cả ngày
                    {
                        bLunch = bWork; eLunch = bWork; eWork = bWork;
                    }
                    else if (Config.GetValue("SatudayWork").ToString() == "1")//làm nữa ngày
                    {
                        eLunch = bLunch; eWork = bLunch;
                    }
                }
                if (bWork.DayOfWeek == DayOfWeek.Sunday)// Chủ nhật
                {
                    bLunch = bWork; eLunch = bWork; eWork = bWork;
                }
                //tính tăng ca trưa
                DataRow[] drTangcaLunch = ctTangCa.Select("isLunch=1 and ngay='" + bWork.ToShortDateString() + "' and (MaMIn is null or MaMIn='" + MaMIn +"')");
                if (drTangcaLunch.Length > 0)
                {
                    bLunch = bLunch.AddMinutes(double.Parse(drTangcaLunch[0]["Sogio"].ToString()) * 60);// Không tính rơi vào ngày thứ 7 vì ngày thứ 7 nếu có tăng cả thì đưa vào tăng ca Night
                }
                DataRow[] drTangcaNight = ctTangCa.Select("isNight=1 and ngay='" + bWork.ToShortDateString() + "' and (MaMIn is null or MaMIn='" + MaMIn + "')");
                if (drTangcaNight.Length > 0)
                {
                    double sophut = double.Parse(drTangcaNight[0]["Sogio"].ToString()) * 60;
                    if (sophut < (DateTime.Parse(bDate.ToShortDateString()).AddDays(1) - eWork).TotalMinutes)
                    {
                        eWork = eWork.AddMinutes(sophut);
                    }
                    else
                    {
                        eWork = DateTime.Parse(bDate.ToShortDateString()).AddDays(1);
                    }
                }


                if (bDate < bLunch)//Bắt đầu trong buổi sáng
                {
                    if (dura.TotalMinutes <= (bLunch - bDate).TotalMinutes)// kết thức luôn trong buổi sáng 
                    {
                        eDate = bDate.Add(dura);
                        return;
                    }
                    else
                    {
                        dura = dura.Add(bDate - bLunch);
                        bDate = eLunch;
                    }
                }
                else if (bDate == bLunch)
                {
                    if (dura.TotalMinutes == _dura.TotalMinutes) _bDate = eLunch;
                    bDate = eLunch;
                }
                if (bDate < eWork)// Bắt đầu trong buổi chiều
                {
                    if (dura.TotalMinutes <= (eWork - bDate).TotalMinutes)// kết thức luôn trong buổi chiều
                    {
                        eDate = bDate.Add(dura);
                        return;
                    }
                    else
                    {
                        dura = dura.Add(bDate - eWork);
                        bDate = bWork.AddDays(1);

                    }
                }
                else
                {
                    bDate = bWork.AddDays(1);
                    if (dura.TotalMinutes == _dura.TotalMinutes) _bDate = bDate;
                }

            }
            eDate = bDate;
        }

        TimeSpan _durationTT;
        public TimeSpan DurationTT
        {
            get
            {
                return _durationTT;
            }
            set
            {
                if (_durationTT != value)
                {
                    _durationTT = value;
                    //Viết lại
                    SoGio = _durationTT.Hours;
                    SoPhut = _durationTT.Minutes;
                    CalculateDenNgay(ref _tungay, ref _denngay, DurationTT);
                }
            }
        }
        double _TongSoGioKH;
        double _TongSoGio;
        public double TongSoGioKH
        {
            get { return _TongSoGioKH; }
            set
            {
                _TongSoGioKH = value;
                _sogioKH = (int)TongSoGioKH;
                _sophutKH = (int)((TongSoGioKH - (double)SoGioKH) * 60);
                _durationKH = new TimeSpan(_sogioKH, _sophutKH, 0);
                CalculateDenNgay(ref _tungayKH, ref _denngayKH, _durationKH);
                TrangThai = _trangthai;
            }
        }
        public double TongSoGio
        {
            get { return _TongSoGio; }
            set
            {
                _TongSoGio = value;
                _sogio = (int)TongSoGio;
                _sophut = (int)((TongSoGio - (double)SoGio) * 60);
                _durationTT = new TimeSpan(_sogio, _sophut, 0);
                CalculateDenNgay(ref _tungay, ref _denngay, _durationTT);
                TrangThai = _trangthai;
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
                    TongSoGioKH = _sogioKH + (double)_sophutKH / 60;
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
                    TongSoGioKH = _sogioKH + (double)_sophutKH / 60;
                }
            }
        }
        public DateTime TuNgay
        {
            get { return _tungay; }
            set
            {
               // if (value < DateTime.Now) TuNgay = _tungay;
                _tungay = value;
               // if (_durationTT != null)
                //    CalculateDenNgay(ref _tungay, ref _denngay, _durationTT);
            }
        }
        public DateTime DenNgay
        {
            get { return _denngay; }
            set
            {
                _denngay = value;

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
                TongSoGio = _sogio + (double)_sophut / 60;
            }
        }
        public int SoPhut
        {
            get { return _sophut; }
            set
            {
                _sophut = value;
                TongSoGio = _sogio + (double)_sophut / 60;
            }
        }
        public string TenHang { get; set; }
        public decimal SoLuong { get; set; }
        public decimal SLDaNhap { get; set; }
        public string SoCT { get; set; }
        public string TenKH { get; set; }
        public DateTime NgayGiao { get; set; }
        int _trangthai;
        public int TrangThai
        {
            get { return _trangthai; }
            set
            {
                _trangthai = value;
                if (TrangThai == 0 || TrangThai == 3)
                {
                    Start = TuNgayKH;
                    End = DenNgayKH;
                }
                else if (TrangThai == 1)
                {
                    Start = TuNgay;
                    if (DenNgayKH > TuNgay)
                        End = DenNgayKH;
                    //else
                      //  End = TuNgay;
                }
                else if (TrangThai == 2)
                {
                    Start = TuNgay;
                    End = DenNgay;
                }
            }
        }
        public void UpdateDr()
        {
            if (dr != null)
            {
                dr["TuNgayKH"] = TuNgayKH;
                dr["DenNgayKH"] = DenNgayKH;
                dr["MaMin"] = MaMIn;
                dr["SoLuong"] = SoLuong;
                dr["GhiChu"] = GhiChu;
                dr.EndEdit();
            }

        }
        Database db = Database.NewDataDatabase();
        public bool Save()
        {
            if (isNew)
            {
                string sql = "insert into ctLichSX(CtLichSXID, MTLSXID, DTLSXID, CTLSXID,TungayKH,DenNgayKH,TongSoGioKH, SoLuong, GhiChu,MaMIn, MaVT, TenHang, TrangThai) " +
                    " values(@CtLichSXID, @MTLSXID, @DTLSXID,@CTLSXID,@TungayKH,@DenNgayKH,@TongSoGioKH, @SoLuong, @GhiChu,@MaMIn, @MaVT, @TenHang, @TrangThai)";
                string[] fields = new string[] { "@CtLichSXID", "@MTLSXID", "@DTLSXID", "@CTLSXID", "@TungayKH", "@DenNgayKH", "@TongSoGioKH", "@SoLuong", "@GhiChu", "@MaMIn", "@MaVT", "@TenHang", "@TrangThai" };
                object[] paras = new object[] { ctLichSXID, MTLSXID, DTLSXID, CTLSXID, TuNgayKH, DenNgayKH, TongSoGioKH, SoLuong, GhiChu, MaMIn, MaVT, TenHang, TrangThai };
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
                    string sql = " update  ctLichSX set  MTLSXID =@MTLSXID, DTLSXID=@DTLSXID, CTLSXID=@CTLSXID,TungayKH=@TungayKH,DenNgayKH=@DenNgayKH,TongSoGioKH=@TongSoGioKH, SoLuong=@SoLuong, GhiChu=@GhiChu,MaMIn=@MaMIn, MaVT=@MaVT, TenHang=@TenHang, TrangThai=@TrangThai " +
                        " where CtlSXID=@CTLSXID";
                    string[] fields = new string[] { "@CtLichSXID", "@MTLSXID", "@DTLSXID", "@CTLSXID", "@TungayKH", "@DenNgayKH", "@TongSoGioKH", "@SoLuong", "@GhiChu", "@MaMIn", "@MaVT", "@TenHang", "@TrangThai" };
                    object[] paras = new object[] { ctLichSXID, MTLSXID, DTLSXID, CTLSXID, TuNgayKH, DenNgayKH, TongSoGioKH, SoLuong, GhiChu, MaMIn, MaVT, TenHang, TrangThai };
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
    public class CustomTimeScaleDay : TimeScaleDay
    {
        DataTable ctTangCa;
        public CustomTimeScaleDay(DataTable _ctTangca)
        {
            DisplayFormat = "dd/MM";
            ctTangCa = _ctTangca;
        }
        public override DateTime Floor(DateTime date)
        {
            return date;
        }
        public override DateTime GetNextDate(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                DataRow[] dr = ctTangCa.Select("isNight=1 and ngay='" + date.AddDays(1).ToShortDateString() + "'");
                if (dr.Length == 0) return date.AddDays(2);
                else return date.AddDays(1);
            }
            else
                return date.AddDays(1);
        }
    }
    public class TimeScaleLessThanDay : TimeScaleHour
    {
        public DataTable ctTangCa;
        public TimeScaleLessThanDay(DataTable _ctTangca)
        {
            this.Width = 40;
            this.DisplayFormat = "h tt";
            ctTangCa = _ctTangca;
        }
        public override DateTime GetNextDate(DateTime date)
        {

            DateTime bLunch = DateTime.Parse(date.ToShortDateString() + " " + Config.GetValue("TimeStartLunch").ToString() + ":00");
            DateTime eLunch = DateTime.Parse(date.ToShortDateString() + " " + Config.GetValue("TimeEndLunch").ToString() + ":00");
            DateTime bWork = DateTime.Parse(date.ToShortDateString() + " " + Config.GetValue("TimeStart").ToString() + ":00");

            DateTime eWork = DateTime.Parse(date.ToShortDateString() + " " + Config.GetValue("TimeEnd").ToString() + ":00");

            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                if (Config.GetValue("SatudayWork").ToString() == "0")
                {
                    bLunch = bWork; eLunch = bWork; eWork = bWork;
                }
                else if (Config.GetValue("SatudayWork").ToString() == "1")
                {
                    eLunch = bLunch; eWork = bLunch;
                }
            }
            if (bWork.DayOfWeek == DayOfWeek.Sunday)// Chủ nhật
            {
                bLunch = bWork; eLunch = bWork; eWork = bWork;
            }
            DataRow[] ldr = ctTangCa.Select("isLunch =1 and ngay='" + date.ToShortDateString() + "'");//Tăng ca cho bất cứ nguồn lực nào
            if (ldr.Length > 0)
            {
                double sogioTca = 0;
                foreach (DataRow dr in ldr) //Lấy thằng nào tăng ca nhiều nhất làm chuẩn
                {
                    if (double.Parse(dr["Sogio"].ToString()) > sogioTca) sogioTca = double.Parse(dr["Sogio"].ToString());
                }
                bLunch = bLunch.AddHours(sogioTca);
            }
            ldr = ctTangCa.Select("isNight=1 and ngay='" + date.ToShortDateString() + "'");
            if (ldr.Length > 0)
            {
                double sogioTca = 0;
                foreach (DataRow dr in ldr)
                {
                    if (double.Parse(dr["Sogio"].ToString()) > sogioTca) sogioTca = double.Parse(dr["Sogio"].ToString());
                }
                if (sogioTca < (DateTime.Parse(eWork.ToShortDateString()).AddDays(1) - eWork).TotalHours)
                {
                    eWork = eWork.AddHours(sogioTca);
                }
                else
                {
                    eWork = DateTime.Parse(eWork.ToShortDateString()).AddDays(1);
                }
            }
            if ((date.AddHours(1) >= bWork && date.AddHours(1) < bLunch) || (date.AddHours(1) >= eLunch && date.AddHours(1) < eWork))
            {

                return date.AddHours(1);
            }
            else
            {
                return GetNextDate(date.AddHours(1));

            }

        }

    }
    public class TimeScaleLessThanDayMinute : TimeScaleFixedInterval
    {
        public DataTable ctTangCa;
        public TimeScaleLessThanDayMinute(DataTable _ctTangca,TimeSpan scaleValue)
            : base(scaleValue) {
            this.DisplayFormat = "mm";
            ctTangCa = _ctTangca;
        }
        public override DateTime GetNextDate(DateTime date)
        {

            DateTime bLunch = DateTime.Parse(date.ToShortDateString() + " " + Config.GetValue("TimeStartLunch").ToString() + ":00");
            DateTime eLunch = DateTime.Parse(date.ToShortDateString() + " " + Config.GetValue("TimeEndLunch").ToString() + ":00");
            DateTime bWork = DateTime.Parse(date.ToShortDateString() + " " + Config.GetValue("TimeStart").ToString() + ":00");

            DateTime eWork = DateTime.Parse(date.ToShortDateString() + " " + Config.GetValue("TimeEnd").ToString() + ":00");

            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                if (Config.GetValue("SatudayWork").ToString() == "0")
                {
                    bLunch = bWork; eLunch = bWork; eWork = bWork;
                }
                else if (Config.GetValue("SatudayWork").ToString() == "1")
                {
                    eLunch = bLunch; eWork = bLunch;
                }
            }
            if (bWork.DayOfWeek == DayOfWeek.Sunday)// Chủ nhật
            {
                bLunch = bWork; eLunch = bWork; eWork = bWork;
            }
            DataRow[] ldr = ctTangCa.Select("isLunch =1 and ngay='" + date.ToShortDateString() + "'");//Tăng ca cho bất cứ nguồn lực nào
            if (ldr.Length > 0)
            {
                double sogioTca = 0;
                foreach(DataRow dr in ldr) //Lấy thằng nào tăng ca nhiều nhất làm chuẩn
                {
                    if (double.Parse(dr["Sogio"].ToString()) > sogioTca) sogioTca = double.Parse(dr["Sogio"].ToString());
                }
                bLunch = bLunch.AddHours(sogioTca);
            }
            ldr = ctTangCa.Select("isNight=1 and ngay='" + date.ToShortDateString() + "'");
            if (ldr.Length > 0)
            {
                double sogioTca = 0;
                foreach (DataRow dr in ldr)
                {
                    if (double.Parse(dr["Sogio"].ToString()) > sogioTca) sogioTca = double.Parse(dr["Sogio"].ToString());
                }
                if (sogioTca < (DateTime.Parse(eWork.ToShortDateString()).AddDays(1) - eWork).TotalHours)
                {
                    eWork = eWork.AddHours(sogioTca);
                }
                else
                {
                    eWork = DateTime.Parse(eWork.ToShortDateString()).AddDays(1);
                }
            }
            if ((date.Add(Value) >= bWork && date.Add(Value) < bLunch) || (date.Add(Value) >= eLunch && date.Add(Value) < eWork))
            {

                return date.Add(Value);
            }
            else
            {
                return GetNextDate(date.Add(Value));

            }

        }

    }
}
