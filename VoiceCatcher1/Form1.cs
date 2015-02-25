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
        AudioRecorder Mic = new AudioRecorder();
        float lastPeak;
        string waveFileName = String.Empty;
        int recStatus = 0;
        bool logStatus = true, requestStatus = false;
        Bitmap Empty = new Bitmap(300, 60);
        int before = 0;
        int step = 0;

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
            lastPeak = Math.Max(e.MaxSample, Math.Abs(e.MinSample)) * 100;
            progressBar1.Value = (int)lastPeak;
            DrawGraph();

            if (requestStatus == false)
            {
                if (Mic.RecordingState == RecordingState.Monitoring)
                {
                    AddLog("Komut Bekleniyor!");
                }
                else if (Mic.RecordingState == RecordingState.Recording)
                {
                    AddLog("[" + Mic.RecordedTime + "]   komut kayıt ediliyor..");
                }
                else if (Mic.RecordingState == RecordingState.RequestedStop)
                {
                    logStatus = false;
                    AddLog("İşlem kullanıcı tarafından durduruldu!");
                }
                else if (Mic.RecordingState == RecordingState.Stopped)
                {
                    //Buraya girdiğinde GoogleSpeechLib ile istek yollatacağız.
                    //Gelen istek SampleAggregator_MaximumCalculated metodunda kontrol edilecek ve 
                    //cevap geldiğinde işlemleri sıfırlayıp tekrar monitoring durumuna alacağız.
                    logStatus = false;
                    AddLog("İstek gönderiliyor..!");
                }

                if (lastPeak > 10)
                    StartRecording();

                // Burada konuşmanın kesildiği süreyi alıp işlem yapacak ve bitirecek
                //Bittiğinde 59. satırdaki if'e girecek
                int sure = 5;
                if (sure > 10)
                    StopRecording();

                recLog.Items[recLog.Items.Count - 1].EnsureVisible();
            }
            else
            {
                AddLog("Cevap Bekleniyor..");
            }
        }

        private void StopRecording()
        {
            Mic.Stop();
        }

        private void AddLog(string p)
        {
            if (logStatus == true)
                recLog.Items.Add(p);
        }

        private void DrawGraph()
        {
            Graphics gr = Graphics.FromImage(pictureBox1.Image);
            if (step > 300)
            {
                gr.Clear(Color.White);
                step = 0;
            }
            lastPeak = (lastPeak / 100) * pictureBox1.Height;
            if (lastPeak == 0) lastPeak = 1;
            float fark = (60 - lastPeak) / 2;
            RectangleF wave = new RectangleF(new PointF(step, fark), new SizeF(1, lastPeak));
            gr.FillRectangle(Brushes.Red, wave);
            gr.Dispose();
            pictureBox1.Refresh();
            step++;
        }

        int GetX(int x)
        {
            return (pictureBox1.Height - x) / 2;
        }

        private void StartRecording()
        {
            waveFileName = Application.StartupPath + "\\files\\command_" + DateTime.Now.Millisecond + ".wav";
            Mic.BeginRecording(waveFileName);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Mic.Stop();
        }

    }
}
