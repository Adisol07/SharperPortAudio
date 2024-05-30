using System;

namespace SharperPortAudio.Base;

public class PortAudioException : Exception
{
    public ErrorCode ErrorCode { get; private set; }

    public PortAudioException(ErrorCode ex) : base()
    { ErrorCode = ex; }
    public PortAudioException(ErrorCode ex, string message) : base(message)
    { ErrorCode = ex; }
    public PortAudioException(ErrorCode ex, string message, Exception inner) : base(message, inner)
    { ErrorCode = ex; }
}