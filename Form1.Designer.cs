namespace SGC
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblInstrucoes = new System.Windows.Forms.Label();
            this.lblCompetencia = new System.Windows.Forms.Label();
            this.txtCompetencia = new System.Windows.Forms.TextBox();
            this.btnProcessar = new System.Windows.Forms.Button();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();

            // lblTitulo
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitulo.Location = new System.Drawing.Point(120, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(268, 30);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "PROCESSADOR DE NFC-e";

            // lblInstrucoes
            this.lblInstrucoes.AutoSize = true;
            this.lblInstrucoes.Location = new System.Drawing.Point(20, 60);
            this.lblInstrucoes.Name = "lblInstrucoes";
            this.lblInstrucoes.Size = new System.Drawing.Size(251, 30);
            this.lblInstrucoes.TabIndex = 1;
            this.lblInstrucoes.Text = "Diretório alvo: C:\\XmlNfce\nFiltro: Modelo 65 (Consumidor Final)";

            // lblCompetencia
            // AQUI FOI APLICADA A REGRA DO PDF (Padrão Nacional pt-BR)
            this.lblCompetencia.AutoSize = true;
            this.lblCompetencia.Location = new System.Drawing.Point(20, 110);
            this.lblCompetencia.Name = "lblCompetencia";
            this.lblCompetencia.Size = new System.Drawing.Size(142, 15);
            this.lblCompetencia.TabIndex = 4;
            this.lblCompetencia.Text = "Competência (MM/AAAA):";

            // txtCompetencia
            this.txtCompetencia.Location = new System.Drawing.Point(175, 107);
            this.txtCompetencia.Name = "txtCompetencia";
            this.txtCompetencia.Size = new System.Drawing.Size(85, 23);
            this.txtCompetencia.TabIndex = 5;

            // btnProcessar
            this.btnProcessar.BackColor = System.Drawing.Color.LightGreen;
            this.btnProcessar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnProcessar.Location = new System.Drawing.Point(20, 150);
            this.btnProcessar.Name = "btnProcessar";
            this.btnProcessar.Size = new System.Drawing.Size(440, 40);
            this.btnProcessar.TabIndex = 6;
            this.btnProcessar.Text = "Processar XMLs";
            this.btnProcessar.UseVisualStyleBackColor = false;
            this.btnProcessar.Click += new System.EventHandler(this.btnProcessar_Click);

            // rtbLog
            this.rtbLog.BackColor = System.Drawing.Color.Black;
            this.rtbLog.ForeColor = System.Drawing.Color.LimeGreen;
            this.rtbLog.Location = new System.Drawing.Point(20, 200);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.Size = new System.Drawing.Size(440, 150);
            this.rtbLog.TabIndex = 7;
            this.rtbLog.Text = "Aguardando início...\n";

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 370);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.btnProcessar);
            this.Controls.Add(this.txtCompetencia);
            this.Controls.Add(this.lblCompetencia);
            this.Controls.Add(this.lblInstrucoes);
            this.Controls.Add(this.lblTitulo);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Processador de NFC-e - Projeto Caridade";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblInstrucoes;
        private System.Windows.Forms.Label lblCompetencia;
        private System.Windows.Forms.TextBox txtCompetencia;
        private System.Windows.Forms.Button btnProcessar;
        private System.Windows.Forms.RichTextBox rtbLog;
    }
}