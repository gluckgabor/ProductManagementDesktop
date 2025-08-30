using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductManagements
{
    public partial class Form1 : Form
    {
        private ProductRepository _productRepository;
        private Product _currentProduct;

        public Form1()
        {
            InitializeComponent();

            try
            {
                _productRepository = new ProductRepository();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba a term�k adatb�zis inicializ�l�sakor: {ex.Message}",
                    "Inicializ�l�si hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProducts();

            // Change column headers to Hungarian
            if (dgvProducts.Columns.Count > 0)
            {
                dgvProducts.Columns["Id"].HeaderText = "Azonos�t�";
                dgvProducts.Columns["Name"].HeaderText = "Term�k neve";
                dgvProducts.Columns["Description"].HeaderText = "Le�r�s";
                dgvProducts.Columns["StockQuantity"].HeaderText = "K�szlet";
                dgvProducts.Columns["Price"].HeaderText = "�r";
                dgvProducts.Columns["CreatedDate"].HeaderText = "L�trehoz�s d�tuma";

                // BUG #7: Fixed column widths - long names will break UI layout
                dgvProducts.Columns["Name"].Width = 120; // Fixed width, too narrow for long names
                dgvProducts.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvProducts.DefaultCellStyle.WrapMode = DataGridViewTriState.False; // No text wrapping
            }
        }

        private void LoadProducts()
        {
            try
            {
                var products = _productRepository.GetAllProducts();
                dgvProducts.DataSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba a term�kek bet�lt�sekor: {ex.Message}",
                    "Bet�lt�si hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // BUG #6: Inconsistent sorting - treats numbers as strings
        private void dgvProducts_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                string columnName = dgvProducts.Columns[e.ColumnIndex].Name;
                var products = dgvProducts.DataSource as List<Product>;

                if (products == null) return;

                List<Product> sortedProducts;

                if (columnName == "Price")
                {
                    // BUG: Sorting price as string instead of decimal (10, 100, 2, 20, 3, 30...)
                    sortedProducts = products.OrderBy(p => p.Price.ToString()).ToList();
                }
                else if (columnName == "StockQuantity")
                {
                    // BUG: Sorting stock quantity as string instead of integer
                    sortedProducts = products.OrderBy(p => p.StockQuantity.ToString()).ToList();
                }
                else if (columnName == "Name")
                {
                    sortedProducts = products.OrderBy(p => p.Name).ToList();
                }
                else
                {
                    return;
                }

                dgvProducts.DataSource = sortedProducts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba a rendez�s sor�n: {ex.Message}",
                    "Rendez�si hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input fields
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("A term�k neve k�telez�.", "�rv�nyes�t�si hiba",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtStockQuantity.Text))
                {
                    MessageBox.Show("A k�szlet mennyis�ge k�telez�.", "�rv�nyes�t�si hiba",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStockQuantity.Focus();
                    return;
                }

                // BUG #1: Missing price validation - will crash on empty price!
                // Original validation commented out:
                /*
                if (string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    MessageBox.Show("Az �r k�telez�.", "�rv�nyes�t�si hiba",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrice.Focus();
                    return;
                }
                */

                // Parse numeric values with error handling
                if (!int.TryParse(txtStockQuantity.Text.Trim(), out int stockQuantity))
                {
                    MessageBox.Show($"�rv�nytelen k�szlet mennyis�g: '{txtStockQuantity.Text}'. K�rj�k, adjon meg egy �rv�nyes sz�mot.",
                        "Beviteli hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtStockQuantity.Focus();
                    txtStockQuantity.SelectAll();
                    return;
                }

                // BUG #1: This will crash if txtPrice.Text is empty!
                decimal price = decimal.Parse(txtPrice.Text.Trim()); // No TryParse!

                // BUG #4: Weak price validation - allows too many decimal places
                // Original strict validation commented out:
                /*
                if (!decimal.TryParse(txtPrice.Text.Trim(), out decimal price))
                {
                    MessageBox.Show($"�rv�nytelen �r: '{txtPrice.Text}'. K�rj�k, adjon meg egy �rv�nyes decim�lis sz�mot.",
                        "Beviteli hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPrice.Focus();
                    txtPrice.SelectAll();
                    return;
                }
                */

                // BUG #2: Missing negative stock validation
                // Original validation commented out:
                /*
                if (stockQuantity < 0)
                {
                    MessageBox.Show("A k�szlet mennyis�ge nem lehet negat�v.", "�rv�nyes�t�si hiba",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStockQuantity.Focus();
                    return;
                }
                */

                if (price <= 0)
                {
                    MessageBox.Show("Az �r null�n�l nagyobb kell legyen.", "�rv�nyes�t�si hiba",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrice.Focus();
                    return;
                }

                var newProduct = new Product
                {
                    Name = txtName.Text.Trim(),
                    Description = txtDescription.Text?.Trim() ?? string.Empty,
                    StockQuantity = stockQuantity,
                    Price = price
                };

                _productRepository.AddProduct(newProduct);
                LoadProducts();

                ClearForm();

                MessageBox.Show("A term�k sikeresen hozz�adva!", "Siker",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // BUG #1: Poor error handling - shows technical details to user
                MessageBox.Show($"Hiba a term�k hozz�ad�sakor: {ex.Message}",
                    "Term�k hozz�ad�si hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label5_Click(object sender, EventArgs e) { }

        private void textBox2_TextChanged(object sender, EventArgs e) { }

        private void label2_Click(object sender, EventArgs e) { }

        private void dvgProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dgvProducts.Rows.Count)
                {
                    var row = dgvProducts.Rows[e.RowIndex];
                    if (row.DataBoundItem is Product selectedProduct)
                    {
                        _currentProduct = selectedProduct;

                        txtName.Text = selectedProduct.Name ?? string.Empty;
                        txtDescription.Text = selectedProduct.Description ?? string.Empty;
                        txtStockQuantity.Text = selectedProduct.StockQuantity.ToString();
                        txtPrice.Text = selectedProduct.Price.ToString("F2");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba a term�k kiv�laszt�sakor: {ex.Message}",
                    "Kiv�laszt�si hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtProductName_TextChanged(object sender, EventArgs e) { }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentProduct != null)
                {
                    var result = MessageBox.Show($"Biztos, hogy t�r�lni szeretn� a(z) '{_currentProduct.Name}' term�ket?",
                        "T�rl�s meger�s�t�se", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        _productRepository.DeleteProduct(_currentProduct.Id);

                        // BUG #5: Missing LoadProducts() call - list won't refresh after deletion!
                        // LoadProducts(); // This should be uncommented for fix

                        ClearForm();
                        _currentProduct = null;

                        MessageBox.Show("A term�k sikeresen t�r�lve!", "Siker",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("K�rj�k, v�lasszon ki egy term�ket a t�rl�shez.", "Nincs kiv�laszt�s",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba a term�k t�rl�sekor: {ex.Message}",
                    "T�rl�si hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentProduct == null)
                {
                    MessageBox.Show("K�rj�k, v�lasszon ki egy term�ket a friss�t�shez.", "Nincs kiv�laszt�s",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate input fields
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("A term�k neve k�telez�.", "�rv�nyes�t�si hiba",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtStockQuantity.Text))
                {
                    MessageBox.Show("A k�szlet mennyis�ge k�telez�.", "�rv�nyes�t�si hiba",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStockQuantity.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    MessageBox.Show("Az �r k�telez�.", "�rv�nyes�t�si hiba",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrice.Focus();
                    return;
                }

                // Parse numeric values with error handling
                if (!int.TryParse(txtStockQuantity.Text.Trim(), out int stockQuantity))
                {
                    MessageBox.Show($"�rv�nytelen k�szlet mennyis�g: '{txtStockQuantity.Text}'. K�rj�k, adjon meg egy �rv�nyes sz�mot.",
                        "Beviteli hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtStockQuantity.Focus();
                    txtStockQuantity.SelectAll();
                    return;
                }

                if (!decimal.TryParse(txtPrice.Text.Trim(), out decimal price))
                {
                    MessageBox.Show($"�rv�nytelen �r: '{txtPrice.Text}'. K�rj�k, adjon meg egy �rv�nyes decim�lis sz�mot.",
                        "Beviteli hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPrice.Focus();
                    txtPrice.SelectAll();
                    return;
                }

                // BUG #2: Same missing negative stock validation in update
                // Original validation commented out:
                /*
                if (stockQuantity < 0)
                {
                    MessageBox.Show("A k�szlet mennyis�ge nem lehet negat�v.", "�rv�nyes�t�si hiba",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStockQuantity.Focus();
                    return;
                }
                */

                if (price <= 0)
                {
                    MessageBox.Show("Az �r null�n�l nagyobb kell legyen.", "�rv�nyes�t�si hiba",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrice.Focus();
                    return;
                }

                _currentProduct.Name = txtName.Text.Trim();
                _currentProduct.Description = txtDescription.Text?.Trim() ?? string.Empty;
                _currentProduct.StockQuantity = stockQuantity;
                _currentProduct.Price = price;

                _productRepository.UpdateProduct(_currentProduct);
                LoadProducts();

                MessageBox.Show("A term�k sikeresen friss�tve!", "Siker",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba a term�k friss�t�sekor: {ex.Message}",
                    "Friss�t�si hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ClearForm();
                _currentProduct = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba az �rlap t�rl�sekor: {ex.Message}",
                    "T�rl�si hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label2_Click_1(object sender, EventArgs e) { }

        private void ClearForm()
        {
            try
            {
                txtName.Clear();
                txtStockQuantity.Clear();
                txtDescription.Clear();
                txtPrice.Clear();
                dgvProducts.ClearSelection();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Hiba az �rlap t�rl�sekor: {ex.Message}");
            }
        }
    }
}