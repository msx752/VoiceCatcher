using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VoiceRecorder.Audio;

namespace VoiceCatcher1
{
    public partial class Form1 : Form
    {
        #region değişkenler
        AudioRecorder Mic = new AudioRecorder();
        float lastPeak;
        Bitmap Empty = new Bitmap(300, 60);
        int step = 0;
        int hassasiyet = 10;
        bool KomutDinleniyor = false;
        bool KomutAnlasildi = false;
        DateTime ListenStart = new DateTime();
        DateTime ListenStop = new DateTime();
        #endregion

        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Empty;
            Mic.MicrophoneLevel = 100;
            Mic.SampleAggregator.MaximumCalculated += new EventHandler<MaxSampleEventArgs>(SampleAggregator_MaximumCalculated);
            Mic.BeginMonitoring(0);
        }

        private void SampleAggregator_MaximumCalculated(object sender, MaxSampleEventArgs e)
        {
            lastPeak = (int)Math.Floor(Math.Max(e.MaxSample, Math.Abs(e.MinSample)) * 100);
            progressBar1.Value = (int)lastPeak;
            label1.Text = lastPeak.ToString();
            DrawWave();
            CallCommand();
        }

        private void CallCommand()
        {
            if (lastPeak >= hassasiyet && !KomutDinleniyor)
            {
                ListenStart = DateTime.Now;
                lblListen.Visible = true;
                lblListenOk.Visible = false;
                KomutDinleniyor = true;
                KomutAnlasildi = false;
                //KOMUT DİNLEME BURADA BAŞLATILACAK
                ///////Mic.BeginRecording();
            }
            else if (hassasiyet > lastPeak && KomutDinleniyor && !KomutAnlasildi)
            {
                ListenStop = DateTime.Now;
                TimeSpan Sure = ListenStop - ListenStart;
                if (Sure.TotalMilliseconds >= 1000)
                {
                    lblListen.Visible = false;
                    lblListenOk.Visible = true;
                    KomutDinleniyor = false;
                    KomutAnlasildi = true;
                    //KOMUT DİNLEME BURADA SONLANDIRILACAK
                    //Mic.Stop();
                }
            }
            else
            {
                DateTime IdleTime = DateTime.Now;
                TimeSpan Sure = IdleTime - ListenStart;
                if (Sure.TotalMilliseconds >= 2000)
                {
                    lblListen.Visible = false;
                    lblListenOk.Visible = false;
                    KomutDinleniyor = false;
                    KomutAnlasildi = false;
                    //DİNLEME İŞLEMİ YAPILMIYORKEN (MONİTORING AKTIFKEN) BURASI DEVREDE OLACAK HEP
                }
            }
        }

        private void DrawWave()
        {
            Graphics gr = Graphics.FromImage(pictureBox1.Image);
            if (step > 300)
            {
                gr.Clear(Color.White);
                step = 0;
            }
            lastPeak = (lastPeak / 100) * pictureBox1.Height;
            if (lastPeak == 0) lastPeak = 1;
            float fark = ((pictureBox1.Height - lastPeak) / 2);
            RectangleF wave = new RectangleF(new PointF(step, fark), new SizeF(1, lastPeak));
            gr.FillRectangle(Brushes.Red, wave);
            gr.Dispose();
            pictureBox1.Refresh();
            step++;
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Mic.Stop();
        }

        public void AddLog()
        {

        }
    }
}
