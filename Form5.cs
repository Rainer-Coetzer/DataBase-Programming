using DocumentUploader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class ApplyForm : Form
    {
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtCitizenship;
        private TextBox txtEmail;
        private Button btnApply;
        private int _jobID;

        private string connectionString = "Data Source=RainerPC\\SQLEXPRESS;Initial Catalog=JobApp;Integrated Security=True;";

        public ApplyForm(int jobID)
        {
            InitializeComponent();
            _jobID = jobID;
            this.Load += new System.EventHandler(this.ApplyForJobForm_Load); // Ensure Load event is wired
        }

        private void ApplyForJobForm_Load(object sender, EventArgs e)
        {
            InitializeControls();
        }

        private void InitializeControls()
        {
            // First Name Label and TextBox
            Label lblFirstName = new Label { Text = "First Name:", Location = new Point(20, 20), Width = 100 };
            txtFirstName = new TextBox { Location = new Point(130, 20), Width = 200 };
            Controls.Add(lblFirstName);
            Controls.Add(txtFirstName);

            // Last Name Label and TextBox
            Label lblLastName = new Label { Text = "Last Name:", Location = new Point(20, 60), Width = 100 };
            txtLastName = new TextBox { Location = new Point(130, 60), Width = 200 };
            Controls.Add(lblLastName);
            Controls.Add(txtLastName);

            // Citizenship Label and TextBox
            Label lblCitizenship = new Label { Text = "Citizenship:", Location = new Point(20, 100), Width = 100 };
            txtCitizenship = new TextBox { Location = new Point(130, 100), Width = 200 };
            Controls.Add(lblCitizenship);
            Controls.Add(txtCitizenship);

            // Email Label and TextBox
            Label lblEmail = new Label { Text = "Email:", Location = new Point(20, 140), Width = 100 };
            txtEmail = new TextBox { Location = new Point(130, 140), Width = 200 };
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);

            // Apply Button
            btnApply = new Button { Text = "Apply", Location = new Point(130, 180), Width = 100 };
            btnApply.Click += new EventHandler(btnApply_Click);
            Controls.Add(btnApply);
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Prepare the command to call the stored procedure
                    int jobSeekerID;
                    using (SqlCommand cmd = new SqlCommand("sp_CreateJobSeeker", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure
                        cmd.Parameters.AddWithValue("@F_name", txtFirstName.Text);
                        cmd.Parameters.AddWithValue("@L_name", txtLastName.Text);
                        cmd.Parameters.AddWithValue("@citizenship", txtCitizenship.Text);
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text);

                        // Get the Job Seeker ID (JS_ID) after insertion
                        SqlParameter outputJSID = new SqlParameter("@JS_ID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputJSID);

                        // Execute the command
                        cmd.ExecuteNonQuery();

                        // Retrieve the Job Seeker ID
                        jobSeekerID = Convert.ToInt32(outputJSID.Value);
                    }

                    using (SqlCommand cmd = new SqlCommand("SubmitJobApplication", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure
                        cmd.Parameters.AddWithValue("@JS_ID", jobSeekerID);
                        cmd.Parameters.AddWithValue("@job_ID", _jobID);

                        // Execute the procedure
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Application submitted successfully!");

                        // Now open the document submission form
                        Form2 docForm = new Form2(jobSeekerID);
                        docForm.Show();  // Show the document submission form
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        private void ApplyForJobForm_Load_1(object sender, EventArgs e)
        {

        }
    }

}
