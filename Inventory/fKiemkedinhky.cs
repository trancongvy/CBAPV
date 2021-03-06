using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CDTDatabase;
using CDTLib;

namespace Inventory
{
    public partial class fKiemkedinhky : DevExpress.XtraEditors.XtraForm
    {
        public fKiemkedinhky()
        {
            InitializeComponent();
            string sql = "select * from dmkho";
            tbKho = _db.GetDataTable(sql);
            sql = "select MaNhomVT, TenNhom from dmnhomvt";
            tbNhom = _db.GetDataTable(sql);
        }
        Database _db = Database.NewDataDatabase();
        DataTable tbKho;
        DataTable tbNhom;
        DataTable data;
        string Makho="";
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (gridLookUpEdit1.EditValue != null && dateEdit1.EditValue != null && dateEdit2.EditValue != null)
            {
                string sql = "GetDataTonkhoCL";
                Makho = gridLookUpEdit1.EditValue.ToString();
                data = _db.GetDataSetByStore(sql, new string[] { "@ngayct1", "@ngayct2", "@makho", "@Nhom" }, new object[] { DateTime.Parse(dateEdit1.EditValue.ToString()), DateTime.Parse(dateEdit2.EditValue.ToString()), gridLookUpEdit1.EditValue.ToString(), gridLookUpEdit2.EditValue.ToString() });
                if (data != null)
                {
                    data.ColumnChanged += new DataColumnChangeEventHandler(data_ColumnChanged);
                    gridControl1.DataSource = data;
                }
            }
        }

        void data_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            if (e.Column.ColumnName == "slTon")
            {
                e.Row["cl"] = double.Parse(e.Row["Dudau"].ToString()) + double.Parse(e.Row["slNhap"].ToString()) - double.Parse(e.Row["slXuat"].ToString()) - double.Parse(e.Row["slTon"].ToString());
                e.Row.EndEdit();
            }
        }

        private void fKiemkedinhky_Load(object sender, EventArgs e)
        {
            gridLookUpEdit1.Properties.DataSource = tbKho;
            gridLookUpEdit1.Properties.ValueMember = "MaKho";
            gridLookUpEdit1.Properties.DisplayMember= "MaKho";
            gridLookUpEdit2.Properties.DataSource = tbNhom;
            gridLookUpEdit2.Properties.ValueMember = "MaNhomVT";
            gridLookUpEdit2.Properties.DisplayMember = "TenNhom";
            if (tbKho.Rows.Count > 0)
            {
                gridLookUpEdit1.EditValue = tbKho.Rows[0]["MaKho"].ToString();
            }
            if (tbKho.Rows.Count > 0)
            {
                gridLookUpEdit2.EditValue = tbNhom.Rows[0]["MaNhomVT"].ToString();
            }
            if (Config.Variables.Contains("NgayCt1"))
            {
                dateEdit1.EditValue = DateTime.Parse(Config.GetValue("NgayCt1").ToString());
            }
            if (Config.Variables.Contains("NgayCt2"))
            {
                dateEdit2.EditValue = DateTime.Parse(Config.GetValue("NgayCt2").ToString());
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Guid id =  Guid.NewGuid();
            string sql = "insert into MT43 (MT43ID, MaCT, SoCT,Ngayct, MaKH, DiaChi, OngBa, DienGiai, MaNT, TyGia, XuatDD) ";
            sql += " values('" + id.ToString() + "','PXK','PXKCL/T" + DateTime.Parse(dateEdit2.EditValue.ToString()).Month.ToString() + "','";
            sql += dateEdit2.EditValue.ToString() + "','CONGTY', ' ', ' ', N'Xuất kho kiểm kê định kỳ', 'VND',1,0)";
            _db.BeginMultiTrans();
            try
            {
                _db.UpdateByNonQuery(sql);
                
                sql = "insert into dt43 (MT43ID,  maKho,tkno,MaVT, maDVT, soluong,  tkco, dt43id) values('" + id.ToString() + "','";
                sql +=  Makho + "','6211','";
                string sqlbltk ="insert into bltk(mtid,Mact, soct, ngayct, diengiai, makh, mant,tygia, ongba, nhomdk, tk,tkdu, mtiddt)";
                sqlbltk +=" values('" + id.ToString()  + "','PXK','PXKCL/T" + DateTime.Parse(dateEdit2.EditValue.ToString()).Month.ToString() + "','";
                sqlbltk += dateEdit2.EditValue.ToString() + "', N'Xuất kho kiểm kê định kỳ','CONGTY',  'VND',1,'','";

                string sqlblvt = "insert into blvt(mtid,Mact, soct, ngayct, diengiai, makh, mant,tygia, ongba, nhomdk, mavt, makho, soluong_x, mtiddt)";
                sqlblvt += " values('" + id.ToString()  + "','PXK','PXKCL/T" + DateTime.Parse(dateEdit2.EditValue.ToString()).Month.ToString() + "','";
                sqlblvt += dateEdit2.EditValue.ToString() + "', N'Xuất kho kiểm kê định kỳ','CONGTY',  'VND',1,'','PXK1',' ";
                
                foreach (DataRow dr in data.Rows)
                {
                    string sql1;
                    string sqlbltk1;
                    string sqlblvt1;
                    Guid dtid = Guid.NewGuid();
                    if (double.Parse(dr["cl"].ToString()) > 0)
                    {
                        sql1 = sql + dr["MaVT"].ToString() + "','" + dr["MaDVT"].ToString() + "'," + dr["cl"].ToString() + ",'" + dr["TkKho"].ToString() +"','" +dtid.ToString()+ "')";
                        _db.UpdateByNonQuery(sql1);
                        sqlbltk1 = sqlbltk + "PXK1', '621','" + dr["tkKho"].ToString() + "','" + dtid.ToString() + "')";
                        _db.UpdateByNonQuery(sqlbltk1);
                        sqlbltk1 = sqlbltk + "PXK2', '" + dr["tkKho"].ToString() + "','621','" + dtid.ToString() + "')";
                        _db.UpdateByNonQuery(sqlbltk1);
                        sqlblvt1 = sqlblvt + dr["Mavt"].ToString() + "','" + Makho + "'," + dr["cl"].ToString() + ",'" + dtid.ToString() + "')";
                        _db.UpdateByNonQuery(sqlblvt1);
                    }
                }
                if (_db.HasErrors)
                {
                    _db.RollbackMultiTrans();
                    MessageBox.Show("Tạo phiếu xuất không thành công");
                }
                else
                {
                    _db.EndMultiTrans();
                    MessageBox.Show("Đã tạo phiếu xuất thành công: PXKCL/T" + DateTime.Parse(dateEdit2.EditValue.ToString()).Month.ToString(), "Thông báo");
                }
            }
            catch
            {
                _db.RollbackMultiTrans();
                MessageBox.Show("Tạo phiếu xuất không thành công");
            }
        }
    }
}