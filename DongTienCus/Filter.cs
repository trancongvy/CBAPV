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
using Formula;
using CDTControl;

namespace DongTienCus
{
    public partial class Filter : DevExpress.XtraEditors.XtraForm
    {
        public Filter()
        {
            InitializeComponent();           

        }
        DateTime tungay;
        DateTime denngay;
        Database db = CDTDatabase.Database.NewDataDatabase();
        DataTable kq;
        DataTable  DataMain;
        int songay;
        int songaygop = 1;
        private void getData()
        {
            DataMain = db.GetDataSetByStore("getSLBanh", new string[] { "@ngayct1", "@ngayct2" }, new object[] { tungay, denngay});
        }
        private void Phanbongay()
        {
            kq = new DataTable();
            DataColumn col1 = kq.Columns.Add("ngayct", typeof(DateTime));
            DataColumn col2 = kq.Columns.Add("mabak", typeof(string));
            DataColumn col3 = kq.Columns.Add("mavt", typeof(string));
            DataColumn col4 = kq.Columns.Add("slNhap", typeof(int));
            DataColumn col5 = kq.Columns.Add("giaban", typeof(decimal));
            DataColumn col6 = kq.Columns.Add("soluong", typeof(int));
            double soluong;
            Random Rand = new Random();
            foreach (DataRow dr in DataMain.Rows)
            {
                soluong = double.Parse(dr["sobanh"].ToString());

                double BN;


                double R;
                double Epx;//Epxilon
                double SL;
                int songayCL;
                for (int i = 0; i < songay; i++)
                {
                    //if (soluong <= 0) break;
                    if (soluong == 0)
                    {
                        SL = 0;
                    }
                    else
                    {
                        songayCL = songay - i;
                        BN = soluong / songayCL;
                        R = Rand.NextDouble();

                        Epx = (BN - 0.8) / (1.8 * BN);

                        if (i == songay - 1)
                        {
                            R = 0;
                            Epx = 1;
                        }
                        SL = BN * (Epx + 2 * R * (1 - Epx));


                        SL = Math.Round(SL);


                        if (SL < 0) SL = 0;
                        if (SL > soluong) SL = soluong;
                    }
                    DataRow kqdr = kq.NewRow();

                    kqdr["ngayct"] = tungay.AddDays(i );
                    kqdr["MaBak"] = dr["MaBak"];
                    kqdr["Mavt"] = dr["Mavt"];
                    kqdr["slNhap"] = SL;
                    kqdr["Giaban"] = double.Parse(dr["Giaban"].ToString());
                    kqdr["soluong"] = SL;
                    kq.Rows.Add(kqdr);
                    soluong = soluong - SL;
                }
            }
        }
        
        private void Ins()
        {
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string sql = "delete hoadon where ngayct between ";
            sql += " cast('" + tungay.ToShortDateString() + "' as datetime)  and cast('" + denngay.ToShortDateString() + "' as datetime)";            
            db.UpdateByNonQuery(sql);
            //db.BeginMultiTrans();
    
            try
            {
                foreach (DataRow dr in kq.Rows)
                {
                    if (double.Parse(dr["soluong"].ToString()) >= 0)
                    {
                        sql = "insert into Hoadon ( ngayct, mavt, soluong,makh,giaban) values(";
                        sql += "cast('" + dr["ngayct"].ToString().Trim() + "' as datetime)";
                        sql += ", '" + dr["MaVT"].ToString() + "'";
                        sql += "," + double.Parse(dr["soluong"].ToString()).ToString("###########0.######");
                        sql += ", '" + dr["MaBak"].ToString() + "'";
                        sql += ", " + double.Parse(dr["giaban"].ToString()).ToString("###########0.######") + ")";
                        db.UpdateByNonQuery(sql);
                        if (db.HasErrors)
                        {
                            db.HasErrors = false;
                        }
                    }
                }
                //db.UpdateDatabyStore("UpdateNgayHD", new string[] { "@ngayct1", "@ngayct2", "@ngayhd" }, new object[] { tungay, denngay, songaygop });

                TimeSpan songay1 = denngay - tungay;
                int songay = songay1.Days + 1;
                sql = "select mabak from dmbakery  ";
                DataTable kh = db.GetDataTable(sql);

                DataTransfer tf = new DataTransfer(db, Config.GetValue("mt32").ToString(), "mt32id");
                sql = "select mt32id from mt32 where ngayct between ";
                sql += " cast('" + tungay.ToShortDateString() + "' as datetime)  and cast('" + denngay.ToShortDateString() + "' as datetime) ";
                sql += " and makh in (select maBak from dmbakery)  ";
                DataTable keyValue = db.GetDataTable(sql);
                foreach (DataRow dr in keyValue.Rows)
                {
                    tf.Transfer(DataAction.Delete, dr["mt32id"].ToString(), new List<DataRow>(), false);                  
                }
                //Phieu thu
                DataTransfer tf1 = new DataTransfer(db, Config.GetValue("mt11").ToString(), "mt11id");
                sql = "select mt11id from mt11 where ngayct between ";
                sql += " cast('" + tungay.ToShortDateString() + "' as datetime)  and cast('" + denngay.ToShortDateString() + "' as datetime)";
                sql += " and makh in (select mabak from dmbakery) and mt32id is not null ";

                DataTable keyValue1 = db.GetDataTable(sql);
                foreach (DataRow dr in keyValue1.Rows)
                {
                    tf1.Transfer(DataAction.Delete, dr["mt11id"].ToString(), new List<DataRow>(), false);
                }
                int SoTTPhieuThu = 0;

                for (int i = 0; i < songay; i++)
                {
                    foreach (DataRow dr in kh.Rows)
                    {
                        string makh = dr["mabak"].ToString();
                        SoTTPhieuThu += 1;
                        db.UpdateDatabyStore("taohd", new string[] { "@makh", "@ngayct", "SoPhieu" }, new object[] { makh, tungay.AddDays(i), SoTTPhieuThu.ToString("00") });
                    }
                }
                //Post

                
                sql = "select mt32id from mt32 where ngayct between ";
                sql += " cast('" + tungay.ToShortDateString() + "' as datetime)  and cast('" + denngay.ToShortDateString() + "' as datetime)";
                sql += " and makh in (select mabak from dmbakery)  ";
                 keyValue = db.GetDataTable(sql);
                foreach (DataRow dr in keyValue.Rows)
                {
                    tf.Transfer(DataAction.Insert, dr["mt32id"].ToString(), new List<DataRow>(), false);
                }
                 
                
                //Post phieu thu


                sql = "select mt11id from mt11 where ngayct between ";
                sql += " cast('" + tungay.ToShortDateString() + "' as datetime)  and cast('" + denngay.ToShortDateString() + "' as datetime)";
                sql += " and makh in (select mabak from dmbakery) and mt32id is not null ";
                keyValue1 = db.GetDataTable(sql);
                foreach (DataRow dr in keyValue1.Rows)
                {
                    tf1.Transfer(DataAction.Insert, dr["mt11id"].ToString(), new List<DataRow>(), false);
                }
                //end
                //db.EndMultiTrans();
                MessageBox.Show("Hoàn thành");
            }
            catch (Exception ex)
            {
                //db.RollbackMultiTrans();
                MessageBox.Show(ex.Message);
            }
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            tungay = DateTime.Parse(this.dateEdit1.EditValue.ToString());
            denngay = DateTime.Parse(this.dateEdit2.EditValue.ToString());
            TimeSpan t = denngay - tungay;
            songay = t.Days + 1;
            getData();
            Phanbongay();
            this.gridControl1.DataSource = kq;
            //MessageBox.Show(kq.Rows.Count.ToString());
            //this.gridView1.DataSource = kq;
        }

        private void simpleButton3_Click_1(object sender, EventArgs e)
        {
            tungay = DateTime.Parse(this.dateEdit1.EditValue.ToString());
            denngay = DateTime.Parse(this.dateEdit2.EditValue.ToString());
            string sql = "delete hoadon where ngayct between ";
            sql += " cast('" + tungay.ToShortDateString() + "' as datetime)  and cast('" + denngay.ToShortDateString() + "' as datetime) ";
            sql += " and makh in (select mabak from dmbakery)";
            db.UpdateByNonQuery(sql);
            //xóa tranfer
            //hóa đơn
            DataTransfer tf = new DataTransfer(db, Config.GetValue("mt32").ToString(), "mt32id");
            sql = "select mt32id from mt32 where ngayct between ";
            sql += " cast('" + tungay.ToShortDateString() + "' as datetime)  and cast('" + denngay.ToShortDateString() + "' as datetime) ";
            sql += " and makh in (select mabak from dmbakery)  ";
            DataTable keyValue = db.GetDataTable(sql);
            foreach (DataRow dr in keyValue.Rows)
            {
                tf.Transfer(DataAction.Delete, dr["mt32id"].ToString(), new List<DataRow>(), false);
            }
            //Phieu thu
            DataTransfer tf1 = new DataTransfer(db, Config.GetValue("mt11").ToString(), "mt11id");
            sql = "select mt11id from mt11 where ngayct between ";
            sql += " cast('" + tungay.ToShortDateString() + "' as datetime)  and cast('" + denngay.ToShortDateString() + "' as datetime)";
            sql += " and makh in (select mabak from dmbakery) and mt32id is not null ";
            keyValue = db.GetDataTable(sql);
            foreach (DataRow dr in keyValue.Rows)
            {
                tf1.Transfer(DataAction.Delete, dr["mt11id"].ToString(), new List<DataRow>(), false);
            }
            //Xóa phiếu
            db.UpdateDatabyStore("xoahd", new string[] { "@ngayct1","@ngayct2" }, new object[] {  tungay,denngay });
            MessageBox.Show("Hoàn thành!");
        }

        private void spinEdit1_EditValueChanged(object sender, EventArgs e)
        {
            songaygop = int.Parse(spinEdit1.EditValue.ToString());
        }

    }
}