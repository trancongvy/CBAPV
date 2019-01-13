using System;
using System.Collections.Generic;
using System.Text;
using Plugins;
using System.Windows.Forms;

namespace Banhang
{
    public class Banhang : ICustom
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
            //Thuwr commit
        }

        private Form ExecuteFunctions(StructInfo si)
        {
            Form f = new Form();
            switch (si.MenuId)
            {
                case 3501:
                    fTaoHoadon frm = new fTaoHoadon();
                    frm.Text = si.MenuName;
                    //frm.ShowDialog();
                    return frm;
                case 3502:
                    fDieuXe frm1 = new fDieuXe();
                    frm1.Text = si.MenuName;
                    //frm.ShowDialog();
                    return frm1;
                case 3503:
                    fTaoPhieuXuatLSX frm2 = new fTaoPhieuXuatLSX();
                    frm2.Text = si.MenuName;
                    //frm.ShowDialog();
                    return frm2;
                case 3504:
                    fNhapTP frm3 = new fNhapTP();
                    frm3.Text = si.MenuName;
                    //frm.ShowDialog();
                    return frm3;
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
