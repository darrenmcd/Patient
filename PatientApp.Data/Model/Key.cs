namespace PatientApp.Data.Model;

public class Key
{
    public Key()
    {
        Value = string.Empty;
    }
    public int IdPatient { get; set; }
    public string Value { get; set; }
}