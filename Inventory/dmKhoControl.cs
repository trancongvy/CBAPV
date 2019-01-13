using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Inventory
{
    public partial class dmKhoControl : UserControl
    {
        public dmKhoControl(ref DataTable tb)
        {
            InitializeComponent();
            gridControl1.DataSource = tb;
        }
    }
}
