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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Vave));
            this.procMicrophoneLevel = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDinle = new System.Windows.Forms.Button();
            this.picWaveWiever = new System.Windows.Forms.PictureBox();
            this.picProcessImage = new System.Windows.Forms.PictureBox();
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lstVideos = new System.Windows.Forms.ListBox();
            this.lstResponse = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWaveWiever)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picProcessImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            this.SuspendLayout();
            // 
            // procMicrophoneLevel
            // 
            this.procMicrophoneLevel.Location = new System.Drawing.Point(4, 16);
            this.procMicrophoneLevel.Name = "procMicrophoneLevel";
            this.procMicrophoneLevel.Size = new System.Drawing.Size(369, 10);
            this.procMicrophoneLevel.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label2.Location = new System.Drawing.Point(577, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "cevaplardan birini seçiniz";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDinle);
            this.groupBox1.Controls.Add(this.lstResponse);
            this.groupBox1.Controls.Add(this.picWaveWiever);
            this.groupBox1.Controls.Add(this.picProcessImage);
            this.groupBox1.Controls.Add(this.procMicrophoneLevel);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupBox1.Location = new System.Drawing.Point(12, 580);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(728, 100);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mikrofon İstatistikleri";
            this.groupBox1.Visible = false;
            // 
            // btnDinle
            // 
            this.btnDinle.Location = new System.Drawing.Point(384, 19);
            this.btnDinle.Margin = new System.Windows.Forms.Padding(0);
            this.btnDinle.Name = "btnDinle";
            this.btnDinle.Size = new System.Drawing.Size(75, 76);
            this.btnDinle.TabIndex = 6;
            this.btnDinle.Text = "DİNLE";
            this.btnDinle.UseVisualStyleBackColor = true;
            this.btnDinle.Click += new System.EventHandler(this.button1_Click);
            // 
            // picWaveWiever
            // 
            this.picWaveWiever.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picWaveWiever.Location = new System.Drawing.Point(4, 32);
            this.picWaveWiever.Margin = new System.Windows.Forms.Padding(0);
            this.picWaveWiever.Name = "picWaveWiever";
            this.picWaveWiever.Size = new System.Drawing.Size(369, 60);
            this.picWaveWiever.TabIndex = 4;
            this.picWaveWiever.TabStop = false;
            // 
            // picProcessImage
            // 
            this.picProcessImage.ErrorImage = null;
            this.picProcessImage.Location = new System.Drawing.Point(462, 11);
            this.picProcessImage.Name = "picProcessImage";
            this.picProcessImage.Size = new System.Drawing.Size(100, 83);
            this.picProcessImage.TabIndex = 5;
            this.picProcessImage.TabStop = false;
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(0, 0);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(688, 529);
            this.axWindowsMediaPlayer1.TabIndex = 7;
            // 
            // comboBox1
            // 
            this.comboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(692, 4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(138, 21);
            this.comboBox1.TabIndex = 9;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // lstVideos
            // 
            this.lstVideos.FormattingEnabled = true;
            this.lstVideos.Location = new System.Drawing.Point(692, 31);
            this.lstVideos.Name = "lstVideos";
            this.lstVideos.Size = new System.Drawing.Size(138, 498);
            this.lstVideos.TabIndex = 8;
            this.lstVideos.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            // 
            // lstResponse
            // 
            this.lstResponse.FormattingEnabled = true;
            this.lstResponse.Location = new System.Drawing.Point(584, 16);
            this.lstResponse.Name = "lstResponse";
            this.lstResponse.Size = new System.Drawing.Size(138, 82);
            this.lstResponse.TabIndex = 8;
            this.lstResponse.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstResponse_MouseDoubleClick);
            // 
            // Vave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(834, 531);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.lstVideos);
            this.Controls.Add(this.axWindowsMediaPlayer1);
            this.Controls.Add(this.groupBox1);
            this.Icon = global::Vave.Properties.Resources.audio_wave_;
            this.MaximizeBox = false;
            this.Name = "Vave";
            this.Text = "Vave";
            this.Load += new System.EventHandler(this.Vave_Load);
            this.Resize += new System.EventHandler(this.Vave_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWaveWiever)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picProcessImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar procMicrophoneLevel;
        private System.Windows.Forms.PictureBox picWaveWiever;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox picProcessImage;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ListBox lstVideos;
        private System.Windows.Forms.Button btnDinle;
        private System.Windows.Forms.ListBox lstResponse;
    }
}

