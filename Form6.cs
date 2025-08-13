using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace test
{
    public partial class jobseeker : Form
    {
        // Connection string for SQL Server
        string connectionString = @"Data Source=RainerPC\SQLEXPRESS;Initial Catalog=JobApp;Integrated Security=True;";

        public jobseeker()
        {
            InitializeComponent();
        }

        private void jobseeker_Load(object sender, EventArgs e)
        {
            LoadJobListings();
        }

        // Method to load job listings from the database
        private void LoadJobListings()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetJobListings", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable jobListingsTable = new DataTable();
                    adapter.Fill(jobListingsTable);

                    // Check if data is returned
                    if (jobListingsTable.Rows.Count > 0)
                    {
                        // Bind data to DataGridView
                        jobListingsGridView.DataSource = jobListingsTable;
                    }
                    else
                    {
                        MessageBox.Show("No job listings found.");
                    }

                    // Add the "Apply" button column if not already added
                    if (jobListingsGridView.Columns["ApplyButton"] == null)
                    {
                        DataGridViewButtonColumn applyButtonColumn = new DataGridViewButtonColumn();
                        applyButtonColumn.Name = "ApplyButton";
                        applyButtonColumn.HeaderText = "Apply";
                        applyButtonColumn.Text = "Apply";
                        applyButtonColumn.UseColumnTextForButtonValue = true; // Display "Apply" text
                        jobListingsGridView.Columns.Add(applyButtonColumn);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading job listings: " + ex.Message);
                }
            }
        }

        // Event handler for clicking the Apply button in the DataGridView
        private void jobListingsGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked column is the "Apply" button column
            if (e.RowIndex >= 0 && jobListingsGridView.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                jobListingsGridView.Columns[e.ColumnIndex].Name == "ApplyButton")
            {
                // Get the job ID from the selected row
                int jobID = Convert.ToInt32(jobListingsGridView.Rows[e.RowIndex].Cells["job_ID"].Value);

                // Open the ApplyForm, passing the selected job ID
                ApplyForm applyForm = new ApplyForm(jobID);
                applyForm.ShowDialog();  // Open as a dialog
            }
        }
    }
}
