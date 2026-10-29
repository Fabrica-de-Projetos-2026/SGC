using SGC.service;
using System;
using System.Windows.Forms;

namespace SGC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnProcessar_Click(object sender, EventArgs e)
        {
            string competencia = txtCompetencia.Text.Trim();

            // Validação simples para garantir que a pessoa digitou algo no padrão MM/AAAA
            if (string.IsNullOrEmpty(competencia) || competencia.Length < 7 || !competencia.Contains("/"))
            {
                MessageBox.Show("Por favor, preencha a Competência no formato MM/AAAA (ex: 01/2026).", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            rtbLog.Clear();
            rtbLog.AppendText("[INFO] Iniciando processamento...\n");
            rtbLog.AppendText($"[INFO] Filtros aplicados - Competência: {competencia}\n\n");

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