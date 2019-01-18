using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CDTDatabase;
using CDTLib;
namespace QLSX
{
    public partial class fNhapTP : Form
    {
        public fNhapTP()
        {
            InitializeComponent();
        }
        Database dbdata = Database.NewDataDatabase();
        Database dbStrucst = Database.NewStructDatabase();
        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fDieuXe_Load(object sender, EventArgs e)
        {
            dNgay.EditValue = ngayct;
            dNgaynhap.EditValue = ngayct;
            
            GetMaXe();
            getData();
        }
        DataTable dmXe;
        DataTable dmMIn;

        DataTable DTLSX;
        DateTime ngayct = DateTime.Parse(DateTime.Now.ToLongDateString());
        string Maxe = null;
       
        string Mayin="";
        string macn = Config.GetValue("MaCN").ToString();
        private void getData()
        {
            //throw new NotImplementedException();
            Mayin = gridLookUpEdit2.EditValue.ToString();
            if (Mayin == "") return;
            DTLSX = dbdata.GetDataSetByStore("getLichSXThuchien", new string[] { "@Ngayct","@MaMin" }, new object[] {DateTime.Parse(dNgay.EditValue.ToString()), Mayin });
            if (DTLSX == null) return;
            gridControl1.DataSource = DTLSX;
            DTLSX.ColumnChanging += DTLSX_ColumnChanging;
        }

        private void DTLSX_ColumnChanging(object sender, DataColumnChangeEventArgs e)
        {
           if(e.Column.ColumnName== "DangChay" && !bool.Parse(e.Row["DangChay"].ToString()))
           {
                if (bool.Parse(e.ProposedValue.ToString()) && e.Row["TuNgay"] == DBNull.Value)
                {
                    if (MessageBox.Show("Bạn có thực sự muốn thực hiện lịch sản xuất này không?","Chắc chắn",MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        string sql = "update CTLichSX set Tungay=cast('" + DateTime.Now.ToString() + "' as datetime), TrangThai=1 where CTLichSXID='" + e.Row["CTLichSXID"].ToString() + "'";
                        dbdata.UpdateByNonQuery(sql);
                        if (dbdata.HasErrors)
                        {
                            e.ProposedValue = false;
                        }
                        else
                        {
                            e.Row["TuNgay"] = DateTime.Now.ToString();
                            e.Row["TrangThai"] = 1;
                        }
                    }
                    else
                    {
                        e.ProposedValue = false;
                    }
                }
                else
                {
                    e.ProposedValue = false;
                }
            }
            else if(e.Column.ColumnName == "HoanThanh" && !bool.Parse(e.Row["HoanThanh"].ToString()) ) {
                if (bool.Parse(e.ProposedValue.ToString()) && e.Row["DenNgay"] == DBNull.Value && bool.Parse(e.Row["DangChay"].ToString()) && e.Row["TrangThai"].ToString()=="1")
                {
                    if (MessageBox.Show("Bạn có chắc chắn thực hiện hoàn thành lịch sản xuất này không?", "Chắc chắn", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        dbdata.BeginMultiTrans();
                        string sql = "update CTLichSX set Denngay=cast('" + DateTime.Now.ToString() + "' as datetime), TrangThai=2 where CTLichSXID='" + e.Row["CTLichSXID"].ToString() + "'";
                        dbdata.UpdateByNonQuery(sql);
                        if (dbdata.HasErrors)
                        {
                            e.ProposedValue = false;
                            dbdata.RollbackMultiTrans();
                        }
                        else
                        {
                            e.Row["DenNgay"] = DateTime.Now.ToString();
                            e.Row["TrangThai"] = 2;
                            if(TaoPhieuNhap())
                            {
                                e.Row["SLTPNhap"] = 0;
                                dbdata.EndMultiTrans();
                            }
                            else
                            {
                                e.ProposedValue = false;
                                dbdata.RollbackMultiTrans();
                            }

                        }
                    }
                    else
                    {
                        e.ProposedValue = false;
                    }
                }
                else
                {
                    e.ProposedValue = false;
                }
            }
        }

        
        private void GetMaXe()
        {
            
            string sql = "select * from dmmin ";
            dmMIn = dbdata.GetDataTable(sql);

            if (dmMIn != null)
            {
                gridLookUpEdit2.Properties.DataSource = dmMIn;
                if (dmMIn.Rows.Count > 0) gridLookUpEdit2.EditValue = dmMIn.Rows[0]["MaMIn"].ToString();
            }
        }

        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            
        }

        private bool TaoPhieuNhap()
        {
            string sql = "";
            string soct = "";
            try
            {
                Guid ID = Guid.NewGuid();
                DataRow[] ldr = DTLSX.Select("HoanThanh=1 and SLTPNhap >0 ");
                if (ldr.Length == 0) return false;
                object o = dbStrucst.GetValue("select dbo.AutoCreate('MT41',2)");
                if (o != null) soct = o.ToString();
                Guid task = Guid.NewGuid();
                o = dbStrucst.GetValue("select dbo.GetBTid('DT41')");
                if (o == null) o = task;
                dbdata.BeginMultiTrans();
                sql = "insert into MT41 (mt41ID,MaCT, NGayCT,SoCT,Approved,Diengiai,MaNT, Tygia, TTienNT, TTien,PrintIndex,sysDBID,TaskID,  MaCN) values (";
                sql += "@mt41ID,@MaCT, @NGayCT,@SoCT,@Approved,@Diengiai,@MaNT, @Tygia, @TTienNT, @TTien,@PrintIndex,@sysDBID,@TaskID, @MaCN)";
                dbdata.UpdateDatabyPara(sql, new string[] { "@mt41ID", "@MaCT", "@NGayCT", "@SoCT", "@Approved", "@Diengiai", "@MaNT", "@Tygia", "@TTienNT", "@TTien", "@PrintIndex", "@sysDBID", "@TaskID", "@MaCN" },
                    new object[] { ID, "NSX", ngayct, soct, 0, "Nhập kho thành phẩm", "VND", 1, 0, 0, 0, 2, o, macn });
                if (dbdata.HasErrors)
                {
                    dbdata.RollbackMultiTrans();
                    dbdata.HasErrors = false;
                    MessageBox.Show("Lỗi");
                    return false;
                }
                foreach (DataRow dr in ldr)
                {
                    sql = "insert into DT41 (mt41ID,DT41ID, MaVT, Soluong, Gia, GiaNT,Ps, PsNT,MTLSXID, DTLSXID,MaMin, MaDVT, tkco, tkno) select ";
                    sql += "@mt41ID,@DT41ID, @MaVT, @Soluong, @Gia, @GiaNT,@Ps, @PsNT,@MTLSXID, @DTLSXID,@MaMin, MaDVT, TkKho, TkGV from dmvt where mavt=@MaVT ";
                    dbdata.UpdateDatabyPara(sql, new string[] { "@mt41ID", "@DT41ID", "@MaVT", "@Soluong", "@Gia", "@GiaNT", "@Ps", "@PsNT", "@MTLSXID", "@DTLSXID", "@MaMin" },
                        new object[] { ID, Guid.NewGuid(), dr["MaVT"].ToString(), double.Parse(dr["SLTPNhap"].ToString()), 0, 0, 0, 0, Guid.Parse(dr["MTLSXID"].ToString()), Guid.Parse(dr["DTLSXID"].ToString()), dr["MaMin"].ToString() });
                    sql = "update dtLSX set SLTPNK=SLTPNK+" + dr["SLTPNhap"].ToString();
                    dbdata.UpdateByNonQuery(sql);
                    if (dbdata.HasErrors)
                    {
                        dbdata.RollbackMultiTrans();
                        dbdata.HasErrors = false;
                        MessageBox.Show("Lỗi");
                        return false;
                    }
                }
                if (!dbdata.HasErrors)
                {
                    dbdata.EndMultiTrans();
                    dbStrucst.UpdateDatabyStore("UpdateSoct", new string[] { "@tableName", "@sysDBID", "@SoCT", "@MaCN" }, new object[] { "MT41", 2, soct, null });
                    MessageBox.Show("Đã tạo phiếu nhập thành phẩm");
                    return true;
                }
                else
                {
                    dbdata.RollbackMultiTrans();
                    dbdata.HasErrors = false;
                    MessageBox.Show("Lỗi");
                    return false;
                }
            }
            catch (Exception ex) { return false; }
            finally
            {
                if (dbdata.Connection.State != ConnectionState.Closed)
                    dbdata.Connection.Close();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            TaoPhieuNhap();
        }

        private void gridLookUpEdit2_EditValueChanged(object sender, EventArgs e)
        {
            Mayin  = gridLookUpEdit2.EditValue.ToString();
            getData();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            getData();
        }

    }
}
