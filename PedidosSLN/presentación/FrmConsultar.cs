using CarpinteriaApp.datos;
using RecetasSLN.datos;
using RecetasSLN.datos.DTOs;
using RecetasSLN.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RecetasSLN.presentación
{
    public partial class FrmConsultar : Form
    {
        HelperDB helper;
        List<Pedido> lPedidos = new List<Pedido>();
        public FrmConsultar()
        {
            InitializeComponent();
            helper = new HelperDB();
        }

        private void FrmConsultar_Load(object sender, EventArgs e)
        {
            CargarCombo();
            lblTotal.Text = "Total de pedidos: 0";

        }

        private void CargarCombo()
        {
            var lista = new List<Parametro>();
            DataTable dt = new DataTable();
            dt = helper.ConsultaSQL("[SP_CONSULTAR_CLIENTES]", lista);
            cboClientes.DataSource = dt;
            cboClientes.ValueMember = dt.Columns[0].ColumnName;
            cboClientes.DisplayMember = dt.Columns[1].ColumnName;
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            
            var listaParams = new List<Parametro>();
            listaParams.Add(new Parametro("@cliente", cboClientes.SelectedValue));
            listaParams.Add(new Parametro("fecha_desde", dtpDesde.Value));
            listaParams.Add(new Parametro("fecha_hasta", dtpHasta.Value));
            DataTable dt = helper.ConsultaSQL("SP_CONSULTAR_PEDIDOS", listaParams);
            dgvPedidos.Rows.Clear();
            int consultas = 0;
            foreach (DataRow fila in dt.Rows)
            {
                consultas += 1;
                dgvPedidos.Rows.Add(new object[] { fila["codigo"].ToString(),fila["cliente"].ToString(),((DateTime)fila["fec_entrega"]).ToShortDateString()});
            }
            lblTotal.Text = "Total de pedidos: "+ consultas.ToString();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvPedidos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPedidos.CurrentCell.ColumnIndex == 3)
            {
                var lParametros = new List<Parametro>();
                lParametros.Add(new Parametro("@codigo", dgvPedidos.CurrentRow.Cells[0]));
                helper.ConsultaSQL("SP_REGISTRAR_ENTREGA", lParametros);


            }
        }
    }
}
