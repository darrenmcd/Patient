using System.Globalization;
using FluentValidation;

namespace PatientApp.Data.Validation;

public class PatientValidator : AbstractValidator<Model.Patient>
{
    public PatientValidator()
    {
        //--------------------------------------------------------------
        // Not Null
        //--------------------------------------------------------------
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.BirthDate).NotEmpty();
        RuleFor(x => x.Gender).NotEmpty();

        //--------------------------------------------------------------
        // Max Length
        //--------------------------------------------------------------
        RuleFor(x => x.FirstName).Length(0,50);
        RuleFor(x => x.LastName).Length(0,50);

        //--------------------------------------------------------------
        // Content Matches
        //--------------------------------------------------------------
        RuleFor(x => x.Gender).IsInEnum();
        
        //--------------------------------------------------------------
        // Date Range
        //--------------------------------------------------------------
        RuleFor(x => x.BirthDate).InclusiveBetween(Convert.ToDateTime("01/01/1900"), System.DateTime.Now.Date);
        
    }
}