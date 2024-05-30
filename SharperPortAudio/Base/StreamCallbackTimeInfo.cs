using System;
using System.Runtime.InteropServices;

namespace SharperPortAudio.Base;

[StructLayout(LayoutKind.Sequential)]
public struct StreamCallbackTimeInfo
{
    public double inputBufferAdcTime; //Time

    public double currentTime; //Time

    public double outputBufferDacTime; //Time
}