using System;

namespace SharperPortAudio.Base;

public enum StreamFlags : uint
{
    NoFlag = 0,

    ClipOff = 0x00000001,

    DitherOff = 0x00000002,

    NeverDropInput = 0x00000004,

    PrimeOutputBuffersUsingStreamCallback = 0x00000008,

    PlatformSpecificFlags = 0xFFFF0000,
}