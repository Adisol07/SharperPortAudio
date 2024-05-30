using System;
using SharperPortAudio.Base;

namespace SharperPortAudio;

public class Device
{
    private DeviceInfo info;

    public int ID { get; private set; }
    public string Name => info.name;
    public int HostAPIIndex => info.hostApi;

    public int MaxInputChannels => info.maxInputChannels;
    public int MaxOutputChannels => info.maxOutputChannels;

    public double DefaultLowInputLatency => info.defaultLowInputLatency;
    public double DefaultHighInputLatency => info.defaultHighInputLatency;

    public double DefaultLowOutputLatency => info.defaultLowOutputLatency;
    public double DefaultHighOutputLatency => info.defaultHighOutputLatency;

    public double DefaultSampleRate => info.defaultSampleRate;

    public static Device DefaultInputDevice => new Device(PortAudio.DefaultInputDevice);
    public static Device DefaultOutputDevice => new Device(PortAudio.DefaultOutputDevice);

    public Device(int id)
    {
        ID = id;
        info = PortAudio.GetDeviceInfo(id);
    }
}