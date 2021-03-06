using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using CDTDatabase;
using Formula;

namespace Inventory
{
    class updateCongthuc
    {
        public string mt;
        public string dt;
        private string mtName;
        private string dtName;
        private string giaNTfield;
        string ngayctField;
        private DateTime Tungay;
        private DateTime Denngay;
        DataTable Struct;
        List<string> CalculatedField=new List<string>();
        List<string> lstSubFormuar=new List<string>();
        List<string> lstham = new List<string>();
        private Database _dbData  =Database.NewDataDatabase();//"server = vuonghuynh; database = CBA11; user = sa; pwd = sa");
        private Database _dbStruct = Database.NewStructDatabase();//"server = vuonghuynh; database = CDT; user = sa; pwd = sa");
        public string makhoApgia;
        public updateCongthuc(Database dbData, string MT, string DT,string GiaNTField,DateTime tungay,DateTime denngay,string ngayCtField)
        {
            _dbData = dbData;
            mt = MT;
            dt = DT;
            Tungay=tungay;
            Denngay = denngay;
            mtName = _dbStruct.GetValue("select tableName from systable where systableid=" + mt).ToString();
            dtName = _dbStruct.GetValue("select tableName from systable where systableid=" + dt).ToString();
            ngayctField = ngayCtField;
            giaNTfield = GiaNTField;
            Struct = GetStruct();
        }
        private DataTable GetStruct()
        {
            string sql = "select systableID,fieldName, Formula from sysfield where systableID=" + mt + " or systableID= " + dt;
            return _dbStruct.GetDataTable(sql);

        }
        public void Update()
        {
            this.updateMTDTCongthuc(giaNTfield.ToUpper());
            this.updateBl();     

        }
        public List<string> LField = new List<string>();
        private void updateMTDTCongthuc(string ChangedField)
        {
            if (!LField.Contains(ChangedField))
                LField.Add(ChangedField);
            else
                return;
            DataRow[] lstField=Struct.Select("Formula like '*" + ChangedField + "*'");
            List<string> lstBien;            
            string varField;
            string sql;
            string formularSql;
            string mtPk = mtName + "ID";
            string dtRefPK = mtPk;

            sql = " select Pk from systable where tableName='" + mtName + "'";
            DataTable Ttmp = _dbStruct.GetDataTable(sql);
            if (Ttmp.Rows.Count > 0) mtPk = Ttmp.Rows[0]["Pk"].ToString();
            sql = "select fieldName from sysfield where  reftable='" + mtName + "' and sysTableID= (select systableID from systable where tableName='" + dtName + "')";
            Ttmp = _dbStruct.GetDataTable(sql);
            if (Ttmp.Rows.Count > 0) dtRefPK = Ttmp.Rows[0]["fieldName"].ToString();
            foreach (DataRow drv in lstField)//duyệt qua các trường có biến là trường đang thay đổi
            {
                formularSql=drv["Formula"].ToString().ToUpper();
               
                lstBien = getLstBien(drv["Formula"].ToString());
                if ((lstBien.Contains(ChangedField.ToUpper())))
                {
                    foreach (string fVar in lstBien)
                    {
                        varField = Struct.Select("fieldName='" + fVar + "'")[0]["systableID"].ToString();
                        if (varField == mt)
                        {
                            varField = "b." + fVar.Trim();//Nếu biến là trường thuộc bảng mt thì b, mt alias b                        
                        }
                        else
                        {
                            varField = "a." + fVar.Trim();//Nếu biến là trường thuộc bảng dt thì b, dt alias b
                        }
                        formularSql = formularSql.Replace("@" + fVar.Trim(), varField);
                    }

                    varField = Struct.Select("fieldName='" + drv["fieldName"].ToString() + "'")[0]["systableID"].ToString();

                    //Tạo câu sql update
                    string whereSql;

                    if (varField == dt)//Nếu trường cần tính là trường trong bảng dt
                    {
                       
                        whereSql = " from " + dtName + " a," + mtName + " b  where a." + dtRefPK + " = b." + mtPk + " and ";
                        whereSql += "b." + ngayctField + " between cast('" + Tungay.ToString() + "' as datetime) and cast('" + Denngay.ToString() + "' as datetime)";
                        sql = "update " + dtName + " set ";
                        sql += drv["fieldName"].ToString() + " = " + formularSql;
                        sql += whereSql;
                    }
                    else
                    {
                        formularSql = formatFormular(formularSql);
                        sql = "update " + mtName + " set ";
                        sql += drv["fieldName"].ToString() + " = " + formularSql;
                        sql += "  ( " + ngayctField + " between cast('" + Tungay.ToString();
                        sql += "' as datetime) and cast('" + Denngay.ToString() + "' as datetime))";                        
                        //MessageBox.Show(sql);
                    }
                    
                    _dbData.UpdateByNonQuery(sql);                    
                }
            }
            foreach (DataRow drv in lstField)//duyệt qua các trường có biến là trường đang thay đổi
            {
                formularSql = drv["Formula"].ToString().ToUpper();
                lstBien = getLstBien(drv["Formula"].ToString());
                if ((lstBien.Contains(ChangedField.ToUpper())))
                { 
                    updateMTDTCongthuc(drv["fieldName"].ToString().ToUpper());
                }
            }
        }


        private void updateBl()
        {

        }
        
        private string formatFormular(string FormularSql)
        {
            lstSubFormuar.Clear();
            lstham.Clear();            
            getSubFormular(FormularSql);
            if (lstSubFormuar.Count==0)
            {
                return FormularSql + " from " + mtName + " b where ";
            }
            for (int i=0; i < lstSubFormuar.Count; i++)
            {
                FormularSql = FormularSql.Replace(lstham[i] + "(" + lstSubFormuar[i] + ")", "x" + i.ToString() + ".x");
            }
            FormularSql += " from " + mtName + " b ";
            for (int i = 0; i < lstSubFormuar.Count; i++)
            {
                FormularSql += ",( select " + lstham[i] + "(" + lstSubFormuar[i] + ") as x," + mtName.Trim() + "id";
                FormularSql += " from " + dtName + " a group by " + mtName.Trim() + "id) x" + i.ToString();
            }
            FormularSql += " where ";
            for (int i = 0; i < lstSubFormuar.Count; i++)
            {
                FormularSql += " b." + mtName.Trim() + "id = x" + i.ToString() + "." + mtName.Trim() + "id and ";
            }
            return FormularSql;
        }
        
        private void getSubFormular(string FormularSql)
        {
            int i;
            int j;
            int daumo = 0;
            string ham;
            string subFormular;
            i = FormularSql.IndexOf("SUM(");
            ham = "SUM";
            if (i < 0)
            {
                i = FormularSql.IndexOf("ABS(");
                ham = "ABS";
            }
            if (i >= 0)
            {
                for (j = i + 1; j < FormularSql.Length; j++)
                {
                    if (FormularSql.Substring(j, 1) == "(")
                    {
                        daumo = daumo + 1;
                    }
                    else if (FormularSql.Substring(j, 1) == ")")
                    {
                        daumo = daumo - 1;
                        if (daumo == 0)
                        {
                            subFormular = FormularSql.Substring(i + 4, j - i-4);
                            FormularSql=FormularSql.Replace(ham + "(" + subFormular + ")", "");
                            lstSubFormuar.Add(subFormular);
                            lstham.Add(ham);
                            getSubFormular(FormularSql);
                            break;
                        }
                    }
                }
            }
        }
        private List<string> getLstBien(string Formular)
        {
            if (Formular.Contains("ROUND("))
                Formular = Formular.Substring(6, Formular.Length - 9);
            BieuThuc a = new Formula.BieuThuc(Formular);
            return a.variables;
        }
    }
}
