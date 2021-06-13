using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace blazor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Build connection string
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "localhost";
                builder.UserID = "sa";
                builder.Password = "yourStrong(!)Password";
                builder.InitialCatalog = "EFDB";

                using (EFContext context = new EFContext(builder.ConnectionString))
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                    Console.WriteLine("Created database schema from C# classes.");

                    // Create a User instance and save it to the database
                    User user = new User { FirstName = "Kohei", LastName = "Saito" };
                    context.Users.Add(user);
                    context.SaveChanges();
                    Console.WriteLine("\nCreated User: " + user.ToString());

                    // Create a new Todo instance and save it to the database
                    TodoItem todoItem = new TodoItem() { Title = "brush my teeth", IsDone = false };
                    context.TodoItems.Add(todoItem);
                    context.SaveChanges();
                    Console.WriteLine("\nCreated Todo item: " + todoItem.ToString());

                    // Assign task to user
                    todoItem.AssignedTo = user;
                    context.SaveChanges();
                    Console.WriteLine("\nAssigned Todo: '" + todoItem.Title + "' to user '" + user.GetFullName() + "'");

                    // Find incomplete todos assigned to user 'Kohei'
                    Console.WriteLine("\nIncomplete todos assigned to 'Kohei':");
                    var query = from t in context.TodoItems
                                where t.IsDone == false &&
                                t.AssignedTo.FirstName.Equals("Kohei")
                                select t;
                    foreach (var t in query)
                    {
                        Console.WriteLine(t.ToString());
                    }

                    // change the 'assigned person' of a todo
                    TodoItem todoItemToUpdate = context.TodoItems.First();
                    Console.WriteLine("\nUpdating todo: " + todoItemToUpdate.ToString());
                    User assignedUser = new User { FirstName = "Hanako", LastName = "Yamada" };
                    todoItemToUpdate.AssignedTo = assignedUser;
                    context.SaveChanges();
                    Console.WriteLine("assigned person has been changed: " + todoItemToUpdate.ToString());

                    // delete all todos
                    Console.WriteLine("\nDeleting all todos...");
                    query = from t in context.TodoItems
                            select t;
                    foreach (TodoItem t in query)
                    {
                        Console.WriteLine("Deleting todo: " + t.ToString());
                        context.TodoItems.Remove(t);
                    }
                    context.SaveChanges();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("All done.");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
