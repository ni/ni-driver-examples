namespace NationalInstruments.Examples.ListModeTimerTriggered
{
    partial class MultiDataGridView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.recordLabel1 = new System.Windows.Forms.Label();
            this.recordLabel2 = new System.Windows.Forms.Label();
            this.recordNumberNumericUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.recordNumberNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView.Location = new System.Drawing.Point(0, 3);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(438, 296);
            this.dataGridView.TabIndex = 0;
            // 
            // recordLabel1
            // 
            this.recordLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.recordLabel1.AutoSize = true;
            this.recordLabel1.Location = new System.Drawing.Point(4, 308);
            this.recordLabel1.Name = "recordLabel1";
            this.recordLabel1.Size = new System.Drawing.Size(42, 13);
            this.recordLabel1.TabIndex = 1;
            this.recordLabel1.Text = "Record";
            // 
            // recordLabel2
            // 
            this.recordLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.recordLabel2.AutoSize = true;
            this.recordLabel2.Location = new System.Drawing.Point(117, 308);
            this.recordLabel2.Name = "recordLabel2";
            this.recordLabel2.Size = new System.Drawing.Size(25, 13);
            this.recordLabel2.TabIndex = 6;
            this.recordLabel2.Text = "of 0";
            // 
            // recordNumberNumericUpDown
            // 
            this.recordNumberNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.recordNumberNumericUpDown.Enabled = false;
            this.recordNumberNumericUpDown.Location = new System.Drawing.Point(52, 305);
            this.recordNumberNumericUpDown.Name = "recordNumberNumericUpDown";
            this.recordNumberNumericUpDown.Size = new System.Drawing.Size(59, 20);
            this.recordNumberNumericUpDown.TabIndex = 7;
            this.recordNumberNumericUpDown.ValueChanged += new System.EventHandler(this.recordNumberNumericUpDown_ValueChanged);
            // 
            // MultiDataGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.recordNumberNumericUpDown);
            this.Controls.Add(this.recordLabel2);
            this.Controls.Add(this.recordLabel1);
            this.Controls.Add(this.dataGridView);
            this.Name = "MultiDataGridView";
            this.Size = new System.Drawing.Size(438, 331);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.recordNumberNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label recordLabel1;
        private System.Windows.Forms.Label recordLabel2;
        private System.Windows.Forms.NumericUpDown recordNumberNumericUpDown;
             

    }
}
