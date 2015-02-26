using GoogleSpeech;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
            WaveViewer.Image = Empty;
            Mic.MicrophoneLevel = 100;
            Mic.SampleAggregator.MaximumCalculated += new EventHandler<MaxSampleEventArgs>(SampleAggregator_MaximumCalculated);
            Mic.BeginMonitoring(0);
            ProcessImage.Image = Properties.Resources.monitoring;
        }

        private void SampleAggregator_MaximumCalculated(object sender, MaxSampleEventArgs e)
        {
            lastPeak = (int)Math.Floor(Math.Max(e.MaxSample, Math.Abs(e.MinSample)) * 100);
            MicrophoneLevel.Value = (int)lastPeak;
            DrawWave();
            CallCommand();
        }

        private void CallCommand()
        {
            if (lastPeak >= hassasiyet && !KomutDinleniyor)
            {
                ListenStart = DateTime.Now;
                KomutDinleniyor = true;
                KomutAnlasildi = false;
                ProcessImage.Image = Properties.Resources.recording;
                Mic.BeginRecording();
            }
            else if (hassasiyet > lastPeak && KomutDinleniyor && !KomutAnlasildi)
            {
                ListenStop = DateTime.Now;
                TimeSpan Sure = ListenStop - ListenStart;
                if (Sure.TotalMilliseconds >= 1000)
                {
                    KomutDinleniyor = false;
                    KomutAnlasildi = true;
                    ProcessImage.Image = Properties.Resources.sending;
                    //KOMUT DİNLEME BURADA SONLANDIRILACAK
                    Mic.Stop();
                    RequestSender();

                    //ProcessImage.Image = Properties.Resourcesdone;
                }
            }
            else
            {
                DateTime IdleTime = DateTime.Now;
                TimeSpan Sure = IdleTime - ListenStart;
                if (Sure.TotalMilliseconds >= 2000)
                {
                    KomutDinleniyor = false;
                    KomutAnlasildi = false;
                    //DİNLEME İŞLEMİ YAPILMIYORKEN (MONİTORING AKTIFKEN) BURASI DEVREDE OLACAK HEP
                    //Mic.BeginMonitoring(0);
                    ProcessImage.Image = Properties.Resources.monitoring;
                    //AddLog("Komut bekleniyor..");
                }
            }
        }

        private bool RequestSender()
        {
            try
            {
                string responseFromServer = GoogleVoice.GoogleSpeechRequest(Mic.waveFileName, "tmp.flac");
                responseFromServer = responseFromServer.Replace("{\"result\":[]}\n{\"result\":[", "").Replace("],\"result_index\":0}", "");
                var table = JsonConvert.DeserializeObject<Results>(responseFromServer).Alternatives;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    var item = table.Rows[i];
                    if (item.ItemArray.Count() > 1)
                    {
                        if (item.ItemArray[1].ToString() != "")
                        {
                            ResponseBox.AppendText(item.ItemArray[0].ToString() + Environment.NewLine);
                            //AddLog(item.ItemArray[0].ToString() + "--" + item.ItemArray[1].ToString() + System.Environment.NewLine);
                        }
                    }
                    else
                    {
                        ResponseBox.AppendText(item.ItemArray[0].ToString() + Environment.NewLine);
                    }
                }
            }
            catch (Exception e)
            {
                AddLog(e.ToString());
                return false;
            }
            return true;
        }

        private void DrawWave()
        {
            Graphics gr = Graphics.FromImage(WaveViewer.Image);
            if (step > 300)
            {
                gr.Clear(Color.White);
                step = 0;
            }
            lastPeak = (lastPeak / 100) * WaveViewer.Height;
            if (lastPeak == 0) lastPeak = 1;
            float fark = ((WaveViewer.Height - lastPeak) / 2);
            RectangleF wave = new RectangleF(new PointF(step, fark), new SizeF(1, lastPeak));
            gr.FillRectangle(Brushes.Red, wave);
            gr.Dispose();
            WaveViewer.Refresh();
            step++;
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Mic.Stop();
        }

        private void AddLog(string p)
        {
            //if (logStatus == true)
            ProcessLogBox.Items.Add(p);
            if (ProcessLogBox.Items.Count > 1)
                ProcessLogBox.Items[ProcessLogBox.Items.Count - 1].EnsureVisible();
        }


        public class Results
        {
            [JsonProperty(PropertyName = "alternative")]
            public System.Data.DataTable Alternatives { get; set; }
        }
    }
}
