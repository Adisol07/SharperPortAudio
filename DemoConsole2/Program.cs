using System;
using SharperPortAudio;

namespace DemoConsole2;

internal class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Loading..");
        AudioRecorder recorder = new AudioRecorder(Device.DefaultInputDevice);
        recorder.Start();
        Console.WriteLine("Recoding..");
        Console.ReadKey();
        Audio audio = recorder.Stop();
        File.WriteAllBytes("./" + DateTime.Now.Ticks + ".wav", audio.ToWave());
        Console.WriteLine("Stopped");
    }
}