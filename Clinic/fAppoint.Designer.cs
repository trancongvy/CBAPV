namespace QLSX
{
    partial class fAppoint
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tTenKH = new DevExpress.XtraEditors.TextEdit();
            this.dNgayGiao = new DevExpress.XtraEditors.DateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAllDay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtStartDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtStartDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtEndDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtStartTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtEndTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtLabel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtShowTimeAs.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSubject.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtResource.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtResources.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkReminder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbDescription.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbReminder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLocation.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tTenKH.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dNgayGiao.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dNgayGiao.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSubject
            // 
            this.lblSubject.Location = new System.Drawing.Point(8, 14);
            this.lblSubject.Size = new System.Drawing.Size(32, 13);
            this.lblSubject.Text = "Số LSX";
            // 
            // lblLocation
            // 
            this.lblLocation.Location = new System.Drawing.Point(248, 70);
            this.lblLocation.Size = new System.Drawing.Size(42, 13);
            this.lblLocation.Text = "Số lượng";
            // 
            // lblLabel
            // 
            this.lblLabel.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lblLabel.Location = new System.Drawing.Point(2, 10);
            this.lblLabel.Size = new System.Drawing.Size(45, 13);
            this.lblLabel.Text = "Mặt hàng";
            // 
            // lblStartTime
            // 
            this.lblStartTime.Location = new System.Drawing.Point(4, 136);
            this.lblStartTime.Size = new System.Drawing.Size(89, 13);
            this.lblStartTime.Text = "Thời điểm bắt đầu:";
            // 
            // lblEndTime
            // 
            this.lblEndTime.Location = new System.Drawing.Point(7, 163);
            this.lblEndTime.Size = new System.Drawing.Size(87, 13);
            this.lblEndTime.Text = "Thời điểm kết thúc";
            // 
            // lblShowTimeAs
            // 
            this.lblShowTimeAs.Location = new System.Drawing.Point(8, 191);
            this.lblShowTimeAs.Size = new System.Drawing.Size(84, 13);
            this.lblShowTimeAs.Text = "Hiển thị thời gian:";
            // 
            // chkAllDay
            // 
            this.chkAllDay.Location = new System.Drawing.Point(69, 33);
            this.chkAllDay.Visible = false;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(11, 333);
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(99, 333);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(187, 333);
            // 
            // btnRecurrence
            // 
            this.btnRecurrence.Location = new System.Drawing.Point(275, 333);
            this.btnRecurrence.Visible = false;
            this.btnRecurrence.Click += new System.EventHandler(this.btnRecurrence_Click);
            // 
            // edtStartDate
            // 
            this.edtStartDate.EditValue = new System.DateTime(2005, 3, 31, 0, 0, 0, 0);
            this.edtStartDate.Location = new System.Drawing.Point(97, 133);
            this.edtStartDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.edtStartDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.edtStartDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.edtStartDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.edtStartDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            // 
            // edtEndDate
            // 
            this.edtEndDate.EditValue = new System.DateTime(2005, 3, 31, 0, 0, 0, 0);
            this.edtEndDate.Location = new System.Drawing.Point(97, 160);
            this.edtEndDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.edtEndDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.edtEndDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.edtEndDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.edtEndDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            // 
            // edtStartTime
            // 
            this.edtStartTime.EditValue = new System.DateTime(2005, 3, 31, 0, 0, 0, 0);
            this.edtStartTime.Location = new System.Drawing.Point(225, 134);
            // 
            // edtEndTime
            // 
            this.edtEndTime.EditValue = new System.DateTime(2005, 3, 31, 0, 0, 0, 0);
            this.edtEndTime.Location = new System.Drawing.Point(225, 160);
            // 
            // edtLabel
            // 
            this.edtLabel.Location = new System.Drawing.Point(75, 7);
            this.edtLabel.Properties.ReadOnly = true;
            // 
            // edtShowTimeAs
            // 
            this.edtShowTimeAs.Location = new System.Drawing.Point(97, 188);
            // 
            // tbSubject
            // 
            this.tbSubject.Location = new System.Drawing.Point(97, 12);
            this.tbSubject.Properties.ReadOnly = true;
            this.tbSubject.Size = new System.Drawing.Size(130, 20);
            // 
            // edtResource
            // 
            this.edtResource.Location = new System.Drawing.Point(75, 33);
            // 
            // lblResource
            // 
            this.lblResource.Location = new System.Drawing.Point(3, 35);
            this.lblResource.Size = new System.Drawing.Size(31, 13);
            this.lblResource.Text = "Máy in";
            // 
            // edtResources
            // 
            this.edtResources.Location = new System.Drawing.Point(75, 33);
            // 
            // chkReminder
            // 
            this.chkReminder.Location = new System.Drawing.Point(226, 33);
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(8, 221);
            this.tbDescription.Size = new System.Drawing.Size(496, 106);
            // 
            // cbReminder
            // 
            this.cbReminder.Location = new System.Drawing.Point(301, 33);
            // 
            // tbLocation
            // 
            this.tbLocation.Location = new System.Drawing.Point(309, 70);
            this.tbLocation.Properties.DisplayFormat.FormatString = "### ### ##0.##";
            this.tbLocation.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.tbLocation.Properties.EditFormat.FormatString = "### ### ##0.##";
            this.tbLocation.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.tbLocation.Properties.ReadOnly = true;
            this.tbLocation.Size = new System.Drawing.Size(82, 20);
            this.tbLocation.EditValueChanged += new System.EventHandler(this.tbLocation_EditValueChanged_1);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(8, 63);
            this.panel1.Size = new System.Drawing.Size(219, 67);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(245, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "Ngày giao";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Tên khách hàng";
            // 
            // tTenKH
            // 
            this.tTenKH.Location = new System.Drawing.Point(99, 39);
            this.tTenKH.Name = "tTenKH";
            this.tTenKH.Properties.ReadOnly = true;
            this.tTenKH.Size = new System.Drawing.Size(417, 20);
            this.tTenKH.TabIndex = 27;
            // 
            // dNgayGiao
            // 
            this.dNgayGiao.EditValue = null;
            this.dNgayGiao.Location = new System.Drawing.Point(309, 97);
            this.dNgayGiao.Name = "dNgayGiao";
            this.dNgayGiao.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dNgayGiao.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dNgayGiao.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dNgayGiao.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dNgayGiao.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dNgayGiao.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dNgayGiao.Properties.ReadOnly = true;
            this.dNgayGiao.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dNgayGiao.Size = new System.Drawing.Size(100, 20);
            this.dNgayGiao.TabIndex = 28;
            // 
            // fAppoint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 368);
            this.Controls.Add(this.dNgayGiao);
            this.Controls.Add(this.tTenKH);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "fAppoint";
            this.Text = "fAppoint";
            this.Load += new System.EventHandler(this.fAppoint_Load);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.edtShowTimeAs, 0);
            this.Controls.SetChildIndex(this.edtEndTime, 0);
            this.Controls.SetChildIndex(this.edtEndDate, 0);
            this.Controls.SetChildIndex(this.btnRecurrence, 0);
            this.Controls.SetChildIndex(this.btnDelete, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lblShowTimeAs, 0);
            this.Controls.SetChildIndex(this.lblEndTime, 0);
            this.Controls.SetChildIndex(this.tbLocation, 0);
            this.Controls.SetChildIndex(this.lblSubject, 0);
            this.Controls.SetChildIndex(this.lblLocation, 0);
            this.Controls.SetChildIndex(this.tbSubject, 0);
            this.Controls.SetChildIndex(this.lblStartTime, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.edtStartDate, 0);
            this.Controls.SetChildIndex(this.edtStartTime, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.tTenKH, 0);
            this.Controls.SetChildIndex(this.dNgayGiao, 0);
            ((System.ComponentModel.ISupportInitialize)(this.chkAllDay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtStartDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtStartDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtEndDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtStartTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtEndTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtLabel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtShowTimeAs.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSubject.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtResource.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtResources.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkReminder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbDescription.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbReminder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLocation.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tTenKH.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dNgayGiao.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dNgayGiao.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit tTenKH;
        private DevExpress.XtraEditors.DateEdit dNgayGiao;
    }
}