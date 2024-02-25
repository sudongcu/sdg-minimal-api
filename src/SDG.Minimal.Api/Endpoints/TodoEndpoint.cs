using Microsoft.EntityFrameworkCore;
using SDG.Minimal.Api.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace SDG.Minimal.Api.Endpoints
{
	public static class TodoEndpoint
	{
		public static void MapTodoEndpoint(this IEndpointRouteBuilder routes)
		{
			var baseRoute = routes.MapGroup("/api/v1/todo");
			
			baseRoute.MapGet("",
				[SwaggerOperation(summary: "Todo List", description: "Get all todo list")]
				async (DBTodo db)
					=> Results.Ok(await db.Todos.ToListAsync().ConfigureAwait(false)));

			baseRoute.MapGet("/{id}",
				[SwaggerOperation(summary: "A Todo", description: "Get a todo information")] 
				async (int id, DBTodo db)
					=> Results.Ok(await db.Todos.Where(w => w.Id == id).ToListAsync().ConfigureAwait(false)));

			baseRoute.MapPost("",
				[SwaggerOperation(summary: "Create Todo", description: "Create a todo information")] 
				async (HttpContext context, Todo arg, DBTodo db) =>
			{
				db.Todos.Add(arg);
				await db.SaveChangesAsync().ConfigureAwait(false);

				return Results.Created(context.Request.Path, null);
			});

			baseRoute.MapPut("/{id}",
				[SwaggerOperation(summary: "Update Todo", description: "Update a todo information")] 
			async (HttpContext context, int id, TodoBase arg, DBTodo db) =>
			{
				var todo = await db.Todos.FindAsync(id);
				if (todo is null)
					return Results.NotFound(id);

				todo.Name = arg.Name ?? todo.Name;
				todo.IsCompleted = arg.IsCompleted ?? todo.IsCompleted;
				todo.GoalDt = arg.GoalDt ?? todo.GoalDt;

				await db.SaveChangesAsync().ConfigureAwait(false);

				return Results.Created(context.Request.Path, null);
			});

			baseRoute.MapDelete("/{id}",
				[SwaggerOperation(summary: "Delete Todo", description: "Delete a todo information")] 
			async (int id, DBTodo db) =>
			{
				var todo = await db.Todos.FindAsync(id);
				if (todo is null)
					return Results.NotFound(id);

				db.Todos.Remove(todo);
				await db.SaveChangesAsync().ConfigureAwait(false);

				return Results.NoContent();
			});
		}
	}
}
