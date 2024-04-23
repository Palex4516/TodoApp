

namespace ToDoList.Services
{
    public class TodoService(HttpClient client)
    {
        private readonly string _base_url = "https://localhost:7135/api/TodoItems";

        public async Task<List<TodoItem>> GetAllTodos()
        {
            List<TodoItem>? todoList = await client.GetFromJsonAsync<List<TodoItem>>(_base_url);

            if (todoList != null)
            {
                todoList.Sort((x, y) => x.Id < y.Id ? -1 : 1);
                todoList.ForEach(x => Console.WriteLine($"Id: {x.Id}, Name: {x.Name}, IsComplete: {x.IsComplete}"));
                return todoList;
            }
            else
            {
                throw new Exception("todoList is null");
            }
        }

        public async Task<TodoItem> CreateTodo(string newTodo)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(_base_url, new TodoItem { Name = newTodo });

            response.EnsureSuccessStatusCode();

            TodoItem? todoItem = await response.Content.ReadFromJsonAsync<TodoItem>();

            if (todoItem != null)
            {
                Console.WriteLine($"New todo: {todoItem.Name}");
                return todoItem;
            }
            else
            {
                throw new Exception("todoItem is null");
            }
        }

        public async Task DeleteTodo(long id)
        {
            var svar = await client.DeleteAsync(_base_url + "/" + id);
            svar.EnsureSuccessStatusCode();
            Console.WriteLine($"Deleted TodoItem with id={id}");
        }

        public async Task UpdateTodo(TodoItem todoItem)
        {
            var svar = await client.PutAsJsonAsync(_base_url + "/" + todoItem.Id, todoItem);
            svar.EnsureSuccessStatusCode();
            Console.WriteLine($"Updated TodoItem, Id: {todoItem.Id}, Name: {todoItem.Name}, IsComplete: {todoItem.IsComplete}");
        }
    }
}
