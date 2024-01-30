using System.ComponentModel.DataAnnotations;
using App.Domain;
using App.Domain.Constants;

namespace Public.DTO.ValidationAttributes;

public class ValidCulturesAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
    {
        var validCultures = LanguageCulture.ALL_LANGUAGES;
        if (value is string culture && validCultures.Contains(culture))
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(RestApiErrorMessages.GeneralInvalidLanguageCulture);
    }
}