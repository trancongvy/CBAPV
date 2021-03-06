namespace Relax
{
    partial class fInve
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gMaDV = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.sSoluong = new DevExpress.XtraEditors.SpinEdit();
            this.btIn = new DevExpress.XtraEditors.SimpleButton();
            this.tMaKH = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.tTien = new DevExpress.XtraEditors.TextEdit();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.tDiem = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.tSothe = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gMaDV.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sSoluong.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tMaKH.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tTien.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tDiem.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tSothe.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gMaDV
            // 
            this.gMaDV.EditValue = "";
            this.gMaDV.EnterMoveNextControl = true;
            this.gMaDV.Location = new System.Drawing.Point(85, 28);
            this.gMaDV.Name = "gMaDV";
            this.gMaDV.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gMaDV.Properties.NullText = "";
            this.gMaDV.Properties.ValueMember = "MaDV";
            this.gMaDV.Properties.View = this.gridLookUpEdit1View;
            this.gMaDV.Size = new System.Drawing.Size(187, 20);
            this.gMaDV.TabIndex = 0;
            this.gMaDV.EditValueChanged += new System.EventHandler(this.gMaDV_EditValueChanged);
            // 
            // gridLookUpEdit1View
            // 
            this.gridLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2});
            this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
            this.gridLookUpEdit1View.OptionsBehavior.Editable = false;
            this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpEdit1View.OptionsView.EnableAppearanceEvenRow = true;
            this.gridLookUpEdit1View.OptionsView.EnableAppearanceOddRow = true;
            this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Mã Dịch vụ";
            this.gridColumn1.FieldName = "MaDV";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Tên dịch vụ";
            this.gridColumn2.FieldName = "TenDV";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // sSoluong
            // 
            this.sSoluong.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.sSoluong.EnterMoveNextControl = true;
            this.sSoluong.Location = new System.Drawing.Point(85, 70);
            this.sSoluong.Name = "sSoluong";
            this.sSoluong.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.sSoluong.Size = new System.Drawing.Size(100, 20);
            this.sSoluong.TabIndex = 1;
            // 
            // btIn
            // 
            this.btIn.Location = new System.Drawing.Point(110, 251);
            this.btIn.Name = "btIn";
            this.btIn.Size = new System.Drawing.Size(75, 23);
            this.btIn.TabIndex = 4;
            this.btIn.Text = "In";
            this.btIn.Click += new System.EventHandler(this.btIn_Click);
            // 
            // tMaKH
            // 
            this.tMaKH.Location = new System.Drawing.Point(85, 211);
            this.tMaKH.Name = "tMaKH";
            this.tMaKH.Properties.ReadOnly = true;
            this.tMaKH.Size = new System.Drawing.Size(175, 20);
            this.tMaKH.TabIndex = 3;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 31);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(42, 13);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "Dịch vụ: ";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 73);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(42, 13);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "Số lượng";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(12, 214);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(56, 13);
            this.labelControl3.TabIndex = 6;
            this.labelControl3.Text = "Khách hàng";
            // 
            // checkEdit1
            // 
            this.checkEdit1.Location = new System.Drawing.Point(185, 190);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "Ghi nợ";
            this.checkEdit1.Properties.ReadOnly = true;
            this.checkEdit1.Size = new System.Drawing.Size(75, 19);
            this.checkEdit1.TabIndex = 30;
            // 
            // tTien
            // 
            this.tTien.EditValue = "0";
            this.tTien.Location = new System.Drawing.Point(85, 138);
            this.tTien.Name = "tTien";
            this.tTien.Properties.DisplayFormat.FormatString = "### ### ##0";
            this.tTien.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.tTien.Properties.EditFormat.FormatString = "### ### ##0";
            this.tTien.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.tTien.Properties.ReadOnly = true;
            this.tTien.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tTien.Size = new System.Drawing.Size(175, 20);
            this.tTien.TabIndex = 5;
            // 
            // radioGroup1
            // 
            this.radioGroup1.EditValue = false;
            this.radioGroup1.Location = new System.Drawing.Point(45, 101);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(true, "Điểm"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(false, "Tiền mặt")});
            this.radioGroup1.Size = new System.Drawing.Size(239, 21);
            this.radioGroup1.TabIndex = 2;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(12, 141);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(51, 13);
            this.labelControl4.TabIndex = 10;
            this.labelControl4.Text = "Thành tiền";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(12, 167);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(55, 13);
            this.labelControl5.TabIndex = 12;
            this.labelControl5.Text = "Thành điểm";
            // 
            // tDiem
            // 
            this.tDiem.EditValue = "0";
            this.tDiem.Location = new System.Drawing.Point(85, 164);
            this.tDiem.Name = "tDiem";
            this.tDiem.Properties.DisplayFormat.FormatString = "### ### ##0";
            this.tDiem.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.tDiem.Properties.EditFormat.FormatString = "### ### ##0";
            this.tDiem.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.tDiem.Properties.ReadOnly = true;
            this.tDiem.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tDiem.Size = new System.Drawing.Size(175, 20);
            this.tDiem.TabIndex = 6;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(197, 251);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 13;
            this.simpleButton1.Text = "Sửa mẫu";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(27, 191);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(31, 13);
            this.labelControl6.TabIndex = 23;
            this.labelControl6.Text = "Số thẻ";
            // 
            // tSothe
            // 
            this.tSothe.EnterMoveNextControl = true;
            this.tSothe.Location = new System.Drawing.Point(85, 188);
            this.tSothe.Name = "tSothe";
            this.tSothe.Size = new System.Drawing.Size(100, 20);
            this.tSothe.TabIndex = 3;
            // 
            // fInve
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 286);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.tSothe);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.tDiem);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.radioGroup1);
            this.Controls.Add(this.tTien);
            this.Controls.Add(this.checkEdit1);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.tMaKH);
            this.Controls.Add(this.btIn);
            this.Controls.Add(this.sSoluong);
            this.Controls.Add(this.gMaDV);
            this.KeyPreview = true;
            this.Name = "fInve";
            this.Text = "In vé";
            this.Load += new System.EventHandler(this.fInve_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gMaDV.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sSoluong.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tMaKH.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tTien.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tDiem.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tSothe.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.GridLookUpEdit gMaDV;
        private DevExpress.XtraGrid.Views.Grid.GridView gridLookUpEdit1View;
        private DevExpress.XtraEditors.SpinEdit sSoluong;
        private DevExpress.XtraEditors.SimpleButton btIn;
        private DevExpress.XtraEditors.TextEdit tMaKH;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraEditors.TextEdit tTien;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit tDiem;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.TextEdit tSothe;
    }
}