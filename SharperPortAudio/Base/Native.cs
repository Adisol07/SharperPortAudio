using System;
using System.Runtime.InteropServices;

namespace SharperPortAudio.Base;

internal static class Native
{
    public const string LIBRARY = "portaudio";

    [DllImport(LIBRARY)]
    public static extern int Pa_GetVersion();

    [DllImport(LIBRARY)]
    public static extern IntPtr Pa_GetVersionInfo();

    [DllImport(LIBRARY)]
    public static extern IntPtr Pa_GetErrorText([MarshalAs(UnmanagedType.I4)] ErrorCode errorCode);

    [DllImport(LIBRARY)]
    [return: MarshalAs(UnmanagedType.I4)]
    public static extern ErrorCode Pa_Initialize();

    [DllImport(LIBRARY)]
    [return: MarshalAs(UnmanagedType.I4)]
    public static extern ErrorCode Pa_Terminate();

    [DllImport(LIBRARY)]
    public static extern int Pa_GetDefaultOutputDevice(); //DeviceIndex

    [DllImport(LIBRARY)]
    public static extern int Pa_GetDefaultInputDevice(); //DeviceIndex

    [DllImport(LIBRARY)]
    public static extern IntPtr Pa_GetDeviceInfo(int device); //DeviceIndex

    [DllImport(LIBRARY)]
    public static extern int Pa_GetDeviceCount(); //DeviceIndex

    [DllImport(LIBRARY)]
    public static extern void Pa_Sleep(int msec);

    // STREAM //

    [DllImport(LIBRARY)]
    [return: MarshalAs(UnmanagedType.I4)]
    public static extern ErrorCode Pa_OpenStream(
            out IntPtr stream,
            IntPtr inputParameters,
            IntPtr outputParameters,
            double sampleRate,
            uint framesPerBuffer,
            StreamFlags streamFlags,
            IntPtr streamCallback,
            IntPtr userData);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    [return: MarshalAs(UnmanagedType.I4)]
    public delegate StreamCallbackResult Callback(
            IntPtr input, IntPtr output,
            uint frameCount,
            ref StreamCallbackTimeInfo timeInfo,
            StreamCallbackFlags statusFlags,
            IntPtr userData);

    [DllImport(LIBRARY)]
    [return: MarshalAs(UnmanagedType.I4)]
    public static extern ErrorCode Pa_CloseStream(IntPtr stream);

    [DllImport(LIBRARY)]
    [return: MarshalAs(UnmanagedType.I4)]
    public static extern ErrorCode Pa_SetStreamFinishedCallback(
            IntPtr stream,
            IntPtr streamFinishedCallback);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void FinishedCallback(IntPtr userData);

    [DllImport(LIBRARY)]
    [return: MarshalAs(UnmanagedType.I4)]
    public static extern ErrorCode Pa_StartStream(IntPtr stream);

    [DllImport(LIBRARY)]
    [return: MarshalAs(UnmanagedType.I4)]
    public static extern ErrorCode Pa_StopStream(IntPtr stream);

    [DllImport(LIBRARY)]
    [return: MarshalAs(UnmanagedType.I4)]
    public static extern ErrorCode Pa_AbortStream(IntPtr stream);

    [DllImport(LIBRARY)]
    [return: MarshalAs(UnmanagedType.I4)]
    public static extern ErrorCode Pa_IsStreamStopped(IntPtr stream);

    [DllImport(LIBRARY)]
    [return: MarshalAs(UnmanagedType.I4)]
    public static extern ErrorCode Pa_IsStreamActive(IntPtr stream);

    [DllImport(LIBRARY)]
    public static extern double Pa_GetStreamTime(IntPtr stream);

    [DllImport(LIBRARY)]
    public static extern double Pa_GetStreamCpuLoad(IntPtr stream);
}