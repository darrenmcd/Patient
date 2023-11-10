namespace PatientApp.API.Test.Unit;

public class Logging
{
    readonly ILogger _output;

    public Logging(ITestOutputHelper output)
    {
        _output = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output, LogEventLevel.Verbose)
            .CreateLogger()
            .ForContext<Logging>();
        
        _output.Information("Logging Constructor Executed");
    }

    /// <summary>
    /// This is just an example showing the logging output to the xunit testing console
    /// </summary>
    [Fact]
    public void Log_To_XUnit_Output()
    {
        _output.Verbose("This is a Log Verbose Message");
        _output.Debug("This is a Log Debug Message");
        _output.Information("This is a Log Informational Message");
        _output.Error("This is a Log Error Message");
        _output.Fatal("This is a Log Fatal Message");
        _output.Warning("This is a Warning Fatal Message");
    }
}