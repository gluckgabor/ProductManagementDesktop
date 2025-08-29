namespace ProductManagements
{
    partial class Form2
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Username = new Label();
            Password = new Label();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            LoginLbael = new Label();
            BtnLogin = new Button();
            BtnInscription = new Button();
            SuspendLayout();
            // 
            // Username
            // 
            Username.AutoSize = true;
            Username.Location = new Point(187, 112);
            Username.Name = "Username";
            Username.Size = new Size(87, 15);
            Username.TabIndex = 0;
            Username.Text = "Felhasználónév";
            Username.Click += label1_Click;
            // 
            // Password
            // 
            Password.AutoSize = true;
            Password.Location = new Point(188, 155);
            Password.Name = "Password";
            Password.Size = new Size(37, 15);
            Password.TabIndex = 1;
            Password.Text = "Jelszó";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(281, 112);
            txtUsername.Margin = new Padding(3, 2, 3, 2);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(199, 23);
            txtUsername.TabIndex = 2;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(281, 155);
            txtPassword.Margin = new Padding(3, 2, 3, 2);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(199, 23);
            txtPassword.TabIndex = 2;
            txtPassword.TextChanged += txtPaswword_TextChanged;
            // 
            // LoginLbael
            // 
            LoginLbael.AutoSize = true;
            LoginLbael.Font = new Font("Segoe UI", 18F);
            LoginLbael.ForeColor = Color.Blue;
            LoginLbael.Location = new Point(303, 37);
            LoginLbael.Name = "LoginLbael";
            LoginLbael.Size = new Size(160, 32);
            LoginLbael.TabIndex = 3;
            LoginLbael.Text = "Bejelentkezés";
            LoginLbael.Click += LoginLbael_Click;
            // 
            // BtnLogin
            // 
            BtnLogin.Location = new Point(381, 198);
            BtnLogin.Margin = new Padding(3, 2, 3, 2);
            BtnLogin.Name = "BtnLogin";
            BtnLogin.Size = new Size(99, 32);
            BtnLogin.TabIndex = 4;
            BtnLogin.Text = "Bejelentkezés";
            BtnLogin.UseVisualStyleBackColor = true;
            BtnLogin.Click += BtnLogin_Click;
            // 
            // BtnInscription
            // 
            BtnInscription.Location = new Point(280, 198);
            BtnInscription.Margin = new Padding(3, 2, 3, 2);
            BtnInscription.Name = "BtnInscription";
            BtnInscription.Size = new Size(94, 32);
            BtnInscription.TabIndex = 5;
            BtnInscription.Text = "Regisztráció";
            BtnInscription.UseVisualStyleBackColor = true;
            BtnInscription.Click += BtnInscription_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(747, 326);
            Controls.Add(BtnInscription);
            Controls.Add(BtnLogin);
            Controls.Add(LoginLbael);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Controls.Add(Password);
            Controls.Add(Username);
            Font = new Font("Segoe UI", 9F);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form2";
            Text = "Form2";
            Load += Form2_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Username;
        private Label Password;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Label LoginLbael;
        private Button BtnLogin;
        private Button BtnInscription;
    }
}