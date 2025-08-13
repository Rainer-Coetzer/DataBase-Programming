namespace test
{
    partial class jobseeker
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView jobListingsGridView;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.jobListingsGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.jobListingsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // jobListingsGridView
            // 
            this.jobListingsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.jobListingsGridView.Location = new System.Drawing.Point(12, 12);
            this.jobListingsGridView.Name = "jobListingsGridView";
            this.jobListingsGridView.Size = new System.Drawing.Size(647, 237);
            this.jobListingsGridView.TabIndex = 0;
            this.jobListingsGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.jobListingsGridView_CellClick);
            // 
            // jobseeker
            // 
            this.ClientSize = new System.Drawing.Size(683, 261);
            this.Controls.Add(this.jobListingsGridView);
            this.Name = "jobseeker";
            this.Load += new System.EventHandler(this.jobseeker_Load);
            ((System.ComponentModel.ISupportInitialize)(this.jobListingsGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}
