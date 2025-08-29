namespace ProductManagements
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            label1 = new Label();
            txtName = new TextBox();
            btnAddProduct = new Button();
            dgvProducts = new DataGridView();
            label3 = new Label();
            txtStockQuantity = new TextBox();
            label4 = new Label();
            txtDescription = new TextBox();
            label5 = new Label();
            txtPrice = new TextBox();
            btnDeleteProduct = new Button();
            BtnCancel = new Button();
            btnUpdateProduct = new Button();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 106);
            label1.Name = "label1";
            label1.Size = new Size(76, 15);
            label1.TabIndex = 0;
            label1.Text = "Termék neve:";
            label1.Click += label1_Click;
            // 
            // txtName
            // 
            txtName.Location = new Point(136, 104);
            txtName.Margin = new Padding(3, 2, 3, 2);
            txtName.Name = "txtName";
            txtName.Size = new Size(213, 23);
            txtName.TabIndex = 1;
            txtName.TextChanged += txtProductName_TextChanged;
            // 
            // btnAddProduct
            // 
            btnAddProduct.Location = new Point(61, 331);
            btnAddProduct.Margin = new Padding(3, 2, 3, 2);
            btnAddProduct.Name = "btnAddProduct";
            btnAddProduct.Size = new Size(91, 30);
            btnAddProduct.TabIndex = 2;
            btnAddProduct.Text = "Mentés";
            btnAddProduct.UseVisualStyleBackColor = true;
            btnAddProduct.Click += button1_Click;
            // 
            // dgvProducts
            // 
            dgvProducts.BackgroundColor = Color.White;
            dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(192, 255, 255);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgvProducts.DefaultCellStyle = dataGridViewCellStyle1;
            dgvProducts.Location = new Point(387, 104);
            dgvProducts.Margin = new Padding(3, 2, 3, 2);
            dgvProducts.Name = "dgvProducts";
            dgvProducts.RowHeadersWidth = 51;
            dgvProducts.RowTemplate.Height = 30;
            dgvProducts.Size = new Size(592, 352);
            dgvProducts.TabIndex = 3;
            dgvProducts.CellClick += dvgProducts_CellContentClick;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(26, 139);
            label3.Name = "label3";
            label3.Size = new Size(104, 15);
            label3.TabIndex = 0;
            label3.Text = "Készletmennyiség:";
            // 
            // txtStockQuantity
            // 
            txtStockQuantity.Location = new Point(136, 137);
            txtStockQuantity.Margin = new Padding(3, 2, 3, 2);
            txtStockQuantity.Name = "txtStockQuantity";
            txtStockQuantity.Size = new Size(213, 23);
            txtStockQuantity.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(26, 173);
            label4.Name = "label4";
            label4.Size = new Size(40, 15);
            label4.TabIndex = 0;
            label4.Text = "Leírás:";
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(136, 172);
            txtDescription.Margin = new Padding(3, 2, 3, 2);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(213, 89);
            txtDescription.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(29, 282);
            label5.Name = "label5";
            label5.Size = new Size(22, 15);
            label5.TabIndex = 0;
            label5.Text = "Ár:";
            label5.Click += label5_Click;
            // 
            // txtPrice
            // 
            txtPrice.Location = new Point(136, 278);
            txtPrice.Margin = new Padding(3, 2, 3, 2);
            txtPrice.Name = "txtPrice";
            txtPrice.Size = new Size(213, 23);
            txtPrice.TabIndex = 1;
            // 
            // btnDeleteProduct
            // 
            btnDeleteProduct.Location = new Point(157, 331);
            btnDeleteProduct.Margin = new Padding(3, 2, 3, 2);
            btnDeleteProduct.Name = "btnDeleteProduct";
            btnDeleteProduct.Size = new Size(92, 30);
            btnDeleteProduct.TabIndex = 2;
            btnDeleteProduct.Text = "Törlés";
            btnDeleteProduct.UseVisualStyleBackColor = true;
            btnDeleteProduct.Click += btnDeleteProduct_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.Location = new Point(254, 376);
            BtnCancel.Margin = new Padding(3, 2, 3, 2);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(95, 27);
            BtnCancel.TabIndex = 2;
            BtnCancel.Text = "Mégse";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // btnUpdateProduct
            // 
            btnUpdateProduct.Location = new Point(254, 331);
            btnUpdateProduct.Margin = new Padding(3, 2, 3, 2);
            btnUpdateProduct.Name = "btnUpdateProduct";
            btnUpdateProduct.Size = new Size(95, 30);
            btnUpdateProduct.TabIndex = 5;
            btnUpdateProduct.Text = "Frissítés";
            btnUpdateProduct.UseVisualStyleBackColor = true;
            btnUpdateProduct.Click += btnUpdateProduct_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 18F);
            label2.ForeColor = Color.Blue;
            label2.Location = new Point(375, 28);
            label2.Name = "label2";
            label2.Size = new Size(161, 32);
            label2.TabIndex = 6;
            label2.Text = "Termékkezelő";
            label2.Click += label2_Click_1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1008, 475);
            Controls.Add(label2);
            Controls.Add(btnUpdateProduct);
            Controls.Add(dgvProducts);
            Controls.Add(BtnCancel);
            Controls.Add(btnDeleteProduct);
            Controls.Add(btnAddProduct);
            Controls.Add(txtPrice);
            Controls.Add(label5);
            Controls.Add(txtDescription);
            Controls.Add(label4);
            Controls.Add(txtStockQuantity);
            Controls.Add(label3);
            Controls.Add(txtName);
            Controls.Add(label1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            RightToLeft = RightToLeft.No;
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtName;
        private Button btnAddProduct;
        private DataGridView dgvProducts;
        private Label label3;
        private TextBox txtStockQuantity;
        private Label label4;
        private TextBox txtDescription;
        private Label label5;
        private TextBox txtPrice;
        private Button btnDeleteProduct;
        private Button BtnCancel;
        private Button btnUpdateProduct;
        private Label label2;
    }
}
