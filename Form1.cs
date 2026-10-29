using SGC.service;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SGC
{
    public partial class Form1 : Form
    {
        private Label lblTitulo;
        private Label lblInstrucoes;
        private Label lblSerie;
        private TextBox txtSerie;
        private Label lblCompetencia;
        private TextBox txtCompetencia;
        private Button btnProcessar;
        private RichTextBox rtbLog;

        public Form1()
        {
            InitializeComponent();
            ConfigurarTela();
        }

        private void ConfigurarTela()
        {

            this.Text = "Processador de NFC-e - Projeto Caridade";
            this.Size = new Size(500, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Titulo
            lblTitulo = new Label();
            lblTitulo.Text = "PROCESSADOR DE NFC-e";
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.Location = new Point(120, 15);
            lblTitulo.AutoSize = true;
            this.Controls.Add(lblTitulo);

            // Instruções
            lblInstrucoes = new Label();
            lblInstrucoes.Text = "Diretório alvo: C:\\XmlNfce\nFiltro: Modelo 65 (Consumidor Final)";
            lblInstrucoes.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblInstrucoes.Location = new Point(20, 60);
            lblInstrucoes.AutoSize = true;
            this.Controls.Add(lblInstrucoes);

            //Campo Competência
            lblCompetencia = new Label();
            lblCompetencia.Text = "Competência (AAAA-MM):";
            lblCompetencia.Location = new Point(10, 120);
            lblCompetencia.AutoSize = true;
            this.Controls.Add(lblCompetencia);

            txtCompetencia = new TextBox();
            txtCompetencia.Location = new Point(200, 120);
            txtCompetencia.Width = 85;
            this.Controls.Add(txtCompetencia);

            //Botão de Processar
            btnProcessar = new Button();
            btnProcessar.Text = "Processar XMLs";
            btnProcessar.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnProcessar.Location = new Point(20, 190);
            btnProcessar.Size = new Size(440, 40);
            btnProcessar.BackColor = Color.LightGreen;
            btnProcessar.Click += BtnProcessar_Click; 
            this.Controls.Add(btnProcessar);

            // Caixa de Log (Feedback visual)
            rtbLog = new RichTextBox();
            rtbLog.Location = new Point(20, 240);
            rtbLog.Size = new Size(440, 150);
            rtbLog.ReadOnly = true;
            rtbLog.BackColor = Color.Black;
            rtbLog.ForeColor = Color.LimeGreen;
            rtbLog.Text = "Aguardando início...\n";
            this.Controls.Add(rtbLog);
        }

        private void BtnProcessar_Click(object sender, EventArgs e)
        {
 
            string competencia = txtCompetencia.Text.Trim();

            if (string.IsNullOrEmpty(competencia))
            {
                MessageBox.Show("Por favor, a Competência.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            rtbLog.Clear();
            rtbLog.AppendText("[INFO] Iniciando processamento...\n");
            rtbLog.AppendText($"[INFO] Competência: {competencia}\n\n");

            try
            {
                string logDoBackEnd = Leitorxml.lerxml(competencia);
                rtbLog.AppendText(logDoBackEnd);
                rtbLog.AppendText("\n[SUCESSO] Processamento finalizado!\n");
            }
            catch (Exception ex)
            {
                rtbLog.AppendText($"[ERRO FATAL] {ex.Message}\n");
                MessageBox.Show($"Ocorreu um erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}