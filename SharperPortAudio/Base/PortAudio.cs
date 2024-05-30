using System;
using System.Runtime.InteropServices;

namespace SharperPortAudio.Base;

public static class PortAudio
{
    public const int NODEVICE = -1; //DeviceIndex
    public const uint FRAMESPERBUFFERUNSPECIFIED = 0;

    public static int Version => Native.Pa_GetVersion();
    public static VersionInfo VersionInfo => Marshal.PtrToStructure<VersionInfo>(Native.Pa_GetVersionInfo());

    public static string GetErrorText(ErrorCode errorCode) => Marshal.PtrToStringAnsi(Native.Pa_GetErrorText(errorCode))!;

    public static void Initialize()
    {
        ErrorCode ex = Native.Pa_Initialize();
        if (ex != ErrorCode.NoError)
            throw new PortAudioException(ex, "Error while initializing PortAudio.");
    }
    public static void Terminate()
    {
        ErrorCode ex = Native.Pa_Terminate();
        if (ex != ErrorCode.NoError)
            throw new PortAudioException(ex, "Error while terminating PortAudio.");
    }

    public static int DefaultOutputDevice => Native.Pa_GetDefaultOutputDevice(); //DeviceIndex
    public static int DefaultInputDevice => Native.Pa_GetDefaultInputDevice(); //DeviceIndex
    public static DeviceInfo GetDeviceInfo(int device) => Marshal.PtrToStructure<DeviceInfo>(Native.Pa_GetDeviceInfo(device)); //DeviceIndex
    public static int DeviceCount => Native.Pa_GetDeviceCount(); //DeviceIndex
}

