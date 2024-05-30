using System;

namespace SharperPortAudio.Base;

public enum StreamCallbackFlags : uint
{
    InputUnderflow = 0x00000001,

    InputOverflow = 0x00000002,

    OutputUnderflow = 0x00000004,

    OutputOverflow = 0x00000008,

    PrimingOutput = 0x00000010
}