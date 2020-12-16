using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Collections;
using System.Text.RegularExpressions;

namespace FaleConosco_System
{
    public partial class FaleConoscoForm : Form
    {
        public FaleConoscoForm()
        {
            InitializeComponent();
            CarregarGrid();
            rbtPendentes.Checked = true;
            
        }

        private void CarregarGrid()
        {
            BLL.FaleConosco fale = new BLL.FaleConosco();
            dgvFale.DataSource = fale.Listar().Tables[0];
        }

        private void rbtPendentes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtPendentes.Checked)
            {
                BLL.FaleConosco fale = new BLL.FaleConosco();
                dgvFale.DataSource = fale.ListarPendentes().Tables[0];
                Joker();
                txtResposta.Enabled = true;
            }
            
        }

        private void PegaValor()
        {
            if (dgvFale.SelectedRows.Count == 1)
            {
                DataRowView dr = (DataRowView)dgvFale.Rows[dgvFale.SelectedRows[0].Index].DataBoundItem;
                lblSituacao.Text = dr["SITUACAO"].ToString();
                lblNome.Text = dr["NOME"].ToString();
                lblEmail.Text = dr["EMAIL"].ToString();
                txtMensagem.Text = dr["MENSAGEM"].ToString();
                txtResposta.Text = dr["RESPOSTA"].ToString();
            }
        }

        private void rbtRespondidas_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtRespondidas.Checked)
            {
                BLL.FaleConosco fale = new BLL.FaleConosco();
                dgvFale.DataSource = fale.ListarRespondidas().Tables[0];
                Joker();
                txtResposta.Enabled = false;

            }
        }

        private void rbtTodas_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtTodas.Checked)
            {
                BLL.FaleConosco fale = new BLL.FaleConosco();
                dgvFale.DataSource = fale.Listar().Tables[0];
                Joker();
                txtResposta.Enabled = false;
                
            }

        }

        private void Joker()
        {
            lblEmail.Text = "";
            lblNome.Text = "";
            lblSituacao.Text = "";
            txtMensagem.Text = "";
            txtResposta.Text = "";
        }

        private void EnviarEmail()
        {
            string email = "EMAIL";
            string senha = "SENHA";
            MailMessage mm = new MailMessage();
            //Email
            mm.From = new MailAddress(email);
            mm.To.Add(lblEmail.Text);
            mm.Subject = "Não responda";
            AlternateView aw = AlternateView.CreateAlternateViewFromString(txtResposta.Text, null, "text/html");
            mm.AlternateViews.Add(aw);

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.live.com";
            smtp.Port = 587;
            //Email e Senha
            NetworkCredential nc = new NetworkCredential(email, senha);
            smtp.EnableSsl = true;
            smtp.Credentials = nc;
            smtp.Send(mm);

            MessageBox.Show("Email enviado para " + lblEmail.Text + " [" + DateTime.Now.ToString() + "].");

        }
        private void Cadastra()
        {
            for (int i = 0; i < dgvFale.Rows.Count - 0;)
            {
                BLL.FaleConosco fale = new BLL.FaleConosco();
                fale.resposta = txtResposta.Text;
                fale.id = Convert.ToInt32(dgvFale.SelectedRows[i].Cells[0].Value);
                fale.Atualizar();

                lblSituacao.Text = "RESPONDIDA";

                MessageBox.Show("CLIENTE RESPONDIDO!", "SUCESSO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CarregarGrid();
                rbtRespondidas.Checked = true;
                break;
            }
        }

        private void dgvFale_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            PegaValor();
            if (lblSituacao.Text == "PENDENTE")
            {
                txtResposta.Enabled = true;
                return;
            }
            if (lblSituacao.Text == "RESPONDIDA")
            {
                txtResposta.Enabled = false;
                return;
            }
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(lblNome.Text))
            {
                MessageBox.Show("SELECIONE ALGUM CLIENTE", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (lblSituacao.Text == "RESPONDIDA")
            {
                MessageBox.Show("CLIENTE JÁ RESPONDIDO", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (String.IsNullOrEmpty(txtResposta.Text))
            {
                MessageBox.Show("DIGITE A RESPOSTA", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
                
            }
            else
            {

                try
                {
                    EnviarEmail();
                }
                catch
                {

                    if (DialogResult.OK == MessageBox.Show("Ocorreu um erro ao responder o cliente. Recomenda-se fechar a aplicação", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error))
                    {

                        Application.Exit();

                    }

                }

                Cadastra();

            }

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Joker();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
