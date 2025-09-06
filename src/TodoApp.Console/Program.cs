using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Infrastructure;

var services = new ServiceCollection();
services.AddInfrastructure();
ServiceProvider serviceProvider = services.BuildServiceProvider();

var todoService = serviceProvider.GetRequiredService<ITodoService>();

bool exit = false;
while (!exit)
{
    Console.Clear();
    Console.WriteLine("===== TODO APP =====");
    Console.WriteLine("1. View all todos");
    Console.WriteLine("2. Add new todo");
    Console.WriteLine("3. View todo details");
    Console.WriteLine("4. Update todo");
    Console.WriteLine("5. Delete todo");
    Console.WriteLine("6. Mark todo as completed");
    Console.WriteLine("7. Mark todo as incomplete");
    Console.WriteLine("0. Exit");
    Console.WriteLine();
    Console.Write("Select an option: ");

    if (int.TryParse(Console.ReadLine(), out int option))
    {
        Console.WriteLine();

        switch (option)
        {
            case 1:
                await ViewAllTodos();
                break;
            case 2:
                await AddNewTodo();
                break;
            case 3:
                await ViewTodoDetails();
                break;
            case 4:
                await UpdateTodo();
                break;
            case 5:
                await DeleteTodo();
                break;
            case 6:
                await CompleteTodo();
                break;
            case 7:
                await ResetTodo();
                break;
            case 0:
                exit = true;
                break;
            default:
                Console.WriteLine("Invalid option. Press any key to continue...");
                Console.ReadKey();
                break;
        }
    }
    else
    {
        Console.WriteLine("Invalid input. Press any key to continue...");
        Console.ReadKey();
    }
}

async Task ViewAllTodos()
{
    var todos = await todoService.GetAllTodosAsync();
    if (!todos.Any())
    {
        Console.WriteLine("No todos found.");
    }
    else
    {
        Console.WriteLine("ID\t\t\t\tTitle\t\tStatus");
        Console.WriteLine("---------------------------------------------------------------");
        foreach (var todo in todos)
        {
            Console.WriteLine($"{todo.Id}\t{todo.Title}\t{(todo.IsCompleted ? "Completed" : "Pending")}");
        }
    }
    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
}

async Task AddNewTodo()
{
    Console.Write("Enter title: ");
    var title = Console.ReadLine() ?? "";

    Console.Write("Enter description: ");
    var description = Console.ReadLine() ?? "";

    var createTodoDto = new CreateTodoDto
    {
        Title = title,
        Description = description
    };

    try
    {
        var newTodo = await todoService.CreateTodoAsync(createTodoDto);
        Console.WriteLine($"Todo created with ID: {newTodo.Id}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }

    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
}

async Task ViewTodoDetails()
{
    Console.Write("Enter todo ID: ");
    if (Guid.TryParse(Console.ReadLine(), out Guid id))
    {
        var todo = await todoService.GetTodoByIdAsync(id);
        if (todo == null)
        {
            Console.WriteLine("Todo not found.");
        }
        else
        {
            Console.WriteLine($"ID: {todo.Id}");
            Console.WriteLine($"Title: {todo.Title}");
            Console.WriteLine($"Description: {todo.Description}");
            Console.WriteLine($"Status: {(todo.IsCompleted ? "Completed" : "Pending")}");
            Console.WriteLine($"Created: {todo.CreatedAt}");
            if (todo.CompletedAt.HasValue)
            {
                Console.WriteLine($"Completed: {todo.CompletedAt}");
            }
        }
    }
    else
    {
        Console.WriteLine("Invalid ID format.");
    }

    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
}

async Task UpdateTodo()
{
    Console.Write("Enter todo ID: ");
    if (Guid.TryParse(Console.ReadLine(), out Guid id))
    {
        var todo = await todoService.GetTodoByIdAsync(id);
        if (todo == null)
        {
            Console.WriteLine("Todo not found.");
        }
        else
        {
            Console.WriteLine($"Current title: {todo.Title}");
            Console.Write("Enter new title (leave empty to keep current): ");
            var title = Console.ReadLine();

            Console.WriteLine($"Current description: {todo.Description}");
            Console.Write("Enter new description (leave empty to keep current): ");
            var description = Console.ReadLine();

            var updateTodoDto = new UpdateTodoDto
            {
                Title = string.IsNullOrWhiteSpace(title) ? todo.Title : title,
                Description = string.IsNullOrWhiteSpace(description) ? todo.Description : description
            };

            try
            {
                var updatedTodo = await todoService.UpdateTodoAsync(id, updateTodoDto);
                if (updatedTodo != null)
                {
                    Console.WriteLine("Todo updated successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to update todo.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
    else
    {
        Console.WriteLine("Invalid ID format.");
    }

    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
}

async Task DeleteTodo()
{
    Console.Write("Enter todo ID: ");
    if (Guid.TryParse(Console.ReadLine(), out Guid id))
    {
        var result = await todoService.DeleteTodoAsync(id);
        if (result)
        {
            Console.WriteLine("Todo deleted successfully.");
        }
        else
        {
            Console.WriteLine("Todo not found or could not be deleted.");
        }
    }
    else
    {
        Console.WriteLine("Invalid ID format.");
    }

    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
}

async Task CompleteTodo()
{
    Console.Write("Enter todo ID: ");
    if (Guid.TryParse(Console.ReadLine(), out Guid id))
    {
        var result = await todoService.CompleteTodoAsync(id);
        if (result)
        {
            Console.WriteLine("Todo marked as completed.");
        }
        else
        {
            Console.WriteLine("Todo not found or could not be updated.");
        }
    }
    else
    {
        Console.WriteLine("Invalid ID format.");
    }

    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
}

async Task ResetTodo()
{
    Console.Write("Enter todo ID: ");
    if (Guid.TryParse(Console.ReadLine(), out Guid id))
    {
        var result = await todoService.ResetTodoAsync(id);
        if (result)
        {
            Console.WriteLine("Todo marked as incomplete.");
        }
        else
        {
            Console.WriteLine("Todo not found or could not be updated.");
        }
    }
    else
    {
        Console.WriteLine("Invalid ID format.");
    }

    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
}
