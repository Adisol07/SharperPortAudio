using System;
using System.Runtime.InteropServices;

namespace SharperPortAudio.Base;

public class Stream : IDisposable
{
    private bool disposed = false;
    private IntPtr streamPtr = IntPtr.Zero;
    private GCHandle userDataHandle;

    private _NativeInterfacingCallback<Callback> streamCallback = null!;
    private _NativeInterfacingCallback<FinishedCallback> finishedCallback = null!;

    public StreamParameters? inputParameters { get; private set; }

    public StreamParameters? outputParameters { get; private set; }


    public Stream(
        StreamParameters? inParams,
        StreamParameters? outParams,
        double sampleRate,
        uint framesPerBuffer,
        StreamFlags streamFlags,
        Callback callback,
        object userData)
    {
        streamCallback = new _NativeInterfacingCallback<Callback>(callback);

        userDataHandle = GCHandle.Alloc(userData);

        inputParameters = inParams;
        outputParameters = outParams;

        IntPtr inParamsPtr = IntPtr.Zero;
        IntPtr outParamsPtr = IntPtr.Zero;
        if (inParams.HasValue)
        {
            inParamsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(inParams.Value));
            Marshal.StructureToPtr<StreamParameters>(inParams.Value, inParamsPtr, false);
        }
        if (outParams.HasValue)
        {
            outParamsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(outParams.Value));
            Marshal.StructureToPtr<StreamParameters>(outParams.Value, outParamsPtr, false);
        }

        ErrorCode ex = Native.Pa_OpenStream(
            out streamPtr,
            inParamsPtr,
            outParamsPtr,
            sampleRate,
            framesPerBuffer,
            streamFlags,
            streamCallback.Ptr,
            GCHandle.ToIntPtr(userDataHandle)
        );
        if (ex != ErrorCode.NoError)
            throw new PortAudioException(ex, "Error while opening PortAudio stream.");

        if (inParamsPtr != IntPtr.Zero)
            Marshal.FreeHGlobal(inParamsPtr);
        if (outParamsPtr != IntPtr.Zero)
            Marshal.FreeHGlobal(outParamsPtr);
    }

    ~Stream()
    {
        dispose(false);
    }
    public void Dispose()
    {
        dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void dispose(bool disposing)
    {
        if (disposed)
            return;

        if (disposing)
        { }

        Close();
        userDataHandle.Free();
        streamCallback.Free();
        if (finishedCallback != null)
            finishedCallback.Free();

        disposed = true;
    }

    public void SetFinishedCallback(FinishedCallback fcb)
    {
        finishedCallback = new _NativeInterfacingCallback<FinishedCallback>(fcb);

        ErrorCode ex = Native.Pa_SetStreamFinishedCallback(streamPtr, finishedCallback.Ptr);
        if (ex != ErrorCode.NoError)
            throw new PortAudioException(ex, "Error while setting finished callback for PortAudio stream.");
    }

    public void Close()
    {
        if (streamPtr == IntPtr.Zero)
            return;

        ErrorCode ex = Native.Pa_CloseStream(streamPtr);
        if (ex != ErrorCode.NoError)
            throw new PortAudioException(ex, "Error while closing PortAudio stream.");

        streamPtr = IntPtr.Zero;
    }

    public void Start()
    {
        ErrorCode ex = Native.Pa_StartStream(streamPtr);
        if (ex != ErrorCode.NoError)
            throw new PortAudioException(ex, "Error while starting PortAudio stream.");
    }

    public void Stop()
    {
        ErrorCode ex = Native.Pa_StopStream(streamPtr);
        if (ex != ErrorCode.NoError)
            throw new PortAudioException(ex, "Error while stopping PortAudio stream.");
    }

    public void Abort()
    {
        ErrorCode ex = Native.Pa_AbortStream(streamPtr);
        if (ex != ErrorCode.NoError)
            throw new PortAudioException(ex, "Error while aborting PortAudio stream.");
    }

    public bool IsStopped
    {
        get
        {
            ErrorCode ex = Native.Pa_IsStreamStopped(streamPtr);

            switch (ex)
            {
                case (ErrorCode)1:
                    return true;
                case ErrorCode.NoError: //0
                    return false;
                default:
                    throw new PortAudioException(ex, "Error while checking if PortAudio stream is stopped.");
            }
        }
    }

    public bool IsActive
    {
        get
        {
            ErrorCode ex = Native.Pa_IsStreamActive(streamPtr);

            switch (ex)
            {
                case (ErrorCode)1:
                    return true;
                case ErrorCode.NoError: //0
                    return false;
                default:
                    throw new PortAudioException(ex, "Error while checking if PortAudio stream is active.");
            }
        }
    }

    public double Time
    {
        get => Native.Pa_GetStreamTime(streamPtr);
    }

    public double CPULoad
    {
        get => Native.Pa_GetStreamCpuLoad(streamPtr);
    }

    public delegate StreamCallbackResult Callback(
        IntPtr input, IntPtr output,
        uint frameCount,
        ref StreamCallbackTimeInfo timeInfo,
        StreamCallbackFlags statusFlags,
        IntPtr userDataPtr);

    public delegate void FinishedCallback(
        IntPtr userDataPtr);

    public static UD GetUserData<UD>(IntPtr userDataPtr) => (UD)GCHandle.FromIntPtr(userDataPtr).Target!;

    private class _NativeInterfacingCallback<CB>
        where CB : Delegate
    {
        private CB callback;

        private GCHandle handle;

        public IntPtr Ptr { get; private set; } = IntPtr.Zero;

        public _NativeInterfacingCallback(CB cb)
        {
            callback = cb ?? throw new ArgumentNullException(nameof(cb));
            handle = GCHandle.Alloc(cb);
            Ptr = Marshal.GetFunctionPointerForDelegate<CB>(cb);
        }

        public void Free() => handle.Free();
    }
}