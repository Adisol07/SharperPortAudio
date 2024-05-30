using System;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace SharperPortAudio.Base;

[StructLayout(LayoutKind.Sequential)]
public struct StreamParameters
{
    /// </summary>
    public int device; //DeviceIndex

    public int channelCount;

    public SampleFormat sampleFormat;

    public double suggestedLatency; //Time

    public IntPtr hostApiSpecificStreamInfo;

    public override string ToString() => JsonSerializer.Serialize(this);
}