using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using test;

namespace DocumentUploader
{
    public partial class Form2 : Form
    {
        // Declare the necessary TextBox and Button controls
        private TextBox txtDocName;
        private TextBox txtDocType;
        private Button btnBrowse;
        private Button btnUpload;
        private Button btnClear;
        private int _jobSeekerID;

        // SQL Connection string (replace with your own database connection details)
        private string connectionString = "Data Source=RainerPC\\SQLEXPRESS;Initial Catalog=JobApp;Integrated Security=True;";

        // File data to store the document binary content
        private byte[] fileData;

        public Form2(int jobSeekerID)
        {
            InitializeComponent();
            _jobSeekerID = jobSeekerID;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialize the form controls
            InitializeControls();
        }

        private void InitializeControls()
        {
            // Document Name Label
            Label lblDocName = new Label { Text = "Document Name:", Location = new Point(20, 20), Width = 120 };
            Controls.Add(lblDocName);

            // Document Name TextBox
            txtDocName = new TextBox { Location = new Point(150, 20), Width = 200 };
            Controls.Add(txtDocName);

            // Document Type Label
            Label lblDocType = new Label { Text = "Document Type:", Location = new Point(20, 60), Width = 120 };
            Controls.Add(lblDocType);

            // Document Type TextBox
            txtDocType = new TextBox { Location = new Point(150, 60), Width = 200 };
            Controls.Add(txtDocType);

            // Browse Button
            btnBrowse = new Button { Text = "Browse", Location = new Point(20, 100), Width = 100 };
            btnBrowse.Click += new EventHandler(btnBrowse_Click);
            Controls.Add(btnBrowse);

            // Upload Button
            btnUpload = new Button { Text = "Upload", Location = new Point(130, 100), Width = 100 };
            btnUpload.Click += new EventHandler(btnUpload_Click);
            Controls.Add(btnUpload);

            // Clear Button
            btnClear = new Button { Text = "Clear", Location = new Point(240, 100), Width = 100 };
            btnClear.Click += new EventHandler(btnClear_Click);
            Controls.Add(btnClear);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // Open file dialog to select a document
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    txtDocName.Text = Path.GetFileName(filePath);  // Fill document name
                    txtDocType.Text = Path.GetExtension(filePath).Replace(".", "");  // Fill document type

                    // Read file content as binary
                    fileData = File.ReadAllBytes(filePath);
                    MessageBox.Show("File loaded successfully.");
                }
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            // Ensure required fields are filled
            if (string.IsNullOrEmpty(txtDocName.Text) || string.IsNullOrEmpty(txtDocType.Text) || fileData == null)
            {
                MessageBox.Show("Please fill in all required fields and load a document.");
                return;
            }

            // Upload the document using the stored procedure
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_UploadDocument", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Supply a hardcoded value of 1 for employer_ID
                        
                        cmd.Parameters.AddWithValue("@JS_ID", _jobSeekerID);
                        cmd.Parameters.AddWithValue("@qual_ID", 1);
                        cmd.Parameters.AddWithValue("@doc_name", txtDocName.Text);
                        cmd.Parameters.AddWithValue("@doc", fileData);
                        cmd.Parameters.AddWithValue("@doc_type", txtDocType.Text);

                        // Execute the stored procedure
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Document uploaded successfully.");
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
            txtDocName.Clear();
            txtDocType.Clear();
            fileData = null;
        }

       
    }
}
