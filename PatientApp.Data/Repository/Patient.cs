using Microsoft.EntityFrameworkCore;

namespace PatientApp.Data.Repository;

public interface IPatient
{
    Task AddPatientAsync(Model.Patient patient);
    void AddPatient(Model.Patient patient);

    Task UpdatePatientAsync(Model.Patient patient);
    void UpdatePatient(Model.Patient patient);
    
    Task<PatientApp.Data.Model.Patient?> GetPatientAsync(int id);
    PatientApp.Data.Model.Patient? GetPatient(int id);

    Task<IList<PatientApp.Data.Model.Patient>> ListPatientAsync(string? firstName,string? lastName,DateTime? birthDate,char? gender);
    IList<PatientApp.Data.Model.Patient> ListPatient(string? firstName,string? lastName,DateTime? birthDate,char? gender);

    Task<bool> DeletePatientAsync(int id);
    bool DeletePatient(int id);
}
public class Patient : IPatient
{
    private readonly PatientContext _patientContext;
    public Patient(PatientContext patientContext)
    {
        _patientContext = patientContext;
    }
    
    //---------------------------------------------------------------------------------------------------
    // Add
    //---------------------------------------------------------------------------------------------------
    public async Task AddPatientAsync(Model.Patient patient)
    {
        await _patientContext.AddAsync(patient);
        await _patientContext.SaveChangesAsync();
    }
    public void AddPatient(Model.Patient patient)
    { 
        _patientContext.Add(patient);
        _patientContext.SaveChanges();
    }
    //---------------------------------------------------------------------------------------------------
    // Update
    //---------------------------------------------------------------------------------------------------
    public async Task UpdatePatientAsync(Model.Patient patient)
    {
        _patientContext.Attach(patient);
        await _patientContext.SaveChangesAsync();
    }
    public void UpdatePatient(Model.Patient patient)
    { 
        _patientContext.Attach(patient);
        _patientContext.SaveChanges();
    }
    //---------------------------------------------------------------------------------------------------
    // Get
    //---------------------------------------------------------------------------------------------------
    public async Task<Model.Patient?> GetPatientAsync(int id)
    {
        return await _patientContext.Patient.SingleOrDefaultAsync(x => x.IdPatient == id);
    }
    public Model.Patient? GetPatient(int id)
    {
        return  _patientContext.Patient.SingleOrDefault(x => x.IdPatient == id);
    }
    //---------------------------------------------------------------------------------------------------
    // List
    //---------------------------------------------------------------------------------------------------
    public async Task<IList<Model.Patient>> ListPatientAsync(string? firstName,string? lastName,DateTime? birthDate,char? gender)
    {
        return await _patientContext.Patient
            .Where(x => (x.FirstName == firstName || firstName == null)
                && (x.LastName == firstName || firstName == null)
                && (x.BirthDate == birthDate || birthDate == null)
                && (Convert.ToChar(x.Gender) == gender) || gender == null)
            .ToListAsync();
    }

    public IList<Model.Patient> ListPatient(string? firstName,string? lastName,DateTime? birthDate,char? gender)
    {
        return _patientContext.Patient.ToList();
    }
    //---------------------------------------------------------------------------------------------------
    // Delete
    //---------------------------------------------------------------------------------------------------
    public async Task<bool> DeletePatientAsync(int id)
    {
        PatientApp.Data.Model.Patient? patient = await _patientContext.Patient.SingleOrDefaultAsync(x => x.IdPatient == id);
        if (patient == null)
            return false;

        _patientContext.Patient.Remove(patient);
        await _patientContext.SaveChangesAsync();
        return true;
    }
    public bool DeletePatient(int id)
    {
        PatientApp.Data.Model.Patient? patient = _patientContext.Patient.SingleOrDefault(x => x.IdPatient == id);
        if (patient == null)
            return false;

        _patientContext.Patient.Remove(patient); _patientContext.SaveChanges();
        return true;
    }
}