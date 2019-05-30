using System;
using System.Collections.Generic;
using System.Text;
using Plugins;
using System.Windows.Forms;

namespace Piriou
{
    public class Piriou:ICustom
    {
        private List<StructInfo> _listStructInfo;
        public Form Execute(int menuId)
        {
            Form f = new Form();
            foreach (StructInfo si in ListStructInfo)
            {
                if (si.MenuId == menuId)
                {
                    return ExecuteFunctions(si);
                }
            }
            return f;
        }

        private Form ExecuteFunctions(StructInfo si)
        {
            Form f = new Form();
            switch (si.MenuId)
            {
                case 9001:
                    fViewSumRFM frm = new fViewSumRFM();
                    frm.Text = si.MenuName;
                    //frm.ShowDialog();
                    return frm;
                case 9002:
                    fCreateRequest frm1 = new fCreateRequest();
                    frm1.Text = si.MenuName;
                    //frm.ShowDialog();
                    return frm1;
                case 9003:
                    ChkStock frm2 = new ChkStock();
                    frm2.Text = si.MenuName;
                    //frm.ShowDialog();
                    return frm2;
                case 9004:
                    fProvider frm3 = new fProvider();
                    frm3.Text = si.MenuName;
                    //frm.ShowDialog();
                    return frm3;
                case 9005:
                    fPOMuber frm4 = new fPOMuber();
                    frm4.Text = si.MenuName;
                    //frm.ShowDialog();
                    return frm4;
                case 9006:
                    ChkStockCutof frm5 = new ChkStockCutof();
                    frm5.Text = si.MenuName;
                    //frm.ShowDialog();
                    return frm5;
            }
            return f;
        }

        public List<StructInfo> ListStructInfo
        {
            get
            {
                return _listStructInfo; 
            }
            set
            {
                _listStructInfo = value;
            }
        }
    }
}
