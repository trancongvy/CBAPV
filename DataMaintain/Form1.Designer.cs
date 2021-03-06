namespace DataMaintain
{
    partial class Form1
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
            this.dtTuNgay = new DevExpress.XtraEditors.DateEdit();
            this.dtDenNgay = new DevExpress.XtraEditors.DateEdit();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.tePackageName = new DevExpress.XtraEditors.TextEdit();
            this.ceDeleteOnly = new DevExpress.XtraEditors.CheckEdit();
            this.tMtName = new DevExpress.XtraEditors.TextEdit();
            this.tCondition = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.dtTuNgay.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTuNgay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDenNgay.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDenNgay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePackageName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceDeleteOnly.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tMtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tCondition.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // dtTuNgay
            // 
            this.dtTuNgay.EditValue = null;
            this.dtTuNgay.Location = new System.Drawing.Point(310, 121);
            this.dtTuNgay.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.dtTuNgay.Name = "dtTuNgay";
            this.dtTuNgay.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.dtTuNgay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtTuNgay.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtTuNgay.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtTuNgay.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtTuNgay.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtTuNgay.Size = new System.Drawing.Size(200, 32);
            this.dtTuNgay.TabIndex = 1;
            // 
            // dtDenNgay
            // 
            this.dtDenNgay.EditValue = null;
            this.dtDenNgay.Location = new System.Drawing.Point(786, 121);
            this.dtDenNgay.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.dtDenNgay.Name = "dtDenNgay";
            this.dtDenNgay.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.dtDenNgay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtDenNgay.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtDenNgay.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtDenNgay.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtDenNgay.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtDenNgay.Size = new System.Drawing.Size(200, 32);
            this.dtDenNgay.TabIndex = 2;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(260, 365);
            this.btnOk.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(150, 44);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(524, 365);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 44);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(96, 127);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(84, 25);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "Từ ngày:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(572, 135);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(97, 25);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "Đến ngày:";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(96, 54);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(145, 25);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "Gói phần mềm:";
            // 
            // tePackageName
            // 
            this.tePackageName.Location = new System.Drawing.Point(310, 38);
            this.tePackageName.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tePackageName.Name = "tePackageName";
            this.tePackageName.Size = new System.Drawing.Size(200, 32);
            this.tePackageName.TabIndex = 0;
            // 
            // ceDeleteOnly
            // 
            this.ceDeleteOnly.Location = new System.Drawing.Point(306, 298);
            this.ceDeleteOnly.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.ceDeleteOnly.Name = "ceDeleteOnly";
            this.ceDeleteOnly.Properties.Caption = "Chỉ xóa (dùng trong đồng bộ)";
            this.ceDeleteOnly.Size = new System.Drawing.Size(418, 30);
            this.ceDeleteOnly.TabIndex = 3;
            this.ceDeleteOnly.Visible = false;
            // 
            // tMtName
            // 
            this.tMtName.Location = new System.Drawing.Point(310, 171);
            this.tMtName.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tMtName.Name = "tMtName";
            this.tMtName.Size = new System.Drawing.Size(200, 32);
            this.tMtName.TabIndex = 8;
            this.tMtName.EditValueChanged += new System.EventHandler(this.tMtName_EditValueChanged);
            // 
            // tCondition
            // 
            this.tCondition.Location = new System.Drawing.Point(310, 221);
            this.tCondition.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tCondition.Name = "tCondition";
            this.tCondition.Size = new System.Drawing.Size(676, 32);
            this.tCondition.TabIndex = 9;
            this.tCondition.EditValueChanged += new System.EventHandler(this.tCondition_EditValueChanged);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(96, 185);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(145, 25);
            this.labelControl4.TabIndex = 10;
            this.labelControl4.Text = "Chạy trên bảng";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(182, 227);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(88, 25);
            this.labelControl5.TabIndex = 11;
            this.labelControl5.Text = "Điều kiện";
            // 
            // Form1
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1002, 452);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.tCondition);
            this.Controls.Add(this.tMtName);
            this.Controls.Add(this.ceDeleteOnly);
            this.Controls.Add(this.tePackageName);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.dtDenNgay);
            this.Controls.Add(this.dtTuNgay);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Bảo trì số liệu";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtTuNgay.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTuNgay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDenNgay.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDenNgay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePackageName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceDeleteOnly.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tMtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tCondition.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.DateEdit dtTuNgay;
        private DevExpress.XtraEditors.DateEdit dtDenNgay;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit tePackageName;
        private DevExpress.XtraEditors.CheckEdit ceDeleteOnly;
        private DevExpress.XtraEditors.TextEdit tMtName;
        private DevExpress.XtraEditors.TextEdit tCondition;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
    }
}

