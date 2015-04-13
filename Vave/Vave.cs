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
using System.Xml;
using WMPLib;

namespace Vave
{
    public partial class Vave : Form
    {

        Dictionary<int, string> Listele = new Dictionary<int, string>();
        public void LoadVideos()
        {
            string xmlpath = Application.StartupPath + "\\kayitlilar.xml";
            List<KeyValuePair<int, string>> thisOrdered = new List<KeyValuePair<int, string>>();
            if (!File.Exists(xmlpath))
            {
                MessageBox.Show(@"gerekli videolar bulunamadı \Videos\ klasörünü kontrol ediniz.");
                #region internetten video güncellemesi
                //XmlTextWriter createXML = new XmlTextWriter(xmlpath, UTF8Encoding.UTF8);
                //createXML.WriteStartDocument();
                //createXML.WriteComment("İşaret Dili Programı: http://www.cmpe.boun.edu.tr/tid/?v=(VALUE)");
                //createXML.WriteStartElement("Kayitlar");
                //createXML.WriteEndDocument();
                //createXML.Close();

                //XmlDocument _data = new XmlDocument();
                //_data.Load(xmlpath);
                //foreach (var item in thisOrdered)
                //{
                //    XmlElement _kelime = _data.CreateElement("kelimeid");
                //    _kelime.SetAttribute(item.Key.ToString(), item.Value.ToLower());
                //    _data.DocumentElement.AppendChild(_kelime);
                //}
                //_data.Save(xmlpath);
                #endregion
            }
            else
            {
                XmlDocument _data = new XmlDocument();
                _data.Load(xmlpath);
                XmlNodeList yeniliste = _data["Kayitlar"].ChildNodes;
                foreach (XmlNode item in yeniliste)
                    Listele.Add(int.Parse(item.Attributes[0].Name.Substring(1)), item.Attributes[0].InnerText);
            }

            thisOrdered = Listele.OrderBy(p => p.Value).ToList();
            foreach (var item in thisOrdered)
            {
                lstVideos.Items.Add(item.Value);
                comboBox1.Items.Add(item.Value);
                comboBox1.AutoCompleteCustomSource.Add(item.Value);
            }
            axWindowsMediaPlayer1.stretchToFit = true;
        }
        #region Variables
        AudioRecorder Mic;
        Bitmap Empty = new Bitmap(300, 60);//resim boyutunu değiştirmeyin
        int step = 0;
        float _lastPeak;
        public float lastPeak { get { return _lastPeak; } set { _lastPeak = value; procMicrophoneLevel.Value = (int)_lastPeak; } }//mikrofon geliştirildi daha hızlı tepki veriyor
        int hassasiyet = 10;
        bool KomutDinleniyor = false;
        bool KomutAnlasildi = false;
        bool KomutIslemiBitti = false;
        public Dictionary<string, int> Mikrofonlar = new Dictionary<string, int>();

        DateTime BitisKontrol;
        DateTime BaslangicKontrol;
        #endregion

        public Vave()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }
        /// <summary>
        /// json kodu burada deserialize(nesne) yapıyoruz.
        /// </summary>
        /// <param name="jsonValue"></param>
        private GoogleJSON DeSerialize(string jsonValue)
        {
            JsonSerializerSettings serSettings = new JsonSerializerSettings();
            serSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            GoogleJSON outObject = JsonConvert.DeserializeObject<GoogleJSON>(jsonValue, serSettings);
            return outObject;
        }

        private void Vave_Load(object sender, EventArgs e)
        {
            LoadVideos();
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

        /// <summary>
        ///  sistemeki mikrofonlar listelenip seçiliyor
        /// </summary>
        public int RefreshMics()
        {
            List<WaveInCapabilities> sources = new List<WaveInCapabilities>();
            for (int i = 0; i < WaveIn.DeviceCount; i++)
                sources.Add(WaveIn.GetCapabilities(i));
            Mikrofonlar.Clear();
            foreach (var source in sources)
                Mikrofonlar.Add(source.ProductName, source.Channels);
            for (int i = 0; i < Mikrofonlar.Count; i++)
                return i;//sistemdeki ilk kayıtlı mikrofon seçiliyor

            return -1;
        }

        /// <summary>
        /// görsel resimleri yeniler
        /// </summary>
        /// <param name="bitmap"></param>
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
            RECORD();
            DrawWave();
        }

        /// <summary>
        /// mikrofon kayıt başlatma, durdurma ve vb faaliyetleri tetikler
        /// </summary>
        private void RECORD()
        {
            if (btnDinle.Text == "DURDUR" && !KomutDinleniyor && !KomutIslemiBitti)//konuşma kayıtı başlatılıyor.
            {
                RefreshImage(Properties.Resources.recording);
                Mic.BeginRecording();
                KomutDinleniyor = true;
                KomutAnlasildi = false;
            }
            else if (btnDinle.Text == "DİNLE" && KomutDinleniyor && !KomutAnlasildi)//kayıt işlemi sonlandırma kontrolleri
            {
                TimeSpan Sure = BitisKontrol - BaslangicKontrol;
                if (Sure.TotalMilliseconds >= 555)//konuşma sona erdikten sonraki geçen süre hesaplanıyor
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
                Thread.Sleep(1111);
                KomutIslemiBitti = false;
                KomutDinleniyor = false;
                KomutAnlasildi = false;
                RefreshImage(Properties.Resources.monitoring);
            }
        }

        /// <summary>
        /// sunucudan gelen yazıyı işlemeye yarayan method
        /// </summary>
        /// <param name="_Data"></param>
        private void DataParser(string _data)
        {
            try
            {
                KomutIslemiBitti = true;
                if (_data == "{\"result\":[]}\n")
                    throw new Exception("Söylediğiniz Anlaşılmadı.");

                _data = _data.Replace("_index\":0}\n", "_index\":0},").Replace("{\"result\":[]}\n", "{\"results\":[{\"result\":[]},");
                _data = _data.Substring(0, _data.LastIndexOf("_index\":0},")) + "_index\":0}]}";
                GoogleJSON speech = DeSerialize(_data);
                lstResponse.Items.Clear();
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
                                lstResponse.Items.Add(item3.Transcript);
                                //aranan_kan_bulundu = true;
                                //break;
                            }
                        }

                        if (item2.Stability != null && !aranan_kan_bulundu)//Stability değeri olanı ekliyoruz sonra işlem durduruluyor
                        {
                            foreach (var item3 in item2.Alternatives)
                            {
                                lstResponse.Items.Add(item3.Transcript);
                                //aranan_kan_bulundu = true;
                                //break;
                            }
                        }
                    }
                }
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //BURASI ÖRNEK COMMAND İŞLEMİNİ TETİKLİYOR BURADAN HEMEN ÖNCE "GramerTX" UYGULAMASI TETİKLENMELİ YAPILACAK İŞLER BELİRLENMELİ
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //List<Command> cm = eng.CallCommand("not defteri");//şimdilik elle tanımlanıyor
            }
            catch (Exception e)
            {
                RefreshImage(Properties.Resources.error);
                return;
            }
            RefreshImage(Properties.Resources.done);
        }

        /// <summary>
        /// mikrofon ses efekti burada ekrana çizdiriliyor
        /// </summary>
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
                sb.Color = Color.Blue;
            else
                sb.Color = Color.Red;

            gr.FillRectangle(sb, new RectangleF(new PointF(step, fark), new SizeF(.5f, lastPeak)));
            gr.Dispose();
            picWaveWiever.Refresh();
            step++;
        }

        private void Vave_FormClosing(object sender, FormClosingEventArgs e)
        {
            Mic.Stop();
        }



        private void ProcessLogBox_ItemActivate(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (btnDinle.Text == "DİNLE")
            {
                btnDinle.Text = "DURDUR";
                btnDinle.BackColor = Color.Green;
            }
            else
            {
                btnDinle.Text = "DİNLE";
                btnDinle.BackColor = this.BackColor;
            }

        }

        void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Kontrol(requestSender.TextType.VideoList);
        }

        void Kontrol(requestSender.TextType _type)
        {
            string myValues = "";
            if (_type == requestSender.TextType.VideoList)
                myValues = lstVideos.SelectedItem.ToString();
            else if (_type == requestSender.TextType.ComboBox)
                myValues = comboBox1.SelectedItem.ToString();
            else if (_type == requestSender.TextType.ResponseList)
            {
                string deger = lstResponse.SelectedItem.ToString();
                int index = -1;
                for (int i = 0; i < comboBox1.Items.Count; i++)
                {
                    if (comboBox1.Items[i].ToString().IndexOf(deger) != -1)
                    {
                        index = i;
                        break;
                    }
                }


                if (index != -1)
                {
                    comboBox1.SelectedIndex = 0;
                    comboBox1.SelectedIndex = index;
                }
                else
                    MessageBox.Show("aranan kelime algılanamadı lütfen yukarıdan seçiniz.");
            }
            if (myValues == "")
                return;

            if (myValues != null)
            {
                int video_ID = Listele.Where(p => p.Value == myValues).ToList()[0].Key;
                string _path = Application.StartupPath + "\\videos\\" + video_ID + ".wmv";
                if (File.Exists(_path))
                {
                    IWMPMedia media = axWindowsMediaPlayer1.newMedia(_path);
                    axWindowsMediaPlayer1.currentPlaylist.clear();
                    axWindowsMediaPlayer1.currentPlaylist.appendItem(media);
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
                else
                    MessageBox.Show("dosya yok");
            }
        }


        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Kontrol(requestSender.TextType.ComboBox);
        }

        private void lstResponse_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstResponse.Items.Count < 1 || lstResponse.SelectedIndex == -1)
                return;
            Kontrol(requestSender.TextType.ResponseList);
        }

        private void Vave_Resize(object sender, EventArgs e)
        {
            this.Width = 850; 
            this.Height = 540;
        }

    }
}
