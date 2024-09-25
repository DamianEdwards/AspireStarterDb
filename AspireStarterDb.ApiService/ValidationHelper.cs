using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AspireStarterDb.ApiService;

internal static class ValidationHelper
{
    public static bool IsValid(object target, [NotNullWhen(false)] out Dictionary<string, string[]>? validationErrors)
    {
        var validationContext = new ValidationContext(target);
        var validationResults = new List<ValidationResult>();

        if (Validator.TryValidateObject(target, validationContext, validationResults, validateAllProperties: true))
        {
            validationErrors = null;
            return true;
        }

        // Convert the validation results into a dictionary of member names and error messages
        validationErrors = validationResults
            .SelectMany(r => r.MemberNames.Select(m => (MemberName: m, r.ErrorMessage)))
            .GroupBy(r => r.MemberName, r => r.ErrorMessage ?? "")
            .ToDictionary(g => g.Key, g => g.ToArray());

        return false;
    }
}
