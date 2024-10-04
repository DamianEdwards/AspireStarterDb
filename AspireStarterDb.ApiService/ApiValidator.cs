using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AspireStarterDb.ApiService;

/// <summary>
/// Helper class for validating API models using <see cref="Validator"/>.
/// </summary>
internal static class ApiValidator
{
    /// <summary>
    /// Validates the specified object using <see cref="Validator"/>.
    /// </summary>
    /// <param name="target">The object to validate.</param>
    /// <param name="validationErrors">The validation errors.</param>
    /// <returns><see langword="true" /> if the object validates; otherwise, <see langword="false"/>.</returns>
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
