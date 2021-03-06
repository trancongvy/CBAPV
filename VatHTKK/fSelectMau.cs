using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CDTLib;
using CDTDatabase;
using System.IO;
using CDTControl;
namespace VatHTKK
{
    public partial class fSelectMau : DevExpress.XtraEditors.XtraForm
    {
        public fSelectMau()
        {
            InitializeComponent();
        }
        Database _db = Database.NewDataDatabase();
       
        int TypeHD;
        DataTable tbKieuHD;
        DataRow _dr;
        string DuongDanInvoice = Config.GetValue("DuongDanInvoice").ToString() + "\\" + Config.GetValue("Package").ToString();
        private SysConfig _sysConfig = new SysConfig();
        DataTable tb = new DataTable();
        List<PictureBox> lP = new List<PictureBox>();
        private void fSelectMau_Load(object sender, EventArgs e)
        {
            Image logo=null;
            _sysConfig.GetUserConfig();
            if (_sysConfig.DsStartConfig.Tables.Count < 1) return;
            tb = _sysConfig.DsStartConfig.Tables[0];
            tbKieuHD = _db.GetDataTable("Select * from dmKieuHoaDon order by sType");
            if (tbKieuHD == null) return;
            TypeHD = int.Parse(Config.GetValue("TypeHDDT").ToString());
            DataRow[] ldr = tbKieuHD.Select("sType=" + TypeHD.ToString());
            if (ldr.Length == 0) return;
            drMauHD = ldr[0];
            //Load Logo
            if (File.Exists(DuongDanInvoice + "\\Logo.png"))
            {
                string path = DuongDanInvoice + "\\Logo.png";
                FileInfo fileInfo = new FileInfo(path);
                byte[] data = new byte[fileInfo.Length];
                using (FileStream fs = fileInfo.OpenRead())
                {
                    fs.Read(data, 0, data.Length);
                }
                //fileInfo.Delete();
                MemoryStream ms = new MemoryStream(data);
                logo = Image.FromStream(ms);
            }
            else if (File.Exists(DuongDanInvoice + "\\Logo.jpg"))
            {
                string path = DuongDanInvoice + "\\Logo.jpg";
                FileInfo fileInfo = new FileInfo(path);
                byte[] data = new byte[fileInfo.Length];
                using (FileStream fs = fileInfo.OpenRead())
                {
                    fs.Read(data, 0, data.Length);
                }
               // fileInfo.Delete();
                MemoryStream ms = new MemoryStream(data);
                logo = Image.FromStream(ms);
            }
                if (logo != null) pLogo.Image = logo;

            tEmailSub.Text = Config.GetValue("EmailSub").ToString();
            tMailBody.Text = Config.GetValue("EmailBody").ToString();
            //Khởi tạo các picbox
            foreach (DataRow dr in tbKieuHD.Rows)
            {
                PictureBox p = new PictureBox();
                p.Tag = int.Parse(dr["stype"].ToString());
                p.SizeMode = PictureBoxSizeMode.StretchImage;
                p.Height = 170;
                p.Width = 100;
                p.Top = 1;
                p.Image = GetImage(dr["Anh"] as byte[]);
                panelControl1.Controls.Add(p);
                lP.Add(p);
            }
            setPic();
        }
        private void setPic()
        {
            for (int i = 0; i < lP.Count; i++)
            {
                if (int.Parse(lP[i].Tag.ToString()) != TypeHD)
                {
                    lP[i].Height = 120;
                    lP[i].Width = 71;
                    lP[i].Top = 50;
                    if(TypeHD >int.Parse(lP[i].Tag.ToString()))
                    lP[i].Left = 310 - (TypeHD - int.Parse(lP[i].Tag.ToString())) * 80;
                    else
                    lP[i].Left = 440 + (int.Parse(lP[i].Tag.ToString())-TypeHD -1) * 80;
                }
                else
                {
                    lP[i].Height = 170;
                    lP[i].Width = 120;
                    lP[i].Top = 1;
                    lP[i].Left = 310;//720/2-50
                }
            }
        }
        private DataRow drMauHD
        {
            get { return _dr; }
            set
            {
                _dr = value;
                sType.EditValue = _dr["sType"].ToString();
                tDes.Text = _dr["Des"].ToString();
                tSodong.Text = _dr["SoDong"].ToString();
                tKieuGiay.Text = _dr["KieuGiay"].ToString();
                tLandcap.EditValue = bool.Parse(_dr["Landcap"].ToString());
                tShow.EditValue = bool.Parse(_dr["ShowSignature"].ToString());
                
            }
        }
        private Image GetImage(byte[] b)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(b);
            if (ms == null)
                return null;
            Image im = Image.FromStream(ms);
            return (im);
        }
        private void btSavemau_Click(object sender, EventArgs e)
        {
            Config.NewKeyValue("EmailSub", tEmailSub.Text);
            Config.NewKeyValue("EmailBody", tMailBody.Text);
            Config.NewKeyValue("TypeHDDT", int.Parse(_dr["sType"].ToString()));
            //System.Drawing.Imaging.ImageFormat f=pLogo.Image
            byte[] byteArray = null; // Put the bytes of the image here....
            System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Png;
            string path = DuongDanInvoice + "\\logo.png";
            using (MemoryStream ms = new MemoryStream())
            {
                Bitmap bmp = new Bitmap(pLogo.Image);
                bmp.Save(ms, format);
                byteArray= ms.ToArray();
               // bmp.Save(DuongDanInvoice + "\\Logo.png",System.Drawing.Imaging.ImageFormat.Png);
                
            }
            //Lưu config
            File.WriteAllBytes(path, byteArray);
            foreach (DataRow dr in tb.Rows)
            {
                try
                {
                    if (Config.Variables.Contains(dr["_Key"].ToString()))
                    {
                        dr["_Value"] = Config.GetValue(dr["_Key"].ToString());
                    }
                }
                catch
                {
                }
            }
            //Luu file mẫu hóa đơn
            path = DuongDanInvoice + "\\InvoiceType\\HDDT" + int.Parse(_dr["sType"].ToString()) + ".repx";
            if(File.Exists(path))
            {
                File.Copy(path, DuongDanInvoice + "\\HoaDonMau.repx", true);
            }
            _sysConfig.UpdateStartConfig();
            MessageBox.Show("UpdateComplete!");
        }

        private void tMailBody_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void tbToLeft_Click(object sender, EventArgs e)
        {
            if (sType.EditValue == null) return;
            int tCurrent = int.Parse(sType.EditValue.ToString());
             TypeHD = tCurrent - 1;
             DataRow[] ldr = tbKieuHD.Select("sType=" + TypeHD.ToString());
            if (ldr.Length == 0) return;
            drMauHD = ldr[0];
            setPic();
        }

        private void tbToRight_Click(object sender, EventArgs e)
        {
            if (sType.EditValue == null) return;
            int tCurrent = int.Parse(sType.EditValue.ToString());
            TypeHD = tCurrent + 1;
            DataRow[] ldr = tbKieuHD.Select("sType=" + TypeHD.ToString());
            if (ldr.Length == 0) return;
            drMauHD = ldr[0];
            setPic();
        }
    }
}