using NAudio.Wave;
using System.IO;

namespace voicio.Models
{
    public class NAudioRecorder
{
    private readonly WaveInEvent Microphone;
    private bool IsRecording = false;
    private readonly WaveFileWriter CustomWaveProvider;
    private readonly MemoryStream CustomStream;
    private void DataAvailableEvent(object? sender, WaveInEventArgs e)
    {
        CustomWaveProvider.Write(e.Buffer, 0, e.BytesRecorded);
    }
    public void StartRecord()
    {
        IsRecording = true;
        Microphone.StartRecording();
    }
    public void StopRecord()
    {
        IsRecording = false;
        Microphone.StopRecording();
    }
    public byte[] GetByteArray()
    {
        return CustomStream.ToArray();
    }
    public float GetRecorderSampleRate()
    {
            return (float)Microphone.WaveFormat.SampleRate;
    }
    public NAudioRecorder()
    {
        Microphone = new WaveInEvent()
        {
            WaveFormat = new WaveFormat(rate: 48000, bits: 16, channels: 1),
            DeviceNumber = 0,
            BufferMilliseconds = 10000,
        };
        Microphone.DataAvailable += DataAvailableEvent;
        CustomStream = new MemoryStream();
        CustomWaveProvider = new WaveFileWriter(CustomStream, Microphone.WaveFormat) { };
    }
}
}
