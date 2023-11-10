using System.Collections;
using System.Globalization;

namespace PatientApp.API.Test.Data;


public class PatientValidationFail:IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new PatientApp.Data.Model.Patient()
            {
                FirstName = string.Empty,
                LastName = "Mouse",
                BirthDate = System.DateTime.Now.AddYears(-1).Date,
                Gender = PatientApp.Data.Enum.Gender.Male
            },
            "'First Name' must not be empty.",
            "FirstName"
        };
        yield return new object[]
        {
            new PatientApp.Data.Model.Patient()
            {
                FirstName = "Daisy",
                LastName = string.Empty,
                BirthDate = System.DateTime.Now.AddYears(-1).Date,
                Gender = PatientApp.Data.Enum.Gender.Male
            },
            "'Last Name' must not be empty.",
            "LastName"
        };
        yield return new object[]
        {
            new PatientApp.Data.Model.Patient()
            {
                FirstName = "Wiley",
                LastName = "Coyote",
                BirthDate = DateTime.MinValue,
                Gender = PatientApp.Data.Enum.Gender.Male
            },
            "'Birth Date' must not be empty.",
            "BirthDate"
        };
        yield return new object[]
        {
            new PatientApp.Data.Model.Patient()
            {
                FirstName = "Tasmanian",
                LastName = "Devil",
                BirthDate = DateTime.MaxValue,
                Gender = PatientApp.Data.Enum.Gender.Male
            },
            $"'Birth Date' must be between 1/1/1900 12:00:00 AM and {DateTime.Now.Date.ToString("MM/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)}. You entered {DateTime.MaxValue.ToString("MM/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)}.",
            "BirthDate"
        };
        yield return new object[]
        {
            new PatientApp.Data.Model.Patient()
            {
                FirstName = "Tasmanian",
                LastName = "Devil",
                BirthDate = DateTime.MinValue,
                Gender = (PatientApp.Data.Enum.Gender)'P'
            },
            $"'Gender' has a range of values which does not include '{(int)'P'}'.",
            "Gender"
        };
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}