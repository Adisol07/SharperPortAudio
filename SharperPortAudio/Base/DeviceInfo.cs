using System;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharperPortAudio.Base;

[StructLayout(LayoutKind.Sequential)]
public struct DeviceInfo
{
    public int structVersion;

    [MarshalAs(UnmanagedType.LPStr)]
    public string name;

    public int hostApi; //HostApiIndex

    public int maxInputChannels;
    public int maxOutputChannels;

    public double defaultLowInputLatency; //Time
    public double defaultLowOutputLatency; //Time

    public double defaultHighInputLatency; //Time
    public double defaultHighOutputLatency; //Time

    public double defaultSampleRate;

    public override string ToString()
    {
        string output = "";
        output += name + ": \n";
        output += "  Max input channels: " + maxInputChannels + "\n";
        output += "  Max output channels: " + maxOutputChannels + "\n";
        output += "  Default sample rate: " + defaultSampleRate;
        return output;
    }
}