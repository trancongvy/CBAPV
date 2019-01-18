using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugins;
using System.Windows.Forms;
using System.Data;

namespace QLSX
{
    public class QLSX:ICustom 
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
                    fLichSX frm = new fLichSX();
                    frm.Text = si.MenuName;
                    //frm.ShowDialog();
                    return frm;
                case 7002:
                    fNhapTP frm1 = new fNhapTP();
                    frm1.Text = si.MenuName;
                    //frm.ShowDialog();
                    return frm1;
                case 7003:
                    fTaoPhieuXuatLSX frm2 = new fTaoPhieuXuatLSX();
                    frm2.Text = si.MenuName;
                    //frm.ShowDialog();
                    return frm2;
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

        internal class CBABTPDataSet
        {
            public CBABTPDataSet()
            {
            }

            public object CtLichSX { get; internal set; }
            public string DataSetName { get; internal set; }
            public object DmMIn { get; internal set; }
            public SchemaSerializationMode SchemaSerializationMode { get; internal set; }
        }

        internal class CBABTPDataSetTableAdapters
        {
            internal class CtLichSXTableAdapter
            {
                public CtLichSXTableAdapter()
                {
                }

                public bool ClearBeforeFill { get; internal set; }

                internal void Fill(object ctLichSX)
                {
                    throw new NotImplementedException();
                }
            }

            internal class DmMInTableAdapter
            {
                public DmMInTableAdapter()
                {
                }

                public bool ClearBeforeFill { get; internal set; }

                internal void Fill(object dmMIn)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
