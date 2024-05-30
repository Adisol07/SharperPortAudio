using System;

namespace SharperPortAudio;

public enum DeviceType
{
    Input = 0,
    Output = 1,
    DefaultInput = 10,
    DefaultOutput = 11,
    None = -1,
}