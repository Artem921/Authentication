using ApplicationContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationContext
{
	public class AppDbContext : DbContext
	{

		public DbSet<User> Users { get; set; }

		/// <summary>
		/// Экземпляр DbContextOptions содержит информацию о конфигурации, такую ​​как строка подключения, используемый поставщик базы данных
		/// </summary>
		/// <param name="options"></param>
		/// 

		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options)
		{
			Database.Migrate();   
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().HasData(
				new User { Id = Guid.NewGuid(), Email = "admin@mail.ru", Password = "123",Role="Administrator" },
				new User { Id = Guid.NewGuid(), Email = "vova@mail.ru", Password = "123", Role = "User" });
		}
	}
}
