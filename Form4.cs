using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace test
{
    public partial class Employer : Form
    {
        // Connection string for SQL Server
        string connectionString = @"Data Source=RainerPC\SQLEXPRESS;Initial Catalog=JobApp;Integrated Security=True;";

        public Employer()
        {
            InitializeComponent();
        }

        private void Employer_Load(object sender, EventArgs e)
        {
            LoadApplicants();
        }

        // Method to load applicants from the database
        private void LoadApplicants()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetApplicants", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable applicantsTable = new DataTable();
                    adapter.Fill(applicantsTable);

                    // Bind data to DataGridView
                    applicantsGridView.DataSource = applicantsTable;

                    // Ensure correct column types for document data
                    foreach (DataGridViewColumn column in applicantsGridView.Columns)
                    {
                        if (column.HeaderText == "Document")
                        {
                            // Prevent invalid data from being treated as an image
                            column.DefaultCellStyle.NullValue = "No Document";
                        }
                    }

                    // Add "Approve" and "Reject" button columns if not already added
                    if (applicantsGridView.Columns["ApproveButton"] == null)
                    {
                        DataGridViewButtonColumn approveButton = new DataGridViewButtonColumn
                        {
                            Name = "ApproveButton",
                            HeaderText = "Approve",
                            Text = "Approve",
                            UseColumnTextForButtonValue = true
                        };
                        applicantsGridView.Columns.Add(approveButton);
                    }

                    if (applicantsGridView.Columns["RejectButton"] == null)
                    {
                        DataGridViewButtonColumn rejectButton = new DataGridViewButtonColumn
                        {
                            Name = "RejectButton",
                            HeaderText = "Reject",
                            Text = "Reject",
                            UseColumnTextForButtonValue = true
                        };
                        applicantsGridView.Columns.Add(rejectButton);
                    }

                    // Add "View Document" button column if not already added
                    if (applicantsGridView.Columns["ViewDocumentButton"] == null)
                    {
                        DataGridViewButtonColumn viewDocumentButton = new DataGridViewButtonColumn
                        {
                            Name = "ViewDocumentButton",
                            HeaderText = "View Document",
                            Text = "View",
                            UseColumnTextForButtonValue = true
                        };
                        applicantsGridView.Columns.Add(viewDocumentButton);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading applicants: " + ex.Message);
                }
            }
        }

        // Event handler for clicking the Approve/Reject button in the DataGridView
        private void applicantsGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Get the selected app_ID and JS_ID
                int appID = Convert.ToInt32(applicantsGridView.Rows[e.RowIndex].Cells["app_ID"].Value);
                int jsID = Convert.ToInt32(applicantsGridView.Rows[e.RowIndex].Cells["JS_ID"].Value);

                if (applicantsGridView.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
                {
                    string action = applicantsGridView.Columns[e.ColumnIndex].Name;

                    // Approve or Reject based on which button was clicked
                    if (action == "ApproveButton")
                    {
                        UpdateApplicantStatus(appID, "Approved");
                    }
                    else if (action == "RejectButton")
                    {
                        UpdateApplicantStatus(appID, "Rejected");
                    }
                    else if (action == "ViewDocumentButton")
                    {
                        // Open the document
                        OpenApplicantDocument(jsID);
                    }
                }
            }
        }

        private void OpenApplicantDocument(int jsID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetApplicantDocuments", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@JS_ID", jsID);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string docName = reader["doc_name"].ToString();
                        byte[] docData = (byte[])reader["doc"];

                        // Save the document locally and open it
                        string tempFilePath = System.IO.Path.GetTempPath() + docName;
                        System.IO.File.WriteAllBytes(tempFilePath, docData);
                        System.Diagnostics.Process.Start(tempFilePath);
                    }
                    else
                    {
                        MessageBox.Show("No document found for the selected applicant.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening document: " + ex.Message);
                }
            }
        }

        // Method to update the applicant status in the database
        private void UpdateApplicantStatus(int appID, string newStatus)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UpdateApplicantStatus", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@app_ID", appID);
                    cmd.Parameters.AddWithValue("@new_status", newStatus);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Applicant status updated to {newStatus}.");
                    LoadApplicants(); // Refresh the applicants list
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating applicant status: " + ex.Message);
                }
            }
        }

        private void applicantsGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Redirect to the CellClick event
            applicantsGridView_CellClick(sender, e);
        }

    }
}
