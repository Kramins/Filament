namespace filament.providers.metadata;

public class FfmpegVideoStreamJsonModel
{
    public long BitRate { get; internal set; }
    public string Codec { get; internal set; }
    public int Duration { get; internal set; }
    public double FrameRate { get; internal set; }
    public int Height { get; internal set; }
    public int Index { get; internal set; }
    public string? Language { get; internal set; }
    public int Width { get; internal set; }
}