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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            lblTitulo = new Label();
            lblInstrucoes = new Label();
            lblCompetencia = new Label();
            txtCompetencia = new TextBox();
            btnProcessar = new Button();
            rtbLog = new RichTextBox();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.Location = new Point(137, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(336, 37);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "PROCESSADOR DE NFC-e";
            // 
            // lblInstrucoes
            // 
            lblInstrucoes.AutoSize = true;
            lblInstrucoes.Location = new Point(23, 93);
            lblInstrucoes.Name = "lblInstrucoes";
            lblInstrucoes.Size = new Size(251, 40);
            lblInstrucoes.TabIndex = 1;
            lblInstrucoes.Text = "Diretório alvo: C:\\XmlNfce\nFiltro: Modelo 65 (Consumidor Final)";
            // 
            // lblCompetencia
            // 
            lblCompetencia.AutoSize = true;
            lblCompetencia.Location = new Point(23, 147);
            lblCompetencia.Name = "lblCompetencia";
            lblCompetencia.Size = new Size(186, 20);
            lblCompetencia.TabIndex = 4;
            lblCompetencia.Text = "Competência (MM/AAAA):";
            // 
            // txtCompetencia
            // 
            txtCompetencia.BorderStyle = BorderStyle.FixedSingle;
            txtCompetencia.Location = new Point(215, 145);
            txtCompetencia.Margin = new Padding(3, 4, 3, 4);
            txtCompetencia.Name = "txtCompetencia";
            txtCompetencia.Size = new Size(176, 27);
            txtCompetencia.TabIndex = 5;
            // 
            // btnProcessar
            // 
            btnProcessar.BackColor = Color.DarkBlue;
            btnProcessar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnProcessar.ForeColor = Color.White;
            btnProcessar.Location = new Point(23, 200);
            btnProcessar.Margin = new Padding(3, 4, 3, 4);
            btnProcessar.Name = "btnProcessar";
            btnProcessar.Size = new Size(503, 53);
            btnProcessar.TabIndex = 6;
            btnProcessar.Text = "Processar XMLs";
            btnProcessar.UseVisualStyleBackColor = false;
            btnProcessar.Click += btnProcessar_Click;
            // 
            // rtbLog
            // 
            rtbLog.BackColor = Color.DarkBlue;
            rtbLog.ForeColor = Color.Linen;
            rtbLog.Location = new Point(23, 267);
            rtbLog.Margin = new Padding(3, 4, 3, 4);
            rtbLog.Name = "rtbLog";
            rtbLog.ReadOnly = true;
            rtbLog.Size = new Size(502, 199);
            rtbLog.TabIndex = 7;
            rtbLog.Text = "Aguardando início...\n";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.White;
            pictureBox1.BackgroundImageLayout = ImageLayout.Center;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.InitialImage = (Image)resources.GetObject("pictureBox1.InitialImage");
            pictureBox1.Location = new Point(3, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(100, 90);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(569, 493);
            Controls.Add(pictureBox1);
            Controls.Add(rtbLog);
            Controls.Add(btnProcessar);
            Controls.Add(txtCompetencia);
            Controls.Add(lblCompetencia);
            Controls.Add(lblInstrucoes);
            Controls.Add(lblTitulo);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Processador de NFC-e - Projeto Caridade";
            Load += Form1_Load_1;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblInstrucoes;
        private System.Windows.Forms.Label lblCompetencia;
        private System.Windows.Forms.TextBox txtCompetencia;
        private System.Windows.Forms.Button btnProcessar;
        private System.Windows.Forms.RichTextBox rtbLog;
        private PictureBox pictureBox1;
    }
}