using SGC.service;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SGC
{
    public partial class Form1 : Form
    {
        Leitorxml leitorxml = new Leitorxml();
        public Form1()
        {
            InitializeComponent();

        }

        private void btnProcessar_Click(object sender, EventArgs e)
        {
            string competencia = txtCompetencia.Text.Trim();

            if (!Regex.IsMatch(competencia, @"^(0[1-9]|1[0-2])/\d{4}$"))
            {
                MessageBox.Show("Por favor, preencha a Competência no formato correto (ex: 01/2026).", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string logDoBackEnd = leitorxml.ProcessarNf(competencia);
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }
}