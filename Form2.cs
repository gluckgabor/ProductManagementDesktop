namespace ProductManagements
{
    public partial class Form2 : Form
    {
        private readonly UserRepository _userRepository;

        public Form2()
        {
            InitializeComponent();
            _userRepository = new UserRepository();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void LoginLbael_Click(object sender, EventArgs e)
        {
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '*';
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Kérjük, adja meg a felhasználónevet és a jelszót.", "Érvényesítési hiba",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool isValidUser = _userRepository.ValidateUser(username, password);

            if (isValidUser)
            {
                MessageBox.Show("Sikeres bejelentkezés!", "Siker",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Érvénytelen felhasználónév vagy jelszó.", "Bejelentkezés sikertelen",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtPaswword_TextChanged(object sender, EventArgs e)
        {
        }

        private void BtnInscription_Click(object sender, EventArgs e)
        {
            InscriptionForm inscriptionF = new InscriptionForm();
            inscriptionF.Show();
            this.Hide();
        }
    }
}