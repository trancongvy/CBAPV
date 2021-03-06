using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using FormFactory;
using DataFactory;
using DevExpress.XtraGrid;
using DevControls;
using DevExpress.XtraLayout;
using DevExpress.XtraGrid.Repository;
using CDTDatabase;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using CDTLib;
using DevExpress.XtraReports.UserDesigner;
namespace FO
{
    public partial class fCheckOut : DevExpress.XtraEditors.XtraForm
    {
        private BindingSource bs=new BindingSource();
        private DataCheckOut _data;
        private DataCheckOut1Room crRoom;
        private string _MT62ID;
        private Database _dbData = Database.NewDataDatabase();
        private string fileReport="";
        object ngayht;
        
        public fCheckOut(string MT62ID)
        {
            InitializeComponent();
            _MT62ID = MT62ID;
            string sql = "select getdate()";
            ngayht = _dbData.GetValue(sql);
            _data = new DataCheckOut(MT62ID);
            bs.DataSource = _data.ds;
            bs.DataMember = _data.ds.Tables[1].TableName;
            bs.CurrentChanged += new EventHandler(bs_CurrentChanged);
            gridControl1.DataSource = bs;
            bs.MoveFirst();
            bs_CurrentChanged(bs, new EventArgs());

            //Biding
            GetBinding();
            GetData4Lookup();
            repositoryItemGridLookUpEdit1.DataSource = _data.dmVT;
            repositoryItemGridLookUpEdit1.EditValueChanged += new EventHandler(repositoryItemGridLookUpEdit1_EditValueChanged);
            repositoryItemGridLookUpEdit2.DataSource = _data.dmDV;
            gridView2.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridView2_FocusedRowChanged);
            gridLookUpEdit1.EditValueChanged += new EventHandler(gridLookUpEdit1_EditValueChanged);
            gridView1.MouseUp += new MouseEventHandler(gridView1_MouseUp);
        }

        

        void gridLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (gridLookUpEdit1.EditValue == null) return;
            fileReport = gridLookUpEdit1.EditValue.ToString();
        }

        private void GetBinding()
        {
            grPDP.DataBindings.Add(new Binding("EditValue", _data.ds.Tables[0], "MT61ID", true, DataSourceUpdateMode.OnValidation));
            grKhach.DataBindings.Add(new Binding("EditValue", _data.ds.Tables[0], "MaKhach", true, DataSourceUpdateMode.OnValidation));
            grPTTT.DataBindings.Add(new Binding("EditValue", _data.ds.Tables[0], "MaPTTT", true, DataSourceUpdateMode.OnValidation));
            grHangDL.DataBindings.Add(new Binding("EditValue", _data.ds.Tables[0], "MaHang", true, DataSourceUpdateMode.OnValidation));
            grLoaiGia.DataBindings.Add(new Binding("EditValue", _data.ds.Tables[0], "MaLoaiGia", true, DataSourceUpdateMode.OnValidation));
            cNgayDen.DataBindings.Add(new Binding("EditValue", _data.ds.Tables[0], "NgayDen", true, DataSourceUpdateMode.OnValidation));
            cNgayDi.DataBindings.Add(new Binding("EditValue", _data.ds.Tables[0], "NgayDi", true, DataSourceUpdateMode.OnValidation));
            textOngBa.DataBindings.Add(new Binding("Text", _data.ds.Tables[0], "OngBa", true, DataSourceUpdateMode.OnValidation));
            spinEdit1.DataBindings.Add(new Binding("Value", _data.ds.Tables[0], "SoNgay", true, DataSourceUpdateMode.OnValidation));
            spinEdit2.DataBindings.Add(new Binding("Value", _data.ds.Tables[0], "SoNguoi", true, DataSourceUpdateMode.OnValidation));
            grNT.DataBindings.Add(new Binding("EditValue", _data.ds.Tables[0], "MaNT", true, DataSourceUpdateMode.OnValidation));
            calTyGia.DataBindings.Add(new Binding("EditValue", _data.ds.Tables[0], "TyGia", true, DataSourceUpdateMode.OnValidation));
            grMaThue.DataBindings.Add(new Binding("EditValue", _data.ds.Tables[0], "MaThue", true, DataSourceUpdateMode.OnValidation));
            calcThuesuat.DataBindings.Add(new Binding("EditValue", _data.ds.Tables[0], "Thuesuat", true, DataSourceUpdateMode.OnValidation));
            calcEdit1.DataBindings.Add(new Binding("EditValue", _data.ds.Tables[0], "TTienH", true, DataSourceUpdateMode.OnValidation));
            calcEdit2.DataBindings.Add(new Binding("EditValue", _data.ds.Tables[0], "TThue", true, DataSourceUpdateMode.OnValidation));
            calcEdit3.DataBindings.Add(new Binding("EditValue", _data.ds.Tables[0], "TongTien", true, DataSourceUpdateMode.OnValidation));
            calcEdit4.DataBindings.Add(new Binding("EditValue", _data.ds.Tables[0], "Datcoc", true, DataSourceUpdateMode.OnValidation));
            calcEdit5.DataBindings.Add(new Binding("EditValue", _data.ds.Tables[0], "Conlai", true, DataSourceUpdateMode.OnValidation));
            calcEdit6.DataBindings.Add(new Binding("EditValue", _data.ds.Tables[0], "TTienMNB", true, DataSourceUpdateMode.OnValidation));
            calcEdit7.DataBindings.Add(new Binding("EditValue", _data.ds.Tables[0], "TThueMNB", true, DataSourceUpdateMode.OnValidation));
            calcEdit8.DataBindings.Add(new Binding("EditValue", _data.ds.Tables[0], "TTienDV", true, DataSourceUpdateMode.OnValidation));
            calcEdit9.DataBindings.Add(new Binding("EditValue", _data.ds.Tables[0], "TThueDV", true, DataSourceUpdateMode.OnValidation));
            calcEdit10.DataBindings.Add(new Binding("EditValue", _data.ds.Tables[0], "PtCk", true, DataSourceUpdateMode.OnValidation));
            calcEdit11.DataBindings.Add(new Binding("EditValue", _data.ds.Tables[0], "CK", true, DataSourceUpdateMode.OnValidation));
            memoEdit1.DataBindings.Add(new Binding("Text", _data.ds.Tables[0], "notice", true, DataSourceUpdateMode.OnValidation));

        }

        void gridView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0)
            {
                gridView2.FocusedColumn = gridView2.Columns[0];
            }
        }

        void repositoryItemGridLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            object value = (sender as GridLookUpEdit).EditValue;
            int index = (sender as GridLookUpEdit).Properties.GetIndexByKeyValue(value);
            DataRow drData = _data.dmVT.Rows[index];
            if (crRoom == null) return;
            if (crRoom.MinibarCr != null) crRoom.MinibarCr["DonGia"] = drData["GiaBan"];
        }

        void bs_CurrentChanged(object sender, EventArgs e)
        {
            if (bs.Current==null) return;
            string dt62id = (bs.Current as DataRowView).Row["DT62ID"].ToString();
            crRoom=_data.FindRoom(dt62id);
            if (crRoom == null) return;    
            gridControl2.DataSource = crRoom.minibar;
            gridControl3.DataSource = crRoom.dichvu;
            gridView2.OptionsBehavior.Editable = !crRoom.isCheckOut;
            gridView3.OptionsBehavior.Editable = !crRoom.isCheckOut;
            gridColumn26.OptionsColumn.AllowEdit = !crRoom.isCheckOut;
        }
        public DataTable tGia;
        public DataTable dmnt;
        public DataTable dmThueSuat;
        public DataTable tLoaigia;
        public DataTable tPhong;
        public DataTable tkhach;
        public DataTable tLoaiphong;
        public DataTable tGiayto;
        public DataTable tLoaiPhong1;
        public DataTable tMt61;
        public DataTable t_hang;
        public DataTable tpttt;
        public DataTable tbPhongTrong;
        public DataTable dmReportFile;
        private void GetData4Lookup()
        {
            string sql = "select a.*, b.* from mt61 a inner join dmkhachdl b on a.makhach=b.makhach where MT61ID in (select MT61ID from MT62 where MT62ID='" + _MT62ID + "')";
            tMt61 = _dbData.GetDataTable(sql);
            tMt61.PrimaryKey = new DataColumn[] { tMt61.Columns["MT61ID"] };

            sql = "select * from dmKhachDL";
            tkhach = _dbData.GetDataTable(sql);
            tkhach.PrimaryKey = new DataColumn[] { tkhach.Columns["MaKhach"] };


            sql = "select * from dmPTTT";
            tpttt = _dbData.GetDataTable(sql);
            tpttt.PrimaryKey = new DataColumn[] { tpttt.Columns["MaPTTT"] };


            sql = "select * from dmHangDL";
            t_hang = _dbData.GetDataTable(sql);
            t_hang.PrimaryKey = new DataColumn[] { t_hang.Columns["MaHang"] };

            sql = "select MaGiayto,TenGiayto from dmGiayto";
            tGiayto = _dbData.GetDataTable(sql);
            tGiayto.PrimaryKey = new DataColumn[] { tGiayto.Columns["MaLoaiPhong"] };

            sql = "select MaLoaiPhong,TenLoaiPhong,SoNguoi from dmLoaiphong";
            tLoaiPhong1 = _dbData.GetDataTable(sql);
            tLoaiPhong1.PrimaryKey = new DataColumn[] { tLoaiPhong1.Columns["MaLoaiPhong"] };

            sql = "select MaLoaiPhong, TenLoaiPhong,SoNguoi from dmloaiphong";
            tLoaiphong = _dbData.GetDataTable(sql);
            tLoaiphong.PrimaryKey = new DataColumn[] { tLoaiphong.Columns["MaLoaiPhong"] };


            sql = "select MaLoaiGia, TenLoaiGia,NgayTinh,Muctinh from dmloaigia order by Ngaytinh";
            tLoaigia = _dbData.GetDataTable(sql);
            tLoaigia.PrimaryKey = new DataColumn[] { tLoaigia.Columns["MaLoaigia"] };


            sql = "select MaPhong,TenPhong,MaLoaiPhong,SoGiuong,GhiChu,MaArea,0 as isSelected from dmphong where MaTT<>'CORRUPT'";
            tPhong = _dbData.GetDataTable(sql);
            tPhong.PrimaryKey = new DataColumn[] { tPhong.Columns["MaPhong"] };





            sql = "select MaLoaiPhong,TenLoaiPhong,SoNguoi from dmLoaiphong";
            tLoaiPhong1 = _dbData.GetDataTable(sql);
            tLoaiPhong1.PrimaryKey = new DataColumn[] { tLoaiPhong1.Columns["MaLoaiPhong"] };


            sql = "select MaGiayto,TenGiayto from dmGiayto";
            tGiayto = _dbData.GetDataTable(sql);
            tGiayto.PrimaryKey = new DataColumn[] { tGiayto.Columns["MaLoaiPhong"] };


            sql = "select MaGia,MaLoaiPhong,MaLoaiGia,Gia,SoNgay from dmgia";
            tGia = _dbData.GetDataTable(sql);
            tGia.PrimaryKey = new DataColumn[] { tGia.Columns["MaGia"] };


            sql = "select * from dmNT";
            dmnt = _dbData.GetDataTable(sql);
            dmnt.PrimaryKey = new DataColumn[] { dmnt.Columns["MaNT"] };


            sql = "select * from dmThueSuat";
            dmThueSuat = _dbData.GetDataTable(sql);
            dmThueSuat.PrimaryKey = new DataColumn[] { dmThueSuat.Columns["MaThue"] };
            sql = "select * from ReportFile_out";
            dmReportFile = _dbData.GetDataTable(sql);
            gridLookUpEdit1.Properties.DataSource = dmReportFile;
            gridLookUpEdit1.Properties.DisplayMember = "DienGiai";
            gridLookUpEdit1.Properties.ValueMember = "ReportFile";
            grPDP.Properties.DisplayMember = "SoCT";

            grPDP.Properties.DataSource = tMt61;
            grPDP.Properties.View.BestFitColumns();

            grKhach.Properties.DataSource = tkhach;
            grKhach.Properties.View.BestFitColumns();

            grPTTT.Properties.DataSource = tpttt;
            grPTTT.Properties.View.BestFitColumns();

            grHangDL.Properties.DataSource = t_hang;
            grHangDL.Properties.View.BestFitColumns();



            grLoaiGia.Properties.DataSource = tLoaigia;
            grLoaiGia.Properties.View.BestFitColumns();

            grNT.Properties.DataSource = dmnt;
            grNT.Properties.View.BestFitColumns();


            grMaThue.Properties.DataSource = dmThueSuat;
            grMaThue.Properties.View.BestFitColumns();

        }
        private void btSave_Click(object sender, EventArgs e)
        {
            gridView2.RefreshData();
            _data.Save(false);

        }
#region in ấn
        private void btEditForm_Click(object sender, EventArgs e)
        {
            btSave_Click(btSave, new EventArgs());
            EditReport(getdata4Print());
         
        }
        private void btPrint_Click(object sender, EventArgs e)
        {
            btSave_Click(btSave, new EventArgs());
            PrintReport(getdata4Print());
        }
        private DataTable getdata4Print()
        {
            string sql = "GetData4CheckOut";
            DataTable tb;
            try
            {
                if (fileReport == "FO_TINHTIEN1ROOM")
                {
                    tb= _dbData.GetDataSetByStore(sql, new string[] { "@key", "@Name" }, new object[] { crRoom.dt["DT62ID"], fileReport });
                }
                else if (fileReport == "FO_MINIBAR1ROOM")
                {
                    tb = _dbData.GetDataSetByStore(sql, new string[] { "@key", "@Name" }, new object[] { crRoom.dt["DT62ID"], fileReport });
                }
                else if (fileReport == "FO_DVK1ROOM")
                {
                    tb = _dbData.GetDataSetByStore(sql, new string[] { "@key", "@Name" }, new object[] { crRoom.dt["DT62ID"], fileReport });
                }
                else if (fileReport == "FO_TTNOSHOW1")
                {
                    tb = _dbData.GetDataSetByStore(sql, new string[] { "@key", "@Name" }, new object[] { crRoom.dt["DT62ID"], fileReport });
                }
                else if (fileReport == "FO_TINHTIENE1R")
                {
                    tb = _dbData.GetDataSetByStore(sql, new string[] { "@key", "@Name" }, new object[] { crRoom.dt["DT62ID"], fileReport });
                }
                else if (fileReport == "FO_TINHTIENE1RE")
                {
                    tb = _dbData.GetDataSetByStore(sql, new string[] { "@key", "@Name" }, new object[] { crRoom.dt["DT62ID"], fileReport });
                }
                else 
                {
                    tb=_dbData.GetDataSetByStore(sql, new string[] { "@key", "@Name" }, new object[] { _data.mt["MT62ID"], fileReport });
                }
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
                designForm.KeyDown += new KeyEventHandler(designForm_KeyDown);
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
        void designForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.X)
                (sender as XRDesignFormEx).Close();
        }
#endregion

        private void tbCheckOut_Click(object sender, EventArgs e)
        {
            if (crRoom != null && crRoom.dt["isCheckOut"].ToString()=="False")
            {
                if(DateTime.Parse(DateTime.Parse( crRoom.dt["NgayDi"].ToString()).ToShortDateString())>DateTime.Parse(DateTime.Parse(ngayht.ToString()).ToShortDateString()))
                {
                   //trường hợp check out sớm
                    DialogResult kq=MessageBox.Show("Khách check out sớm! Giữ nguyên số tiền (Yes) hay giảm tiền cho khách(No) hay tính theo thực tế số ngày ở (Cancel)? ", "Thông báo", MessageBoxButtons.YesNoCancel);
                    if (kq == DialogResult.No)
                    {
                        fEarlyCheckout fe = new fEarlyCheckout();
                        fe.ShowDialog();
                        if (fe.calcEdit1.EditValue != null)
                        {
                            //crRoom.dt["GiaEarly"] = double.Parse(fe.calcEdit1.EditValue.ToString());
                          //  TimeSpan tsp=DateTime.Parse(DateTime.Parse( crRoom.dt["NgayDi"].ToString()).ToShortDateString())-DateTime.Parse(DateTime.Parse(ngayht.ToString()).ToShortDateString());
                            crRoom.dt["TienEarly"] = -double.Parse(fe.calcEdit1.EditValue.ToString());// *tsp.TotalDays;
                        }
                    }
                    else if (kq == DialogResult.Cancel)
                    {
                        crRoom.Giusongay_eralycheckout = false;
                    }
                }
                else 
                {
                    crRoom.Giusongay_eralycheckout = false;
                    DateTime GioDi;
                    GioDi = DateTime.Parse(DateTime.Parse(ngayht.ToString()).ToShortDateString() + " " + crRoom.TimeOut.ToString());
                    if (DateTime.Parse(ngayht.ToString()) > GioDi)
                    {
                        if (MessageBox.Show("Khách check out muộn giờ so với quy định! Có cộng thêm tiền phòng?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            fEarlyCheckout fe = new fEarlyCheckout();
                            fe.ShowDialog();
                            if (fe.calcEdit1.EditValue != null)
                            {
                                //crRoom.dt["GiaEarly"] = double.Parse(fe.calcEdit1.EditValue.ToString());
                                //  TimeSpan tsp=DateTime.Parse(DateTime.Parse( crRoom.dt["NgayDi"].ToString()).ToShortDateString())-DateTime.Parse(DateTime.Parse(ngayht.ToString()).ToShortDateString());
                                crRoom.dt["TienEarly"] = double.Parse(fe.calcEdit1.EditValue.ToString());// *tsp.TotalDays;
                            }
                        }
                    }
                }
                crRoom.dt["NgayDi"] = DateTime.Parse(ngayht.ToString());
                if (_data.CheckOut1Room(crRoom))
                {
                    // crRoom.CheckOut();
                    crRoom.dt["isCheckOut"] = true;
                    gridControl2.DataSource = crRoom.minibar;
                    gridControl3.DataSource = crRoom.dichvu;
                    gridView2.OptionsBehavior.Editable = !crRoom.isCheckOut;
                    gridView3.OptionsBehavior.Editable = !crRoom.isCheckOut;
                }
            }
           
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void gridControl2_Click(object sender, EventArgs e)
        {

        }

        private void gridLookUpEdit1_EditValueChanged_1(object sender, EventArgs e)
        {

        }

        private void grMaThue_EditValueChanged(object sender, EventArgs e)
        {
           
        }

        private void Hoantat_Click(object sender, EventArgs e)
        {

                if (_data.CheckOut())
                {

                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("Check Out không hợp lệ");
                }
        }
            
    
        void gridView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right &&this.crRoom!=null  &&!this.crRoom.isCheckOut)
            {
                this.popupMenu1.ShowPopup(new Point(e.X + gridControl1.Left  + this.Left+this.Parent.Left + this.Parent.Parent.Left,50+ e.Y + gridControl1.Top + this.Top+this.Parent.Top+this.Parent.Parent.Top));
            }
        }
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            fChonPhong fchonphong = new fChonPhong(DateTime.Parse(crRoom.dt["NgayDi"].ToString()));
            fchonphong.ShowDialog();
            if (fchonphong.DialogResult == DialogResult.OK && fchonphong.MaPhong!=string.Empty && !this.crRoom.isCheckOut)
            {
                if (this._data.ChuyenPhong(crRoom, fchonphong.MaPhong))
                {
                    gridControl2.DataSource = crRoom.minibar;
                    gridControl3.DataSource = crRoom.dichvu;
                    gridView2.OptionsBehavior.Editable = !crRoom.isCheckOut;
                    gridView3.OptionsBehavior.Editable = !crRoom.isCheckOut;
                    MessageBox.Show("Đã chuyển phòng");
                }
                else
                {
                    MessageBox.Show("Lỗi khi chuyển phòng");
                }
            }
        }

        private void btThanhtoan_Click(object sender, EventArgs e)
        {
            fThanhtoan f = new fThanhtoan(_dbData, _data.mt, _data.dt);
            f.ShowDialog();
            _data.mt["Datcoc"] = double.Parse(_data.mt["Datcoc"].ToString()) + f.sotienOld;
            //_data.mt["Conlai"] = double.Parse(_data.mt["Conlai"].ToString()) - f.sotienOld;
            _data.mt.EndEdit();
        }

        
       

    }
}