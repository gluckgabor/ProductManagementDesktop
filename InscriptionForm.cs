using System.Data.SQLite;

namespace ProductManagements
{
    public partial class InscriptionForm : Form
    {
        private const string ConnectionString = "Data Source=products.db;Version=3;";

        public InscriptionForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 Form2 = new Form2();
            Form2.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();
                string telephone = txtTelephone.Text.Trim();
                string address = txtAddress.Text.Trim();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Username and Password are required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
                    {
                        conn.Open();
                        string query = "INSERT INTO Users (Username, Password, Telephone, Address) VALUES (@Username, @Password, @Telephone, @Address)";
                        using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Username", username);
                            cmd.Parameters.AddWithValue("@Password", password);
                            cmd.Parameters.AddWithValue("@Telephone", telephone);
                            cmd.Parameters.AddWithValue("@Address", address);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("User registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Form2 loginForm = new Form2();
                    loginForm.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void InscriptionForm_Load(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '*';

        }
    }
}
