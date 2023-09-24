namespace filament.providers.metadata;

public class FfmpegAudioStreamJsonModel
{
    public long BitRate { get; internal set; }
    public int Channels { get; internal set; }
    public string Codec { get; internal set; }
    public int Duration { get; internal set; }
    public int Index { get; internal set; }
    public string? Language { get; internal set; }
    public int SampleRateHz { get; internal set; }
}