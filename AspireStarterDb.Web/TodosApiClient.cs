using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AspireStarterDb.Web;

public class TodosApiClient(HttpClient httpClient)
{
    public async Task<Todo[]> GetTodosAsync(int maxItems = 50, CancellationToken cancellationToken = default)
    {
        List<Todo>? todos = null;

        await foreach (var todo in httpClient.GetFromJsonAsAsyncEnumerable<Todo>("/todos", cancellationToken))
        {
            if (todos?.Count >= maxItems)
            {
                break;
            }
            if (todo is not null)
            {
                todos ??= [];
                todos.Add(todo);
            }
        }

        return todos?.ToArray() ?? [];
    }

    public async Task<Todo?> GetTodoAsync(int id, CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<Todo>($"/todos/{id}", cancellationToken);
    }

    public async Task<(bool WasCreated, Todo? CreatedTodo, HttpValidationProblemDetails? ProblemDetails)> CreateTodoAsync(
        Todo todo,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("/todos", todo, cancellationToken);

        return response.StatusCode switch
        {
            HttpStatusCode.Created => (true, await response.Content.ReadFromJsonAsync<Todo>(cancellationToken), null),
            HttpStatusCode.BadRequest => (false, default, await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>(cancellationToken)),
            _ => throw new InvalidOperationException($"Unexpected status code returned from endpoint: {response.StatusCode}")
        };
    }

    public async Task<(bool WasUpdated, HttpValidationProblemDetails? ProblemDetails)> UpdateTodoAsync(Todo todo, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PutAsJsonAsync($"/todos/{todo.Id}", todo, cancellationToken);

        return response.StatusCode switch
        {
            HttpStatusCode.OK => (true, null),
            HttpStatusCode.BadRequest => (false, await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>(cancellationToken)),
            _ => throw new InvalidOperationException($"Unexpected status code returned from endpoint: {response.StatusCode}")
        };
    }

    public async Task<HttpValidationProblemDetails?> DeleteTodoAsync(Todo todo, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.DeleteAsync($"/todos/{todo.Id}", cancellationToken);

        return response.StatusCode switch
        {
            HttpStatusCode.OK or HttpStatusCode.NotFound => null,
            _ => throw new InvalidOperationException($"Unexpected status code returned from endpoint: {response.StatusCode}")
        };
    }
}

public class Todo(int id, string title, bool isComplete)
{
    public int Id { get; set; } = id;

    [Required]
    public required string Title { get; set; } = title;

    public bool IsComplete { get; set; } = isComplete;
}
