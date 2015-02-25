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
        private double lastPeak;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Mic.MicrophoneLevel = 100;
            Mic.SampleAggregator.MaximumCalculated += SampleAggregator_MaximumCalculated;
            Mic.BeginMonitoring(0);
            Mic.BeginRecording(Application.StartupPath + "\\my1.wav");
        }
        private void SampleAggregator_MaximumCalculated(object sender, MaxSampleEventArgs e)
        {
            lastPeak = Math.Floor(Math.Max(e.MaxSample, Math.Abs(e.MinSample)) * float.Parse("100"));
            label1.Text = lastPeak.ToString();
            //if (lastPeak > 1)
            //else
            //    label1.Text = "Komut Bekliyor..";

            //if (Mic.RecordingState == RecordingState.Recording)
            //    label1.Text = "Kayıt Ediyor..";
            //else if (Mic.RecordingState == RecordingState.Stopped || Mic.RecordingState == RecordingState.RequestedStop)
            //    label1.Text = "Kayıt Tamamlandı..";
        }

        private void BeginRecording(string path)
        {
            string waveFileName = path;
            Mic.BeginRecording(path);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Mic.Stop();
        }

    }
}
