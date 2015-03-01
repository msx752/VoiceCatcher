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
using System.Runtime.Serialization;
using Newtonsoft.Json.Serialization;

namespace Vave
{
    public partial class Vave : Form
    {
        #region Variables
        AudioRecorder Mic;
        Bitmap Empty = new Bitmap(300, 60);//resim boyutunu değiştirmeyin
        int step = 0;
        private float _lastPeak;
        public float lastPeak { get { return _lastPeak; } set { _lastPeak = value; procMicrophoneLevel.Value = (int)_lastPeak; } }//mikrofon geliştirildi daha hızlı tepki veriyor
        int hassasiyet = 10;
        bool KomutDinleniyor = false;
        bool KomutAnlasildi = false;
        bool KomutIslemiBitti = false;
        public Dictionary<string, int> Mikrofonlar = new Dictionary<string, int>();
        Engine eng = new Engine();
        DateTime BitisKontrol;
        DateTime BaslangicKontrol;
        #endregion

        public Vave()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private GoogleJSON DeSerialize(string jsonValue)
        {
            JsonSerializerSettings serSettings = new JsonSerializerSettings();
            serSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            GoogleJSON outObject = JsonConvert.DeserializeObject<GoogleJSON>(jsonValue, serSettings);
            return outObject;
        }

        private void Vave_Load(object sender, EventArgs e)
        {
            Directory.Delete(Application.StartupPath + "\\files", true);//tüm eski ses dosyaları siliniyor
            int DVNumber = RefreshMics();//geliştirilmesi lazım
            Mic = new AudioRecorder(DVNumber);
            Empty = new Bitmap(picWaveWiever.Width, picWaveWiever.Height);
            if (!Directory.Exists(Application.StartupPath + "\\files"))
                Directory.CreateDirectory(Application.StartupPath + "\\files");
            picWaveWiever.Image = Empty;
            Mic.MicrophoneLevel = 100;//değiştirilmesi tavsiye edilmez
            Mic.SampleAggregator.MaximumCalculated += new EventHandler<MaxSampleEventArgs>(SampleAggregator_MaximumCalculated);
            Mic.BeginMonitoring();
            RefreshImage(Properties.Resources.monitoring);
        }

        public int RefreshMics()//varsayılan olarak sistemdeki ilk mikrofon seçiliyor
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
            picProcessImage.Image = bitmap;
            picProcessImage.Refresh();
        }
        private void SampleAggregator_MaximumCalculated(object sender, MaxSampleEventArgs e)
        {
            lastPeak = e.LastPeak;
            if (lastPeak > hassasiyet)
                BaslangicKontrol = DateTime.Now;
            else
                BitisKontrol = DateTime.Now;
            CallCommand();
            DrawWave();
        }


        private void CallCommand()
        {
            if (lastPeak >= hassasiyet && !KomutDinleniyor && !KomutIslemiBitti)//konuşma kayıtı başlatılıyor.
            {
                RefreshImage(Properties.Resources.recording);
                Mic.BeginRecording();
                KomutDinleniyor = true;
                KomutAnlasildi = false;
            }
            else if (hassasiyet > lastPeak && KomutDinleniyor && !KomutAnlasildi)//kayıt işlemi sonlandırma kontrolleri
            {
                TimeSpan Sure = BitisKontrol - BaslangicKontrol;
                if (Sure.TotalMilliseconds >= 500)//konuşma sona erdikten sonraki geçen süre hesaplanıyor
                {
                    KomutDinleniyor = false;
                    KomutAnlasildi = true;
                    RefreshImage(Properties.Resources.sending);
                    Mic.Stop();//bu işlem monitoring işlemide dahil olmak üzere tüm mikrofon hareketini durduruyor
                    DataParser(requestSender.Send(Mic.waveFile));
                    Mic.BeginMonitoring();//bu aktif edilmezse işlemler geçersiz.
                }
            }
            else if (KomutIslemiBitti)//hiçbir işlem yapmıyorken
            {
                Thread.Sleep(1000);
                KomutIslemiBitti = false;
                KomutDinleniyor = false;
                KomutAnlasildi = false;
                RefreshImage(Properties.Resources.monitoring);
            }
        }
        private void DataParser(string _data)
        {
            try
            {
                KomutIslemiBitti = true;
                if (_data == "{\"result\":[]}\n") throw new Exception("Ne o cacık yemiş gibi yayık yayık konuşuyorsun. adamakıllı söyle de anlayalım dediğini!!");
                _data = _data.Replace("_index\":0}\n", "_index\":0},").Replace("{\"result\":[]}\n", "{\"results\":[{\"result\":[]},");
                _data = _data.Substring(0, _data.LastIndexOf("_index\":0},")) + "_index\":0}]}";
                GoogleJSON speech = DeSerialize(_data);
                lstResponseBox.Clear();
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //bu komut şimdilik bir sonuç döndürüyor fakat stability null ve confidence null 
                //olan değerlerde gelebiliyor duruma göre onlarda eklenmeli.
                //hala arasından doğru olanı seçemiyoruz
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                bool aranan_kan_bulundu = false;
                foreach (var item in speech.Results)
                {
                    if (aranan_kan_bulundu)
                        break;

                    foreach (var item2 in item.Result)
                    {
                        foreach (var item3 in item2.Alternatives)
                        {
                            if (item3.Confidence != null)//Confidence değeri olanı ekliyoruz sonra işlem durduruluyor
                            {
                                lstResponseBox.Text += item3.Transcript + "\r\n";
                                aranan_kan_bulundu = true;
                                break;
                            }
                        }

                        if (item2.Stability != null && !aranan_kan_bulundu)//Stability değeri olanı ekliyoruz sonra işlem durduruluyor
                        {
                            foreach (var item3 in item2.Alternatives)
                            {
                                lstResponseBox.Text += item3.Transcript + "\r\n";
                                aranan_kan_bulundu = true;
                                break;
                            }
                        }
                    }

                }
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //BURASI COMMAND İŞLEMİNİ TETİKLİYOR AYRI METHOD TANIMLANA BİLİR
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                List<Command> cm = eng.CallCoommand("not defteri");//şimdilik elle tanımlanıyor
                if (cm != null)
                {
                    if (cm.Count == 1)
                    {
                        cm[0].Exec("selam nasılsın");
                    }
                    else
                    {

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
            Graphics gr = Graphics.FromImage(picWaveWiever.Image);
            if (step > picWaveWiever.Image.Width)
            {
                gr.Clear(Color.White);
                step = 0;
            }
            lastPeak = (lastPeak / 100) * picWaveWiever.Height;
            if (lastPeak == 0) lastPeak = 1;
            float fark = ((picWaveWiever.Height - lastPeak) / 2);
            SolidBrush sb = new SolidBrush(Color.Black);
            if (KomutDinleniyor)
            {
                sb.Color = Color.Blue;
            }
            else if (!KomutDinleniyor)
            {
                sb.Color = Color.Red;
            }
            gr.FillRectangle(sb, new RectangleF(new PointF(step, fark), new SizeF(1, lastPeak)));
            gr.Dispose();
            picWaveWiever.Refresh();
            step++;
        }

        private void Vave_FormClosing(object sender, FormClosingEventArgs e)
        {
            Mic.Stop();
        }

        private void AddLog(string p)
        {
            //if (logStatus == true)
            lstProcessLogBox.Items.Add(p);
            if (lstProcessLogBox.Items.Count > 1)
                lstProcessLogBox.Items[lstProcessLogBox.Items.Count - 1].EnsureVisible();
        }

        private void ProcessLogBox_ItemActivate(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(lstProcessLogBox.SelectedItems[0].Text.ToString());
            }
            catch (Exception ef)
            {

            }
        }
    }
}
