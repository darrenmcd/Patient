using System.ComponentModel.DataAnnotations;

namespace PatientApp.Data.Model;

public class Patient: IValidatableObject
{
    public Patient()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        BirthDateStr = string.Empty;
        BirthDate = Convert.ToDateTime("01/01/1900");
    }
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Enum.Gender Gender { get; set; }
    public string BirthDateStr { get; set; }
    public DateTime BirthDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FirstName == LastName)
        {
            yield return new ValidationResult("First Name and Last Name Must Be Unique");
        }
    }
}