using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatDrugiPut.Server.EF
{
	public class Baza : DbContext
	{
		public DbSet<Shared.User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Shared.User>().HasKey(u => u.Username);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=tcp:blazortest.database.windows.net,1433;Initial Catalog=ChatApp;Persist Security Info=False;User ID=blazor;Password=#12!sifra!21#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
		}
	}
}
