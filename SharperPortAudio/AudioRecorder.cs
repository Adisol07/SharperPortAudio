using System;
using System.Runtime.InteropServices;
using SharperPortAudio.Base;

namespace SharperPortAudio;

public class AudioRecorder
{
    private StreamParameters parameters;
    private Base.Stream? stream;
    private List<float>? samples;

    public int DeviceCount => PortAudio.DeviceCount;
    public int SampleRate { get; private set; }
    public uint FramesPerBuffer { get; set; } = 256;
    public bool BufferEnabled { get; set; } = true;
    public bool SendBufferInsteadOfChunk { get; set; } = false;

    public DataReceivedEventHandler? DataReceived;

    public AudioRecorder(Device device)
    {
        PortAudio.Initialize();

        SampleRate = (int)device.DefaultSampleRate;
        parameters = new StreamParameters()
        {
            device = device.ID,
            channelCount = 1,
            sampleFormat = SampleFormat.Float32,
            suggestedLatency = device.DefaultLowInputLatency,
            hostApiSpecificStreamInfo = IntPtr.Zero,
        };
    }
    public AudioRecorder(Device device, int sampleRate)
    {
        PortAudio.Initialize();

        SampleRate = sampleRate;
        parameters = new StreamParameters()
        {
            device = device.ID,
            channelCount = 1,
            sampleFormat = SampleFormat.Float32,
            suggestedLatency = device.DefaultLowInputLatency,
            hostApiSpecificStreamInfo = IntPtr.Zero,
        };
    }
    public AudioRecorder(Device device, int sampleRate, double suggestedLatency)
    {
        PortAudio.Initialize();

        SampleRate = sampleRate;
        parameters = new StreamParameters()
        {
            device = device.ID,
            channelCount = 1,
            sampleFormat = SampleFormat.Float32,
            suggestedLatency = suggestedLatency,
            hostApiSpecificStreamInfo = IntPtr.Zero,
        };
    }
    ~AudioRecorder()
    {
        PortAudio.Terminate();
    }

    public void Start()
    {
        samples = new List<float>();

        Base.Stream.Callback callback = (IntPtr input, IntPtr output,
                                         uint frameCount,
                                         ref StreamCallbackTimeInfo timeInfo, StreamCallbackFlags statusFlags,
                                         IntPtr userData) =>
        {
            int framesToCopy = (int)frameCount;
            if (framesToCopy > 0)
            {
                float[] buffer = new float[framesToCopy];
                Marshal.Copy(input, buffer, 0, framesToCopy);

                if (BufferEnabled) samples.AddRange(buffer);

                if (DataReceived != null)
                {
                    if (SendBufferInsteadOfChunk) DataReceived(new Audio(samples.ToArray(), SampleRate, 1));
                    else DataReceived(new Audio(buffer, SampleRate, 1));
                }
            }
            return StreamCallbackResult.Continue;
        };

        stream = new SharperPortAudio.Base.Stream(parameters, null,
                                                  SampleRate, FramesPerBuffer,
                                                  StreamFlags.ClipOff, callback, IntPtr.Zero);
        stream.Start();
    }
    public Audio Stop()
    {
        stream?.Stop();
        stream?.Close();

        Audio audio = new Audio(samples!.ToArray(), SampleRate, 1);
        return audio;
    }

    public delegate void DataReceivedEventHandler(Audio audio);
}