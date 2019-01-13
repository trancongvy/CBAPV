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
namespace QLSX
{
    public partial class fLichSX : Form
    {
        public fLichSX()
        {
            InitializeComponent();
        }
        Database dbdata = Database.NewDataDatabase();
        Database dbStrucst = Database.NewStructDatabase();
        DataSet ds = new DataSet();
        DataTable ctLichSX;
        DataTable dmMayin;
        DataTable dmVT;
        DataTable LSXChuasap;
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (dateEdit1.EditValue == null || dateEdit1.EditValue == null) return;
            getData(dateEdit1.DateTime, dateEdit2.DateTime);
            if (ctLichSX != null)
            {
                if (ds.Tables.Contains("CtLichSX")) ds.Tables.Remove("CtLichSX");
                ds.Tables.Add(ctLichSX);
                ctLichSXBindingSource.DataSource = ds;
                ctLichSXBindingSource.DataMember = "CtLichSX";
                dmMInBindingSource.DataSource = ds;
                dmMInBindingSource.DataMember = "DmMin";
                BindingList<LSXappoint> ctLichSXBind = new BindingList<LSXappoint>();

                this.schedulerStorage1.BeginInit();
                this.schedulerStorage1.Appointments.DataSource = ctLichSXBind;                
                this.schedulerStorage1.Appointments.Mappings.Description ="GhiChu" ;
                this.schedulerStorage1.Appointments.Mappings.End = "DenNgayKH";
                this.schedulerStorage1.Appointments.Mappings.Location = "SoLuong";
                this.schedulerStorage1.Appointments.Mappings.RecurrenceInfo = "MaMIn";
                this.schedulerStorage1.Appointments.Mappings.ResourceId = "MaMIn";
                this.schedulerStorage1.Appointments.Mappings.Start = "TuNgayKH";
               // this.schedulerStorage1.Appointments.Mappings.Type = "LichSXID";
                this.schedulerStorage1.Appointments.Mappings.Subject = "SoCT";
                this.schedulerStorage1.Appointments.Mappings.Label = "TenHang";
                this.schedulerStorage1.Resources.DataSource = this.dmMInBindingSource;
                this.schedulerStorage1.Resources.Mappings.Caption = "TenMIn";
                this.schedulerStorage1.Resources.Mappings.Id = "MaMIn";
                this.schedulerStorage1.Resources.Mappings.Image = "Hinh";
                this.schedulerStorage1.EndInit();
            }
        }
        public LSXappoint GenAppoint(DataRow dr)
        {
            LSXappoint apt = new LSXappoint();
            apt.dr = dr;
            return apt;
        }
        private void getData(DateTime ngayct1, DateTime ngayct2)
        {
            //throw new NotImplementedException();
           
            
           ctLichSX= dbdata.GetDataSetByStore("GetLichSX", new string[] { "@Tungay","@Denngay" }, new object[] { ngayct1, ngayct2 });
            ctLichSX.TableName = "CtLichSX";
        }
        private void fLichSX_Load(object sender, EventArgs e)
        {
            dmMayin = dbdata.GetDataTable("select * from dmMin");
            dmMayin.TableName = "DmMIn";
            ds.Tables.Add(dmMayin);
            dockManager1.BeginUpdate();
            dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
            dockPanel2.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
            dockPanel3.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
            dockPanel3.HideImmediately();
            dockPanel2.HideImmediately();
            dockPanel1.HideImmediately();
            dockManager1.EndUpdate();
            this.barEditItem1.Edit.EditValueChanging += Edit_EditValueChanging;
            this.barEditItem2.Edit.EditValueChanging += Edit_EditValueChanging1;
            schedu.AllowAppointmentDelete += Schedu_AllowAppointmentDelete;
            schedu.AllowAppointmentCreate += Schedu_AllowAppointmentCreate;
            schedu.EditAppointmentFormShowing += Schedu_EditAppointmentFormShowing;
            schedu.GoToToday();
            
        }

        private void Schedu_EditAppointmentFormShowing(object sender, AppointmentFormEventArgs e)
        {
            e.Handled = true;
            fAppoint fA = new fAppoint(schedu, e.Appointment);
            fA.ShowDialog();
        }

        private void Schedu_AllowAppointmentCreate(object sender, AppointmentOperationEventArgs e)
        {
            e.Allow = false;
        }

        private void Schedu_AllowAppointmentDelete(object sender, AppointmentOperationEventArgs e)
        {
            e.Allow = true;
        }

        private void Edit_EditValueChanging1(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            schedu.ActiveView.ResourcesPerPage = int.Parse(e.NewValue.ToString());
        }

        private void Edit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            
            if (schedu.Views.TimelineView.Scales[5].Enabled)
                schedu.Views.TimelineView.Scales[5].Width = int.Parse(e.NewValue.ToString());
            else if (schedu.Views.TimelineView.Scales[4].Enabled)
                schedu.Views.TimelineView.Scales[4].Width = int.Parse(e.NewValue.ToString());
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            schedu.ActiveViewType = SchedulerViewType.Timeline;
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            schedu.ActiveViewType = SchedulerViewType.Day;
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            schedu.ActiveViewType = SchedulerViewType.Week;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            fAppoint fA = new fAppoint(schedu, new Appointment());
            fA.ShowDialog();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            
            for (int i =0; i<schedu.Storage.Appointments.Count; i++)
            {
                Appointment apt = schedu.Storage.Appointments[i];
            }
        }
    }
}
