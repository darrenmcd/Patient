using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PatientApp.Data.Enum;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// remove default logging providers
builder.Logging.ClearProviders();

//Add Serilog as our logging output
// Serilog configuration		
var logger = new LoggerConfiguration()
        .WriteTo.Console()
        .CreateLogger();

// Register Serilog
builder.Logging.AddSerilog(logger);

// Add services to the container.

//Add our DBContext so that we can use it with DI.
builder.Services.AddDbContext<PatientApp.Data.PatientContext>(options =>
{
    //Because this application does not need to be volatile and save state
    //we can just use a in-memory database
    options.UseInMemoryDatabase(Guid.NewGuid().ToString());

    //Uncomment below if you would like to use a sql server
    //options.UseSqlServer(builder.Configuration.GetConnectionString("PatientDB"));
});

//Add our Validators
builder.Services.AddScoped<IValidator<PatientApp.Data.Model.Patient>, PatientApp.Data.Validation.PatientValidator>();

//Add our Repository
builder.Services.AddScoped<PatientApp.Data.Repository.IPatient, PatientApp.Data.Repository.Patient>();


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
//Only use swagger if we are in a development environment.
app.UseStaticFiles();

//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//--  Simple Heartbeat to make sure api is up and running --
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
app.MapGet("/heartbeat/ping", () => "API is Online.");
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//--  API that will upload our file 
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
app.MapPost("/file/upload",
    async Task<IResult> (HttpRequest request,PatientApp.Data.PatientContext patientContext) =>
    {
        if (!request.HasFormContentType)
            return Results.BadRequest();

        var form = await request.ReadFormAsync();

        if (form.Files.Any() == false)
            return Results.BadRequest("There are no files");

        var file = form.Files.FirstOrDefault();

        if (file is null || file.Length == 0)
            return Results.BadRequest("File cannot be empty");

        //create a list for each line
        List<string> patientLines = new List<string>();
        using var stream = file.OpenReadStream();
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    string? line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                        patientLines.Add(line);
                }
            }
    
        //loop through each line and insert data into our patient table
        foreach (string line in patientLines)
        {
            if (line.StartsWith("First Name"))
                continue;

            string[] patientData = line.Split(",");
            PatientApp.Data.Model.Patient patient = new PatientApp.Data.Model.Patient();
            patient.FirstName = patientData[0];
            patient.LastName = patientData[1];
            patient.Gender = Gender.Unknown;
            if (Enum.TryParse(patientData[2],out PatientApp.Data.Enum.Gender genderOut))
                patient.Gender = genderOut;
            if (DateTime.TryParse(patientData[3],out DateTime birthDate))
                patient.BirthDate = birthDate;
            
            //check to see if client is already in the database
            //We Use Count() here it is faster then Any().
            if (patientContext.Patient.Count(x => x.FirstName == patient.FirstName
                                                  && x.LastName == patient.LastName
                                                  && x.Gender == patient.Gender
                                                  && x.BirthDate == patient.BirthDate) == 0)
            {
                patientContext.Patient.Add(patient);
            }
        }
        await patientContext.SaveChangesAsync();
        
        return Results.Ok();
    }).Accepts<IFormFile>("multipart/form-data"); 

//
// app.MapPost("/file/upload", async (HttpRequest request) =>
// {
//     using (var reader = new StreamReader(request.Body, System.Text.Encoding.UTF8))
//     {
//         // Read the raw file as a `string`.
//         string fileContent = await reader.ReadToEndAsync();
//
//         // Do something with `fileContent`...
//
//         return "File Was Processed Sucessfully!";
//     }
// }).Accepts("multipart/form-data")
//     .Produces(200);

// app.MapPost("/file/upload", async (HttpRequest request) =>
// {
//     using (var reader = new StreamReader(request.Body, System.Text.Encoding.UTF8))
//     {
//         // Read the raw file as a `string`.
//         string fileContent = await reader.ReadToEndAsync();
//
//         // Do something with `fileContent`...
//
//         return "File Was Processed Sucessfully!";
//     }
// }).Accepts<IFormFile>("text/plain");

// app.MapPost("/upload", async (HttpRequest req) =>
//     {
//         if (!req.HasFormContentType)
//         {
//             return Results.BadRequest();
//         }
//  
//         var form = await req.ReadFormAsync();
//         var file = form.Files["file"];
//  
//         if (file is null)
//         {
//             return Results.BadRequest();
//         }
//  
//         //var uploads = Path.Combine(uploadsPath, file.FileName);
//         //await using var fileStream = File.OpenWrite(uploads);
//         //await using var uploadStream = file.OpenReadStream();
//         //await uploadStream.CopyToAsync(fileStream);
//  
//         return Results.NoContent();
//     })
//     .Accepts<IFormFile>("multipart/form-data");

// app.MapPost("/upload", (HttpRequest request) =>
//     {
//         //Do something with the file
//         var files = request.Form.Files;
//         
//         
//         return Results.Ok();
//     })
//     .Accepts("multipart/form-data")
//     .Produces(200);

//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//--  Patient Crud Commands --
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

//List (get) endpoint : filter by first/last name
app.MapGet("/data/patients", async (string? firstName,string? lastName,DateTime? birthDate,char? gender, PatientApp.Data.Repository.IPatient? repository) =>
{
    IList<PatientApp.Data.Model.Patient> patients = await repository.ListPatientAsync(firstName,lastName,birthDate,gender);
    return Results.Ok(patients);
});

//Post endpoint
app.MapPost("/data/patient", async (IValidator<PatientApp.Data.Model.Patient> validator, PatientApp.Data.Repository.IPatient repository, PatientApp.Data.Model.Patient patient) =>
{
    var validationResult = await validator.ValidateAsync(patient);
    if (!validationResult.IsValid)
        return Results.ValidationProblem(validationResult.ToDictionary());

    await repository.AddPatientAsync(patient);
    return Results.Created($"/{patient.IdPatient}", patient);
});

//update endpoint
app.MapPut("/data/patient", async  (IValidator<PatientApp.Data.Model.Patient> validator, PatientApp.Data.Repository.IPatient repository, PatientApp.Data.Model.Patient patient) =>
{
    var validationResult = await validator.ValidateAsync(patient);
    if (!validationResult.IsValid)
        return Results.ValidationProblem(validationResult.ToDictionary());

    await repository.UpdatePatientAsync(patient);
    return Results.NoContent();
});

// delete endpoint.
app.MapGet("/data/patient/{idPatient}", async (PatientApp.Data.Repository.IPatient repository, int idPatient) =>
{
    PatientApp.Data.Model.Patient? patient = await repository.GetPatientAsync(idPatient);

    return patient == null ? Results.NoContent():Results.Ok(patient);
});

// delete endpoint.
app.MapDelete("/data/patient/{idPatient}", async (PatientApp.Data.Repository.IPatient repository, int idPatient) =>
{
    bool result = await repository.DeletePatientAsync(idPatient);

    return !result ? Results.NoContent() : Results.Ok();
});


//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

app.MapFallbackToFile("index.html");


app.Run();


//The class declaration below is there to initialize our unit tests
public partial class Program { }
