namespace Common.Logging;

public class LoggingOptions
{
    public string ServiceName { get; set; } = "UnknownService";
    public string? ElasticsearchUrl { get; set; }
    public string? SeqUrl { get; set; }
}
