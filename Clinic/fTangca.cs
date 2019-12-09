using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraScheduler;
using System.ComponentModel;
using DevExpress.XtraScheduler.UI;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon;
using DevExpress.Utils.Menu;
using CDTLib;
using CDTDatabase;
namespace QLSX
{
    public partial class fTangca : XtraForm
    {
        public TimeInterval Inteval;
        public DataTable ctTangca;
        Database db = Database.NewDataDatabase();
        public DataTable dmMIn { get; set; }
        Resource _Resource;
        public fTangca(TimeInterval _Inteval, Resource _Res)
        {
            InitializeComponent();
            Inteval = _Inteval;
            _Resource = _Res;
             bWork = DateTime.Parse(Inteval.Start.ToShortDateString() + " " + Config.GetValue("TimeStart").ToString() + ":00");
             bLunch = DateTime.Parse(Inteval.Start.ToShortDateString() + " " + Config.GetValue("TimeStartLunch").ToString() + ":00");
            eLunch = DateTime.Parse(Inteval.Start.ToShortDateString() + " " + Config.GetValue("TimeEndLunch").ToString() + ":00");
            eWork = DateTime.Parse(Inteval.Start.ToShortDateString() + " " + Config.GetValue("TimeEnd").ToString() + ":00");

        }
        DateTime bWork;
        DateTime bLunch;
        DateTime eLunch;
        DateTime eWork;


        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckLunch.Checked)
            {
                ckNight.Checked = false;
                sGio.Properties.MaxValue =(decimal) (eLunch - bLunch).TotalHours;
                sGio.Properties.MinValue = (decimal)(0.5);
            }

        }

        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckNight.Checked)
            {
                ckLunch.Checked = false;
                sGio.Properties.MaxValue = (decimal)(bWork.AddDays(1) - eWork).TotalHours;
                sGio.Properties.MinValue = (decimal)(0.5);
            }

        }
        BindingSource bdind;
        DataRow Dr;
        private void fTangca_Load(object sender, EventArgs e)
        {
            bdind = new BindingSource();
            bdind.DataSource = ctTangca;
            gMaMin.Properties.DataSource = dmMIn;
            dNgay.DataBindings.Add("EditValue", bdind, "Ngay");
            sGio.DataBindings.Add("EditValue", bdind, "SoGio");
            ckLunch.DataBindings.Add("EditValue", bdind, "isLunch",true,DataSourceUpdateMode.OnPropertyChanged);
            ckNight.DataBindings.Add("EditValue", bdind, "isNight",true, DataSourceUpdateMode.OnPropertyChanged);
            gMaMin.DataBindings.Add("EditValue", bdind, "MaMIn", true, DataSourceUpdateMode.OnPropertyChanged);
            if (Inteval.Start < eLunch) ckLunch.Checked = true;
            if (Inteval.Start >eWork) ckNight.Checked = true;

            if (ctTangca.Rows.Count == 0)
            {
                Dr = ctTangca.NewRow();
                Dr["CTTangcaID"] = Guid.NewGuid();
                Dr["Ngay"] = DateTime.Parse(Inteval.Start.ToShortDateString());
                Dr["SoGio"] = Inteval.Duration.TotalHours;
                Dr["isLunch"] = ckLunch.EditValue;
                Dr["isNight"] = ckNight.EditValue;
                if (_Resource.GetType() != typeof(DevExpress.XtraScheduler.Native.EmptyResource))
                    Dr["MaMIn"] = _Resource.Id;
                ctTangca.Rows.Add(Dr);
                Dr.EndEdit();
            }
            else
            {
                ckLunch.Properties.ReadOnly = true;
                ckNight.Properties.ReadOnly = true;
                gMaMin.Properties.ReadOnly = true;
                dNgay.Properties.ReadOnly = true;
                Dr = ctTangca.Rows[0];
            }
           
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Dr.EndEdit();
            Dr.AcceptChanges();
            ctTangca.AcceptChanges();
            db.BeginMultiTrans();
            try {
                string sql = "select count(*) from ctTangca where ngay='" + Dr["ngay"].ToString() + "' and isLunch=" + (Dr["isLunch"].ToString() == "True" ? "1" : "0") + " and isNight=" + (Dr["isNight"].ToString() == "True" ? "1" : "0") + " and " + (Dr["MaMIn"] == DBNull.Value ? " MaMIn is null" : "MaMin='" + Dr["MaMin"].ToString() + "'") ;
                object c = db.GetValue(sql);
                if (c == null)
                { db.EndMultiTrans(); return; }
                if (int.Parse(c.ToString()) > 0)
                {
                    sql = "update ctTangca set SoGio=@SoGio where ctTangCaID=@ctTangcaID";
                    db.UpdateDatabyPara(sql, new string[] { "@ctTangcaID", "@SoGio" }, new object[] { Dr["ctTangcaID"], Dr["SoGio"] });
                    db.EndMultiTrans();

                }
                else
                {
                    sql = "insert into ctTangca (ctTangcaID,Ngay,Sogio, isLunch, isNight, MaMIn) values (@ctTangcaID, @Ngay,@SoGio, @isLunch, @isNight,@MaMIn)";

                    db.UpdateDatabyPara(sql, new string[] { "@ctTangcaID", "@Ngay", "@SoGio", "@isLunch", "@isNight", "@MaMIn" }, new object[] { Dr["ctTangcaID"], Dr["Ngay"], Dr["SoGio"], Dr["isLunch"], Dr["isNight"], Dr["MaMIn"] });
                    if (db.HasErrors)
                    {
                        db.RollbackMultiTrans();
                        return;
                    }
                    else
                    {
                        db.EndMultiTrans();
                    }
                }
            } catch(Exception ex) { db.RollbackMultiTrans(); }
            this.DialogResult = DialogResult.OK;
        }

        private void dNgay_EditValueChanged(object sender, EventArgs e)
        {
            if(DateTime.Parse(dNgay.EditValue.ToString()).DayOfWeek ==DayOfWeek.Saturday || DateTime.Parse(dNgay.EditValue.ToString()).DayOfWeek == DayOfWeek.Sunday)
            {
                ckLunch.Checked = false;
                ckLunch.Enabled = false;
            }
            else
            {
                ckLunch.Enabled = true;
            }
        }

        private void chAll_CheckedChanged(object sender, EventArgs e)
        {
            if (ckAll.Checked)

                Dr["MaMin"] = null;
            else if (_Resource.GetType() != typeof(DevExpress.XtraScheduler.Native.EmptyResource))
                Dr["MaMIn"] = _Resource.Id;
            Dr.EndEdit();

        }
    }
}
