using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Inventory
{
    public partial class GiaVT : DevExpress.XtraEditors.XtraForm
    {
        public GiaVT(int tuthang, int denthang, int selectedIndex,string[] makho)
        {
            _tuThang = tuthang;
            _denThang = denthang;
            lMakho = makho;
            if (_tuThang > _denThang)
            {
                MessageBox.Show("Thông tin không hợp lệ ! ");
                return;
            }
            _selectedIndex = selectedIndex;
            InitializeComponent();
        }
        string[] lMakho;
        private void gridControlGiaVT_Load(object sender, EventArgs e)
        {
            switch (_selectedIndex)
            {
                case 0:
                    if (lMakho.Length == 0)
                    {
                        GiaTrungBinh gtb = new GiaTrungBinh(_tuThang, _denThang, null);
                        Cursor.Current = Cursors.WaitCursor;
                        gtb.TinhGia();
                        Cursor.Current = Cursors.Default;
                        gridControlGiaVT.DataSource = gtb.DtVatTu;
                    }
                    else
                    {
                        DataTable source = null;
                        foreach (string makho in lMakho)
                        {
                            GiaTrungBinh gtb = new GiaTrungBinh(_tuThang, _denThang, makho);
                            Cursor.Current = Cursors.WaitCursor;
                            gtb.TinhGia();
                            Cursor.Current = Cursors.Default;
                            if (source == null) source = gtb.DtVatTu;
                            else
                            {
                                source.Merge(gtb.DtVatTu, true);
                            }
                        }
                        gridControlGiaVT.DataSource = source;
                    }
                    break;
                case 1:
                    GiaNTXT gntxt = new GiaNTXT(_tuThang, _denThang);
                    DataTable source1 = null;
                    foreach (string makho in lMakho)
                    {
                        gntxt.MaKho = makho;
                        Cursor.Current = Cursors.WaitCursor;
                        gntxt.TinhGia();
                        Cursor.Current = Cursors.Default;
                        if (source1 == null) source1 = gntxt.DtVatTu;
                        else
                        {
                            source1.Merge(gntxt.DtVatTu, true);
                        }
                    }

                    gridControlGiaVT.DataSource = source1;
                    //gridControlGiaVT.DataSource = gntxt.DtVatTu;
                    gridColumn6.Visible = true;
                    gridColumn7.Visible = true;
                    gridColumn8.Visible = true;
                    break;
                default:
                    break;
            }
        }


        private int _tuThang;
        private int _denThang;
        private int _selectedIndex;

        private void gridControlGiaVT_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }

    }
}