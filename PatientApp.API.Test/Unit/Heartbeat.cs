using Microsoft.AspNetCore.Mvc.Testing;

namespace PatientApp.API.Test.Unit;

public class Heartbeat
{
    ILogger _output;

    public Heartbeat(ITestOutputHelper output)
    {
        _output = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output, LogEventLevel.Verbose)
            .CreateLogger()
            .ForContext<Heartbeat>();
        
        _output.Information("Heartbeat Constructor Executed");

    }
    [Fact]
    public async Task Heartbeat_Application_Is_Online_Success()
    {
        _output.Information("Heartbeat_Application_Is_Online_Success Started");
        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateClient();
        
        var response = await client.GetStringAsync("/heartbeat");
        
        Assert.Equal("API is Online.", response);
        _output.Information("Heartbeat_Application_Is_Online_Success Complete");
    }
}