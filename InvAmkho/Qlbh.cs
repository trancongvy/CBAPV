using System;
using System.Collections.Generic;
using System.Text;
using Plugins;
using System.Data;
using System.Data.ProviderBase;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Windows.Forms;
using CDTDatabase;
namespace TestICustomData
{
    class TestICustomData : ICustomData
    {
        private bool _result = true;
        private StructPara _info;
       
        #region ICustomData Members
      
        public void ExecuteBefore()
        {

           
            if (_info.TableName == "XuatHang")
             {

              _result = true;
             
              
               DataRow drspxuat = _info.DsData.Tables[0].Rows[_info.CurMasterIndex];
               if (drspxuat.RowState != DataRowState.Deleted)
               {
                   double sl = 0;
                   string mamh = "";
                   string makho = "";
                   DateTime ngay = DateTime.Parse(_info.DsData.Tables[0].Rows[_info.CurMasterIndex][1].ToString());
                   //số lượng tồn kho tức thời
                   //string sql = "select makho,mahh,sum(SLnhap)as slnhap,sum(SLxuat)as slxuat,sum(slnhap-slxuat)as tonkho from tonghop where NgayCT <='"+ngay+"' group by makho,mahh";
                  // DataTable data = _info.DbData.GetDataTable(sql);                  
                   DataTable data = _info.DbData.GetDataSetByStore("tonkho", new string[] { "@ngayct" }, new object[] { ngay });
                   string fk = _info.DrTableMaster["PK"].ToString();

                   //số lượng đã nhập
                  // string sql2 = "select * from chitietxuat where sophieuxuat='" + drspxuat[fk] + "'";
                   //DataTable data2 = _info.DbData.GetDataTable(sql2);
                   DataTable data2 = _info.DbData.GetDataSetByStore("sldanhap", new string[] { "@sophieu" }, new object[] { drspxuat[fk] });
                   _info.DsData.Tables[1].DefaultView.RowFilter = fk + "='" + drspxuat[fk].ToString() + "'";
                   double temp=0; 
                   for (int i = 0; i < _info.DsData.Tables[1].DefaultView.Count; i++)
                   {
                       mamh = _info.DsData.Tables[1].DefaultView[i]["mahh"].ToString().Trim();
                       makho = _info.DsData.Tables[1].DefaultView[i]["makho"].ToString().Trim();
                       sl = Convert.ToDouble(_info.DsData.Tables[1].DefaultView[i]["soluong"].ToString());//số lượng mới nhập
                      // MessageBox.Show(sl.ToString());
                       foreach (DataRow dr1 in data.Rows)
                       {
                          // MessageBox.Show(dr1["tonkho"].ToString());
                           foreach (DataRow dr2 in data2.Rows)
                           {
                               temp = 0;//số lượng đã nhập
                               if(dr2["makho"].ToString().Trim()==dr1["makho"].ToString().Trim()&& dr2["mahh"].ToString().Trim()==dr1["mahh"].ToString().Trim()  )
                             {
                                 temp = Convert.ToDouble(dr2["soluong"].ToString());
                                 break;
                                
                             }
                           }
                          
                          
                               if (makho == dr1["makho"].ToString().Trim() && mamh == dr1["mahh"].ToString().Trim() && sl >Convert.ToDouble(dr1["tonkho"].ToString()) + temp) 
                               {

                                   _result = false;
                                   
                                   _info.DsData.Tables[1].DefaultView[i]["soluong"] = temp;


                               }
                          

                       }
                      

                   }
                   _info.DsData.Tables[1].DefaultView.RowFilter = string.Empty;
                   if (_result == false)
                   {
                       MessageBox.Show("Không đủ số lượng");
                   }
                       else {
                             
                           }
               }
                      
              
                
             }
            
            
        }

        public void ExecuteAfter()
        {
            
        }

        #endregion

        #region ICustomData Members

        public bool Result
        {
            get
            {
                return _result;
            }
        }

        public StructPara Info
        {
            set
            {
                _info = value;
            }
        }

        #endregion

    
    }
}
