﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataFactory;
using CDTDatabase;
using FormFactory;
using CDTLib;
using DevExpress.XtraGrid;
namespace Piriou
{
    public partial class fPOMuber : Form
    {
        public fPOMuber()
        {
            InitializeComponent();
        }
        DataTable PurList;
        DataTable tb;
        Database db = Database.NewDataDatabase();
        Database dbStr = Database.NewStructDatabase();
        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void fPurchaser_Load(object sender, EventArgs e)
        {
            string sql = "select MaKH, TenKH, Department, Leve from dmkh where Department='Procurement' ";
            PurList = db.GetDataTable(sql);
            gridLookUpEdit1.Properties.DataSource = PurList;
            gridLookUpEdit1.Properties.ValueMember = "MaKH";
            gridLookUpEdit1.Properties.DisplayMember = "TenKH";
            gridLookUpEdit1.EditValue = Config.GetValue("FullName");
        }

        private void tbGet_Click(object sender, EventArgs e)
        {
            if (gridLookUpEdit1.EditValue == null) return;
            tb = db.GetDataSetByStore("GetPROPoNumber", new string[] {"@puric" }, new object[] { gridLookUpEdit1.EditValue.ToString()});
            if (tb == null) return;
            gridControl1.DataSource = tb;
        }



        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (tPONo.Text == string.Empty) return;
            int[] i = gridView1.GetSelectedRows();
            foreach (int j in i)
            {
                DataRow dr = gridView1.GetDataRow(j);
                if (tPONo.Text.Trim() != string.Empty)
                    dr["PoNo"] = tPONo.Text;
                
                dr.EndEdit();
            }
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            db.BeginMultiTrans();
            bool r = true;
            try
            {
                foreach (DataRow dr in tb.Rows)
                {
                    if (dr["Pono"] != DBNull.Value && dr["Pono"].ToString().Trim()!=string.Empty)
                    {
                        string upsql = "update dt29 set ";
                        upsql += " PONo='" + dr["PONo"].ToString() + "'";
                        upsql += " where dt29id='" + dr["DT29ID"].ToString() + "'";
                        db.UpdateByNonQuery(upsql);
                        if (db.HasErrors)
                        {
                            db.RollbackMultiTrans();
                            MessageBox.Show("Can not update, error occur");
                            return;
                        }
                    }
                    if (dr["Provider"] != DBNull.Value && dr["Pono"].ToString().Trim() != string.Empty)
                    {
                        string upsql = "update dt29 set ";
                        upsql += " Provider='" + dr["Provider"].ToString() + "'";
                        upsql += " where dt29id='" + dr["DT29ID"].ToString() + "'";
                        db.UpdateByNonQuery(upsql);
                        if (db.HasErrors)
                        {
                            db.RollbackMultiTrans();
                            MessageBox.Show("Can not update, error occur");
                            return;
                        }
                    }
                    if (dr["QtyDiff"] != DBNull.Value && dr["QtyDiff"].ToString().Trim() != string.Empty )
                    {
                        string upsql = "update dt29 set ";
                        upsql += " QtyDiff='" + dr["QtyDiff"].ToString() + "'";
                        upsql += " where dt29id='" + dr["DT29ID"].ToString() + "'";
                        db.UpdateByNonQuery(upsql);
                        if (db.HasErrors)
                        {
                            db.RollbackMultiTrans();
                            MessageBox.Show("Can not update, error occur");
                            return;
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Can not update, error occur");
                r = false;
            }
            finally
            {
                if (r) db.EndMultiTrans();
                else db.RollbackMultiTrans();
            }
            tbGet_Click(sender, e);
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            if (tPONo.Text == string.Empty) return;
            int[] i = gridView1.GetSelectedRows();
            foreach (int j in i)
            {
                DataRow dr = gridView1.GetDataRow(j);
                if (tProvider.Text.Trim() != string.Empty)
                    dr["Provider"] = tProvider.Text;
                dr.EndEdit();
            }
        }
    }
}
