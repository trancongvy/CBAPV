using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CDTLib;
using CDTDatabase;
using DevExpress.XtraScheduler;
using System.ComponentModel;
using DevExpress.XtraScheduler.UI;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon;
using DevExpress.Utils.Menu;
using DevExpress.XtraScheduler.Drawing;
namespace QLSX
{
   
    public partial class fLichSX : XtraForm
    {
        public fLichSX()
        {
            InitializeComponent();

            sHeight.EditValueChanging += SHeight_EditValueChanging;

            sWidth.EditValueChanging += SWidth_EditValueChanging;

            sResource.EditValueChanging += SResource_EditValueChanging;
            schedu.CustomDrawTimeCell += Schedu_CustomDrawTimeCell;
            this.FormClosing += FLichSX_FormClosing;
            
        }

        private void FLichSX_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Changed)
            {
                if(MessageBox.Show("Bạn có muốn lưu những thay đổi không?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    bool result = true;
                    foreach (LSXappoint apt in ctLichSXBind)
                    {
                        result = result && apt.Save();
                    }
                    if (result)
                    {
                        MessageBox.Show("Đã lưu thành công");
                    }
                }
            }
        }

        bool Changed = false;
        Color lightColor;
        Color grayColor;
        Resource res = null;
        private void Schedu_CustomDrawTimeCell(object sender, CustomDrawObjectEventArgs e)
        {
            Color workColor;
            //if (!chkShowWork.Checked) return;
            
           TimeCell Cellinfo = e.ObjectInfo as TimeCell;

            DateTime bLunch = DateTime.Parse(Cellinfo.Interval.Start.ToShortDateString() + " " + Config.GetValue("TimeStartLunch").ToString() + ":00");
            DateTime eLunch = DateTime.Parse(Cellinfo.Interval.Start.ToShortDateString() + " " + Config.GetValue("TimeEndLunch").ToString() + ":00");
            DateTime bWork = DateTime.Parse(Cellinfo.Interval.Start.ToShortDateString() + " " + Config.GetValue("TimeStart").ToString() + ":00");

            DateTime eWork = DateTime.Parse(Cellinfo.Interval.Start.ToShortDateString() + " " + Config.GetValue("TimeEnd").ToString() + ":00");
           
            if (Cellinfo.Interval.Start.DayOfWeek == DayOfWeek.Saturday)
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
            string MayinCondition;
           
            if (Cellinfo.Resource.Id.GetType()==typeof(DevExpress.XtraScheduler.Native.EmptyResource))
                MayinCondition = " MaMIn is null";
            else
                MayinCondition = " MaMIn='" + Cellinfo.Resource.Id.ToString() + "'";
            DataRow[] dr = ctTangCa.Select("isLunch =1 and ngay='" + Cellinfo.Interval.Start.ToShortDateString() + "' and " +MayinCondition);
            if (dr.Length > 0)
            {
                bLunch = bLunch.AddHours(double.Parse(dr[0]["Sogio"].ToString()));
            }
            dr = ctTangCa.Select("isNight=1 and ngay='" + Cellinfo.Interval.Start.ToShortDateString() + "' and " + MayinCondition);
            if (dr.Length > 0)
            {
                double sogio = double.Parse(dr[0]["Sogio"].ToString()) ;
                if (sogio < (DateTime.Parse(eWork.ToShortDateString()).AddDays(1) - eWork).TotalHours)
                {
                    eWork = eWork.AddHours(sogio);
                }
                else
                {
                    eWork = DateTime.Parse(eWork.ToShortDateString()).AddDays(1);
                }
            }
            //if (Cellinfo != null && Cellinfo.Interval.Duration.Hours == 1 && Cellinfo.IsWorkTime)
            //{//
            //    lightColor = Color.White;//Cellinfo.Appearance.BackColor;//
            //}
            //else if (Cellinfo != null && Cellinfo.Interval.Duration.Hours == 1 && !Cellinfo.IsWorkTime)
            //{
            //    grayColor = Color.Gray;// ellinfo.Appearance.BackColor;
            //}

            //     Color.White;//

            res = Cellinfo.Resource;
            DateTime start = Cellinfo.Interval.Start;
            if (schedu.Storage == null) return;
            int idx = schedu.Storage.Resources.Items.IndexOf(Cellinfo.Resource);
            if (idx == -1) return;
            if (Cellinfo.Interval.Duration.Hours >= 1)
            {
                if ((start >= bWork && start < bLunch) || (start >= eLunch && start < eWork))
                {

                    Cellinfo.Appearance.BackColor = schedu.ResourceColorSchemas[idx].CellLight;
                }
                else
                {
                    Cellinfo.Appearance.BackColor = schedu.ResourceColorSchemas[idx].Cell;


                }
            }
            else if (Cellinfo.Interval.Duration.Days == 1)
            {
                if (bWork < eWork)
                    Cellinfo.Appearance.BackColor = schedu.ResourceColorSchemas[idx].CellLight;
                else Cellinfo.Appearance.BackColor = schedu.ResourceColorSchemas[idx].Cell;
            }
            if(Cellinfo.Interval.Duration.Minutes >= 15)
            {
                if ((start >= bWork && start < bLunch) || (start >= eLunch && start < eWork))
                {

                    Cellinfo.Appearance.BackColor = schedu.ResourceColorSchemas[idx].CellLight;
                }
                else
                {
                    Cellinfo.Appearance.BackColor = schedu.ResourceColorSchemas[idx].Cell;


                }
            }


            }

        Database dbdata = Database.NewDataDatabase();
        Database dbStrucst = Database.NewStructDatabase();
        DataSet ds = new DataSet();
        DataTable ctLichSX;
        DataTable dmMayin;
        DataTable dmVT;
        DataTable ctTangCa;
        DataTable LSXChuaSapLich;
        BindingList<LSXappoint> ctLichSXBind=new BindingList<LSXappoint>();
        TimeScaleCollection scales;
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (dateEdit1.EditValue == null || dateEdit1.EditValue == null) return;
            getData(dateEdit1.DateTime, dateEdit2.DateTime);
            if (ctLichSX != null)
            {
                if (ds.Tables.Contains("CtLichSX")) ds.Tables.Remove("CtLichSX");
                ds.Tables.Add(ctLichSX); 
            }
        }
        public LSXappoint GenAppoint(DataRow dr)
        {
            LSXappoint apt = new LSXappoint(schedu,dr, ref ctTangCa);
            return apt;
        }
        private void getData(DateTime ngayct1, DateTime ngayct2)
        {
            //throw new NotImplementedException();
            ctTangCa = dbdata.GetDataTable("select * from ctTangCa where ngay>='" + ngayct1.AddDays(-1).ToString() + "'");
            LSXChuaSapLich = dbdata.GetDataSetByStore("GetMTLSXchuaThuchien", new string[] { }, new object[] { });
            LSXChuaSapLich.TableName = "MTLSX";
            gridControl1.DataSource = LSXChuaSapLich;
            ctLichSX = dbdata.GetDataSetByStore("getLichSX", new string[] { "@Tungay", "@Denngay" }, new object[] { ngayct1, ngayct2 });
            ctLichSX.TableName = "CtLichSX";
            this.schedulerStorage1.BeginUpdate();
            ctLichSXBind.Clear();
            this.schedulerStorage1.Appointments.Clear();
            
            foreach (DataRow dr in ctLichSX.Rows)
            {
                ctLichSXBind.Add(GenAppoint(dr));
            }
            this.schedulerStorage1.Appointments.DataSource = ctLichSXBind;


            ShowDefaultTimescale(!chkShowWork.Checked);
            this.schedulerStorage1.EndUpdate();

        }
        private void fLichSX_Load(object sender, EventArgs e)
        {
            lightColor = Color.Black;
            grayColor = Color.Black;
            dockManager1.BeginUpdate();
            DateTime bWork = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + Config.GetValue("TimeStart").ToString() + ":00");
            DateTime eWork = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + Config.GetValue("TimeEnd").ToString() + ":00");
            schedu.TimelineView.WorkTime = new TimeOfDayInterval(new TimeSpan(bWork.Hour, bWork.Minute, bWork.Second), new TimeSpan(eWork.Hour, eWork.Minute, eWork.Second));
            schedu.WorkWeekView.WorkTime = new TimeOfDayInterval(new TimeSpan(bWork.Hour, bWork.Minute, bWork.Second), new TimeSpan(eWork.Hour, eWork.Minute, eWork.Second));
            schedu.DayView.WorkTime = new TimeOfDayInterval(new TimeSpan(bWork.Hour, bWork.Minute, bWork.Second), new TimeSpan(eWork.Hour, eWork.Minute, eWork.Second));
            dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
            dockPanel2.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
            dockPanel3.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
            dockPanel3.HideImmediately();
            dockPanel2.HideImmediately();
            dockPanel1.HideImmediately();
            dockManager1.EndUpdate();
            //this.barEditItem1.Edit.EditValueChanging += Edit_EditValueChanging;
            //this.barEditItem2.Edit.EditValueChanging += Edit_EditValueChanging1;
            schedu.AllowAppointmentDelete += Schedu_AllowAppointmentDelete;
            schedu.AllowAppointmentCreate += Schedu_AllowAppointmentCreate;
            schedu.EditAppointmentFormShowing += Schedu_EditAppointmentFormShowing;
            schedu.AppointmentDrag += Schedu_AppointmentDrag;
            schedu.Storage.AppointmentChanging += Storage_AppointmentChanging;
            
            dateEdit1.DateTime = DateTime.Parse(DateTime.Now.ToShortDateString());
            dateEdit2.DateTime = DateTime.Parse(DateTime.Now.AddDays(7).ToShortDateString());
            schedu.QueryWorkTime += Schedu_QueryWorkTime;
            //Resource

            dmMayin = dbdata.GetDataTable("select * from dmMIn  order by Sorted");
            dmMayin.TableName = "DmMIn";
            ds.Tables.Add(dmMayin);
            dmMInBindingSource.DataSource = ds;
            dmMInBindingSource.DataMember = "DmMIn";
            //Schedu
            scales = new TimeScaleCollection();
            foreach (TimeScale scl in schedu.TimelineView.Scales)
            {
                scales.Add(scl);
            }
            getData(dateEdit1.DateTime, dateEdit2.DateTime);
            //
           // ctLichSXBind = new BindingList<LSXappoint>();
            
            this.schedulerStorage1.BeginUpdate();
            this.schedulerStorage1.Appointments.DataSource =  ctLichSXBind;//ctLichSX;//
            this.schedulerStorage1.Appointments.Mappings.Description = "GhiChu";
            this.schedulerStorage1.Appointments.Mappings.End = "End";
            this.schedulerStorage1.Appointments.Mappings.Location = "TenHang";
            this.schedulerStorage1.Appointments.Mappings.RecurrenceInfo = "MaMIn";
            this.schedulerStorage1.Appointments.Mappings.ResourceId = "MaMIn";
            this.schedulerStorage1.Appointments.Mappings.Start = "Start";
            // this.schedulerStorage1.Appointments.Mappings.Type = "LichSXID";
            this.schedulerStorage1.Appointments.Mappings.Subject = "SoCT";

            this.schedulerStorage1.Appointments.Mappings.Label = "TrangThai";
            
            this.schedulerStorage1.Resources.DataSource = this.dmMInBindingSource;
            schedulerStorage1.Appointments.CustomFieldMappings["ctID"].Member = "ctLichSXID";
            this.schedulerStorage1.Resources.Mappings.Caption = "TenMIn";
            this.schedulerStorage1.Resources.Mappings.Id = "MaMIn";
            this.schedulerStorage1.Resources.Mappings.Image = "Hinh";
            this.schedulerStorage1.EndUpdate();
            schedu.GoToToday();
            
            // getData(dateEdit1.DateTime, dateEdit2.DateTime);

        }

        private void Schedu_QueryWorkTime(object sender, QueryWorkTimeEventArgs e)
        {
            
        }

        private void Storage_AppointmentChanging(object sender, PersistentObjectCancelEventArgs e)
        {
            List<LSXappoint> x = ctLichSXBind.ToList();
            LSXappoint apt = x.Find(m => m.ctLichSXID.ToString() == e.Object.CustomFields["ctID"].ToString());

            if (apt != null)
            {
                if (apt.TrangThai !=0 )
                {
                    e.Cancel = true;
                    return;
                }
                if((e.Object as Appointment).Start <= DateTime.Now)
                {
                    e.Cancel = true;
                    return;
                }
                    if ((e.Object as Appointment).Start != apt.Start) apt.TuNgayKH = (e.Object as Appointment).Start;
                if ((e.Object as Appointment).ResourceId != apt.ResourceId)
                {
                    apt.ResourceId= (e.Object as Appointment).ResourceId;
                    apt.TuNgayKH = (e.Object as Appointment).Start;
                }
                    (e.Object as Appointment).Start = apt.Start;
                (e.Object as Appointment).End = apt.End;
                apt.UpdateDr();
                //fAppoint fA = new fAppoint(schedu, apt, false);

                //if (fA.ShowDialog() == DialogResult.OK)
                //{
                //    (e.Object as Appointment).Start = apt.Start;
                //    (e.Object as Appointment).End = apt.End;
                //}
                Changed = true;
            }
        
        }



        private void Schedu_AppointmentDrag(object sender, AppointmentDragEventArgs e)
        {
            Changed = true;
            // throw new NotImplementedException();
        }

        private void Schedu_EditAppointmentFormShowing(object sender, AppointmentFormEventArgs e)
        {

           
            List<LSXappoint> x = ctLichSXBind.ToList();
            if (e.Appointment.CustomFields["ctID"] == null) {
                 e.Handled = true;
                fTangca fTca;
                if (schedu.SelectedInterval != null)
                {
                    fTca = new fTangca(schedu.SelectedInterval,schedu.SelectedResource);
                }
                else
                {
                    fTca = new fTangca(new TimeInterval(e.Appointment.Start, e.Appointment.End), null);
                }
                fTca.ctTangca = this.ctTangCa.Clone();
                DataRow drTangca = findTangca(schedu.SelectedInterval, schedu.SelectedResource);
                
                if (drTangca!=null)
                {
                    DataRow dr = fTca.ctTangca.NewRow();
                    dr.ItemArray = (object[])drTangca.ItemArray.Clone();
                    fTca.ctTangca.Rows.Add(dr);
                }
                fTca.dmMIn = dmMayin;

                if (fTca.ShowDialog() == DialogResult.OK)
                {
                    if (fTca.ctTangca.Rows.Count > 0)
                    {
                        if (drTangca == null)
                        {
                            DataRow dr = this.ctTangCa.NewRow();
                            dr.ItemArray = (object[])fTca.ctTangca.Rows[0].ItemArray.Clone();
                            ctTangCa.Rows.Add(dr);
                        }
                        ctTangCa.AcceptChanges();
                        //Tính lại DenNgayKH 
                        foreach(LSXappoint apt1 in x)
                        {
                            apt1.ctTangCa = ctTangCa;
                            if (apt1.TrangThai <2 ) //khởi tạo hoặc đang chạy
                            {                               
                                apt1.TongSoGioKH = apt1.TongSoGioKH;
                                
                            }
                        }
                        schedu.RefreshData();
                    }
                }
                    return;

            }
            else e.Handled = true;
            LSXappoint apt = x.Find(m => m.ctLichSXID.ToString() == e.Appointment.CustomFields["ctID"].ToString());
            if (apt != null) {
                fAppoint fA = new fAppoint(schedu,apt,false);
                if (fA.ShowDialog() == DialogResult.OK)
                {
                    e.Appointment.Start = apt.Start;
                    e.Appointment.End = apt.End;
                }

            } 
        }

        private DataRow findTangca(TimeInterval selectedInterval, Resource selectedResource)
        {
            DateTime bLunch = DateTime.Parse(selectedInterval.Start.ToShortDateString() + " " + Config.GetValue("TimeStartLunch").ToString() + ":00");
            DateTime eLunch = DateTime.Parse(selectedInterval.Start.ToShortDateString() + " " + Config.GetValue("TimeEndLunch").ToString() + ":00");
            DateTime bWork = DateTime.Parse(selectedInterval.Start.ToShortDateString() + " " + Config.GetValue("TimeStart").ToString() + ":00");
            DateTime eWork = DateTime.Parse(selectedInterval.Start.ToShortDateString() + " " + Config.GetValue("TimeEnd").ToString() + ":00");
            DataRow[] ldr = ctTangCa.Select("MaMin='" + selectedResource.Id.ToString() + "' and Ngay='" + selectedInterval.Start.ToShortDateString() + "'");
            if (ldr.Length == 0)
                return null;
            else
            {
                DateTime date = DateTime.Parse(selectedInterval.Start.ToShortDateString());
                foreach(DataRow dr in ldr)
                {
                    if(bool.Parse(dr["isLunch"].ToString()) )
                    {
                        if (selectedInterval.Start == bLunch) return dr;
                        if (selectedInterval.Start > bLunch && selectedInterval.Start < eLunch) return dr;
                    }
                    if(bool.Parse(dr["isNight"].ToString()))
                    {
                        if (selectedInterval.Start == eWork) return dr;
                        if (selectedInterval.Start > eWork && selectedInterval.Start < bWork.AddDays(1)) return dr;
                    }
                }
                return null;
            }
        }

        private void Schedu_AllowAppointmentCreate(object sender, AppointmentOperationEventArgs e)
        {
            // e.Allow = false;
            Changed = true;
        }

        private void Schedu_AllowAppointmentDelete(object sender, AppointmentOperationEventArgs e)
        {
            e.Allow = true;
            Changed = true;
        }

        

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            schedu.ActiveViewType = SchedulerViewType.Week;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            
            if (gridView1.SelectedRowsCount == 0) return;
            DataRow drLSX = gridView1.GetDataRow(gridView1.GetSelectedRows()[0]);
            DataRow dr = ctLichSX.NewRow();
            dr["MTLSXID"] = drLSX["MTLSXID"];
            dr["DTLSXID"] = drLSX["DTLSXID"];
            dr["CTLSXID"] = drLSX["CTLSXID"];
            dr["MaMin"] = drLSX["MaMin"];
            dr["SoCT"] = drLSX["SoCT"];
            dr["NgayGiao"] = drLSX["NgayGiao"];
            dr["GhiChu"] = drLSX["GhiChu"];
            dr["Description1"] = drLSX["Description1"];
            dr["MaKH"] = drLSX["MaKH"];
            dr["TenKH"] = drLSX["TenKH"];
            dr["MaVT"] = drLSX["MaVT"];
            dr["TenHang"] = drLSX["TenHang"];
            dr["SoLuong"] = drLSX["SoLuong"];
            dr["TrangThai"] = 0;
            dr["SLDaNhap"] = 0;
            if (schedu.SelectedInterval == null)
            {
                dr["TuNgayKH"] = DateTime.Parse(DateTime.Now.ToShortDateString());
                dr["DenNgayKH"] = DateTime.Parse(DateTime.Parse(dr["TuNgayKH"].ToString()).AddHours(2).ToString());
                dr["TongsoGioKH"] = 2;
            }
            else
            {
                dr["TuNgayKH"] = schedu.SelectedInterval.Start;
                dr["DenNgayKH"] = schedu.SelectedInterval.End;
                dr["TongsoGioKH"] = schedu.SelectedInterval.Duration.TotalHours;// + schedu.SelectedInterval.Duration.Minutes/60;
            }
           
            LSXappoint apt = new LSXappoint(schedu,dr, ref ctTangCa);
          
           // apt.Start = DateTime.Parse(DateTime.Now.ToShortDateString());
          //  apt.End = DateTime.Parse(DateTime.Parse(dr["TuNgayKH"].ToString()).AddHours(2).ToString());

            fAppoint Af = new fAppoint(schedu,apt,false);
            if (Af.ShowDialog() == DialogResult.OK)
            {
                ctLichSXBind.Add(apt);
                ctLichSX.Rows.Add(dr); 
                LSXChuaSapLich.Rows.Remove(drLSX);
                LSXChuaSapLich.AcceptChanges();
            }
            

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            
            for (int i =0; i<schedu.Storage.Appointments.Count; i++)
            {
                Appointment apt = schedu.Storage.Appointments[i];
            }
        }



        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            schedu.ActiveViewType = SchedulerViewType.Day;
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            schedu.ActiveViewType = SchedulerViewType.Week;
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            schedu.ActiveViewType = SchedulerViewType.Timeline;
        }
        private void SResource_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {try
            {
                schedu.ActiveView.ResourcesPerPage = int.Parse(e.NewValue.ToString());

            }
            catch
            {
                e.Cancel = true;
            }
        }

        private void SWidth_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                if (schedu.Views.TimelineView.Scales[5].Enabled)
                    schedu.Views.TimelineView.Scales[5].Width = int.Parse(e.NewValue.ToString());
                else if (schedu.Views.TimelineView.Scales[4].Enabled)
                    schedu.Views.TimelineView.Scales[4].Width = int.Parse(e.NewValue.ToString());
            }
            catch { e.Cancel = true; }
        }

        private void SHeight_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                schedu.Views.DayView.RowHeight = int.Parse(e.NewValue.ToString());
            }
            catch
            { e.Cancel = true; }

        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bool result = true;
            foreach(LSXappoint apt in ctLichSXBind)
            {
               result=result && apt.Save();
            }
            if (result)
            {
                MessageBox.Show("Đã lưu thành công");
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            simpleButton1_Click(sender, new EventArgs());
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            schedu.ActiveViewType = SchedulerViewType.WorkWeek;
        }

        private void chkShowWork_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           
                ShowDefaultTimescale(!chkShowWork.Checked);


        }

        private void ShowDefaultTimescale(bool isDefault)
        {
            schedu.TimelineView.Scales.Clear();
            if (isDefault)
            {
                schedu.TimelineView.Scales.AddRange(scales.ToList());
            }
            else
            { 

                TimeScaleDay ts = new CustomTimeScaleDay(ctTangCa);
                TimeScaleLessThanDay tsh = new TimeScaleLessThanDay(ctTangCa);
                TimeScaleLessThanDayMinute tsm = new TimeScaleLessThanDayMinute(ctTangCa,new TimeSpan(0,15,0));
                tsm.Enabled = false;
                schedu.TimelineView.Scales.Add(scales[0]);
                schedu.TimelineView.Scales.Add(scales[1]);
                schedu.TimelineView.Scales.Add(scales[2]);
                schedu.TimelineView.Scales.Add(scales[3]);
                schedu.TimelineView.Scales.Add(scales[4]);
                schedu.TimelineView.Scales.Add(tsh);
                schedu.TimelineView.Scales.Add(tsm);
            }
        }

        private void bHeight_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void schedu_Click(object sender, EventArgs e)
        {

        }
    }
}
