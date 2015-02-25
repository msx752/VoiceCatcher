namespace VoiceCatcher1
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli designer değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        ///Designer desteği için gerekli metottur; bu metodun
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblListen = new System.Windows.Forms.Label();
            this.lblListenOk = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(329, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "0";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 12);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(300, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(12, 42);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(300, 60);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // lblListen
            // 
            this.lblListen.AutoSize = true;
            this.lblListen.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblListen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblListen.Location = new System.Drawing.Point(14, 106);
            this.lblListen.Name = "lblListen";
            this.lblListen.Size = new System.Drawing.Size(81, 17);
            this.lblListen.TabIndex = 3;
            this.lblListen.Text = "Dinleniyor";
            this.lblListen.Visible = false;
            // 
            // lblListenOk
            // 
            this.lblListenOk.AutoSize = true;
            this.lblListenOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblListenOk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lblListenOk.Location = new System.Drawing.Point(193, 106);
            this.lblListenOk.Name = "lblListenOk";
            this.lblListenOk.Size = new System.Drawing.Size(119, 17);
            this.lblListenOk.TabIndex = 3;
            this.lblListenOk.Text = "Komut Anlaşıldı";
            this.lblListenOk.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 134);
            this.Controls.Add(this.lblListenOk);
            this.Controls.Add(this.lblListen);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblListen;
        private System.Windows.Forms.Label lblListenOk;
    }
}

