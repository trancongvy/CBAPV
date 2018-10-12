using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugins;
using System.Windows.Forms;
namespace Clinic
{
    public class Clinic:ICustom 
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
                case 7001:
                    fBocso frm = new fBocso();
                    frm.Text = si.MenuName;
                    //frm.ShowDialog();
                    return frm;
                
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
