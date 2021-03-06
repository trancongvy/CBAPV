using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CDTLib;

namespace GLKC
{
    public partial class Filter : DevExpress.XtraEditors.XtraForm
    {
        private string namLv;
        public List<DataRow> _TransList;
        public Filter(List<DataRow> TransList)
        {
            InitializeComponent();
            namLv = Config.GetValue("NamLamViec").ToString();
            _TransList = TransList;
        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (spinEdit1.Value > spinEdit2.Value)
            {
                return;
            }
            GlKcCal CK;
            int j=int.Parse(spinEdit1.Value.ToString()); 
            int i=int.Parse(spinEdit2.Value.ToString());
            try
            {
                foreach (DataRow dr in _TransList)
                {
                    CK = new GlKcCal(j,i, namLv, dr);
                    if (!CK.Createbt())
                    {
                        MessageBox.Show("Không tạo được bút toán kết chuyển");
                        return;
                    }
                    else
                    {

                    }
                }
                MessageBox.Show("OK!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            
            
        }
        private void Filter_Keyup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Escape)
            {
                this.Dispose();
            }
            else if (e.KeyCode == Keys.F2)
            {
                simpleButton1_Click(simpleButton1,new EventArgs());
            }
            else if (e.KeyCode == Keys.F4)
            {
                 simpleButton2_Click(simpleButton2,new EventArgs());
            }
        }


        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (spinEdit1.Value > spinEdit2.Value)
            {
                return;
            }
            GlKcCal CK;
            int j = int.Parse(spinEdit1.Value.ToString());
            int i = int.Parse(spinEdit2.Value.ToString());
            try
            {
                foreach (DataRow dr in _TransList)
                {
                    CK = new GlKcCal(j, i, namLv, dr);
                    if (!CK.DeleteBt())
                    {
                        MessageBox.Show("Không tạo được bút toán kết chuyển");
                        return;
                    }
                    else
                    {

                    }
                }
                MessageBox.Show("OK!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

        }

    }
}