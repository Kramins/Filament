
using System.Text.Json;
using FFMpegCore;
using filament.data.models;
using Newtonsoft.Json;

namespace filament.providers.metadata;
public class FfmpegMetaDataProvider : IFileMetaDataProvider
{
    private readonly ILogger _logger;

    public FfmpegMetaDataProvider(ILogger logger)
    {
        _logger = logger;
    }
    public async Task<List<LibraryFileMetaData>> ScanAsync(string filePath)
    {
        FileInfo fileInfo = new(filePath);
        List<LibraryFileMetaData> metadata = new();

        if (!fileInfo.Exists)
        {
            _logger.LogWarning($"File {filePath} does not exist");
            throw new FileNotFoundException($"File {filePath} does not exist");
        }

        var mediaInfo = await FFProbe.AnalyseAsync(filePath);

        foreach (var videoStream in mediaInfo.VideoStreams)
        {
            var mediaVideoStreamModel = new FfmpegVideoStreamJsonModel()
            {
                BitRate = videoStream.BitRate,
                Codec = videoStream.CodecName,
                Duration = videoStream.Duration.Milliseconds,
                FrameRate = videoStream.FrameRate,
                Height = videoStream.Height,
                Index = videoStream.Index,
                Language = videoStream.Language,
                Width = videoStream.Width
            };

            var json = JsonConvert.SerializeObject(mediaVideoStreamModel);
            var filemetadata = new LibraryFileMetaData()
            {
                Category = "media",
                Name = "video-stream",
                Value = JsonDocument.Parse(json),
                Source = "ffmpeg"
            };

            metadata.Add(filemetadata);
        }

        foreach (var audioStream in mediaInfo.AudioStreams)
        {
            var mediaAudioStreamModel = new FfmpegAudioStreamJsonModel()
            {
                BitRate = audioStream.BitRate,
                Channels = audioStream.Channels,
                Codec = audioStream.CodecName,
                Duration = audioStream.Duration.Milliseconds,
                Index = audioStream.Index,
                Language = audioStream.Language,
                SampleRateHz = audioStream.SampleRateHz
            };

            var json = JsonConvert.SerializeObject(mediaAudioStreamModel);
            var filemetadata = new LibraryFileMetaData()
            {
                Category = "media",
                Name = "audio-stream",
                Value = JsonDocument.Parse(json),
                Source = "ffmpeg"
            };

            metadata.Add(filemetadata);
        }


        return metadata;
    }
}