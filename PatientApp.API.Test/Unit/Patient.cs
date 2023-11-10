using System.Collections;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using PatientApp.API.Test.Data;

namespace PatientApp.API.Test.Unit;

public class Patient
{
    readonly ILogger _output;
    
    public Patient(ITestOutputHelper output)
    {
        _output = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output, LogEventLevel.Verbose)
            .CreateLogger()
            .ForContext<Logging>();
        
        _output.Information("Logging Constructor Executed");
    }

    #region "Post"

    [Theory] 
    [ClassData(typeof(Data.PatientValidationFail))]
    public async Task Post_Validate_Contents_Fail(PatientApp.Data.Model.Patient patient,string messageShouldBe,string colName)
    {
        _output.Information("Starting Post_Validate_Contents_Fail");
        await using var application = new WebApplicationFactory<Program>();

        var client = application.CreateClient(); 

        var result = await client.PostAsJsonAsync("/api/patient", patient);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

        var validationResult = await result.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();

        _output.Information("Result Returned");
        Assert.NotNull(validationResult);
        Assert.Equal(messageShouldBe,validationResult!.Errors[colName][0]);
        _output.Information("Completed Post_Validate_Contents_Fail");
    }
    
    [Fact] 
    public async Task Post_Success()
    {
        string firstName = "Bob";
        string lastName = "Barker";
        DateTime birthDate = Convert.ToDateTime("01/01/2000");
        PatientApp.Data.Enum.Gender gender = PatientApp.Data.Enum.Gender.Male;

        _output.Information("Starting Post_Success");
        PatientApp.Data.Model.Patient patient = new PatientApp.Data.Model.Patient()
        {
            FirstName = firstName,
            LastName = lastName,
            BirthDate = birthDate,
            Gender = gender
        };
        
        await using var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient(); 
        
        var result = await client.PostAsJsonAsync("/api/patient", patient);

        _output.Information(result.StatusCode.ToString());

        //We failed so log as to why
        if (result.StatusCode != HttpStatusCode.Created)
        {
            var validationResult = await result.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
            if (validationResult != null)
                foreach (var valResult in validationResult.Errors)
                    _output.Information($"Validation Error: Key:{valResult.Key} Value:{valResult.Value[0]}");
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            return;
        }

        //Serialize our result
        PatientApp.Data.Model.Patient? patientResponse = await result.Content.ReadFromJsonAsync<PatientApp.Data.Model.Patient>();
 
        _output.Information("Record Created");
        Assert.NotNull(patientResponse?.IdPatient);
        Assert.Equal(firstName,patient.FirstName);
        Assert.Equal(lastName,patient.LastName);
        Assert.Equal(birthDate,patient.BirthDate);
        Assert.Equal(gender,patient.Gender);

        _output.Information("Completed Post Success");
    }
    
    #endregion

    #region "Put"
    
    [Fact] 
    public async Task Put_NoContent()
    {
        int idPatent = int.MaxValue;
        string firstName = "Bob";
        string lastName = "Barker";
        DateTime birthDate = Convert.ToDateTime("01/01/2000");
        PatientApp.Data.Enum.Gender gender = PatientApp.Data.Enum.Gender.Male;

        _output.Information("Starting Put_NoContent");
        PatientApp.Data.Model.Patient patient = new PatientApp.Data.Model.Patient()
        {
            IdPatient = idPatent,
            FirstName = firstName,
            LastName = lastName,
            BirthDate = birthDate,
            Gender = gender
        };
        
        await using var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient(); 
        
        var result = await client.PutAsJsonAsync($"/api/patient",patient);

        _output.Information(result.StatusCode.ToString());
        
        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);

        _output.Information("Completed Put_NoContent");
    }
    
    

    #endregion
    
    #region "List"
    
    [Fact] 
    public async Task List_NoFilter_ReturnAllData()
    {
        _output.Information("Starting List_NoData");
        
        await using var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient(); 
        
        var result = await client.GetAsync($"/api/patients");

        _output.Information(result.StatusCode.ToString());
        
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        _output.Information("Completed List_NoData");
    }

    

    #endregion

    #region "Get"

    [Fact] 
    public async Task Get_NoContent()
    {
        int idPatent = int.MaxValue;

        _output.Information("Starting Get_NoContent");
        
        await using var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient(); 
        
        var result = await client.GetAsync($"/api/patient/{idPatent}");

        _output.Information(result.StatusCode.ToString());
        
        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);

        _output.Information("Completed Get_NoContent");
    }
    
    [Fact] 
    public async Task Get_Success()
    {
        int idPatent = 1;

        _output.Information("Starting Get_NoContent");
        
        await using var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder
            .ConfigureServices(services =>
            {
                //services.AddScoped<IPeopleService, TestPeopleService>();
            }));
        
        var client = application.CreateClient(); 
        
        var result = await client.GetAsync($"/api/patient/{idPatent}");

        _output.Information(result.StatusCode.ToString());
        
        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);

        _output.Information("Completed Get_NoContent");
    }
    

    #endregion

    #region "Delete"
    
    [Fact] 
    public async Task Delete_NoContent()
    {
        int idPatent = int.MaxValue;

        _output.Information("Starting Get_NoContent");
        
        await using var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient(); 
        
        var result = await client.DeleteAsync($"/api/patient/{idPatent}");

        _output.Information(result.StatusCode.ToString());
        
        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);

        _output.Information("Completed Get_NoContent");
    }


    #endregion

}
 