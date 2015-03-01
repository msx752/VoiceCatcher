using System;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using NAudio.Wave;
using NAudio.Mixer;
using System.IO;

namespace Vave
{
    public class AudioRecorder : IAudioRecorder
    {
        public WaveIn waveIn;
        readonly SampleAggregator sampleAggregator;
        UnsignedMixerControl volumeControl;
        double desiredVolume = 50;//varsayılan değer sonradan değişiyor.
        RecordingState recordingState;
        WaveFileWriter writer;
        WaveFormat recordingFormat;
        public FileInfo waveFile;
        public event EventHandler Stopped = delegate { };
        public int DeviceNumber { get; set; }

        public AudioRecorder(int _DNumber)
        {
            DeviceNumber = _DNumber;
            sampleAggregator = new SampleAggregator();
            RecordingFormat = new WaveFormat(8000, 1);
            //RecordingFormat = new WaveFormat(8000, WaveIn.GetCapabilities(DeviceNumber).Channels);
        }

        public WaveFormat RecordingFormat
        {
            get
            {
                return recordingFormat;
            }
            set
            {
                recordingFormat = value;
                sampleAggregator.NotificationCount = value.SampleRate / 10;
            }
        }

        public void BeginMonitoring()
        {
            if (recordingState != RecordingState.Stopped)
            {
                //throw new InvalidOperationException("Can't begin monitoring while we are in this state: " + recordingState.ToString());
            }
            waveIn = new WaveIn();
            waveIn.DeviceNumber = DeviceNumber;
            waveIn.DataAvailable += OnDataAvailable;
            waveIn.RecordingStopped += OnRecordingStopped;
            waveIn.WaveFormat = recordingFormat;

            waveIn.StartRecording();
            TryGetVolumeControl();
            recordingState = RecordingState.Monitoring;
        }

        void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            recordingState = RecordingState.Stopped;
            writer.Dispose();
            Stopped(this, EventArgs.Empty);
        }

        public void BeginRecording()
        {
            if (recordingState != RecordingState.Monitoring)
            {
                //throw new InvalidOperationException("Can't begin recording while we are in this state: " + recordingState.ToString());
            }
            waveFile = new FileInfo(Application.StartupPath + "\\files\\cmd_" + new Random().Next() + ".wav");
            writer = new WaveFileWriter(waveFile.FullName, recordingFormat);
            recordingState = RecordingState.Recording;
        }

        public void Stop()
        {
            if (recordingState == RecordingState.Recording)
            {
                recordingState = RecordingState.RequestedStop;
                waveIn.StopRecording();
                writer.Close();
            }
        }

        private void TryGetVolumeControl()
        {
            int waveInDeviceNumber = waveIn.DeviceNumber;
            if (Environment.OSVersion.Version.Major >= 6) // Vista and over
            {
                var mixerLine = waveIn.GetMixerLine();
                //new MixerLine((IntPtr)waveInDeviceNumber, 0, MixerFlags.WaveIn);
                foreach (var control in mixerLine.Controls)
                {
                    if (control.ControlType == MixerControlType.Volume)
                    {
                        this.volumeControl = control as UnsignedMixerControl;
                        MicrophoneLevel = desiredVolume;
                        break;
                    }
                }
            }
            else
            {
                var mixer = new Mixer(waveInDeviceNumber);
                foreach (var destination in mixer.Destinations
                    .Where(d => d.ComponentType == MixerLineComponentType.DestinationWaveIn))
                {
                    foreach (var source in destination.Sources
                        .Where(source => source.ComponentType == MixerLineComponentType.SourceMicrophone))
                    {
                        foreach (var control in source.Controls
                            .Where(control => control.ControlType == MixerControlType.Volume))
                        {
                            volumeControl = control as UnsignedMixerControl;
                            MicrophoneLevel = desiredVolume;
                            break;
                        }
                    }
                }
            }

        }

        public double MicrophoneLevel
        {
            get
            {
                return desiredVolume;
            }
            set
            {
                desiredVolume = value;
                if (volumeControl != null)
                {
                    volumeControl.Percent = value;
                }
            }
        }

        public SampleAggregator SampleAggregator
        {
            get
            {
                return sampleAggregator;
            }
        }

        public RecordingState RecordingState
        {
            get
            {
                return recordingState;
            }
        }

        public TimeSpan RecordedTime
        {
            get
            {
                if (writer == null)
                {
                    return TimeSpan.Zero;
                }
                return TimeSpan.FromSeconds((double)writer.Length / writer.WaveFormat.AverageBytesPerSecond);
            }
        }

        void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            byte[] buffer = e.Buffer;
            int bytesRecorded = e.BytesRecorded;


            long maxFileLength = this.recordingFormat.AverageBytesPerSecond * 60;
            if (recordingState == RecordingState.Recording
                || recordingState == RecordingState.RequestedStop)
            {
                var toWrite = (int)Math.Min(maxFileLength - writer.Length, bytesRecorded);
                if (toWrite > 0)
                {
                    writer.Write(buffer, 0, bytesRecorded);
                }
                else
                {
                    Stop();
                }
            }


            //mikrofon ses seviye hesaplaması
            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                short sample = (short)((buffer[index + 1] << 8) |
                                        buffer[index + 0]);
                float sample32 = sample / 32768f;
                sampleAggregator.Add(sample32);
            }
        }
    }

    public interface IAudioRecorder
    {
        void BeginMonitoring();
        void BeginRecording();
        void Stop();
        double MicrophoneLevel { get; set; }
        RecordingState RecordingState { get; }
        SampleAggregator SampleAggregator { get; }
        event EventHandler Stopped;
        WaveFormat RecordingFormat { get; set; }
        TimeSpan RecordedTime { get; }
    }

    public enum RecordingState
    {
        Stopped,
        Monitoring,
        Recording,
        RequestedStop
    }
}
