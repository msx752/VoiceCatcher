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
using NAudio.Wave;

namespace Vave
{
    public partial class Vave : Form
    {
        #region Variables
        AudioRecorder Mic;
        float lastPeak;
        Bitmap Empty = new Bitmap(300, 60);
        int step = 0;
        int hassasiyet = 10;
        bool KomutDinleniyor = false;
        bool KomutAnlasildi = false;
        DateTime ListenStart = new DateTime();
        DateTime ListenStop = new DateTime();
        bool KomutIslemiBitti = false;
        public Dictionary<string, int> Mikrofonlar = new Dictionary<string, int>();
        #endregion

        public Vave()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void Vave_Load(object sender, EventArgs e)
        {
            int DVNumber = RefreshMics();//geliştirilmesi lazım
            Mic = new AudioRecorder(DVNumber);
            Empty = new Bitmap(WaveViewer.Width, WaveViewer.Height);
            if (!Directory.Exists(Application.StartupPath + "\\files"))
                Directory.CreateDirectory(Application.StartupPath + "\\files");
            WaveViewer.Image = Empty;
            Mic.MicrophoneLevel = 100;
            Mic.SampleAggregator.MaximumCalculated += new EventHandler<MaxSampleEventArgs>(SampleAggregator_MaximumCalculated);
            Mic.BeginMonitoring(DVNumber);
            RefreshImage(Properties.Resources.monitoring);
        }
        public int RefreshMics()
        {
            List<WaveInCapabilities> sources = new List<WaveInCapabilities>();
            for (int i = 0; i < WaveIn.DeviceCount; i++)
                sources.Add(WaveIn.GetCapabilities(i));
            Mikrofonlar.Clear();
            foreach (var source in sources)
                Mikrofonlar.Add(source.ProductName, source.Channels);
            for (int i = 0; i < Mikrofonlar.Count; i++)
                return i;

            return -1;
        }
        public void RefreshImage(Bitmap bitmap)
        {
            ProcessImage.Image = bitmap;
            ProcessImage.Refresh();
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
            if (lastPeak >= hassasiyet && !KomutDinleniyor && !KomutIslemiBitti)
            {
                ListenStart = DateTime.Now;
                KomutDinleniyor = true;
                KomutAnlasildi = false;
                RefreshImage(Properties.Resources.recording);
                Mic.BeginRecording();
            }
            else if (hassasiyet > lastPeak && KomutDinleniyor && !KomutAnlasildi)
            {
                ListenStop = DateTime.Now;
                TimeSpan Sure = ListenStop - ListenStart;
                if (Sure.TotalMilliseconds >= 1500)
                {
                    KomutDinleniyor = false;
                    KomutAnlasildi = true;
                    //KOMUT DİNLEME BURADA SONLANDIRILACAK
                    Mic.Stop();

                    requestSender._dir = Application.StartupPath;
                    RefreshImage(Properties.Resources.sending);
                    string _data = requestSender.Send(Mic.waveFileName);
                    DataParser(_data);
                    Mic.BeginMonitoring(0);
                }
            }
            else
            {
                DateTime IdleTime = DateTime.Now;
                TimeSpan Sure = IdleTime - ListenStart;
                if (Sure.TotalMilliseconds >= 2000 && KomutIslemiBitti)
                {
                    //BEKLEME DURUMU BURADAN KONTROL EDİLECEK
                    KomutIslemiBitti = false;
                    KomutDinleniyor = false;
                    KomutAnlasildi = false;
                    RefreshImage(Properties.Resources.monitoring);
                 
                }
            } 
            //AddLog("Komut bekleniyor..");
        }

        private void DataParser(string _data)
        {
            try
            {
                _data = _data.Replace("{\"result\":[]}\n{\"result\":[", "").Replace("],\"result_index\":0}", "");
                KomutIslemiBitti = true;
                var table = JsonConvert.DeserializeObject<Results>(_data).Alternatives;
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
                RefreshImage(Properties.Resources.error);
                AddLog(e.Message);
                return;
            } 
            RefreshImage(Properties.Resources.done);
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

        private void ProcessLogBox_ItemActivate(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(ProcessLogBox.SelectedItems[0].Text.ToString());
            }
            catch (Exception ef)
            {
                
            }
        }
    }
}
