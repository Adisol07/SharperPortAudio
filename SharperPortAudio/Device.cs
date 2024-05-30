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

    public static Device DefaultInputDevice => new Device(DeviceType.DefaultInput);
    [Obsolete("This structure is not yet implemented")]
    public static Device DefaultOutputDevice => new Device(DeviceType.DefaultOutput);

    public Device(DeviceType type)
    {
        PortAudio.Initialize();

        switch (type)
        {
            case DeviceType.DefaultInput:
                ID = PortAudio.DefaultInputDevice;
                info = PortAudio.GetDeviceInfo(ID);
                break;
            default:
                throw new Exception("Device of this type does not have implementation yet.");
        }
    }
    public Device(int id)
    {
        PortAudio.Initialize();

        ID = id;
        info = PortAudio.GetDeviceInfo(id);
    }
}