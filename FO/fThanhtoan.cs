using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CDTDatabase;
using CDTLib;
using DataFactory;
using DevExpress.XtraReports.UserDesigner;
using DevExpress.XtraReports.UI;
namespace FO
{
    public partial class fThanhtoan : DevExpress.XtraEditors.XtraForm
    {
        public DataRow mt;
        public DataTable dt;
        public Database _dbData;
        private DataTable dmnt;
        private DataTable dmpttt;
        bool isnew;
        public double sotienOld = 0;
        object tt62id;
        public fThanhtoan(Database db, DataRow _mt, DataTable _dt)
        {
            InitializeComponent();
            _dbData = db;
            mt = _mt;
            dt = _dt;
            getdata();
            isnew = true;
        }

        private void getdata()
        {
            string sql = "select MaNT, TenNT from dmnt";
            dmnt=_dbData.GetDataTable(sql);

            gNT.Properties.DataSource = dmnt;
            gNT.Properties.DisplayMember = "MaNT";
            gNT.Properties.ValueMember = "MaNT";
            sql = "select MaPTTT, TenPTTT from dmPTTT";
            dmpttt = _dbData.GetDataTable(sql);
            gPTTT.Properties.DataSource = dmpttt;
            gPTTT.Properties.DisplayMember = "MaPTTT";
            gPTTT.Properties.ValueMember = "MaPTTT";
            gPhong.Properties.DataSource = dt;
            gPhong.Properties.DisplayMember = "MaPhong";
            gPhong.Properties.ValueMember = "DT62ID";
        }
        string fileReport = "InPThuTT";
        private DataTable getdata4Print()
        {
            string sql = "GetData4PrintThanhToan";
            DataTable tb;
            try
            {

                tb = _dbData.GetDataSetByStore(sql, new string[] { "@TT62Id" }, new object[] {tt62id });

                return tb;
            }
            catch
            {
                return null;
            }
        }
        private void EditReport(DataTable tb)
        {
            if (tb == null) return;
            DataTable dataPrint = tb;
            DevExpress.XtraReports.UI.XtraReport rptTmp = null;
            string path;
            if (Config.GetValue("DuongDanBaoCao") != null)
                path = Config.GetValue("DuongDanBaoCao").ToString() + "\\" + Config.GetValue("Package").ToString() + "\\" + fileReport + ".repx";
            else
                path = Application.StartupPath + "\\Reports\\" + Config.GetValue("Package").ToString() + "\\" + fileReport + ".repx";
            string pathTmp;
            if (Config.GetValue("DuongDanBaoCao") != null)
                pathTmp = Config.GetValue("DuongDanBaoCao").ToString() + "\\" + Config.GetValue("Package").ToString() + "\\" + fileReport + ".repx";
            else
                pathTmp = Application.StartupPath + "\\" + Config.GetValue("Package").ToString() + "\\Reports\\template.repx";
            if (System.IO.File.Exists(path))
                rptTmp = DevExpress.XtraReports.UI.XtraReport.FromFile(path, true);
            else if (System.IO.File.Exists(pathTmp))
                rptTmp = DevExpress.XtraReports.UI.XtraReport.FromFile(pathTmp, true);
            else
                rptTmp = new DevExpress.XtraReports.UI.XtraReport();

            if (rptTmp != null)
            {
                rptTmp.DataSource = dataPrint;
                XRDesignFormEx designForm = new XRDesignFormEx();
                designForm.OpenReport(rptTmp);
                if (System.IO.File.Exists(path))
                    designForm.FileName = path;
                designForm.KeyPreview = true;
               // designForm.KeyDown += new KeyEventHandler(designForm_KeyDown);
                designForm.Show();
            }
        }
        private void PrintReport(DataTable tb)
        {
            if (tb == null) return;
            DataTable dataPrint = tb;
            DevExpress.XtraReports.UI.XtraReport rptTmp = null;
            string path;
            if (Config.GetValue("DuongDanBaoCao") != null)
                path = Config.GetValue("DuongDanBaoCao").ToString() + "\\" + Config.GetValue("Package").ToString() + "\\" + fileReport + ".repx";
            else
                path = Application.StartupPath + "\\Reports\\" + Config.GetValue("Package").ToString() + "\\" + fileReport + ".repx";
            string pathTmp;
            if (Config.GetValue("DuongDanBaoCao") != null)
                pathTmp = Config.GetValue("DuongDanBaoCao").ToString() + "\\" + Config.GetValue("Package").ToString() + "\\" + fileReport + ".repx";
            else
                pathTmp = Application.StartupPath + "\\" + Config.GetValue("Package").ToString() + "\\Reports\\template.repx";
            if (System.IO.File.Exists(path))
                rptTmp = DevExpress.XtraReports.UI.XtraReport.FromFile(path, true);
            else if (System.IO.File.Exists(pathTmp))
                rptTmp = DevExpress.XtraReports.UI.XtraReport.FromFile(pathTmp, true);
            else
                rptTmp = new DevExpress.XtraReports.UI.XtraReport();
            SetVariables(rptTmp);
            rptTmp.ScriptReferences = new string[] { Application.StartupPath + "\\CDTLib.dll" };
            rptTmp.DataSource = dataPrint;
            rptTmp.ShowPreview();
        }
        private void SetVariables(DevExpress.XtraReports.UI.XtraReport rptTmp)
        {
            foreach (DictionaryEntry de in Config.Variables)
            {
                string key = de.Key.ToString();
                if (key.Contains("@"))
                    key = key.Remove(0, 1);
                XRControl xrc = rptTmp.FindControl(key, true);
                if (xrc != null)
                {
                    string value = de.Value.ToString();
                    if (value.Contains("/"))
                        xrc.Text = DateTime.Parse(value).ToShortDateString();
                    else
                        xrc.Text = value;
                    xrc = null;
                }
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            string sql;
            _dbData.BeginMultiTrans();
            try
            {
                if (isnew)
                {
                    sql = "insert into TT62(MT62ID,MaPTTT,SoTien,MaNT,ws";
                    if (gPhong.EditValue.ToString() != string.Empty) sql += ",DT62ID";//1320005615939
                    sql += ") values('" + mt["MT62ID"].ToString() +"','"+ gPTTT.EditValue.ToString() + "'," + tSotien.EditValue.ToString() + ",'" + gNT.EditValue.ToString() + "','" + Config.GetValue("sysUserID") + "'";
                    if (gPhong.EditValue.ToString() != string.Empty) sql += ",'" + gPhong.EditValue.ToString() + "'";//1320005615939
                    sql += ")";
                    isnew = false;
                    sotienOld = double.Parse(tSotien.EditValue.ToString());
                    _dbData.UpdateByNonQuery(sql);
                    sql = "select @@Identity";
                    tt62id = _dbData.GetValue(sql);
                }
                else
                {
                    if (tt62id == null) return;
                    sql = "update TT62 set sotien=" + tSotien.EditValue.ToString() + ", mapttt='" + gPTTT.EditValue.ToString() + "' where TT62ID=" + tt62id.ToString() ;
                    _dbData.UpdateByNonQuery(sql);
                }
                //if (sql != null)
                //{
                    
                    sql = "update mt62 set Datcoc=b.sotien from mt62 a inner join (select mt62id,sum(sotien) as sotien from tt62 where mt62id='" + mt["MT62ID"].ToString() + "' group by mt62id) b on a.mt62id=b.mt62id";
                    _dbData.UpdateByNonQuery(sql);
                    sql = "update mt62 set conlai=TongTien-Datcoc where mt62id='" + mt["MT62ID"].ToString() + "'";
                    _dbData.UpdateByNonQuery(sql);
                    _dbData.EndMultiTrans();
                    MessageBox.Show("Ok");
                //}
            }
            catch(Exception ex)
            {
                _dbData.RollbackMultiTrans();
                MessageBox.Show("Lưu dữ liệu bị lỗi!");
            }
        }

        private void btPrint_Click(object sender, EventArgs e)
        {
            //btSave_Click(sender, e);
            PrintReport(getdata4Print());
        }

        private void btEditform_Click(object sender, EventArgs e)
        {
            //btSave_Click(sender, e);
            EditReport(getdata4Print());
        }

        private void fThanhtoan_Load(object sender, EventArgs e)
        {
            this.KeyUp += new KeyEventHandler(fThanhtoan_KeyUp);
        }

        void fThanhtoan_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F12:
                    btSave_Click(sender, new EventArgs());
                    break;
                case Keys.F7:
                    btPrint_Click(sender, new EventArgs());
                    break;
                case Keys.Escape:
                    this.Dispose();
                    break;
            }
        }
 
    }
}