using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CDTDatabase;
using CDTLib;
namespace Banhang
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
            vNgaynhap.EditValue = ngayct;
            
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
            DTLSX = dbdata.GetDataSetByStore("GetMTLSXDangThuchien", new string[] { "@MaMayin" }, new object[] { Mayin });
            if (DTLSX == null) return;
            gridControl1.DataSource = DTLSX;

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



        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string sql = "";
            string soct = "";
            try
            {
                Guid ID = Guid.NewGuid();
                DataRow[] ldr = DTLSX.Select("Chon=1");
                if (ldr.Length == 0) return;
                object o = dbStrucst.GetValue("select dbo.AutoCreate('MT41',2)");
                if (o != null) soct = o.ToString();
                Guid task = Guid.NewGuid();
                o = dbStrucst.GetValue("select CDTBTP.dbo.GetBTid('DT41')");
                if (o == null) o = task;
                dbdata.BeginMultiTrans();
                sql = "insert into MT41 (mt41ID,MaCT, NGayCT,SoCT,Approved,Diengiai,MaNT, Tygia, TTienNT, TTien,PrintIndex,sysDBID,TaskID,  MaCN) values (";
                sql += "@mt41ID,@MaCT, @NGayCT,@SoCT,@Approved,@Diengiai,@MaNT, @Tygia, @TTienNT, @TTien,@PrintIndex,@sysDBID,@TaskID, @MaCN)";
                dbdata.UpdateDatabyPara(sql, new string[] { "@mt41ID","@MaCT", "@NGayCT","@SoCT","@Approved","@Diengiai","@MaNT","@Tygia", "@TTienNT", "@TTien","@PrintIndex","@sysDBID","@TaskID", "@MaCN" },
                    new object[] { ID, "NSX", ngayct, soct, 0, "Nhập kho thành phẩm", "VND", 1, 0, 0, 0, 2, o, macn });
                if (dbdata.HasErrors)
                {
                    dbdata.RollbackMultiTrans();
                    dbdata.HasErrors = false;
                    MessageBox.Show("Lỗi");
                    return;
                }
                foreach (DataRow dr in ldr)
                {
                    sql = "insert into DT41 (mt41ID,DT41ID, MaVT, Soluong, Gia, GiaNT,Ps, PsNT,MTLSXID, DTLSXID,MaMin, MaDVT, tkco, tkno) select ";
                    sql += "@mt41ID,@DT41ID, @MaVT, @Soluong, @Gia, @GiaNT,@Ps, @PsNT,@MTLSXID, @DTLSXID,@MaMin, MaDVT, TkKho, TkGV from dmvt where mavt=@MaVT ";
                    dbdata.UpdateDatabyPara(sql, new string[] { "@mt41ID","@DT41ID","@MaVT", "@Soluong", "@Gia", "@GiaNT","@Ps", "@PsNT","@MTLSXID", "@DTLSXID","@MaMin" },
                        new object[] { ID, Guid.NewGuid(), dr["MaVT"].ToString(), double.Parse(dr["SLTPNhap"].ToString()), 0, 0, 0, 0, Guid.Parse(dr["MTLSXID"].ToString()), Guid.Parse(dr["DTLSXID"].ToString()), dr["MaMin"].ToString() });
                    sql = "update dtLSX set SLTPNK=SLTPNK+" + dr["SLTPNhap"].ToString();
                    dbdata.UpdateByNonQuery(sql);
                    if (dbdata.HasErrors)
                    {
                        dbdata.RollbackMultiTrans();
                        dbdata.HasErrors = false;
                        MessageBox.Show("Lỗi");
                        return;
                    }
                }
                if (!dbdata.HasErrors)
                {
                    dbdata.EndMultiTrans();
                    dbStrucst.UpdateDatabyStore("UpdateSoct", new string[] { "@tableName", "@sysDBID", "@SoCT", "@MaCN" }, new object[] { "MT41", 2, soct, null });
                    MessageBox.Show("Đã tạo phiếu nhập thành phẩm");
                }
                else
                {
                    dbdata.RollbackMultiTrans();
                    dbdata.HasErrors = false;
                    MessageBox.Show("Lỗi");
                    return;
                }
            }
            catch (Exception ex) { }
            finally
            {
                if (dbdata.Connection.State != ConnectionState.Closed)
                    dbdata.Connection.Close();
            }
        }

        private void gridLookUpEdit2_EditValueChanged(object sender, EventArgs e)
        {
            Mayin  = gridLookUpEdit2.EditValue.ToString();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            getData();
        }

    }
}
