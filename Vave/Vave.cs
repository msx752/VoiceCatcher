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
using Newtonsoft.Json;
using System.IO;

namespace Vave
{
    public partial class Vave : Form
    {
        #region Variables
        AudioRecorder Mic = new AudioRecorder();
        float lastPeak;
        Bitmap Empty = new Bitmap(300, 60);
        int step = 0;
        int hassasiyet = 1;
        bool KomutDinleniyor = false;
        bool KomutAnlasildi = false;
        DateTime ListenStart = new DateTime();
        DateTime ListenStop = new DateTime();
        #endregion

        public Vave()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void Vave_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(Application.StartupPath + "\\files"))
                Directory.CreateDirectory(Application.StartupPath + "\\files");
            WaveViewer.Image = Empty;
            Mic.MicrophoneLevel = 100;
            Mic.SampleAggregator.MaximumCalculated += new EventHandler<MaxSampleEventArgs>(SampleAggregator_MaximumCalculated);
            Mic.BeginMonitoring(0);
            ProcessImage.Image = Properties.Resources.monitoring;
        }

        private void CheckDir()
        {
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

                    requestSender GoogleSender = new requestSender();
                    GoogleSender._dir = Application.StartupPath;
                    string _data = GoogleSender.Send(Mic.waveFileName);
                    DataParser(_data);
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

        private void DataParser(string _data)
        {
            _data = _data.Replace("{\"result\":[]}\n{\"result\":[", "").Replace("],\"result_index\":0}", "");
            var table = JsonConvert.DeserializeObject<Results>(_data).Alternatives;
            //for (int i = 0; i < table.Rows.Count; i++)
            //{
            //    var item = table.Rows[i];
            //    if (item.ItemArray.Count() > 1)
            //    {
            //        if (item.ItemArray[1].ToString() != "")
            //        {
            //            ResponseBox.AppendText(item.ItemArray[0].ToString() + Environment.NewLine);
            //            //AddLog(item.ItemArray[0].ToString() + "--" + item.ItemArray[1].ToString() + System.Environment.NewLine);
            //        }
            //    }
            //    else
            //    {
            //        ResponseBox.AppendText(item.ItemArray[0].ToString() + Environment.NewLine);
            //    }
            //}
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


        private void Vave_FormClosing(object sender, FormClosingEventArgs e)
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
