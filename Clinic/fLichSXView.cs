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
namespace QLSX
{
    public partial class fLichSXView : XtraForm
    {
        public fLichSXView()
        {
            InitializeComponent();

            sHeight.EditValueChanging += SHeight_EditValueChanging;

            sWidth.EditValueChanging += SWidth_EditValueChanging;

            sResource.EditValueChanging += SResource_EditValueChanging;
           
        }

       

        Database dbdata = Database.NewDataDatabase();
        Database dbStrucst = Database.NewStructDatabase();
        DataSet ds = new DataSet();
        DataTable ctLichSX;
        DataTable dmMayin;
        DataTable dmVT;
        DataTable LSXChuaSapLich;
        DataTable ctTangCa;
        BindingList<LSXappoint> ctLichSXBind=new BindingList<LSXappoint>();
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
            this.schedulerStorage1.EndUpdate();

        }
        private void fLichSX_Load(object sender, EventArgs e)
        {
            dockManager1.BeginUpdate();
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

            dmMayin = dbdata.GetDataTable("select * from dmMin");
            dmMayin.TableName = "DmMIn";
            ds.Tables.Add(dmMayin);
            dmMInBindingSource.DataSource = ds;
            dmMInBindingSource.DataMember = "DmMin";
            //Schedu

            getData(dateEdit1.DateTime, dateEdit2.DateTime);
            //
           // ctLichSXBind = new BindingList<LSXappoint>();
            
            this.schedulerStorage1.BeginUpdate();
            this.schedulerStorage1.Appointments.DataSource =  ctLichSXBind;//ctLichSX;//
            this.schedulerStorage1.Appointments.Mappings.Description = "GhiChu";
            this.schedulerStorage1.Appointments.Mappings.End = "DenNgayKH";
            this.schedulerStorage1.Appointments.Mappings.Location = "TenHang";
            this.schedulerStorage1.Appointments.Mappings.RecurrenceInfo = "MaMIn";
            this.schedulerStorage1.Appointments.Mappings.ResourceId = "MaMIn";
            this.schedulerStorage1.Appointments.Mappings.Start = "TuNgayKH";
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
                if (apt.TuNgayKH<= DateTime.Now || (e.Object as Appointment).Start <= DateTime.Now || apt.TrangThai>0)
                {
                    e.Cancel = true;
                    return;
                }
                apt.TuNgayKH = (e.Object as Appointment).Start;
                apt.DenNgayKH = (e.Object as Appointment).End;
                apt.UpdateDr();
            }
        }



        private void Schedu_AppointmentDrag(object sender, AppointmentDragEventArgs e)
        {
           
            // throw new NotImplementedException();
        }

        private void Schedu_EditAppointmentFormShowing(object sender, AppointmentFormEventArgs e)
        {

           
            List<LSXappoint> x = ctLichSXBind.ToList();
            if (e.Appointment.CustomFields["ctID"] == null) {
                 e.Handled = false;
                
               // AppointmentForm f = new AppointmentForm(schedu, new Appointment());
               // f.ShowDialog();
                return; }
            else e.Handled = true;
            LSXappoint apt = x.Find(m => m.ctLichSXID.ToString() == e.Appointment.CustomFields["ctID"].ToString());
            if (apt != null) {
                fAppoint fA = new fAppoint(schedu,apt,true);
                if (fA.ShowDialog() == DialogResult.OK)
                {
                    e.Appointment.Start = apt.TuNgayKH;
                    e.Appointment.End = apt.DenNgayKH;
                }

            } 
        }

        private void Schedu_AllowAppointmentCreate(object sender, AppointmentOperationEventArgs e)
        {
           // e.Allow = false;
        }

        private void Schedu_AllowAppointmentDelete(object sender, AppointmentOperationEventArgs e)
        {
            e.Allow = true;
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
            dr["SoLuong"] = drLSX["SoLuongTP"];
            dr["TrangThai"] = 0;
            dr["SLDaNhap"] = 0;
            if (schedu.SelectedInterval == null)
            {
                dr["TuNgayKH"] = DateTime.Parse(DateTime.Now.ToShortDateString());
                dr["DenNgayKH"] = DateTime.Parse(DateTime.Parse(dr["TuNgayKH"].ToString()).AddHours(2).ToString());
            }
            else
            {
                dr["TuNgayKH"] = schedu.SelectedInterval.Start;
                dr["DenNgayKH"] = schedu.SelectedInterval.End;
            }
           
            LSXappoint apt = new LSXappoint(schedu,dr, ref ctTangCa);
          
            apt.Start = DateTime.Parse(DateTime.Now.ToShortDateString());
            apt.End = DateTime.Parse(DateTime.Parse(dr["TuNgayKH"].ToString()).AddHours(2).ToString());

            fAppoint Af = new fAppoint(schedu,apt,true);
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
            foreach(LSXappoint apt in ctLichSXBind)
            {
                apt.Save();
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

       
        
    }
}
