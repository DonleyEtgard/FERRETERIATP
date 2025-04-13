using FERRETERIATP.TABLAS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FERRETERIATP
{
    public partial class FrmFerreteria : Form
    {
        public FrmFerreteria()
        {
            InitializeComponent();
        }

        private void pRODUCTOSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtUsuarios.Text == "ferreteria00" && txtPassword.Text == "12345")
            {
                FrmProductos productos= new FrmProductos();
                productos.ShowDialog();
            }
            else 
            {
                MessageBox.Show("HAY QUE INGRESAR BIEN LOS DATOS");
            }
        }

        private void pROVEEDORESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtUsuarios.Text == "ferreteria00" && txtPassword.Text == "12345")
            {
                FrmProveedores proveedores = new FrmProveedores();
                proveedores.ShowDialog();
            }
            else
            {
                MessageBox.Show("HAY QUE INGRESAR BIEN LOS DATOS");
            }
        }

        private void FrmFerreteria_Load(object sender, EventArgs e)
        {
 
        }
    }
}
