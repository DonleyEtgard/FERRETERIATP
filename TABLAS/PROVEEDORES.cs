using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity.Infrastructure;

namespace FERRETERIATP.TABLAS
{
    public partial class FrmProveedores : Form
    {
        public FrmProveedores()
        {
            InitializeComponent();
            dgvProveedores.SelectionChanged += dgvProveedores_SelectionChanged;
            CargarDatos();   
        }

        private void FrmProveedores_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }
        private void CargarDatos()
        {
            using (FERRETERIAEntities3 dt = new FERRETERIAEntities3 ())
            {
                dgvProveedores.DataSource = dt.PROVEEDORES.ToList();
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                using (FERRETERIAEntities3 dbContext = new FERRETERIAEntities3())
                {
                    // Validación de campos antes de agregar
                    if (int.TryParse(NumProveedoresID.Value.ToString(), out int proveedorID) &&
                        int.TryParse(NumProductosID.Value.ToString(), out int productoID) &&
                        decimal.TryParse(txtTotalComprado.Text, out decimal totalComprado) &&
                        !string.IsNullOrWhiteSpace(txtProveedores.Text) &&
                        !string.IsNullOrWhiteSpace(txtTelefono.Text) &&
                        !string.IsNullOrWhiteSpace(txtDireccion.Text))
                    {
                        DateTime fechaEntrega = dateTimePicker.Value;

                        var nuevoProveedor = new PROVEEDORES
                        {
                            PROVEEDORID = proveedorID,
                            PROVEEDOR = txtProveedores.Text, // Asegúrate de que este nombre sea correcto
                            PRODUCTOID = productoID,
                            TELEFONO = txtTelefono.Text,
                            DIRECCION = txtDireccion.Text,
                            FECHAENTREGA = fechaEntrega,
                            TOTALCOMPRADO = totalComprado
                        };

                        dbContext.PROVEEDORES.Add(nuevoProveedor);
                        dbContext.SaveChanges();
                        MessageBox.Show("Proveedor agregado con éxito.");

                        // Cargar datos dentro del using
                        CargarDatos();
                    }
                    else
                    {
                        MessageBox.Show("Por favor, ingrese valores válidos en todos los campos.");
                    }
                }
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
                if (dgvProveedores.CurrentRow != null &&
                    int.TryParse(dgvProveedores.CurrentRow.Cells["PROVEEDORID"].Value?.ToString(), out int proveedorID))
                {
                    using (FERRETERIAEntities3 dbContext = new FERRETERIAEntities3())
                    {
                        var proveedorExistente = dbContext.PROVEEDORES.Find(proveedorID);

                        if (proveedorExistente != null)
                        {
                            if (int.TryParse(NumProductosID.Value.ToString(), out int productoID) &&
                                decimal.TryParse(txtTotalComprado.Text, out decimal totalComprado))
                            {
                                proveedorExistente.PROVEEDOR = txtProveedores.Text;
                                proveedorExistente.PRODUCTOID = productoID;
                                proveedorExistente.TELEFONO = txtTelefono.Text;
                                proveedorExistente.DIRECCION = txtDireccion.Text;
                                proveedorExistente.FECHAENTREGA = dateTimePicker.Value;
                                proveedorExistente.TOTALCOMPRADO = totalComprado;

                                dbContext.Entry(proveedorExistente).State = EntityState.Modified;
                                dbContext.SaveChanges();
                                MessageBox.Show("Proveedor modificado con éxito.");
                            }
                            else
                            {
                                MessageBox.Show("Por favor, ingrese valores válidos.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Proveedor no encontrado.");
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
                if (dgvProveedores.CurrentRow != null &&
                    int.TryParse(dgvProveedores.CurrentRow.Cells["PROVEEDORID"].Value.ToString(), out int proveedorID))
                {
                    using (var dt = new FERRETERIAEntities3())
                    {
                        var proveedor = dt.PROVEEDORES.Find(proveedorID);
                        if (proveedor != null)
                        {
                            dt.PROVEEDORES.Remove(proveedor);
                            dt.SaveChanges();
                            MessageBox.Show("Proveedor eliminado con éxito.");
                        }
                        else
                        {
                            MessageBox.Show("Proveedor no encontrado.");
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

                if (int.TryParse(NumProductosID.Value.ToString(), out int proveedorID))
                {
                    using (FERRETERIAEntities3 dt = new FERRETERIAEntities3())
                    {

                        var producto = dt.PROVEEDORES
                                         .Where(p => p.PROVEEDORID == proveedorID)
                                         .ToList();

                        if (producto.Any())
                        {
                            dgvProveedores.DataSource = producto;
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
        private void dgvProveedores_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProveedores.CurrentRow != null)
            {

                if (dgvProveedores.CurrentRow.Cells["PROVEEDOR"].Value != null)
                {
                    txtProveedores.Text = dgvProveedores.CurrentRow.Cells["PROVEEDOR"].Value.ToString();
                }

                if (dgvProveedores.CurrentRow.Cells["PROVEEDORID"].Value != null)
                {
                    NumProveedoresID.Value = Convert.ToInt32(dgvProveedores.CurrentRow.Cells["PROVEEDORID"].Value.ToString());
                }

                if (dgvProveedores.CurrentRow.Cells["PRODUCTOID"].Value != null)
                {
                    NumProveedoresID.Value = Convert.ToInt32(dgvProveedores.CurrentRow.Cells["PRODUCTOID"].Value.ToString());
                }

                if (dgvProveedores.CurrentRow.Cells["TELEFONO"].Value != null)
                {
                    txtTelefono.Text = dgvProveedores.CurrentRow.Cells["TELEFONO"].Value.ToString();
                }

                if (dgvProveedores.CurrentRow.Cells["DIRECCION"].Value != null)
                {
                    txtDireccion.Text = dgvProveedores.CurrentRow.Cells["DIRECCION"].Value.ToString();
                }

                if (dgvProveedores.CurrentRow.Cells["FECHAENTREGA"].Value != null)
                {
                    dateTimePicker.Value = Convert.ToDateTime(dgvProveedores.CurrentRow.Cells["FECHAENTREGA"].Value);
                }
                else
                {
                    dateTimePicker.Value = DateTime.Now;
                }

                if (dgvProveedores.CurrentRow.Cells["TOTALCOMPRADO"].Value != null)
                {
                    txtTotalComprado.Text = dgvProveedores.CurrentRow.Cells["TOTALCOMPRADO"].Value.ToString();
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            NumProductosID.Value = 0;
            txtDireccion.Clear();
            txtProveedores.Clear();
            NumProveedoresID.Value = 0;
            txtTelefono.Clear();
            txtTotalComprado.Clear();

        }
        private void EnviarCorreo(string telefono, string asunto, string mensaje)
        {
            try
            {
                // Configurar cliente SMTP
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,  // Puerto SMTP de Gmail
                    Credentials = new NetworkCredential("tuemail@gmail.com", "tucontraseña"), // Tu cuenta de Gmail y contraseña
                    EnableSsl = true, // Habilitar SSL
                };

                // Crear el mensaje de correo
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("tuemail@gmail.com"), // El correo desde el que envías
                    Subject = asunto,  // Asunto del correo
                    Body = mensaje,    // Cuerpo del correo
                    IsBodyHtml = false // Definir si el cuerpo es HTML o texto plano
                };
                mail.To.Add(telefono); // Añadir destinatario

                // Enviar el correo
                smtpClient.Send(mail);

                MessageBox.Show("Correo enviado con éxito.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message);
            }
        }

        private void txtProveedores_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnPedido_Click(object sender, EventArgs e)
        {
            if (int.TryParse(NumProveedoresID.Value.ToString(), out int proveedorID))
            {
                using (var dt = new FERRETERIAEntities3())
                {
                    var proveedor = dt.PROVEEDORES
                        .Where(p => p.PROVEEDORID == proveedorID)
                        .Select(p => new
                        {
                            p.PROVEEDOR,        // Nombre del proveedor
                            p.TELEFONO,          // Teléfono del proveedor
                            p.TOTALCOMPRADO,     // Cantidad comprada
                            p.FECHAENTREGA,      // Fecha de entrega          
                        })
                        .FirstOrDefault();

                    if (proveedor != null)
                    {
                        // Información del pedido
                        string subject = "Notificación de Pedido Realizado";
                        string body = $"Estimado {proveedor.PROVEEDOR},\n\n" +
                                      $"Se ha realizado un pedido con los siguientes detalles:\n" +
                                      $"Cantidad Comprada: {proveedor.TOTALCOMPRADO}\n" +
                                      $"Fecha de Entrega: {proveedor.FECHAENTREGA?.ToShortDateString() ?? "No especificada"}\n\n" +
                                      $"Por favor confirme la recepción del pedido.";

                        // Llamar a la función que envía el correo
                        EnviarCorreo(proveedor.TELEFONO, subject, body);

                        MessageBox.Show("Correo enviado al proveedor.");
                    }
                    else
                    {
                        MessageBox.Show("No se encontró ningún proveedor con el ID proporcionado.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Ingrese un ID de proveedor válido.");
            }
        }
    }
}
