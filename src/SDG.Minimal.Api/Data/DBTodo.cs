using Microsoft.EntityFrameworkCore;

namespace SDG.Minimal.Api.Data
{
	public class DBTodo : DbContext
	{
		public DBTodo(DbContextOptions<DBTodo> options) : base(options) { }

		public DbSet<Todo> Todos => Set<Todo>();
	}
}
