namespace GlPB
{
    partial class Filter
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
            this.components = new System.ComponentModel.Container();
            this.vSpinEdit1 = new CBSControls.VSpinEdit(this.components);
            this.vSpinEdit2 = new CBSControls.VSpinEdit(this.components);
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.vSpinEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vSpinEdit2.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // vSpinEdit1
            // 
            this.vSpinEdit1.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.vSpinEdit1.EnterMoveNextControl = true;
            this.vSpinEdit1.Location = new System.Drawing.Point(114, 24);
            this.vSpinEdit1.Name = "vSpinEdit1";
            this.vSpinEdit1.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.vSpinEdit1.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(245)))), ((int)(((byte)(241)))));
            this.vSpinEdit1.Properties.Appearance.Options.UseBackColor = true;
            this.vSpinEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.vSpinEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.vSpinEdit1.Properties.MaxValue = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.vSpinEdit1.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.vSpinEdit1.Size = new System.Drawing.Size(48, 18);
            this.vSpinEdit1.TabIndex = 0;
            // 
            // vSpinEdit2
            // 
            this.vSpinEdit2.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.vSpinEdit2.EnterMoveNextControl = true;
            this.vSpinEdit2.Location = new System.Drawing.Point(114, 60);
            this.vSpinEdit2.Name = "vSpinEdit2";
            this.vSpinEdit2.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.vSpinEdit2.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(245)))), ((int)(((byte)(241)))));
            this.vSpinEdit2.Properties.Appearance.Options.UseBackColor = true;
            this.vSpinEdit2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.vSpinEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.vSpinEdit2.Properties.MaxValue = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.vSpinEdit2.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.vSpinEdit2.Size = new System.Drawing.Size(48, 18);
            this.vSpinEdit2.TabIndex = 1;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(72, 107);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 2;
            this.simpleButton1.Text = "Ok";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(27, 29);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(44, 13);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "Từ tháng";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(27, 65);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(51, 13);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "Đến tháng";
            // 
            // Filter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(206, 137);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.vSpinEdit2);
            this.Controls.Add(this.vSpinEdit1);
            this.Name = "Filter";
            this.Text = "Filter";
            this.Load += new System.EventHandler(this.Filter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.vSpinEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vSpinEdit2.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CBSControls.VSpinEdit vSpinEdit1;
        private CBSControls.VSpinEdit vSpinEdit2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}