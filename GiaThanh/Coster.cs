using System;
using System.Collections.Generic;
using System.Text;
using CDTLib;
using System.Data;
using System.Windows.Forms;
namespace GiaThanh
{
    public class Coster
    {
        private DateTime _Tungay;
        private DateTime _Denngay;
        private DataTable _Gia;
        //private List<NhomGt> lstNhom;
        private NhomGt NhomGia;
        string Manhom = "";
        static void Main()
        {
            Coster co = new Coster();
        }
        public Form Co;
        public Coster()
        {
            Filter f = new Filter();
            
            if (f.ShowDialog() == DialogResult.OK)
            {
                
                _Tungay = f.Tungay;
                _Denngay = f.DenNgay;
                Manhom = f.manhom;
                TinhGia();
            }
        }
        
        private void TinhGia()
        {
            //Lấy các nhóm giá thành
            
            NhomGia =new NhomGt(_Tungay,_Denngay,Manhom);
            //NhomGia.TinhGiaThanh();
            _Gia = NhomGia.BangGia;
            //Preview
            GTPreview GTPre = new GTPreview(_Gia);            
            GTPre.UpdateGia += new EventHandler(this.UpdateGia);
            Co= GTPre;
            //GTPre.ShowDialog();
        }
        private void UpdateGia(object ob, EventArgs e )
        {
            bool kqUpdate= NhomGia.UpdateGia();
            if (!kqUpdate)
            {
                MessageBox.Show("Không cập nhật được giá thành!");
            }else
            {
                MessageBox.Show("Cập nhật thành công!");
            }

        }

    }
}
