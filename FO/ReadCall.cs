using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CDTLib;
using CDTDatabase;
namespace FO
{
    public partial class ReadCall : Form
    {
        Database _db = Database.NewDataDatabase();
        string FileName;
        public ReadCall()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Left = Screen.PrimaryScreen.Bounds.Width - this.Width;
            this.Top = Screen.PrimaryScreen.Bounds.Height - this.Height;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ReadCallLog(FileName);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(textEdit1.Text))
            {
                FileName = textEdit1.Text;
                timer1.Enabled = true;
                this.Visible = false;
            }
            else
            {
                MessageBox.Show("Không tìm thấy file dữ liệu tổng dài");
            }
        }
        private void ReadCallLog(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                string[] CallLog = System.IO.File.ReadAllLines(fileName);
                for (int i = CallLog.Length - 1; i > 0; i =i- 1)
                {
                    try
                    {
                        string MaPhong;
                        DateTime Ngayct;
                        DateTime DenNgay;
                        string Tg;
                        string Call = CallLog[i];
                        Call = RemovedSpace(Call);
                        string[] c = Call.Split((" ").ToCharArray());
                        if (c.Length < 6) continue;
                        string[] t = c[0].Split("=".ToCharArray());
                        if (t.Length < 2) continue;
                        string[] dmy = t[1].Split("/".ToCharArray());
                        if (dmy.Length < 3) continue;
                        Ngayct = DateTime.Parse(dmy[1] + "/" + dmy[0] + "/" + dmy[2] + " " + t[0]);
                        MaPhong = c[2];
                        Tg = c[5];
                        Tg = Tg.Replace("'", ":");
                        string[] hms = Tg.Split(":".ToCharArray());
                        if (hms.Length < 3) continue;
                        TimeSpan tg = new TimeSpan(int.Parse(hms[0]), int.Parse(hms[1]), int.Parse(hms[2]));
                        DenNgay = Ngayct.Add(tg);
                        if (!CheckReaded(Ngayct, MaPhong))
                        {
                            string sql = "insert into ctCuocgoi(NgayCt, std, Soden, MaPhong, DenNgay) values('" +
                                  Ngayct.ToLongDateString() + "','" + MaPhong + "','" + c[4] + "','" + MaPhong + "','" + DenNgay.ToLongDateString() + "')";

                            _db.UpdateByNonQuery(sql);
                           //_db.HasErrors = false;
                            if (_db.HasErrors)
                            {
                                timer1.Enabled = false;
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }
        private string RemovedSpace(string s)
        {
            while (s.Contains("  "))
            {
                s = s.Replace("  ", " ");
            }
            return s;
        }
        private bool CheckReaded(DateTime d, string maphong)
        {
            object value = _db.GetValue("select count(*) from ctCuocgoi where NgayCt='" + d.ToString() + "' and MaPhong='" + maphong + "'");
            if (value == null) return false;
            if (int.Parse(value.ToString()) > 0) return true;
            else return false;
            //return (value == null && int.Parse(value.ToString()) > 0);
            
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != null)
                textEdit1.Text = openFileDialog1.FileName;
        }
    }
}