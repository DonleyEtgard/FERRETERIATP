using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FERRETERIATP.TABLAS
{
    public partial class FrmProductos : Form
    {
        decimal totalPrecio = 0;
        int cantidadRestante = 0;

        public FrmProductos()
        {
            InitializeComponent();
            dgvProductos.SelectionChanged += dgvProductos_SelectionChanged;
            CargarDatos();
        }

        private void FrmProductos_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void CargarDatos()
        {
            using (FERRETERIAEntitidades0 dt = new FERRETERIAEntitidades0())
            {
                dgvProductos.DataSource = dt.PRODUCTOS.ToList();
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                using (FERRETERIAEntitidades0 dt = new FERRETERIAEntitidades0())
                {
                    // Validación de campos
                    if (int.TryParse(NumProductosID.Value.ToString(), out int productoID) &&
                        decimal.TryParse(txtPrecios.Text, out decimal precio) &&
                        int.TryParse(txtCantProductos.Text, out int cantidad))
                    {
                        var producto = new PRODUCTOS
                        {
                            PRODUCTOID = productoID,
                            PRODUCTO = txtProductos.Text,
                            PRECIOS = precio,
                            CANTPRODUCTO = cantidad,
                            DESCRIPCION = txtDescripcion.Text
                        };

                        dt.PRODUCTOS.Add(producto);
                        dt.SaveChanges();
                        MessageBox.Show("Producto agregado con éxito.");
                    }
                    else
                    {
                        MessageBox.Show("Por favor, ingrese valores válidos.");
                    }
                }
                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductos.CurrentRow != null &&
                    int.TryParse(dgvProductos.CurrentRow.Cells["PRODUCTOID"].Value.ToString(), out int productoID))
                {
                    using (FERRETERIAEntitidades0 dt = new FERRETERIAEntitidades0())
                    {
                        var producto = dt.PRODUCTOS.Find(productoID);
                        if (producto != null)
                        {
                            // Validación de campos
                            if (decimal.TryParse(txtPrecios.Text, out decimal precio) &&
                                int.TryParse(txtCantProductos.Text, out int cantidad))
                            {
                                producto.PRODUCTO = txtProductos.Text;
                                producto.PRECIOS = precio;
                                producto.CANTPRODUCTO = cantidad;
                                producto.DESCRIPCION = txtDescripcion.Text;

                                dt.Entry(producto).State = EntityState.Modified;
                                dt.SaveChanges();
                                MessageBox.Show("Producto modificado con éxito.");
                            }
                            else
                            {
                                MessageBox.Show("Por favor, ingrese valores válidos.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Producto no encontrado.");
                        }
                    }
                    CargarDatos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductos.CurrentRow != null &&
                    int.TryParse(dgvProductos.CurrentRow.Cells["PRODUCTOID"].Value.ToString(), out int productoID))
                {
                    using (FERRETERIAEntitidades0 dt = new FERRETERIAEntitidades0())
                    {
                        var producto = dt.PRODUCTOS.Find(productoID);
                        if (producto != null)
                        {
                            dt.PRODUCTOS.Remove(producto);
                            dt.SaveChanges();
                            MessageBox.Show("Producto eliminado con éxito.");
                        }
                        else
                        {
                            MessageBox.Show("Producto no encontrado.");
                        }
                    }
                    CargarDatos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (int.TryParse(NumProductosID.Value.ToString(), out int productoID))
                {
                    using (FERRETERIAEntitidades0 dt = new FERRETERIAEntitidades0())
                    {
                       
                        var producto = dt.PRODUCTOS
                                         .Where(p => p.PRODUCTOID == productoID)
                                         .ToList();

                        if (producto.Any())
                        {
                            dgvProductos.DataSource = producto;
                        }
                        else
                        {
                            MessageBox.Show("No se encontró ningún producto con ese ID.");
                            CargarDatos(); // Refresca los datos si no se encuentran resultados
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, ingrese un ID de producto válido.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar producto: " + ex.Message);
            }
        }

        private void dgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow != null)
            {
                // Verifica si la celda tiene un valor antes de asignarlo a los TextBoxes
                if (dgvProductos.CurrentRow.Cells["PRODUCTO"].Value != null)
                    txtProductos.Text = dgvProductos.CurrentRow.Cells["PRODUCTO"].Value.ToString();

                if (dgvProductos.CurrentRow.Cells["PRODUCTOID"].Value != null)
                    NumProductosID.Value = Convert.ToInt32(dgvProductos.CurrentRow.Cells["PRODUCTOID"].Value);

                if (dgvProductos.CurrentRow.Cells["PRECIOS"].Value != null)
                    txtPrecios.Text = dgvProductos.CurrentRow.Cells["PRECIOS"].Value.ToString();

                if (dgvProductos.CurrentRow.Cells["CANTPRODUCTO"].Value != null)
                    txtCantProductos.Text = dgvProductos.CurrentRow.Cells["CANTPRODUCTO"].Value.ToString();

                if (dgvProductos.CurrentRow.Cells["DESCRIPCION"].Value != null)
                    txtDescripcion.Text = dgvProductos.CurrentRow.Cells["DESCRIPCION"].Value.ToString();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtCantProductos.Clear();
            txtPrecios.Clear();
            txtProductos.Clear();
            NumProductosID.Value = 0;
            txtDescripcion.Clear();
        }

        private void chckVenta_CheckedChanged(object sender, EventArgs e)
        {
            if (chckVenta.Checked && dgvProductos.CurrentRow != null)
            {
                decimal precioProducto = Convert.ToDecimal(dgvProductos.CurrentRow.Cells["PRECIOS"].Value);
                int cantidadProducto = Convert.ToInt32(dgvProductos.CurrentRow.Cells["CANTPRODUCTO"].Value);

                if (cantidadProducto > 0)
                {
                    totalPrecio += precioProducto;
                    cantidadRestante = cantidadProducto - 1;

                    dgvProductos.CurrentRow.Cells["CANTPRODUCTO"].Value = cantidadRestante;

                    // Actualizar la cantidad en la base de datos
                    using (FERRETERIAEntitidades0 dt = new FERRETERIAEntitidades0())
                    {
                        var producto = dt.PRODUCTOS.Find(Convert.ToInt32(dgvProductos.CurrentRow.Cells["PRODUCTOID"].Value));
                        if (producto != null)
                        {
                            producto.CANTPRODUCTO = cantidadRestante;
                            dt.Entry(producto).State = EntityState.Modified;
                            dt.SaveChanges();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No hay suficiente cantidad disponible.");
                    chckVenta.Checked = false;
                }
            }
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow != null)
            {
                string nombreProducto = dgvProductos.CurrentRow.Cells["PRODUCTO"].Value.ToString();
                MessageBox.Show($"Producto: {nombreProducto}\n" +
                                $"Cantidad Restante: {cantidadRestante}\n" +
                                $"Total Acumulado: {totalPrecio.ToString("C")}",
                                "Información del Producto");
            }
            else
            {
                MessageBox.Show("Seleccione un producto.");
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Este evento se encuentra vacío. Si no lo necesitas, puedes eliminarlo.
        }
    }
}

