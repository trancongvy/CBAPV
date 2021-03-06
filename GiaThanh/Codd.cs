    using System;
using System.Collections.Generic;
using System.Text;
using CDTDatabase;
using System.Data;

namespace GiaThanh
{
    class Codd
    {
        private DateTime _Tungay;
        private DateTime _Denngay;
        private Database _dbData = Database.NewDataDatabase();//"server = vuonghuynh; database = CBA11; user = sa; pwd = sa");
        private Database _dbStruct = Database.NewStructDatabase();

        
        public Codd(DateTime tungay, DateTime denngay)
        {
            _Tungay = tungay;
            _Denngay = denngay;

        }

        //--Tính tiền dở dang từ thành phẩm dở dang
            //Chuyển tất cả các sản phẩm dở dang thành nguyên liệu dở dang + masp
            //Chuyển tất cả các nguyên liệu dở dang - masp thành tiền + mã sản phẩm
        public bool ddFromTP()
        {
            bool result = _dbData.UpdateDatabyStore("UpdateTienddFromTP", new string[] { "@tungay", "@denngay" }, new object[] { _Tungay, _Denngay });
            return result;
        }
        //--Tính tiền dỡ dang từ nguyên liệu dở dang
            //Chuyển tất cả các nguyên liệu dở dang  thành tiền + mã sản phẩm
        public bool ddFromNVL()
        {
            bool result = _dbData.UpdateDatabyStore("UpdateTienddFromNVL", new string[] { "@tungay", "@denngay" }, new object[] { _Tungay, _Denngay });
            return result;
        }
    }
}
