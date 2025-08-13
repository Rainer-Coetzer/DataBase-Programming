using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace DocumentReviewer
{
    public partial class Form3 : Form
    {
        // Declare necessary TextBox, ComboBox, and Button controls
        private TextBox txtDocID;
        private TextBox txtFeedback;
        private ComboBox cmbStatus;
        private Button btnReview;
        private Button btnClear;
        private Button btnLoadDoc;

        // SQL Connection string (replace with your own database connection details)
        private string connectionString = "Data Source=RainerPC\\SQLEXPRESS;Initial Catalog=JobApp;Integrated Security=True;";

        public Form3()
        {
            InitializeComponent();
        }

        private void ReviewForm_Load(object sender, EventArgs e)
        {
            // Initialize the form controls
            InitializeControls();
        }

        private void InitializeControls()
        {
            // Document ID Label
            Label lblDocID = new Label { Text = "Document ID:", Location = new Point(20, 20), Width = 120 };
            Controls.Add(lblDocID);

            // Document ID TextBox
            txtDocID = new TextBox { Location = new Point(150, 20), Width = 200 };
            Controls.Add(txtDocID);

            // Feedback Label
            Label lblFeedback = new Label { Text = "Feedback:", Location = new Point(20, 60), Width = 120 };
            Controls.Add(lblFeedback);

            // Feedback TextBox
            txtFeedback = new TextBox { Location = new Point(150, 60), Width = 200 };
            Controls.Add(txtFeedback);

            // Status Label
            Label lblStatus = new Label { Text = "Review Status:", Location = new Point(20, 100), Width = 120 };
            Controls.Add(lblStatus);

            // Status ComboBox
            cmbStatus = new ComboBox { Location = new Point(150, 100), Width = 200 };
            cmbStatus.Items.AddRange(new string[] { "Approved", "Rejected", "Pending" });
            Controls.Add(cmbStatus);

            // Load Document Button
            btnLoadDoc = new Button { Text = "Load Document", Location = new Point(20, 140), Width = 120 };
            btnLoadDoc.Click += new EventHandler(btnLoadDoc_Click);
            Controls.Add(btnLoadDoc);

            // Review Button
            btnReview = new Button { Text = "Submit Review", Location = new Point(150, 140), Width = 120 };
            btnReview.Click += new EventHandler(btnReview_Click);
            Controls.Add(btnReview);

            // Clear Button
            btnClear = new Button { Text = "Clear", Location = new Point(280, 140), Width = 120 };
            btnClear.Click += new EventHandler(btnClear_Click);
            Controls.Add(btnClear);
        }

        private void btnLoadDoc_Click(object sender, EventArgs e)
        {
            // Load the selected document details (You can extend this to fetch more details)
            try
            {
                if (string.IsNullOrEmpty(txtDocID.Text))
                {
                    MessageBox.Show("Please enter a Document ID.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT doc_name, Status, Feedback FROM documents WHERE doc_ID = @doc_ID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@doc_ID", int.Parse(txtDocID.Text));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string docName = reader["doc_name"].ToString();
                                string status = reader["Status"].ToString();
                                string feedback = reader["Feedback"].ToString();

                                MessageBox.Show($"Document Name: {docName}\nCurrent Status: {status}\nFeedback: {feedback}");

                                // Set the current status and feedback in the form
                                cmbStatus.SelectedItem = status;
                                txtFeedback.Text = feedback;
                            }
                            else
                            {
                                MessageBox.Show("Document not found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnReview_Click(object sender, EventArgs e)
        {
            // Ensure required fields are filled
            if (string.IsNullOrEmpty(txtDocID.Text) || cmbStatus.SelectedItem == null || string.IsNullOrEmpty(txtFeedback.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Call the ReviewDocument stored procedure to submit the review
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("ReviewDocument", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@DocumentID", int.Parse(txtDocID.Text));
                        cmd.Parameters.AddWithValue("@EmployerID", 3); // Hardcoded employer_ID, replace as needed
                        cmd.Parameters.AddWithValue("@Status", cmbStatus.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@Feedback", txtFeedback.Text);

                        // Execute the stored procedure
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Review submitted successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Clear all fields
            txtDocID.Clear();
            txtFeedback.Clear();
            cmbStatus.SelectedItem = null;
        }
    }
}
