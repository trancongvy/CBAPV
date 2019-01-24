namespace QLSX
{
    partial class fTangca
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
            this.dNgay = new DevExpress.XtraEditors.DateEdit();
            this.sGio = new DevExpress.XtraEditors.SpinEdit();
            this.ckLunch = new DevExpress.XtraEditors.CheckEdit();
            this.ckNight = new DevExpress.XtraEditors.CheckEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.gMaMin = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.ckAll = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.dNgay.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dNgay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sGio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckLunch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckNight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gMaMin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckAll.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // dNgay
            // 
            this.dNgay.EditValue = null;
            this.dNgay.Location = new System.Drawing.Point(84, 22);
            this.dNgay.Name = "dNgay";
            this.dNgay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dNgay.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dNgay.Size = new System.Drawing.Size(100, 20);
            this.dNgay.TabIndex = 0;
            this.dNgay.EditValueChanged += new System.EventHandler(this.dNgay_EditValueChanged);
            // 
            // sGio
            // 
            this.sGio.EditValue = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.sGio.Location = new System.Drawing.Point(84, 59);
            this.sGio.Name = "sGio";
            this.sGio.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.sGio.Size = new System.Drawing.Size(100, 20);
            this.sGio.TabIndex = 1;
            // 
            // ckLunch
            // 
            this.ckLunch.Location = new System.Drawing.Point(84, 86);
            this.ckLunch.Name = "ckLunch";
            this.ckLunch.Properties.Caption = "Buổi trưa";
            this.ckLunch.Size = new System.Drawing.Size(75, 19);
            this.ckLunch.TabIndex = 2;
            this.ckLunch.CheckedChanged += new System.EventHandler(this.checkEdit1_CheckedChanged);
            // 
            // ckNight
            // 
            this.ckNight.EditValue = true;
            this.ckNight.Location = new System.Drawing.Point(84, 112);
            this.ckNight.Name = "ckNight";
            this.ckNight.Properties.Caption = "Buổi tối";
            this.ckNight.Size = new System.Drawing.Size(75, 19);
            this.ckNight.TabIndex = 3;
            this.ckNight.CheckedChanged += new System.EventHandler(this.checkEdit2_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Ngày";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Số giờ";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(84, 164);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(96, 23);
            this.simpleButton1.TabIndex = 6;
            this.simpleButton1.Text = "OK";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // gMaMin
            // 
            this.gMaMin.EditValue = "";
            this.gMaMin.Location = new System.Drawing.Point(86, 138);
            this.gMaMin.Name = "gMaMin";
            this.gMaMin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gMaMin.Properties.NullText = "";
            this.gMaMin.Properties.View = this.gridLookUpEdit1View;
            this.gMaMin.Size = new System.Drawing.Size(113, 20);
            this.gMaMin.TabIndex = 7;
            // 
            // gridLookUpEdit1View
            // 
            this.gridLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2});
            this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
            this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            this.gMaMin.Properties.DisplayMember = "TenMIn";
            this.gMaMin.Properties.ValueMember = "MaMIn";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Mã máy";
            this.gridColumn1.FieldName = "MaMIn";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Tên máy";
            this.gridColumn2.FieldName = "TenMIn";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Máy in";
            // 
            // ckAll
            // 
            this.ckAll.Location = new System.Drawing.Point(205, 138);
            this.ckAll.Name = "ckAll";
            this.ckAll.Properties.Caption = "All";
            this.ckAll.Size = new System.Drawing.Size(64, 19);
            this.ckAll.TabIndex = 9;
            this.ckAll.CheckedChanged += new System.EventHandler(this.chAll_CheckedChanged);
            // 
            // fTangca
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 220);
            this.Controls.Add(this.ckAll);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.gMaMin);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ckNight);
            this.Controls.Add(this.ckLunch);
            this.Controls.Add(this.sGio);
            this.Controls.Add(this.dNgay);
            this.Name = "fTangca";
            this.Text = "Quyết định tăng ca";
            this.Load += new System.EventHandler(this.fTangca_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dNgay.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dNgay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sGio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckLunch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckNight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gMaMin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckAll.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.DateEdit dNgay;
        private DevExpress.XtraEditors.SpinEdit sGio;
        private DevExpress.XtraEditors.CheckEdit ckLunch;
        private DevExpress.XtraEditors.CheckEdit ckNight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.GridLookUpEdit gMaMin;
        private DevExpress.XtraGrid.Views.Grid.GridView gridLookUpEdit1View;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.CheckEdit ckAll;
    }
}