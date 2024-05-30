using System;
using System.Runtime.InteropServices;

namespace SharperPortAudio.Base;

[StructLayout(LayoutKind.Sequential)]
public struct VersionInfo
{
    public int versionMajor;
    public int versionMinor;
    public int versionSubMinor;

    [MarshalAs(UnmanagedType.LPStr)]
    public string versionControlRevision;

    [MarshalAs(UnmanagedType.LPStr)]
    public string versionText;

    public override string ToString() => "VersionInfo: v" + versionMajor + "." + versionMinor + "." + versionSubMinor;
}