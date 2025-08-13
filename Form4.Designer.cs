namespace test
{
    partial class Employer
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView applicantsGridView;

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
            this.applicantsGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.applicantsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // applicantsGridView
            // 
            this.applicantsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.applicantsGridView.Location = new System.Drawing.Point(12, 12);
            this.applicantsGridView.Name = "applicantsGridView";
            this.applicantsGridView.Size = new System.Drawing.Size(966, 400);
            this.applicantsGridView.TabIndex = 0;
            this.applicantsGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.applicantsGridView_CellClick);
            this.applicantsGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.applicantsGridView_CellContentClick);
            // 
            // Employer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 461);
            this.Controls.Add(this.applicantsGridView);
            this.Name = "Employer";
            this.Text = "Manage Applicants";
            this.Load += new System.EventHandler(this.Employer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.applicantsGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}
