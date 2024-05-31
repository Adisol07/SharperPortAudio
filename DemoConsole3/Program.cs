using System;
using SharperPortAudio;

namespace DemoConsole3;

internal class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Loading..");
        AudioRecorder recorder = new AudioRecorder(Device.DefaultInputDevice);
        recorder.SendBufferInsteadOfChunk = true;
        recorder.BufferEnabled = true;
        recorder.DataReceived += (data) =>
        {
            Console.WriteLine("Buffer size: " + data.Samples?.Length);
        };
        Console.WriteLine("Recording");
        recorder.Start();
        Console.ReadKey();
        recorder.Stop();
        Console.WriteLine("Stopped");
    }
}