using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.UI;
namespace QLSX
{
    public partial class fAppoint : AppointmentForm
    {
        Appointment _apt;
        public fAppoint(SchedulerControl schedu, Appointment apt) :  base(schedu, apt)
        {
            InitializeComponent();
            _apt = apt;
        }

        private void btnRecurrence_Click(object sender, EventArgs e)
        {

        }

        private void tbLocation_EditValueChanged_1(object sender, EventArgs e)
        {

        }

        private void fAppoint_Load(object sender, EventArgs e)
        {
            LSXappoint lsxappoint = this._apt as LSXappoint;
            if (lsxappoint == null) return;
            tTenKH.Text = lsxappoint.TenKH;
            dNgayGiao.EditValue = lsxappoint.NgayGiao;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            LSXappoint lsxappoint = this._apt as LSXappoint;
            if (lsxappoint == null) return;
            lsxappoint.UpdateDr();
        }
    }
}
