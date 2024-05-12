using System.Collections;
using System.ComponentModel.DataAnnotations;
using App.Domain;
using App.Domain.Constants;

namespace Public.DTO.ValidationAttributes;

public class IncludesAllCulturesAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var validCultures = LanguageCulture.ALL_LANGUAGES.Count;
        if (value is IList list)
        {
            if (list.Count == 2)
            {
            return ValidationResult.Success;
            }
        }
        return new ValidationResult(RestApiErrorMessages.GeneralMissingTranslationValue);
    }
}