using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CDTLib;
using CDTDatabase;
namespace FO
{
    public partial class cPhong : DevExpress.XtraEditors.XtraUserControl
    {
        Database _db = Database.NewDataDatabase();
        private string _ttphong;
        private string _Mt62id;
        public DateTime ngayht = DateTime.Now;
        DataTable tb;
        public event System.EventHandler Check_Room;
        public event System.EventHandler FineGroup;
        public List<string> TenKhach=new List<string>();
        public string Mt62id
        {
            get { return _Mt62id; }
            set
            {
                _Mt62id = value;
            }
        }
        public string MaLoaiPhong
        {
            set { label1.Text = value;
            labelControl1.Left = (this.Width - labelControl1.Width) / 2;
            label1.Left = (this.Width - label1.Width) / 2;
        }
            get { return label1.Text; }
        }
        public string MaPhong
        {
            set 
            {
                labelControl1.Text = value;
                labelControl1.Left = (this.Width - labelControl1.Width) / 2;
                label1.Left = (this.Width - label1.Width) / 2;
                string sql ="select getdate()";
                object o=_db.GetValue(sql);
                if (o != null)
                {

 
                    ngayht = DateTime.Parse(o.ToString());
                    sql = "select a.Ongba,a.notice, a.color, b.*,c.* from mt62 a inner join dt62 b on a.mt62id=b.mt62id left join ct62 c on b.dt62id=c.dt62id where b.maphong='" + MaPhong + "' and '" + ngayht.ToLongDateString() + "' > b.ngayden and b.ischeckout=0 and b.ischeckin=1 and b.Mt62id not in (select mt62id from mt62 where iscancel=1)";
                    tb = _db.GetDataTable(sql);
                    if (tb.Rows.Count > 0)
                    {
                        Mt62id = tb.Rows[0]["MT62ID"].ToString();
                        
                        simpleButton1.ToolTip = tb.Rows[0]["Ongba"].ToString();

                        if (tb.Rows[0]["notice"] != DBNull.Value)
                        {
                            simpleButton1.ToolTip += " --" + tb.Rows[0]["notice"].ToString();
                        }
                        foreach(DataRow dr in tb.Rows)
                        {
                            if (dr["TenKhach"].ToString() != "Chưa có tên")
                            {
                                simpleButton1.ToolTip += "/n " + dr["TenKhach"].ToString();
                                TenKhach.Add(dr["TenKhach"].ToString());
                            }
                        }
                        try
                        {
                            if (tb.Rows[0]["Color"] != DBNull.Value)
                            {
                                ColorConverter cl = new ColorConverter();
                                RoomColor = (Color)cl.ConvertFromString(tb.Rows[0]["Color"].ToString());
                            }
                        }
                        catch
                        {
                        }
                    }
                    sql = "select a.Ongba,a.notice, a.color, b.*,c.* from mt62 a inner join dt62 b on a.mt62id=b.mt62id  inner join ct62 c on b.dt62id=c.dt62id  where b.maphong='" + MaPhong + "' and ('" + ngayht.ToShortDateString() + "' = convert(datetime, convert(nvarchar(11), b.ngayden)) or '" + ngayht.ToShortDateString() + "'between b.ngayden and b.ngaydi)  and b.ischeckout=0 and b.ischeckin=0 and b.Mt62id not in (select mt62id from mt62 where iscancel=1) order by b.ngayden";
                    tb = _db.GetDataTable(sql);
                    if (tb.Rows.Count > 0 && TTPhong == "READY")
                    {
                        TTPhong = "RESV";
                        Mt62id = tb.Rows[0]["MT62ID"].ToString();
                        simpleButton1.ToolTip = tb.Rows[0]["Ongba"].ToString();
                        if (tb.Rows[0]["notice"] != DBNull.Value)
                            simpleButton1.ToolTip += " --" + tb.Rows[0]["notice"].ToString();
                        foreach (DataRow dr in tb.Rows)
                        {
                            if (dr["TenKhach"].ToString() != "Chưa có tên")
                            {
                                simpleButton1.ToolTip += "-- " + dr["TenKhach"].ToString();
                                TenKhach.Add(dr["TenKhach"].ToString());
                            }
                        }
                        try
                        {
                            if (tb.Rows[0]["Color"] != DBNull.Value)
                            {
                                ColorConverter cl = new ColorConverter();
                                RoomColor = (Color)cl.ConvertFromString(tb.Rows[0]["Color"].ToString());
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            
            }
            get { return labelControl1.Text; }
        }
        public string TTPhong
        {
            set
            {
                _ttphong = value;
                switch (_ttphong)
                {
                    case "IN":
                        simpleButton1.ImageIndex = 0;
                        barButtonItem1.Enabled = false;
                        barButtonItem2.Enabled = true;
                        barButtonItem3.Enabled = false;
                        barButtonItem4.Enabled = false;
                        barButtonItem5.Enabled = true;
                        break;
                    case "READY":
                        simpleButton1.ImageIndex = 1;
                        barButtonItem1.Enabled = true;
                        barButtonItem2.Enabled = false;
                        barButtonItem3.Enabled = false;
                        barButtonItem4.Enabled = true;
                        barButtonItem5.Enabled = false;
                        if(this.Mt62id==null)
                        RoomColor = Color.Transparent;
                        break;
                    case "RESV":
                        simpleButton1.ImageIndex = 4;
                        barButtonItem1.Enabled = true;
                        barButtonItem2.Enabled = false;
                        barButtonItem3.Enabled = false;
                        barButtonItem4.Enabled = true;
                        barButtonItem5.Enabled = true;
                        RoomColor = Color.Transparent;
                        break;
                    case "DURTY":
                        simpleButton1.ImageIndex = 2;
                        barButtonItem1.Enabled = false;
                        barButtonItem2.Enabled = false;
                        barButtonItem3.Enabled = true;
                        barButtonItem4.Enabled = true;
                        barButtonItem5.Enabled = false;
                        RoomColor = Color.Transparent;
                        break;
                    case "CORRUPT":
                        simpleButton1.ImageIndex = 3;
                        barButtonItem1.Enabled = false;
                        barButtonItem2.Enabled = false;
                        barButtonItem3.Enabled = true;
                        barButtonItem4.Enabled = false;
                        barButtonItem5.Enabled = false;
                        RoomColor = Color.Transparent;
                        break;
                }
            }
            get { return _ttphong; }
        }
        public cPhong()
        {
            InitializeComponent();
            this.Resize += new EventHandler(cPhong_Resize);
            simpleButton1.MouseUp += new MouseEventHandler(simpleButton1_MouseUp);
        }
        Color _roomColor;
        public  Color RoomColor
        {
            set
            {
                _roomColor = value;
                panel1.BackColor = _roomColor;
                ColorConverter cl=new ColorConverter();
                if (Mt62id != null)
                {
                    string sql = "update mt62 set color='" + cl.ConvertToString(_roomColor).ToString() + "'  where mt62id='" + Mt62id + "'";
                    _db.UpdateByNonQuery(sql);
                }
            }
            get
            {
                return _roomColor;
            }
            
        }
        void simpleButton1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                popupMenu1.ShowPopup(new Point(e.X + this.Left + this.FindForm().Left, e.Y + this.Top + this.FindForm().Top+120));
            }
        }

        void cPhong_Resize(object sender, EventArgs e)
        {
            labelControl1.Left = (this.Width - labelControl1.Width) / 2;
            label1.Left = (this.Width - label1.Width) / 2;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //check in
            fMT62 f = new fMT62();
            f.MdiParent = this.FindForm().MdiParent;
            f.Show();
            DataRow dr = f._data.dt.NewRow();
            f._data.dt.Rows.Add(dr);
            dr["MaPhong"] = MaPhong;
            f._data.tbPhongtrong_tmp.Rows.Remove(f._data.tbPhongtrong_tmp.Rows.Find(dr["MaPhong"]));
            string sql ="select getdate()";
                object o=_db.GetValue(sql);
                if (o != null)
                {
                    ngayht = DateTime.Parse(o.ToString());
                }
            //f._data.mt["NgayDen"] = ngayht.AddMinutes(5);
            f.Disposed += new EventHandler(f_Disposed);
           
        }

        void f_Disposed(object sender, EventArgs e)
        {
            if (Check_Room != null) Check_Room(sender, e);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //check out
            if (Mt62id == null) return;
           
            fCheckOut f = new fCheckOut(Mt62id);
            f.MdiParent = this.FindForm().MdiParent;
            f.Show();
             f.Disposed += new EventHandler(f_Disposed);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //set ready
            string sql = "update dmphong set MaTT='READY' where maphong='" + MaPhong + "'";
            _db.UpdateByNonQuery(sql);
            if (!_db.HasErrors) TTPhong = "READY";
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //set Corrupt
            string sql = "update dmphong set MaTT='CORRUPT' where maphong='" + MaPhong + "'";
            _db.UpdateByNonQuery(sql);
            if (!_db.HasErrors) TTPhong = "CORRUPT";
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Mt62id == null) return;
            if (TTPhong == "IN")
            {
                fMT62 f = new fMT62(Mt62id);
                f.isGiaHan = true;
                f.MdiParent = this.FindForm().MdiParent;
                f.Show();

                f.Disposed += new EventHandler(f_Disposed);
            }
            else if (TTPhong == "RESV")
            {
                fMT62 f = new fMT62(Mt62id);                
                f.MdiParent = this.FindForm().MdiParent;
                f.Show();
                f.Disposed += new EventHandler(f_Disposed);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            // FineGroup(this, e);
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ColorDialog d = new ColorDialog();
            d.ShowDialog();
            if (d.Color != null)
            {
                this.RoomColor = d.Color;
                FineGroup(this, e);
            }

        }
    }
}
