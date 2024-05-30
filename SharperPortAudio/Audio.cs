using System;
using NAudio.Wave;

namespace SharperPortAudio;

public class Audio
{
    public float[]? Samples { get; set; }
    public int SampleRate { get; set; }
    public int Channels { get; set; }

    public Audio()
    { }
    public Audio(float[] samples, int sampleRate, int channels)
    {
        Samples = samples;
        SampleRate = sampleRate;
        Channels = channels;
    }

    public byte[] ToWave()
    {
        WaveFormat waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(SampleRate, Channels);

        using (MemoryStream memory = new MemoryStream())
        {
            using (WaveFileWriter waveFileWriter = new WaveFileWriter(memory, waveFormat))
            {
                waveFileWriter.WriteSamples(Samples, 0, Samples!.Length);
            }

            return memory.ToArray();
        }
    }
}