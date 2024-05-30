using System;

namespace SharperPortAudio.Base;

public enum StreamCallbackResult
{
    Continue = 0,

    Complete = 1,

    Abort = 2,
}