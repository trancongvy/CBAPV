using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.UI;

namespace QLSX
{
    public partial class fAppoint : XtraForm
    {
        LSXappoint _apt;
        public AppointmentFormController controller;
        public fAppoint(SchedulerControl schedu, Appointment apt, bool isView)
        {
            InitializeComponent();
            _apt = apt as LSXappoint;
            gMayIn.Properties.DataSource = schedu.Storage.Resources.DataSource;
            Appointment ap = schedu.Storage.CreateAppointment(AppointmentType.Normal);

            //controller = new AppointmentFormController(schedu,ap);

            tbOk.Visible = !isView;
          
        }

        private void fAppont_Load(object sender, EventArgs e)
        {
            tSoCT.DataBindings.Add("Text", _apt, "SoCT");
            dNgayGiao.DataBindings.Add("EditValue", _apt, "NgayGiao");
            tMaKH.DataBindings.Add("Text", _apt, "MaKH");
            tTenKH.DataBindings.Add("Text", _apt, "TenKH");
            tMaVT.DataBindings.Add("Text", _apt, "MaVT");
            tTenHang.DataBindings.Add("Text", _apt, "TenHang");
            cSoluong.DataBindings.Add("EditValue", _apt, "SoLuong");
            cSLDaNhap.DataBindings.Add("EditValue", _apt, "SLDaNhap");
            dTuNgayKH.DataBindings.Add("EditValue", _apt, "TuNgayKH");
            tTungayKH.DataBindings.Add("EditValue", _apt, "TuNgayKH");
            dDenNgayKH.DataBindings.Add("EditValue", _apt, "DenNgayKH");
            tDenNgayKH.DataBindings.Add("EditValue", _apt, "DenNgayKH");
            sSoGioKH.DataBindings.Add("EditValue", _apt, "SoGioKH");
            sSoPhutKH.DataBindings.Add("EditValue", _apt, "SoPhutKH");
            sTrangThai.DataBindings.Add("EditValue", _apt, "TrangThai");
            dTuNgay.DataBindings.Add("EditValue", _apt, "TuNgay");
            tTuNgay.DataBindings.Add("EditValue", _apt, "TuNgay");
            dDenNgay.DataBindings.Add("EditValue", _apt, "DenNgay");
            tDenNgay.DataBindings.Add("EditValue", _apt, "DenNgay");
            sSoGio.DataBindings.Add("EditValue", _apt, "SoGio");
            sSoPhut.DataBindings.Add("EditValue", _apt, "SoPhut");

            gMayIn.DataBindings.Add("EditValue", _apt, "MaMIn");
            mDes.DataBindings.Add("Text", _apt, "Description1");
            mGhiChu.DataBindings.Add("Text", _apt, "GhiChu");

            //disalbe
            if (_apt.TrangThai > 0)
            {
                dTuNgayKH.Properties.ReadOnly = true;
                tTungayKH.Properties.ReadOnly = true;
                dDenNgayKH.Properties.ReadOnly = true;
                tDenNgayKH.Properties.ReadOnly = true;
                sSoGioKH.Properties.ReadOnly = true;
                sSoPhutKH.Properties.ReadOnly = true;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (_apt.TuNgayKH < DateTime.Now) return;
            // controller.ResourceId = _apt.ResourceId;
            // controller.ApplyChanges();
            if (MessageBox.Show("Bạn có muốn lưu không?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if ( _apt.Save())
                {
                    MessageBox.Show("Đã lưu thành công!");
                    this.DialogResult = DialogResult.OK;
                }
            }
            else
                this.DialogResult = DialogResult.Cancel;
        }
    }
}
