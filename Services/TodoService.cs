using Gen.TodoApi;

namespace ToDoList.Services
{
    public class TodoService(HttpClient client, IConfiguration configuration)
    {
        private readonly string _base_url = configuration["Urls:TodoApi"] + "/api/TodoItems";

        public async Task<List<TodoItemDTO>> GetAllTodos()
        {
            List<TodoItemDTO>? todoList = await client.GetFromJsonAsync<List<TodoItemDTO>>(_base_url);

            if (todoList != null)
            {
                todoList.Sort((x, y) => x.Id < y.Id ? -1 : 1);
                todoList.ForEach(x => Console.WriteLine($"Id: {x.Id}, Name: {x.Name}, Done: {x.Done}"));
                return todoList;
            }
            else
            {
                throw new Exception("todoList is null");
            }
        }

        public async Task<TodoItemDTO> CreateTodo(string newTodo)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(_base_url, new TodoItemDTO { Name = newTodo });

            response.EnsureSuccessStatusCode();

            TodoItemDTO? todoItem = await response.Content.ReadFromJsonAsync<TodoItemDTO>();

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

        public async Task UpdateTodo(TodoItemDTO todoItem)
        {
            var svar = await client.PutAsJsonAsync(_base_url + "/" + todoItem.Id, todoItem);
            svar.EnsureSuccessStatusCode();
            Console.WriteLine($"Updated TodoItem, Id: {todoItem.Id}, Name: {todoItem.Name}, Done: {todoItem.Done}");
        }
    }
}
