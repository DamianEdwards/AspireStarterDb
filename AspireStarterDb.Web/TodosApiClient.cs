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
}

public record Todo(int Id, string Title, bool IsComplete);
