using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using CusCDTData;

using DataFactory;
using System.Windows.Forms;
using DevExpress;
using DevExpress.XtraGrid;
using DevExpress.XtraLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevControls;
namespace CusPiriou
{
    public class CusPiriou : CusCDTData.ICDTData
    {

        public event EventHandler Refresh;
        CDTData _data;
        DevExpress.XtraGrid.GridControl _gc;
        DevExpress.XtraGrid.Views.Grid.GridView _gv;

        List<DevExpress.XtraLayout.LayoutControlItem> _lo;
        List<DevExpress.XtraEditors.BaseEdit> _be;
        List<CDTGridLookUpEdit> _glist;
        List<CDTRepGridLookup> _rlist;
        List<DevExpress.XtraGrid.GridControl> _gridList;
        bool isAddedEvent = false;
        string _Name;
        void CusData_ColumnChanged(object sender, System.Data.DataColumnChangeEventArgs e)
        {

            switch (e.Column.Table.TableName.ToLower())
            {
                case "dmvt":
                    {
                        string mtgroup = "";

                        string subgroup = "";
                        string type = "";
                        string standard = "";
                        string grade = "";
                        string material = "";

                        List<string> lst = new List<string>() { "mtgroup", "subgroup", "type", "standard", "grade", "material", "numcode" };
                        if (lst.Contains(e.Column.ColumnName.ToLower()))
                        {
                            foreach (CDTGridLookUpEdit g in _glist)
                            {
                                if (g.Data.DsData.Tables[0].PrimaryKey.Length == 0)
                                {
                                    g.Data.DsData.Tables[0].PrimaryKey = new DataColumn[] { g.Data.DsData.Tables[0].Columns[g.Data.DrTable["pk"].ToString()] };
                                }
                                DataRow dr;

                                switch (g.fieldName.ToLower())
                                {
                                    case "mtgroup":
                                        dr = g.Data.DsData.Tables[0].Rows.Find(e.Row["mtgroup"].ToString());
                                        if (dr != null)
                                            mtgroup = dr["NhomVTCode"].ToString();
                                        break;
                                    case "subgroup":
                                        dr = g.Data.DsData.Tables[0].Rows.Find(e.Row["subgroup"].ToString());
                                        if (dr != null)
                                            subgroup = dr["NhomSubCode"].ToString(); break;     
                                    case "type":
                                        dr = g.Data.DsData.Tables[0].Rows.Find(e.Row["type"].ToString());
                                        if (dr != null)
                                            type = dr["typecode"].ToString(); break;
                                    case "standard":
                                        dr = g.Data.DsData.Tables[0].Rows.Find(e.Row["standard"].ToString());
                                        if (dr != null)
                                            standard = dr["Stancode"].ToString(); break;
                                    case "grade":
                                        dr = g.Data.DsData.Tables[0].Rows.Find(e.Row["grade"].ToString());
                                        if (dr != null)
                                            grade = dr["GradeName"].ToString(); break;
                                    case "material":
                                        dr = g.Data.DsData.Tables[0].Rows.Find(e.Row["material"].ToString());
                                        if (dr != null)
                                            material = dr["MaterialCode"].ToString(); break;
                                }
                            }
                            e.Row["TenVT"] = mtgroup + subgroup + type + material +standard+ grade;

                            string numcode = "";
                            if (e.Row["Numcode"] != DBNull.Value) numcode = e.Row["Numcode"].ToString();
                            string tenvt = "";
                            if (e.Row["TenVT"] != DBNull.Value) tenvt = e.Row["TenVT"].ToString();
                            e.Row["MaVT"] = tenvt + numcode;
                            e.Row.EndEdit();
                        }
                        




                    }
                    break;
            }
            //ValidMtID(e);


        }

      

        


        #region ICDTData Members

        public void AddEvent()
        {
            if (isAddedEvent) return;
            _data.DsData.Tables[0].ColumnChanged += new System.Data.DataColumnChangeEventHandler(CusData_ColumnChanged);
            //_data.DsData.Tables[0].ColumnChanging += new DataColumnChangeEventHandler(CusData_ColumnChanging);

            foreach(CDTGridLookUpEdit gl in _glist)
                gl.Validated += gl_Validated;
            isAddedEvent = true;
        }

        void gl_Validated(object sender, EventArgs e)
        {
            
        }



        private void CusData1_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
           

        }

        private void UpdateTonkho(DataRow dataRow)
        {
        }

        private void UpdateGiaBan(DataRow dr)//Khải hoàng
        {
        
        }

        private void CusData1_ColumnChanging(object sender, DataColumnChangeEventArgs e)
        {
          
        }



        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }


        public List<BaseEdit> be
        {
            get
            {
                return _be;
            }
            set
            {
                _be = value;
            }
        }

        public CDTData data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        public GridControl gc
        {
            get
            {
                return _gc;
            }
            set
            {
                _gc = value;
            }
        }
        public List<GridControl> gridList
        {
            get
            {
                return _gridList;
            }
            set
            {
                _gridList = value;
            }
        }
        public List<CDTGridLookUpEdit> glist
        {
            get
            {
                return _glist;
            }
            set
            {
                _glist = value;
            }
        }

        public DevExpress.XtraGrid.Views.Grid.GridView gv
        {
            get
            {
                return _gv;
            }
            set
            {
                _gv = value;
            }
        }

        public List<LayoutControlItem> lo
        {
            get
            {
                return _lo;
            }
            set
            {
                _lo = value;
            }
        }

        public List<CDTRepGridLookup> rlist
        {
            get
            {
                return _rlist;
            }
            set
            {
                _rlist = value;
            }
        }

        #endregion
    }
}

