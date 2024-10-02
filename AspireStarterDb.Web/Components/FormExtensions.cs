using System.Linq.Expressions;
using Microsoft.AspNetCore.Components.Forms;

namespace AspireStarterDb.Web.Components;

public static class FormExtensions
{
    public static void AddValidationErrors(this ValidationMessageStore? store, EditContext? editContext, HttpValidationProblemDetails? validationProblemDetails)
    {
        if (store is null || editContext is null || validationProblemDetails is null)
        {
            return;
        }

        if (validationProblemDetails.Errors.Count > 0)
        {
            foreach (var (key, errors) in validationProblemDetails.Errors)
            {
                var field = editContext.Field(key);
                store.Add(field, errors);
            }
        }
        else
        {
            store.Add(() => editContext.Model, validationProblemDetails.Detail ?? "An error occurred");
        }
    }

    public static string GetFieldClass<TValue>(this EditContext? editContext, Expression<Func<TValue>>? accessor)
    {
        if (accessor is not null && editContext?.IsValid(FieldIdentifier.Create(accessor)) == false)
        {
            return "form-control is-invalid";
        }
        return "form-control";
    }
}
