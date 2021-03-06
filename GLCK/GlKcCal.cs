using System;
using System.Collections.Generic;
using System.Text;
using CDTDatabase;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace GLKC
{
    class GlKcCal
    {
        private DateTime _ngayCt1;
        private DateTime _ngayCt2;
        public Database _dbData = Database.NewDataDatabase();
        public DataTable TienCK;
        public DataRow _TransRow;
        public GlKcCal(int j,int i, string namlv,DataRow TransRow)//i tháng cần tính
        {
            string str = j.ToString() +"/01/"  +  namlv;
            _ngayCt1 = DateTime.Parse(str);
            str = i.ToString() + "/01/" + namlv;
            _ngayCt2 = DateTime.Parse(str);
            _ngayCt2 = _ngayCt2.AddMonths(1).AddDays(-1);
            

            _TransRow = TransRow;
            
            
        }
        private DataTable getData()
        {
            DeleteBt();
            string sql;
            DataTable dt45 = new DataTable("GLCK");
            sql = "soduCK";
            string[] paranames;
            object[] paraValues;

            paranames = new string[] { "@tk", "@ngayCt1", "@ngayCt2", "@mabp", "@mavv", "@maphi", "@macongtrinh","@maCN" };
            paraValues = new object[] { _TransRow["TkNguon"].ToString(), _ngayCt1, _ngayCt2, bool.Parse(_TransRow["MaBP"].ToString()) ? 1 : 0, bool.Parse(_TransRow["MaVV"].ToString()) ? 1 : 0, bool.Parse(_TransRow["MaPhi"].ToString()) ? 1 : 0, bool.Parse(_TransRow["MaCongtrinh"].ToString()) ? 1 : 0, bool.Parse(_TransRow["MaCN"].ToString()) ? 1 : 0 };

            try
            {
                dt45 = _dbData.GetDataSetByStore(sql, paranames, paraValues);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            return dt45;


        }
        public bool Createbt()
        {
            TienCK = getData();
            _dbData.BeginMultiTrans();
            try
            {
                //MessageBox.Show(TienCK.Rows.Count.ToString());
                foreach (DataRow dr in TienCK.Rows)
                {
                    createBtdt(dr);
                    if (_dbData.HasErrors)
                    {
                        _dbData.RollbackMultiTrans();
                        break;
                    }
                }
                if (!_dbData.HasErrors)
                    _dbData.EndMultiTrans(); 
            }
            catch (Exception ex)
            {                
                return false;
            }
            return true; 
        }
        public bool createBtdt(DataRow dr)
        {
            try
            {
                //MessageBox.Show("running createbt in Glkc");
                string tableName = "bltk";
                List<string> fieldName = new List<string>();
                List<string> Values = new List<string>();
                Guid id = new Guid();
                fieldName.Add("MTID");
                fieldName.Add("Nhomdk");
                fieldName.Add("MaCT");
                fieldName.Add("SoCT");
                fieldName.Add("NgayCT");
                fieldName.Add("Ongba");
                fieldName.Add("DienGiai");
                fieldName.Add("TK");
                fieldName.Add("TKdu");
                fieldName.Add("Psno");
                fieldName.Add("Psco");
                if (bool.Parse(_TransRow["Maphi"].ToString()))
                {
                    if (!(dr["MaPhi"] is DBNull))
                    {
                        fieldName.Add("Maphi");
                    }
                }
                if (bool.Parse(_TransRow["MaVV"].ToString()))
                {
                    if (!(dr["MaVV"] is DBNull))
                    {
                        fieldName.Add("MaVV");
                    }
                }
                if (bool.Parse(_TransRow["MaBP"].ToString()))
                {
                    if (!(dr["MaBP"] is DBNull))
                    {
                        fieldName.Add("MaBP");
                    }
                }
                if (!(_TransRow["MaSP"] is DBNull))
                {
                    if (bool.Parse(_TransRow["MaSP"].ToString()))
                    {
                        if (!(dr["MaSP"] is DBNull))
                        {
                            fieldName.Add("MaSP");
                        }
                    }
                }

                if (!(_TransRow["MaCongtrinh"] is DBNull))
                {
                    if (bool.Parse(_TransRow["MaCongtrinh"].ToString()))
                    {
                        if (!(dr["MaCongtrinh"] is DBNull))
                        {
                            fieldName.Add("MaCongtrinh");
                        }
                    }
                }
                if (!(_TransRow["MaCN"] is DBNull))
                {
                    if (bool.Parse(_TransRow["MaCN"].ToString()))
                    {
                        if (!(dr["MaCN"] is DBNull))
                        {
                            fieldName.Add("MaCN");
                        }
                    }
                }
                Values.Add("convert( uniqueidentifier,'" + id.ToString() + "')");
                Values.Add("'KC'");
                Values.Add("'PKC'");
                Values.Add("'" + _TransRow["MaCT"].ToString() + "/" + _ngayCt2.Month.ToString() + "'");
                Values.Add("cast('" + _ngayCt2.ToString() + "' as datetime)");
                //Values.Add("''");
                Values.Add("''");
                Values.Add("N' " + _TransRow["DienGiai"] + " tháng " + _ngayCt2.Month.ToString() + "\\" + _ngayCt2.Year.ToString() + "'");
                if (double.Parse(dr["Sodu"].ToString())>0)
                {
                    Values.Add("'" + _TransRow["TKDich"].ToString() + "'");
                    Values.Add("'" + dr["Tk"].ToString() + "'");
                }
                else
                {
                    Values.Add("'" + dr["Tk"].ToString() + "'");
                    Values.Add("'" + _TransRow["TKDich"].ToString() + "'");
                }
                Values.Add(double.Parse(dr["Sodu"].ToString()).ToString("###########0.######").Replace("-", ""));
                Values.Add("0");

                if (bool.Parse(_TransRow["Maphi"].ToString()))
                {
                    if (!(dr["MaPhi"] is DBNull))
                    {
                        Values.Add("'" + dr["MaPhi"].ToString() + "'");
                    }
                }
                if (bool.Parse(_TransRow["MaVV"].ToString()))
                {
                    if (!(dr["MaVV"] is DBNull))
                    {
                        Values.Add("'" + dr["MaVV"].ToString() + "'");
                    }
                }
                if (bool.Parse(_TransRow["MaBP"].ToString()))
                {
                    if (!(dr["MaBP"] is DBNull))
                    {
                        Values.Add("'" + dr["MaBP"].ToString() + "'");
                    }
                }
                if (!(_TransRow["MaSP"] is DBNull))
                {
                    if (bool.Parse(_TransRow["MaSP"].ToString()))
                    {
                        if (!(dr["MaSP"] is DBNull))
                        {
                            Values.Add("'" + dr["MaSP"].ToString() + "'");
                        }
                    }
                }

                if (!(_TransRow["MaCongtrinh"] is DBNull))
                {
                    if (bool.Parse(_TransRow["MaCongtrinh"].ToString()))
                    {
                        if (!(dr["MaCongtrinh"] is DBNull))
                        {
                            Values.Add("'" + dr["MaCongtrinh"].ToString() + "'");
                        }
                    }
                }

                if (!(_TransRow["MaCN"] is DBNull))
                {
                    if (bool.Parse(_TransRow["MaCN"].ToString()))
                    {
                        if (!(dr["MaCN"] is DBNull))
                        {
                            Values.Add("'" + dr["MaCN"].ToString() + "'");
                        }
                    }
                }
                if (!_dbData.insertRow(tableName, fieldName, Values))
                {
                    return false;
                }
                Values.Clear();
                Values.Add("convert( uniqueidentifier,'" + id.ToString() + "')");
                Values.Add("'KC'");
                Values.Add("'PKC'");
                Values.Add("'" + _TransRow["MaCT"].ToString() + "/" + _ngayCt2.Month.ToString() + "'");
                Values.Add("cast('" + _ngayCt2.ToString() + "' as datetime)");
                //Values.Add("''");
                Values.Add("''");
                Values.Add("N' " + _TransRow["DienGiai"] + " tháng " + _ngayCt2.Month.ToString() + "\\" + _ngayCt2.Year.ToString() + "'");
                if (double.Parse(dr["Sodu"].ToString())>0)
                {

                    Values.Add("'" + dr["Tk"].ToString() + "'");
                    Values.Add("'" + _TransRow["TKDich"].ToString() + "'");
                }
                else
                {
                    Values.Add("'" + _TransRow["TKDich"].ToString() + "'");
                    Values.Add("'" + dr["Tk"].ToString() + "'");
                }

                Values.Add("0");
                Values.Add(double.Parse(dr["Sodu"].ToString()).ToString("###########0.######").Replace("-", ""));

                if (bool.Parse(_TransRow["Maphi"].ToString()))
                {
                    if (!(dr["MaPhi"] is DBNull))
                    {
                        Values.Add("'" + dr["MaPhi"].ToString() + "'");
                    }
                }
                if (bool.Parse(_TransRow["MaVV"].ToString()))
                {
                    if (!(dr["MaVV"] is DBNull))
                    {
                        Values.Add("'" + dr["MaVV"].ToString() + "'");
                    }
                }
                if (bool.Parse(_TransRow["MaBP"].ToString()))
                {
                    if (!(dr["MaBP"] is DBNull))
                    {
                        Values.Add("'" + dr["MaBP"].ToString() + "'");
                    }
                }
                if (!(_TransRow["MaSP"] is DBNull))
                {
                    if (bool.Parse(_TransRow["MaSP"].ToString()))
                    {
                        if (!(dr["MaSP"] is DBNull))
                        {
                            Values.Add("'" + dr["MaSP"].ToString() + "'");
                        }
                    }
                }

                if (!(_TransRow["MaCongtrinh"] is DBNull))
                {
                    if (bool.Parse(_TransRow["MaCongtrinh"].ToString()))
                    {
                        if (!(dr["MaCongtrinh"] is DBNull))
                        {
                            Values.Add("'" + dr["MaCongtrinh"].ToString() + "'");
                        }
                    }
                }
                if (!(_TransRow["MaCN"] is DBNull))
                {
                    if (bool.Parse(_TransRow["MaCN"].ToString()))
                    {
                        if (!(dr["MaCN"] is DBNull))
                        {
                            Values.Add("'" + dr["MaCN"].ToString() + "'");
                        }
                    }
                }
                if (!_dbData.insertRow(tableName, fieldName, Values))
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;

            } 
            return true;
        }
        public bool DeleteBt()
        {
            string sql = "delete bltk where nhomdk='KC' and  NgayCt between cast('" + _ngayCt1.ToString() + "' as datetime) and cast('" + _ngayCt2.ToString() + "' as datetime) and SoCT like '" + _TransRow["MaCT"].ToString() + "%'";
            //MessageBox.Show(sql);
            _dbData.UpdateByNonQuery(sql);
            return true;
        }
    }
}
