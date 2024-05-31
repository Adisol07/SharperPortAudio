using System;
using SharperPortAudio;

namespace DemoConsole3;

internal class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Loading..");
        AudioRecorder recorder = new AudioRecorder(Device.DefaultInputDevice){
            SendBufferInsteadOfChunk = true,
            BufferEnabled = true,
        };
        int x = 0;
        int y = 0;
        recorder.DataReceived += (data) =>
        {
            Console.SetCursorPosition(x, y);
            Console.Write("                                  ");
            Console.SetCursorPosition(x, y);
            Console.Write(data.Samples?.Length);
        };
        Console.WriteLine("Recording");
        Console.Write("Buffer size: ");
        x = Console.GetCursorPosition().Left;
        y = Console.GetCursorPosition().Top;
        recorder.Start();
        Console.ReadKey();
        recorder.Stop();
        Console.WriteLine("Stopped");
    }
}