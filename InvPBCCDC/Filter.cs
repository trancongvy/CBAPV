    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CDTLib;
using FormFactory;
using DataFactory;
namespace InvPBCCDC
{
    public partial class Filter : DevExpress.XtraEditors.XtraForm
    {
        private string namLv;
        public Filter()
        {
            InitializeComponent();
            namLv = Config.GetValue("NamLamViec").ToString();
        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            CDTData data=new DataSingle("dtCCDC",Config.GetValue("sysPackageID").ToString());
            data.Condition="soky=0";
            
            data.GetData();
            BindingSource bs = new BindingSource();
            bs.DataSource = data.DsData.Tables[0];
            if (data.DsData.Tables[0].Rows.Count > 0)
            {
                FrmSingle fs = new FrmSingle(data);
                fs.ShowDialog();
            }
            if (spinEdit1.Value > spinEdit2.Value)
            {
                return;
            }
            CalPB cal;
            for( int i=int.Parse(spinEdit1.Value.ToString()); i<=spinEdit2.Value; i++)
            {   
                try
                {
                    cal = new CalPB(i,namLv);
                    cal.calculate();
                    fbangPB f=new fbangPB(cal);
                    f.Text = this.Text + " tháng " + i.ToString() + " năm " + namLv.ToString() ;
                    f.ShowDialog();
                }
                catch
                {
                }
                
            }
        }


        private void simpleButton2_Click(object sender, EventArgs e)
        {
            CalPB cal;
            for (int i = int.Parse(spinEdit1.Value.ToString()); i <= spinEdit2.Value; i++)
            {
                try
                {
                    cal = new CalPB(i, namLv);
                    if (cal.deleteBt())
                    {
                        MessageBox.Show("Ok!");
                    }
                    else
                    {
                        MessageBox.Show("Có lỗi");
                    }
                }
                catch
                {
                }

            }
        }

        private void Filter_Load(object sender, EventArgs e)
        {

        }

    }
}