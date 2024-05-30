using System;
using System.Runtime.InteropServices;
using SharperPortAudio.Base;
using System.Text;
using NAudio.Wave;

namespace DemoConsole1;

internal class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Loading");
        PortAudio.Initialize();

        int deviceCount = PortAudio.DeviceCount;
        if (deviceCount < 0)
        {
            Console.WriteLine("ERROR: CountDevices returned 0x" + deviceCount.ToString("x"));
            return;
        }

        Console.WriteLine("Available audio devices:");
        for (int i = 0; i < deviceCount; i++)
        {
            var deviceInfo = PortAudio.GetDeviceInfo(i);
            Console.WriteLine($"Device {i}: {deviceInfo.name}");
            Console.WriteLine($"  Max input channels: {deviceInfo.maxInputChannels}");
            Console.WriteLine($"  Max output channels: {deviceInfo.maxOutputChannels}");
            Console.WriteLine($"  Default sample rate: {deviceInfo.defaultSampleRate}");
            Console.WriteLine();
        }

        Console.Write("Select: ");
        int inputDevice = Convert.ToInt32(Console.ReadLine());

        StreamParameters inputParams = new StreamParameters()
        {
            device = inputDevice,
            channelCount = 1,
            sampleFormat = SampleFormat.Float32,
            suggestedLatency = PortAudio.GetDeviceInfo(inputDevice).defaultLowInputLatency,
            hostApiSpecificStreamInfo = IntPtr.Zero,
        };

        int sampleRate = (int)PortAudio.GetDeviceInfo(inputDevice).defaultSampleRate;
        uint framesPerBuffer = 256;
        float[] recordedSamples = new float[sampleRate * 3];
        int sampleIndex = 0;
        int totalFrames = recordedSamples.Length;

        SharperPortAudio.Base.Stream.Callback callback = (IntPtr input, IntPtr output,
                                                     uint frameCount,
                                                     ref StreamCallbackTimeInfo timeInfo, StreamCallbackFlags statusFlags,
                                                     IntPtr userData) =>
        {
            int framesToCopy = (int)Math.Min(frameCount, totalFrames - sampleIndex);
            if (framesToCopy > 0)
            {
                var buffer = new float[framesToCopy];
                Marshal.Copy(input, buffer, 0, framesToCopy);
                Array.Copy(buffer, 0, recordedSamples, sampleIndex, framesToCopy);
                sampleIndex += framesToCopy;
            }
            return StreamCallbackResult.Continue;
        };

        SharperPortAudio.Base.Stream stream = new SharperPortAudio.Base.Stream(inputParams, null, sampleRate, framesPerBuffer, StreamFlags.ClipOff, callback, IntPtr.Zero);
        Console.WriteLine("Recording");
        stream.Start();

        Thread.Sleep(3000);

        stream.Stop();
        stream.Close();

        PortAudio.Terminate();

        Console.WriteLine("Writing");
        WriteWavFile("./" + DateTime.Now.Ticks + ".wav", recordedSamples, sampleRate, 1);

        Console.WriteLine("Stopped");
    }

    public static void WriteWavFile(string filePath, float[] audioData, int sampleRate, int channels)
    {
        WaveFormat waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channels);

        using (WaveFileWriter waveFileWriter = new WaveFileWriter(filePath, waveFormat))
        {
            waveFileWriter.WriteSamples(audioData, 0, audioData.Length);
        }
    }
}