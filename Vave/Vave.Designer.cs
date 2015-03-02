namespace Vave
{
    partial class Vave
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
            this.lstResponseBox = new System.Windows.Forms.TextBox();
            this.procMicrophoneLevel = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.picWaveWiever = new System.Windows.Forms.PictureBox();
            this.picProcessImage = new System.Windows.Forms.PictureBox();
            this.lstProcessLogBox = new System.Windows.Forms.ListView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWaveWiever)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picProcessImage)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(411, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hata / Uyarı Mesajları";
            // 
            // lstResponseBox
            // 
            this.lstResponseBox.Location = new System.Drawing.Point(15, 152);
            this.lstResponseBox.Multiline = true;
            this.lstResponseBox.Name = "lstResponseBox";
            this.lstResponseBox.Size = new System.Drawing.Size(396, 221);
            this.lstResponseBox.TabIndex = 2;
            // 
            // procMicrophoneLevel
            // 
            this.procMicrophoneLevel.Location = new System.Drawing.Point(4, 19);
            this.procMicrophoneLevel.Name = "procMicrophoneLevel";
            this.procMicrophoneLevel.Size = new System.Drawing.Size(274, 23);
            this.procMicrophoneLevel.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Gelen Cevaplar";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.picWaveWiever);
            this.groupBox1.Controls.Add(this.picProcessImage);
            this.groupBox1.Controls.Add(this.procMicrophoneLevel);
            this.groupBox1.Location = new System.Drawing.Point(15, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(390, 118);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mikrofon İstatistikleri";
            // 
            // picWaveWiever
            // 
            this.picWaveWiever.Location = new System.Drawing.Point(4, 48);
            this.picWaveWiever.Name = "picWaveWiever";
            this.picWaveWiever.Size = new System.Drawing.Size(274, 60);
            this.picWaveWiever.TabIndex = 4;
            this.picWaveWiever.TabStop = false;
            // 
            // picProcessImage
            // 
            this.picProcessImage.ErrorImage = null;
            this.picProcessImage.Location = new System.Drawing.Point(281, 11);
            this.picProcessImage.Name = "picProcessImage";
            this.picProcessImage.Size = new System.Drawing.Size(100, 100);
            this.picProcessImage.TabIndex = 5;
            this.picProcessImage.TabStop = false;
            // 
            // lstProcessLogBox
            // 
            this.lstProcessLogBox.Location = new System.Drawing.Point(414, 36);
            this.lstProcessLogBox.Margin = new System.Windows.Forms.Padding(0);
            this.lstProcessLogBox.Name = "lstProcessLogBox";
            this.lstProcessLogBox.Size = new System.Drawing.Size(200, 337);
            this.lstProcessLogBox.TabIndex = 7;
            this.lstProcessLogBox.UseCompatibleStateImageBehavior = false;
            this.lstProcessLogBox.View = System.Windows.Forms.View.Tile;
            this.lstProcessLogBox.ItemActivate += new System.EventHandler(this.ProcessLogBox_ItemActivate);
            // 
            // Vave
            // 
            this.Icon = Properties.Resources.audio_wave_;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(619, 380);
            this.Controls.Add(this.lstProcessLogBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstResponseBox);
            this.Name = "Vave";
            this.Text = "Vave";
            this.Load += new System.EventHandler(this.Vave_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picWaveWiever)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picProcessImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox lstResponseBox;
        private System.Windows.Forms.ProgressBar procMicrophoneLevel;
        private System.Windows.Forms.PictureBox picWaveWiever;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox picProcessImage;
        private System.Windows.Forms.ListView lstProcessLogBox;
    }
}

