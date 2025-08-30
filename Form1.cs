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
                MessageBox.Show($"Hiba a termék adatbázis inicializálásakor: {ex.Message}",
                    "Inicializálási hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProducts();

            // Change column headers to Hungarian
            if (dgvProducts.Columns.Count > 0)
            {
                dgvProducts.Columns["Id"].HeaderText = "Azonosító";
                dgvProducts.Columns["Name"].HeaderText = "Termék neve";
                dgvProducts.Columns["Description"].HeaderText = "Leírás";
                dgvProducts.Columns["StockQuantity"].HeaderText = "Készlet";
                dgvProducts.Columns["Price"].HeaderText = "Ár";
                dgvProducts.Columns["CreatedDate"].HeaderText = "Létrehozás dátuma";

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
                MessageBox.Show($"Hiba a termékek betöltésekor: {ex.Message}",
                    "Betöltési hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show($"Hiba a rendezés során: {ex.Message}",
                    "Rendezési hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("A termék neve kötelezõ.", "Érvényesítési hiba",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtStockQuantity.Text))
                {
                    MessageBox.Show("A készlet mennyisége kötelezõ.", "Érvényesítési hiba",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStockQuantity.Focus();
                    return;
                }

                // BUG #1: Missing price validation - will crash on empty price!
                // Original validation commented out:
                /*
                if (string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    MessageBox.Show("Az ár kötelezõ.", "Érvényesítési hiba",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrice.Focus();
                    return;
                }
                */

                // Parse numeric values with error handling
                if (!int.TryParse(txtStockQuantity.Text.Trim(), out int stockQuantity))
                {
                    MessageBox.Show($"Érvénytelen készlet mennyiség: '{txtStockQuantity.Text}'. Kérjük, adjon meg egy érvényes számot.",
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
                    MessageBox.Show($"Érvénytelen ár: '{txtPrice.Text}'. Kérjük, adjon meg egy érvényes decimális számot.",
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
                    MessageBox.Show("A készlet mennyisége nem lehet negatív.", "Érvényesítési hiba",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStockQuantity.Focus();
                    return;
                }
                */

                if (price <= 0)
                {
                    MessageBox.Show("Az ár nullánál nagyobb kell legyen.", "Érvényesítési hiba",
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

                MessageBox.Show("A termék sikeresen hozzáadva!", "Siker",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // BUG #1: Poor error handling - shows technical details to user
                MessageBox.Show($"Hiba a termék hozzáadásakor: {ex.Message}",
                    "Termék hozzáadási hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show($"Hiba a termék kiválasztásakor: {ex.Message}",
                    "Kiválasztási hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtProductName_TextChanged(object sender, EventArgs e) { }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentProduct != null)
                {
                    var result = MessageBox.Show($"Biztos, hogy törölni szeretné a(z) '{_currentProduct.Name}' terméket?",
                        "Törlés megerõsítése", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        _productRepository.DeleteProduct(_currentProduct.Id);

                        // BUG #5: Missing LoadProducts() call - list won't refresh after deletion!
                        // LoadProducts(); // This should be uncommented for fix

                        ClearForm();
                        _currentProduct = null;

                        MessageBox.Show("A termék sikeresen törölve!", "Siker",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Kérjük, válasszon ki egy terméket a törléshez.", "Nincs kiválasztás",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba a termék törlésekor: {ex.Message}",
                    "Törlési hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentProduct == null)
                {
                    MessageBox.Show("Kérjük, válasszon ki egy terméket a frissítéshez.", "Nincs kiválasztás",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate input fields
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("A termék neve kötelezõ.", "Érvényesítési hiba",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtStockQuantity.Text))
                {
                    MessageBox.Show("A készlet mennyisége kötelezõ.", "Érvényesítési hiba",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStockQuantity.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    MessageBox.Show("Az ár kötelezõ.", "Érvényesítési hiba",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrice.Focus();
                    return;
                }

                // Parse numeric values with error handling
                if (!int.TryParse(txtStockQuantity.Text.Trim(), out int stockQuantity))
                {
                    MessageBox.Show($"Érvénytelen készlet mennyiség: '{txtStockQuantity.Text}'. Kérjük, adjon meg egy érvényes számot.",
                        "Beviteli hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtStockQuantity.Focus();
                    txtStockQuantity.SelectAll();
                    return;
                }

                if (!decimal.TryParse(txtPrice.Text.Trim(), out decimal price))
                {
                    MessageBox.Show($"Érvénytelen ár: '{txtPrice.Text}'. Kérjük, adjon meg egy érvényes decimális számot.",
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
                    MessageBox.Show("A készlet mennyisége nem lehet negatív.", "Érvényesítési hiba",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStockQuantity.Focus();
                    return;
                }
                */

                if (price <= 0)
                {
                    MessageBox.Show("Az ár nullánál nagyobb kell legyen.", "Érvényesítési hiba",
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

                MessageBox.Show("A termék sikeresen frissítve!", "Siker",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba a termék frissítésekor: {ex.Message}",
                    "Frissítési hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show($"Hiba az ûrlap törlésekor: {ex.Message}",
                    "Törlési hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                System.Console.WriteLine($"Hiba az ûrlap törlésekor: {ex.Message}");
            }
        }
    }
}