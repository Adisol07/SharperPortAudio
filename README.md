# SharperPortAudio
SharperPortAudio let's you record audio for all desktop operating systems.\
Latest version: 1.0.3\
Wiki: https://github.com/Adisol07/SharperPortAudio/wiki \
NuGet: https://www.nuget.org/packages/SharperPortAudio/
___
## About
Code for base portaudio implementation is from [PortAudioSharp2](https://github.com/csukuangfj/PortAudioSharp2/tree/master) ([PortAudioSharp](https://github.com/BeaQueen/portaudiosharp))\
This library contains base portaudio wrapper and most runtimes bundled in (win-64x, osx-64x, linux-64x, win-arm, osx-arm, linux-arm)\
Also this library uses NAudio for Wave format processing in Audio class.
___
## Examples
You can find examples in source code (DemoConsole1 and DemoConsole2)
- DemoConsole1 contains implementation of 3 second recording using just base port audio wrapper.
- DemoConsole2 contains implementation of audio recording that stops when user presses any key using AudioRecorder class.
___
## Upcoming features
 - [x] Make PortAudio auto-initialize
 - [ ] Add bonus functions to AudioRecorder
 - [ ] Add AudioPlayer
 - [x] Add ability to save recordings to file
 - [ ] Add support for .NET6.0, .NET7.0, .NET9.0
